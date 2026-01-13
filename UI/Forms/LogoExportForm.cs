using nRun.Models;
using nRun.Services;

namespace nRun.UI.Forms;

public partial class LogoExportForm : Form
{
    private readonly LogoExportPlatform _platform;
    private readonly List<ProfileInfo> _profiles;
    private LogoExportService? _exportService;
    private CancellationTokenSource? _cts;
    private bool _isExporting;

    // Internal class to hold profile information
    private class ProfileInfo
    {
        public string Username { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
    }

    public LogoExportForm(IEnumerable<TkProfile> profiles, LogoExportPlatform platform)
        : this(profiles.Select(p => new ProfileInfo { Username = p.Username, AvatarUrl = p.AvatarUrl }), platform)
    {
    }

    public LogoExportForm(IEnumerable<FbProfile> profiles, LogoExportPlatform platform)
        : this(profiles.Select(p => new ProfileInfo { Username = p.Username, AvatarUrl = p.AvatarUrl }), platform)
    {
    }

    private LogoExportForm(IEnumerable<ProfileInfo> profiles, LogoExportPlatform platform)
    {
        InitializeComponent();
        _platform = platform;
        _profiles = profiles.ToList();

        SetupForm();
        SetupEventHandlers();
    }

    private void SetupForm()
    {
        // Set title based on platform
        Text = _platform == LogoExportPlatform.TikTok
            ? "Export TikTok Logos"
            : "Export Facebook Logos";

        // Set selected count
        lblSelectedCount.Text = $"Selected: {_profiles.Count} profile{(_profiles.Count != 1 ? "s" : "")}";

        // Set default format
        cboFormat.SelectedIndex = 0; // PNG

        // Set default path
        txtSavePath.Text = LogoExportService.GetDefaultOutputFolder(_platform);

        // Update size controls state
        UpdateSizeControlsState();
    }

    private void SetupEventHandlers()
    {
        btnBrowse.Click += BtnBrowse_Click;
        btnExport.Click += BtnExport_Click;
        btnCancel.Click += BtnCancel_Click;
        rbOriginalSize.CheckedChanged += RbSize_CheckedChanged;
        rbCustomSize.CheckedChanged += RbSize_CheckedChanged;
    }

    private void RbSize_CheckedChanged(object? sender, EventArgs e)
    {
        UpdateSizeControlsState();
    }

    private void UpdateSizeControlsState()
    {
        var enableCustom = rbCustomSize.Checked;
        numWidth.Enabled = enableCustom;
        numHeight.Enabled = enableCustom;
        lblWidth.Enabled = enableCustom;
        lblHeight.Enabled = enableCustom;
    }

    private void BtnBrowse_Click(object? sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog
        {
            Description = "Select output folder for logos",
            UseDescriptionForTitle = true,
            SelectedPath = txtSavePath.Text
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            txtSavePath.Text = dialog.SelectedPath;
        }
    }

