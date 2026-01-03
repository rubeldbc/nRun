using nRun.Models;
using nRun.Services;

namespace nRun.UI.Forms;

public partial class FacebookIdManagerForm : Form
{
    private FacebookScraperService? _scraper;
    private FbProfile? _fetchedProfile;
    private CancellationTokenSource? _bulkCts;
    private List<FbBulkImportItem> _bulkItems = new();
    private bool _isBulkRunning;
    private bool _scraperEventsAttached;

    // Event handler references for cleanup
    private EventHandler<string>? _scraperStatusHandler;
    private EventHandler<string>? _scraperErrorHandler;

    public FbProfile? ResultProfile => _fetchedProfile;

    public FacebookIdManagerForm()
    {
        InitializeComponent();
        SetupEventHandlers();
        SetupBulkListColumns();
        LoadSettings();
    }

    private void LoadSettings()
    {
        var settings = ServiceContainer.Settings.LoadSettings();
        numDelay.Value = Math.Clamp(settings.FacebookBulkDelaySeconds, 1, 3600);
    }

    private void SaveSettings()
    {
        var settings = ServiceContainer.Settings.LoadSettings();
        settings.FacebookBulkDelaySeconds = (int)numDelay.Value;
        ServiceContainer.Settings.SaveSettings(settings);
    }

    private void SetupEventHandlers()
    {
        btnFetch.Click += BtnFetch_Click;
        btnSave.Click += BtnSave_Click;
        btnCancel.Click += BtnCancel_Click;
        txtProfileUrl.KeyDown += TxtProfileUrl_KeyDown;

        // Bulk import event handlers
        btnImport.Click += BtnImport_Click;
        btnExport.Click += BtnExport_Click;
        btnStartBulk.Click += BtnStartBulk_Click;
        btnStopBulk.Click += BtnStopBulk_Click;

        // Drag and drop support
        olvBulkList.DragEnter += OlvBulkList_DragEnter;
        olvBulkList.DragDrop += OlvBulkList_DragDrop;
    }

    private void SetupBulkListColumns()
    {
        olvColBulkUsername.AspectGetter = obj => (obj as FbBulkImportItem)?.Username;
        olvColBulkCompanyName.AspectGetter = obj => (obj as FbBulkImportItem)?.CompanyName;
        olvColBulkStatus.AspectGetter = obj => (obj as FbBulkImportItem)?.Status;
    }

