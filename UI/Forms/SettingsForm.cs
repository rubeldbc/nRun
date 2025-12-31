using nRun.Data;

namespace nRun.UI.Forms;

public partial class SettingsForm : Form
{
    public SettingsForm()
    {
        InitializeComponent();
        SetupEventHandlers();
        LoadSettings();
    }

    private void SetupEventHandlers()
    {
        btnSave.Click += BtnSave_Click;
        btnDatabase.Click += BtnDatabase_Click;
    }

    private void LoadSettings()
    {
        var settings = SettingsManager.LoadSettings();

        numCheckInterval.Value = settings.CheckIntervalMinutes;
        numDelayBetweenLinks.Value = settings.DelayBetweenLinksSeconds;
        numMaxArticles.Value = settings.MaxArticlesPerSite;
        numBrowserTimeout.Value = settings.BrowserTimeoutSeconds;
        chkUseHeadless.Checked = settings.UseHeadlessBrowser;
        chkAutoStart.Checked = settings.AutoStartScraping;
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        try
        {
            var settings = SettingsManager.LoadSettings();

            settings.CheckIntervalMinutes = (int)numCheckInterval.Value;
            settings.DelayBetweenLinksSeconds = (int)numDelayBetweenLinks.Value;
            settings.MaxArticlesPerSite = (int)numMaxArticles.Value;
            settings.BrowserTimeoutSeconds = (int)numBrowserTimeout.Value;
            settings.UseHeadlessBrowser = chkUseHeadless.Checked;
            settings.AutoStartScraping = chkAutoStart.Checked;

            SettingsManager.SaveSettings(settings);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnDatabase_Click(object? sender, EventArgs e)
    {
        using var form = new DatabaseConnectionForm();
        form.ShowDialog(this);
    }
}