    private async void BtnExport_Click(object? sender, EventArgs e)
    {
        if (_isExporting)
        {
            return;
        }

        // Validate
        if (string.IsNullOrWhiteSpace(txtSavePath.Text))
        {
            MessageBox.Show("Please select an output folder.", "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_profiles.Count == 0)
        {
            MessageBox.Show("No profiles selected.", "No Selection",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SetExporting(true);
        _cts = new CancellationTokenSource();

        var format = GetSelectedFormat();
        var outputFolder = txtSavePath.Text;
        var useOriginalSize = rbOriginalSize.Checked;
        var customWidth = (int)numWidth.Value;
        var customHeight = (int)numHeight.Value;
        var delaySeconds = (int)numDelay.Value;

        progressBar.Maximum = _profiles.Count;
        progressBar.Value = 0;

        int exported = 0;
        int errors = 0;
        int fetched = 0;

        try
        {
            _exportService ??= new LogoExportService();
            _exportService.StatusChanged += (s, msg) => UpdateStatus(msg);
            _exportService.ErrorOccurred += (s, msg) => UpdateStatus($"Error: {msg}");

            // Ensure output folder exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            for (int i = 0; i < _profiles.Count; i++)
            {
                if (_cts.Token.IsCancellationRequested)
                {
                    break;
                }

                var profile = _profiles[i];
                var avatarUrl = profile.AvatarUrl;

                // If no avatar URL, fetch it
                if (string.IsNullOrEmpty(avatarUrl))
                {
                    UpdateStatus($"Fetching avatar URL for {profile.Username}...");

                    avatarUrl = await FetchAvatarUrlAsync(profile.Username, _cts.Token);

                    if (!string.IsNullOrEmpty(avatarUrl))
                    {
                        profile.AvatarUrl = avatarUrl;
                        fetched++;
                    }
                    else
                    {
                        UpdateStatus($"Failed to fetch avatar for {profile.Username}");
                        errors++;
                        progressBar.Value = i + 1;
                        UpdateProgress(i + 1, _profiles.Count);

                        // Add delay before next fetch if not last item
                        if (i < _profiles.Count - 1 && !_cts.Token.IsCancellationRequested)
                        {
                            await DelayWithCountdown(delaySeconds, _cts.Token);
                        }
                        continue;
                    }
                }

                // Export the logo
                var success = await _exportService.ExportLogoAsync(
                    avatarUrl,
                    profile.Username,
                    outputFolder,
                    format,
                    useOriginalSize,
                    customWidth,
                    customHeight,
                    _cts.Token);

                if (success)
                {
                    exported++;
                }
                else
                {
                    errors++;
                }

                progressBar.Value = i + 1;
                UpdateProgress(i + 1, _profiles.Count);

                // Add delay before next profile if we had to fetch and not last item
                if (i < _profiles.Count - 1 && !_cts.Token.IsCancellationRequested)
                {
                    // Only delay if we fetched the avatar (to avoid rate limiting)
                    if (fetched > 0 && string.IsNullOrEmpty(_profiles[i + 1].AvatarUrl))
                    {
                        await DelayWithCountdown(delaySeconds, _cts.Token);
                    }
                }
            }

            if (_cts.Token.IsCancellationRequested)
            {
                UpdateStatus($"Cancelled. Exported {exported} logo(s).");
            }
            else
            {
                var fetchMsg = fetched > 0 ? $", {fetched} fetched" : "";
                UpdateStatus($"Completed! Exported {exported} logo(s){fetchMsg}, {errors} error(s).");
            }

            if (exported > 0)
            {
                var openFolder = MessageBox.Show(
                    $"Exported {exported} logo(s) to:\n{outputFolder}\n\nOpen folder?",
                    "Export Complete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (openFolder == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", outputFolder);
                }
            }
        }
        catch (OperationCanceledException)
        {
            UpdateStatus($"Cancelled. Exported {exported} logo(s).");
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error: {ex.Message}");
            MessageBox.Show($"Export failed: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetExporting(false);
        }
    }

    private async Task<string?> FetchAvatarUrlAsync(string username, CancellationToken cancellationToken)
    {
        try
        {
            if (_platform == LogoExportPlatform.TikTok)
            {
                using var scraper = new TikTokScraperService();
                scraper.StatusChanged += (s, msg) => UpdateStatus(msg);

                var url = $"https://www.tiktok.com/@{username}";
                var profile = await scraper.FetchProfileInfoAsync(url, cancellationToken);

                return profile?.AvatarUrl;
            }
            else
            {
                using var scraper = new FacebookScraperService();
                scraper.StatusChanged += (s, msg) => UpdateStatus(msg);

                var url = $"https://www.facebook.com/{username}";
                var profile = await scraper.FetchProfileInfoAsync(url, null, cancellationToken);

                return profile?.AvatarUrl;
            }
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error fetching {username}: {ex.Message}");
            return null;
        }
    }

    private async Task DelayWithCountdown(int seconds, CancellationToken cancellationToken)
    {
        for (int i = seconds; i > 0; i--)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            UpdateStatus($"Waiting {i} seconds before next request...");
            await Task.Delay(1000, cancellationToken);
        }
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        if (_isExporting)
        {
            _cts?.Cancel();
            UpdateStatus("Cancelling...");
        }
        else
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }

    private LogoImageFormat GetSelectedFormat()
    {
        return cboFormat.SelectedIndex switch
        {
            0 => LogoImageFormat.Png,
            1 => LogoImageFormat.WebP,
            2 => LogoImageFormat.Jpg,
            _ => LogoImageFormat.Png
        };
    }

    private void SetExporting(bool exporting)
    {
        if (InvokeRequired)
        {
            Invoke(() => SetExporting(exporting));
            return;
        }

        _isExporting = exporting;
        btnExport.Enabled = !exporting;
        btnBrowse.Enabled = !exporting;
        cboFormat.Enabled = !exporting;
        txtSavePath.Enabled = !exporting;
        grpSize.Enabled = !exporting;
        numDelay.Enabled = !exporting;
        btnCancel.Text = exporting ? "Stop" : "Cancel";
        Cursor = exporting ? Cursors.WaitCursor : Cursors.Default;
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

    private void UpdateProgress(int current, int total)
    {
        if (InvokeRequired)
        {
            Invoke(() => UpdateProgress(current, total));
            return;
        }

        lblProgress.Text = $"{current}/{total}";
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        base.OnFormClosing(e);

        // Cancel any running export
        _cts?.Cancel();

        // Cleanup
        _exportService?.Dispose();
        _cts?.Dispose();

        // Unsubscribe event handlers
        btnBrowse.Click -= BtnBrowse_Click;
        btnExport.Click -= BtnExport_Click;
        btnCancel.Click -= BtnCancel_Click;
        rbOriginalSize.CheckedChanged -= RbSize_CheckedChanged;
        rbCustomSize.CheckedChanged -= RbSize_CheckedChanged;
    }
}