    private void TxtProfileUrl_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            BtnFetch_Click(sender, e);
        }
    }

    private async void BtnFetch_Click(object? sender, EventArgs e)
    {
        var url = txtProfileUrl.Text.Trim();
        if (string.IsNullOrEmpty(url))
        {
            MessageBox.Show("Please enter a Facebook page URL.", "Input Required",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var username = FacebookScraperService.ExtractUsernameFromUrl(url);
        if (string.IsNullOrEmpty(username))
        {
            MessageBox.Show("Invalid Facebook URL. Expected format: https://www.facebook.com/pagename",
                "Invalid URL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Check if profile already exists
        var existingProfile = ServiceContainer.Database.GetFbProfileByUsername(username);
        if (existingProfile != null)
        {
            var result = MessageBox.Show(
                $"Profile {username} already exists in database.\n\nDo you want to update it?",
                "Profile Exists",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }
        }

        SetBusy(true);
        UpdateStatus("Fetching profile data...");

        try
        {
            _scraper ??= new FacebookScraperService();

            // Only attach events once to prevent memory leaks
            if (!_scraperEventsAttached)
            {
                _scraperStatusHandler = (s, msg) => UpdateStatus(msg);
                _scraperErrorHandler = (s, msg) => UpdateStatus($"Error: {msg}");
                _scraper.StatusChanged += _scraperStatusHandler;
                _scraper.ErrorOccurred += _scraperErrorHandler;
                _scraperEventsAttached = true;
            }

            var profile = await _scraper.FetchProfileInfoAsync(url);

            if (profile != null)
            {
                _fetchedProfile = profile;
                DisplayProfile(profile);
                UpdateStatus("Profile fetched successfully!");
                btnSave.Enabled = true;
            }
            else
            {
                UpdateStatus("Failed to fetch profile data.");
                MessageBox.Show("Could not fetch profile data. Please check the URL and try again.",
                    "Fetch Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error: {ex.Message}");
            MessageBox.Show($"Error fetching profile: {ex.Message}",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void DisplayProfile(FbProfile profile)
    {
        txtUserId.Text = profile.UserId.ToString();
        txtUsername.Text = profile.Username;
        txtNickname.Text = profile.Nickname;
        txtCompanyName.Text = profile.CompanyName;
        txtCompanyType.Text = profile.CompanyType;
        txtPageType.Text = profile.PageType;
        txtRegion.Text = profile.Region;
    }

    private async void BtnSave_Click(object? sender, EventArgs e)
    {
        if (_fetchedProfile == null)
        {
            MessageBox.Show("Please fetch a profile first.", "No Data",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            // Update profile with any edits
            _fetchedProfile.Username = txtUsername.Text.Trim();
            _fetchedProfile.Nickname = txtNickname.Text.Trim();
            _fetchedProfile.CompanyName = txtCompanyName.Text.Trim();
            _fetchedProfile.CompanyType = txtCompanyType.Text.Trim();
            _fetchedProfile.PageType = txtPageType.Text.Trim();
            _fetchedProfile.Region = txtRegion.Text.Trim();

            // Ensure logo is saved to fb-logos folder
            if (!string.IsNullOrEmpty(_fetchedProfile.AvatarUrl))
            {
                // Check if logo file exists, if not download it
                if (string.IsNullOrEmpty(_fetchedProfile.AvatarLocalPath) ||
                    !File.Exists(_fetchedProfile.AvatarLocalPath))
                {
                    UpdateStatus("Saving logo...");
                    _scraper ??= new FacebookScraperService();
                    _fetchedProfile.AvatarLocalPath = await _scraper.DownloadAndConvertAvatarAsync(
                        _fetchedProfile.AvatarUrl, _fetchedProfile.Username);
                }
            }

            // Check if profile exists
            if (ServiceContainer.Database.FbProfileExists(_fetchedProfile.UserId))
            {
                ServiceContainer.Database.UpdateFbProfile(_fetchedProfile);
                UpdateStatus("Profile updated successfully!");
            }
            else
            {
                ServiceContainer.Database.AddFbProfile(_fetchedProfile);
                UpdateStatus("Profile saved successfully!");
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving profile: {ex.Message}",
                "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void SetBusy(bool busy)
    {
        if (InvokeRequired)
        {
            Invoke(() => SetBusy(busy));
            return;
        }

        btnFetch.Enabled = !busy;
        txtProfileUrl.Enabled = !busy;
        progressBar.Visible = busy;
        Cursor = busy ? Cursors.WaitCursor : Cursors.Default;
    }

    private void UpdateStatus(string message)
    {
        if (InvokeRequired)
        {
            Invoke(() => UpdateStatus(message));
            return;
        }

        lblStatus.Text = message;
    }

    #region Bulk Import Methods

    private static readonly string[] SupportedImportExtensions = { ".csv", ".txt" };

    private void OlvBulkList_DragEnter(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
        {
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files?.Length > 0)
            {
                var ext = Path.GetExtension(files[0]);
                if (SupportedImportExtensions.Any(x => x.Equals(ext, StringComparison.OrdinalIgnoreCase)))
                {
                    e.Effect = DragDropEffects.Copy;
                    return;
                }
            }
        }
        e.Effect = DragDropEffects.None;
    }

    private void OlvBulkList_DragDrop(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetDataPresent(DataFormats.FileDrop) != true) return;

        var files = e.Data.GetData(DataFormats.FileDrop) as string[];
        if (files?.Length == 0) return;

        var filePath = files![0];
        var ext = Path.GetExtension(filePath);
        if (!SupportedImportExtensions.Any(x => x.Equals(ext, StringComparison.OrdinalIgnoreCase))) return;

        ImportFromFile(filePath);
    }

    private void BtnImport_Click(object? sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog
        {
            Title = "Import Facebook Pages",
            Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
            FilterIndex = 1
        };

        if (ofd.ShowDialog() != DialogResult.OK) return;

        ImportFromFile(ofd.FileName);
    }

    private void ImportFromFile(string filePath)
    {
        try
        {
            var lines = File.ReadAllLines(filePath);
            var newItems = new List<FbBulkImportItem>();
            var isCsv = filePath.EndsWith(".csv", StringComparison.OrdinalIgnoreCase);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                FbBulkImportItem? item = null;

                if (isCsv)
                {
                    // CSV format: page_link, company_type, page_type, region
                    // Note: company_name is extracted from page source during scraping
                    item = new FbBulkImportItem(line);
                }
                else
                {
                    // TXT format: one URL per line
                    var username = FbBulkImportItem.ExtractUsernameFromUrl(line.Trim());
                    if (!string.IsNullOrEmpty(username))
                    {
                        item = new FbBulkImportItem { Username = username };
                    }
                }

                if (item == null || string.IsNullOrWhiteSpace(item.Username)) continue;

                // Check for duplicates in existing list
                if (_bulkItems.Any(x => x.Username.Equals(item.Username, StringComparison.OrdinalIgnoreCase)))
                    continue;
                if (newItems.Any(x => x.Username.Equals(item.Username, StringComparison.OrdinalIgnoreCase)))
                    continue;

                newItems.Add(item);
            }

            _bulkItems.AddRange(newItems);
            olvBulkList.SetObjects(_bulkItems);
            UpdateBulkStatus($"Imported {newItems.Count} pages. Total: {_bulkItems.Count}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error importing file: {ex.Message}", "Import Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnExport_Click(object? sender, EventArgs e)
    {
        if (_bulkItems.Count == 0)
        {
            MessageBox.Show("No pages to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var sfd = new SaveFileDialog
        {
            Title = "Export Facebook Pages",
            Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
            FilterIndex = 1,
            FileName = "facebook_pages.csv"
        };

        if (sfd.ShowDialog() != DialogResult.OK) return;

        try
        {
            var csvLines = _bulkItems.Select(x =>
                $"https://www.facebook.com/{x.Username},{x.CompanyName},{x.CompanyType},{x.PageType},{x.Region}");
            File.WriteAllLines(sfd.FileName, csvLines);
            UpdateBulkStatus($"Exported {_bulkItems.Count} pages to file.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error exporting file: {ex.Message}", "Export Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnStartBulk_Click(object? sender, EventArgs e)
    {
        if (_bulkItems.Count == 0)
        {
            MessageBox.Show("Please import pages first.", "No Data",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Save settings before starting
        SaveSettings();

        SetBulkRunning(true);
        _bulkCts = new CancellationTokenSource();

        var pendingItems = _bulkItems.Where(x => x.Status == "Pending" || x.Status.StartsWith("Error")).ToList();

        progressBarBulk.Minimum = 0;
        progressBarBulk.Maximum = pendingItems.Count;
        progressBarBulk.Value = 0;

        // Reuse scraper like btnFetch does
        var baseDelay = (int)numDelay.Value;
        // Jitter is 2x base for range base to base*3 (e.g., 12 → 12-36 seconds)
        var jitterMax = Math.Max(1, baseDelay * 2);

        _scraper ??= new FacebookScraperService(baseDelay, jitterMax);
        if (!_scraperEventsAttached)
        {
            _scraper.StatusChanged += (s, msg) => UpdateBulkStatus(msg);
            _scraper.ErrorOccurred += (s, msg) => UpdateBulkStatus($"Error: {msg}");
            _scraperEventsAttached = true;
        }

        // Update rate limiter settings
        _scraper.RateLimiter.BaseDelaySeconds = baseDelay;
        _scraper.RateLimiter.JitterMaxSeconds = jitterMax;

        int processed = 0;
        int saved = 0;
        int skipped = 0;
        int errors = 0;

        try
        {
            foreach (var item in pendingItems)
            {
                if (_bulkCts.Token.IsCancellationRequested) break;

                item.Status = "Fetching...";
                RefreshBulkItem(item);

                try
                {
                    // Use full URL like btnFetch does
                    var url = $"https://www.facebook.com/{item.Username}";
                    var profile = await _scraper.FetchProfileInfoAsync(url, item, _bulkCts.Token);

                    if (profile != null)
                    {
                        // Update item with fetched profile data
                        item.UpdateFromProfile(profile);

                        // Check if already exists in database
                        if (ServiceContainer.Database.FbProfileExists(profile.UserId))
                        {
                            item.Status = "Skipped (exists)";
                            skipped++;
                        }
                        else
                        {
                            ServiceContainer.Database.AddFbProfile(profile);
                            item.Status = "Saved";
                            saved++;
                        }
                    }
                    else
                    {
                        item.Status = "Error: No data";
                        errors++;
                    }
                }
                catch (OperationCanceledException)
                {
                    item.Status = "Cancelled";
                    break;
                }
                catch (Exception ex)
                {
                    item.Status = $"Error: {ex.Message}";
                    errors++;
                }

                RefreshBulkItem(item);
                processed++;
                progressBarBulk.Value = processed;

                // Wait before next fetch using rate limiter with jitter
                if (!_bulkCts.Token.IsCancellationRequested && processed < pendingItems.Count)
                {
                    UpdateBulkStatus($"Waiting ({_scraper.GetDelayInfo()}) before next fetch...");
                    try
                    {
                        await _scraper.WaitBeforeNextRequestAsync(_bulkCts.Token);
                    }
                    catch (OperationCanceledException) { break; }
                }
            }
        }
        finally
        {
            SetBulkRunning(false);
            UpdateBulkStatus($"Completed: {saved} saved, {skipped} skipped, {errors} errors");
        }
    }

    private void BtnStopBulk_Click(object? sender, EventArgs e)
    {
        _bulkCts?.Cancel();
        UpdateBulkStatus("Stopping...");
    }

    private void SetBulkRunning(bool running)
    {
        if (InvokeRequired)
        {
            Invoke(() => SetBulkRunning(running));
            return;
        }

        _isBulkRunning = running;
        btnStartBulk.Enabled = !running;
        btnStopBulk.Enabled = running;
        btnImport.Enabled = !running;
        numDelay.Enabled = !running;
    }

    private void UpdateBulkStatus(string message)
    {
        if (InvokeRequired)
        {
            Invoke(() => UpdateBulkStatus(message));
            return;
        }

        lblBulkStatus.Text = message;
    }

    private void RefreshBulkItem(FbBulkImportItem item)
    {
        if (InvokeRequired)
        {
            Invoke(() => RefreshBulkItem(item));
            return;
        }

        olvBulkList.RefreshObject(item);
    }

    #endregion

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);

        // Cancel any running operations
        _bulkCts?.Cancel();

        // Unsubscribe event handlers to prevent memory leaks
        if (_scraper != null && _scraperEventsAttached)
        {
            if (_scraperStatusHandler != null)
                _scraper.StatusChanged -= _scraperStatusHandler;
            if (_scraperErrorHandler != null)
                _scraper.ErrorOccurred -= _scraperErrorHandler;
        }

        // Unsubscribe form event handlers
        btnFetch.Click -= BtnFetch_Click;
        btnSave.Click -= BtnSave_Click;
        btnCancel.Click -= BtnCancel_Click;
        txtProfileUrl.KeyDown -= TxtProfileUrl_KeyDown;
        btnImport.Click -= BtnImport_Click;
        btnExport.Click -= BtnExport_Click;
        btnStartBulk.Click -= BtnStartBulk_Click;
        btnStopBulk.Click -= BtnStopBulk_Click;
        olvBulkList.DragEnter -= OlvBulkList_DragEnter;
        olvBulkList.DragDrop -= OlvBulkList_DragDrop;

        // Dispose resources
        _scraper?.Dispose();
        _bulkCts?.Dispose();

        // Clear collections
        _bulkItems.Clear();
    }
}
