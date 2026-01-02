using nRun.Models;
using nRun.Services;

namespace nRun.UI.Forms;

public partial class TikTokIdManagerForm : Form
{
    private TikTokScraperService? _scraper;
    private TkProfile? _fetchedProfile;
    private CancellationTokenSource? _bulkCts;
    private List<BulkImportItem> _bulkItems = new();
    private bool _isBulkRunning;
    private bool _scraperEventsAttached;

    public TkProfile? ResultProfile => _fetchedProfile;

    public TikTokIdManagerForm()
    {
        InitializeComponent();
        SetupEventHandlers();
        SetupBulkListColumns();
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
        olvColBulkUsername.AspectGetter = obj => (obj as BulkImportItem)?.Username;
        olvColBulkNickname.AspectGetter = obj => (obj as BulkImportItem)?.Nickname;
        olvColBulkStatus.AspectGetter = obj => (obj as BulkImportItem)?.Status;
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
            MessageBox.Show("Please enter a TikTok profile URL.", "Input Required",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var username = TikTokScraperService.ExtractUsernameFromUrl(url);
        if (string.IsNullOrEmpty(username))
        {
            MessageBox.Show("Invalid TikTok URL. Expected format: https://www.tiktok.com/@username",
                "Invalid URL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Check if profile already exists
        var existingProfile = ServiceContainer.Database.GetTkProfileByUsername(username);
        if (existingProfile != null)
        {
            var result = MessageBox.Show(
                $"Profile @{username} already exists in database.\n\nDo you want to update it?",
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
            _scraper ??= new TikTokScraperService();
            _scraper.StatusChanged += (s, msg) => UpdateStatus(msg);
            _scraper.ErrorOccurred += (s, msg) => UpdateStatus($"Error: {msg}");

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

    private void DisplayProfile(TkProfile profile)
    {
        txtUserId.Text = profile.UserId.ToString();
        txtUsername.Text = profile.Username;
        txtNickname.Text = profile.Nickname;
        txtRegion.Text = profile.Region;
        txtCreatedAt.Text = profile.CreatedAtTs?.ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A";
    }

    private void BtnSave_Click(object? sender, EventArgs e)
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
            _fetchedProfile.Region = txtRegion.Text.Trim();

            // Check if profile exists
            if (ServiceContainer.Database.TkProfileExists(_fetchedProfile.UserId))
            {
                ServiceContainer.Database.UpdateTkProfile(_fetchedProfile);
                UpdateStatus("Profile updated successfully!");
            }
            else
            {
                ServiceContainer.Database.AddTkProfile(_fetchedProfile);
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

    private static readonly string[] SupportedImportExtensions = { ".txt", ".csv" };

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
            Title = "Import Usernames",
            Filter = "Text Files (*.txt)|*.txt|CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
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
            var newItems = new List<BulkImportItem>();
            var isCsv = filePath.EndsWith(".csv", StringComparison.OrdinalIgnoreCase);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                // For CSV, split by comma and process each field
                var fields = isCsv ? line.Split(',') : new[] { line };

                foreach (var field in fields)
                {
                    var username = ExtractUsername(field.Trim());
                    if (string.IsNullOrWhiteSpace(username)) continue;

                    // Check for duplicates in existing list
                    if (_bulkItems.Any(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                        continue;
                    if (newItems.Any(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                        continue;

                    newItems.Add(new BulkImportItem(username));
                }
            }

            _bulkItems.AddRange(newItems);
            olvBulkList.SetObjects(_bulkItems);
            UpdateBulkStatus($"Imported {newItems.Count} usernames. Total: {_bulkItems.Count}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error importing file: {ex.Message}", "Import Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private static string ExtractUsername(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        // Remove quotes if present (CSV fields)
        input = input.Trim().Trim('"').Trim();

        // If it's a TikTok URL, extract username
        if (input.Contains("tiktok.com", StringComparison.OrdinalIgnoreCase))
        {
            var extracted = TikTokScraperService.ExtractUsernameFromUrl(input);
            if (!string.IsNullOrEmpty(extracted)) return extracted;
        }

        // Remove @ prefix if present
        return input.TrimStart('@');
    }

    private void BtnExport_Click(object? sender, EventArgs e)
    {
        if (_bulkItems.Count == 0)
        {
            MessageBox.Show("No usernames to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var sfd = new SaveFileDialog
        {
            Title = "Export Usernames",
            Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
            FilterIndex = 1,
            FileName = "tiktok_usernames.txt"
        };

        if (sfd.ShowDialog() != DialogResult.OK) return;

        try
        {
            var usernames = _bulkItems.Select(x => x.Username);
            File.WriteAllLines(sfd.FileName, usernames);
            UpdateBulkStatus($"Exported {_bulkItems.Count} usernames to file.");
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
            MessageBox.Show("Please import usernames first.", "No Data",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SetBulkRunning(true);
        _bulkCts = new CancellationTokenSource();

        var delaySeconds = (int)numDelay.Value;
        var pendingItems = _bulkItems.Where(x => x.Status == "Pending" || x.Status.StartsWith("Error")).ToList();

        progressBarBulk.Minimum = 0;
        progressBarBulk.Maximum = pendingItems.Count;
        progressBarBulk.Value = 0;

        // Reuse scraper like btnFetch does
        _scraper ??= new TikTokScraperService();
        if (!_scraperEventsAttached)
        {
            _scraper.StatusChanged += (s, msg) => UpdateBulkStatus(msg);
            _scraper.ErrorOccurred += (s, msg) => UpdateBulkStatus($"Error: {msg}");
            _scraperEventsAttached = true;
        }

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
                    var url = $"https://www.tiktok.com/@{item.Username}";
                    var profile = await _scraper.FetchProfileInfoAsync(url, _bulkCts.Token);

                    if (profile != null)
                    {
                        // Update item with fetched profile data
                        item.UpdateFromProfile(profile);

                        // Check if already exists in database
                        if (ServiceContainer.Database.TkProfileExists(profile.UserId))
                        {
                            item.Status = "Skipped (exists)";
                            skipped++;
                        }
                        else
                        {
                            ServiceContainer.Database.AddTkProfile(profile);
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

                // Wait before next fetch (unless cancelled or last item)
                if (!_bulkCts.Token.IsCancellationRequested && processed < pendingItems.Count)
                {
                    UpdateBulkStatus($"Waiting {delaySeconds}s before next fetch...");
                    try
                    {
                        await Task.Delay(delaySeconds * 1000, _bulkCts.Token);
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

    private void RefreshBulkItem(BulkImportItem item)
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
        _bulkCts?.Cancel();
        _scraper?.Dispose();
    }
}
