using nRun.Services;

namespace nRun.UI.Forms;

public partial class SettingsForm : Form
{
    public SettingsForm()
    {
        InitializeComponent();
        SetupEventHandlers();
        LoadSettings();
        LoadChromeVersionInfo();
    }

    private void SetupEventHandlers()
    {
        btnSave.Click += BtnSave_Click;
        btnDatabase.Click += BtnDatabase_Click;
        btnCheckVersion.Click += BtnCheckVersion_Click;
        btnDownloadDriver.Click += BtnDownloadDriver_Click;
        btnTestMemurai.Click += BtnTestMemurai_Click;
    }

    private void LoadSettings()
    {
        var settings = ServiceContainer.Settings.LoadSettings();

        numCheckInterval.Value = settings.CheckIntervalMinutes;
        numDelayBetweenLinks.Value = settings.DelayBetweenLinksSeconds;
        numMaxArticles.Value = settings.MaxArticlesPerSite;
        numBrowserTimeout.Value = settings.BrowserTimeoutSeconds;
        numMaxDisplayed.Value = settings.MaxDisplayedArticles;
        chkUseHeadless.Checked = settings.UseHeadlessBrowser;
        chkAutoStart.Checked = settings.AutoStartScraping;

        // Memurai settings
        txtMemuraiHost.Text = settings.MemuraiHost;
        numMemuraiPort.Value = settings.MemuraiPort;
        txtMemuraiPassword.Text = settings.MemuraiPassword;
        numMemuraiSyncInterval.Value = settings.MemuraiSyncIntervalSeconds;

        // Reset Memurai status
        lblMemuraiStatus.Text = "Not tested";
        lblMemuraiStatus.ForeColor = Color.Gray;
    }

    private void LoadChromeVersionInfo()
    {
        try
        {
            lblChromeVersion.Text = "Checking...";
            lblDriverVersion.Text = "Checking...";
            lblVersionStatus.Text = "";

            // Check version in background
            Task.Run(() =>
            {
                var result = ChromeVersionService.CheckVersionCompatibility();

                this.Invoke(() =>
                {
                    lblChromeVersion.Text = result.ChromeVersion ?? "Not found";
                    lblDriverVersion.Text = result.ChromeDriverVersion ?? "Not found";

                    UpdateVersionStatus(result);
                });
            });
        }
        catch (Exception ex)
        {
            lblVersionStatus.Text = $"Error: {ex.Message}";
            lblVersionStatus.ForeColor = Color.Red;
        }
    }

    private void UpdateVersionStatus(VersionCheckResult result)
    {
        lblVersionStatus.Text = result.Message;

        switch (result.Status)
        {
            case VersionStatus.Compatible:
                lblVersionStatus.ForeColor = Color.Green;
                btnDownloadDriver.Enabled = false;
                btnDownloadDriver.Text = "✓ OK";
                break;
            case VersionStatus.Mismatch:
                lblVersionStatus.ForeColor = Color.OrangeRed;
                btnDownloadDriver.Enabled = true;
                btnDownloadDriver.Text = "Download";
                break;
            case VersionStatus.DriverNotFound:
                lblVersionStatus.ForeColor = Color.Red;
                btnDownloadDriver.Enabled = true;
                btnDownloadDriver.Text = "Download";
                break;
            case VersionStatus.ChromeNotFound:
                lblVersionStatus.ForeColor = Color.Red;
                btnDownloadDriver.Enabled = false;
                btnDownloadDriver.Text = "No Chrome";
                break;
        }
    }

    private void BtnCheckVersion_Click(object? sender, EventArgs e)
    {
        LoadChromeVersionInfo();
    }

    private async void BtnDownloadDriver_Click(object? sender, EventArgs e)
    {
        btnDownloadDriver.Enabled = false;
        btnDownloadDriver.Text = "Downloading...";
        progressBarDownload.Visible = true;
        progressBarDownload.Value = 0;

        try
        {
            var progress = new Progress<int>(value =>
            {
                if (progressBarDownload.InvokeRequired)
                    progressBarDownload.Invoke(() => progressBarDownload.Value = value);
                else
                    progressBarDownload.Value = value;
            });

            var result = await ChromeVersionService.DownloadMatchingChromeDriverAsync(progress);

            if (result.Success)
            {
                MessageBox.Show(result.Message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadChromeVersionInfo(); // Refresh version info
            }
            else
            {
                MessageBox.Show(result.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Download error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            progressBarDownload.Visible = false;
            btnDownloadDriver.Enabled = true;
            LoadChromeVersionInfo();
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        try
        {
            var settings = ServiceContainer.Settings.LoadSettings();

            settings.CheckIntervalMinutes = (int)numCheckInterval.Value;
            settings.DelayBetweenLinksSeconds = (int)numDelayBetweenLinks.Value;
            settings.MaxArticlesPerSite = (int)numMaxArticles.Value;
            settings.BrowserTimeoutSeconds = (int)numBrowserTimeout.Value;
            settings.MaxDisplayedArticles = (int)numMaxDisplayed.Value;
            settings.UseHeadlessBrowser = chkUseHeadless.Checked;
            settings.AutoStartScraping = chkAutoStart.Checked;

            // Memurai settings
            settings.MemuraiHost = txtMemuraiHost.Text.Trim();
            settings.MemuraiPort = (int)numMemuraiPort.Value;
            settings.MemuraiPassword = txtMemuraiPassword.Text;
            settings.MemuraiSyncIntervalSeconds = (int)numMemuraiSyncInterval.Value;

            ServiceContainer.Settings.SaveSettings(settings);

            // Update Memurai service settings
            ServiceContainer.Memurai.UpdateSettings();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnTestMemurai_Click(object? sender, EventArgs e)
    {
        btnTestMemurai.Enabled = false;
        lblMemuraiStatus.Text = "Testing connection...";
        lblMemuraiStatus.ForeColor = Color.DarkOrange;

        try
        {
            // Validate inputs
            var host = txtMemuraiHost.Text.Trim();
            if (string.IsNullOrWhiteSpace(host))
            {
                lblMemuraiStatus.Text = "Host is required";
                lblMemuraiStatus.ForeColor = Color.Red;
                return;
            }

            // Temporarily update settings for testing
            var settings = ServiceContainer.Settings.LoadSettings();
            settings.MemuraiHost = host;
            settings.MemuraiPort = (int)numMemuraiPort.Value;
            settings.MemuraiPassword = txtMemuraiPassword.Text;
            ServiceContainer.Settings.SaveSettings(settings);

            // Update service and test
            ServiceContainer.Memurai.UpdateSettings();
            var success = await ServiceContainer.Memurai.TestConnectionAsync();

            if (success)
            {
                lblMemuraiStatus.Text = "Connected successfully";
                lblMemuraiStatus.ForeColor = Color.Green;
            }
            else
            {
                lblMemuraiStatus.Text = "Connection failed - Server not responding";
                lblMemuraiStatus.ForeColor = Color.Red;
            }
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            if (message.Contains("No connection"))
                message = "Server not reachable";
            else if (message.Contains("WRONGPASS") || message.Contains("AUTH"))
                message = "Invalid password";
            else if (message.Contains("timeout", StringComparison.OrdinalIgnoreCase))
                message = "Connection timed out";
            else if (message.Length > 40)
                message = message.Substring(0, 40) + "...";

            lblMemuraiStatus.Text = $"Error: {message}";
            lblMemuraiStatus.ForeColor = Color.Red;
        }
        finally
        {
            btnTestMemurai.Enabled = true;
        }
    }

    private void BtnDatabase_Click(object? sender, EventArgs e)
    {
        using var form = new DatabaseConnectionForm();
        form.ShowDialog(this);
    }
}
