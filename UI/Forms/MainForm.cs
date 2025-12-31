using System.ComponentModel;
using System.Diagnostics;
using nRun.Models;
using nRun.Services;

namespace nRun.UI.Forms;

public partial class MainForm : Form
{
    private BackgroundScraperService? _backgroundScraper;
    private List<NewsInfo> _articles = new();
    private const int MaxDisplayedArticles = 100;
    private const int MaxLogLines = 500;

    public MainForm()
    {
        InitializeComponent();

        // Exit early in design mode
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;

        _backgroundScraper = new BackgroundScraperService();
        SetupEventHandlers();
    }

    private void SetupEventHandlers()
    {
        // Form events
        this.Load += MainForm_Load;
        this.FormClosing += MainForm_FormClosing;

        // Toolbar events
        btnStartStop.Click += BtnStartStop_Click;
        btnScrapeNow.Click += BtnScrapeNow_Click;
        btnSettings.Click += BtnSettings_Click;

        // Site list events
        btnAddSite.Click += BtnAddSite_Click;
        btnEditSite.Click += BtnEditSite_Click;
        btnDeleteSite.Click += BtnDeleteSite_Click;
        btnRefreshSites.Click += BtnRefreshSites_Click;
        btnManageSites.Click += BtnManageSites_Click;
        listBoxSites.DoubleClick += ListBoxSites_DoubleClick;

        // Article list events
        olvArticles.DoubleClick += OlvArticles_DoubleClick;

        // Context menu events
        menuItemOpenUrl.Click += MenuItemOpenUrl_Click;
        menuItemCopyUrl.Click += MenuItemCopyUrl_Click;
        menuItemCopyTitle.Click += MenuItemCopyTitle_Click;
        menuItemViewBody.Click += MenuItemViewBody_Click;
        menuItemDelete.Click += MenuItemDelete_Click;

        // Debug log events
        btnClearLog.Click += BtnClearLog_Click;
        btnClearErrorLog.Click += BtnClearErrorLog_Click;

        // Background scraper events
        if (_backgroundScraper != null)
        {
            _backgroundScraper.ArticleScraped += BackgroundScraper_ArticleScraped;
            _backgroundScraper.StatusChanged += BackgroundScraper_StatusChanged;
            _backgroundScraper.ProgressChanged += BackgroundScraper_ProgressChanged;
            _backgroundScraper.RunningStateChanged += BackgroundScraper_RunningStateChanged;
        }
    }

    private void MainForm_Load(object? sender, EventArgs e)
    {
        LogDebug("Application started", "INFO");

        // Check database connection
        if (!ServiceContainer.Database.IsConnected)
        {
            UpdateStatus("Database not connected - click Settings to configure");
            lblStatus.ForeColor = Color.Red;
        }
        else
        {
            LogDebug("Database connected successfully", "SUCCESS");
        }

        LoadSites();
        LogDebug($"Loaded {listBoxSites.Items.Count} sites", "INFO");

        LoadArticles();
        UpdateArticleCount();

        // Auto-start if configured and connected
        var settings = ServiceContainer.Settings.LoadSettings();
        if (settings.AutoStartScraping && ServiceContainer.Database.IsConnected)
        {
            LogDebug("Auto-start enabled, starting background scraper", "INFO");
            _backgroundScraper?.Start();
        }
    }

    private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        // Prevent closing if scraping is in progress
        if (_backgroundScraper?.IsRunning == true)
        {
            var result = MessageBox.Show(
                "A scraping operation is currently running.\n\n" +
                "Please stop the operation first before closing the application.\n\n" +
                "Do you want to stop the scraping operation now?",
                "Operation In Progress",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _backgroundScraper.Stop();
                LogDebug("Scraping stopped by user request", "INFO");
            }
            else
            {
                e.Cancel = true;
                return;
            }
        }

        try
        {
            // Stop and dispose background scraper
            _backgroundScraper?.Stop();
            _backgroundScraper?.Dispose();
            _backgroundScraper = null;

            // Force cleanup all Chrome/ChromeDriver processes
            ProcessCleanupService.ForceCleanupAll();
        }
        catch
        {
            // Ensure we exit even if cleanup fails
        }
    }

    #region Site Management

    private void LoadSites()
    {
        var sites = ServiceContainer.Database.GetAllSites();
        listBoxSites.DataSource = null;
        listBoxSites.DataSource = sites;
        listBoxSites.DisplayMember = "SiteName";
    }

    private void BtnAddSite_Click(object? sender, EventArgs e)
    {
        if (!ServiceContainer.Database.IsConnected)
        {
            MessageBox.Show("Please configure database connection first.", "Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var form = new SiteEditForm();
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            LoadSites();
        }
    }

    private void BtnEditSite_Click(object? sender, EventArgs e)
    {
        EditSelectedSite();
    }

    private void BtnDeleteSite_Click(object? sender, EventArgs e)
    {
        if (listBoxSites.SelectedItem is SiteInfo site)
        {
            var result = MessageBox.Show(
                $"Delete '{site.SiteName}' and all its articles?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                ServiceContainer.Database.DeleteSite(site.SiteId);
                LoadSites();
                LoadArticles();
                UpdateArticleCount();
            }
        }
    }

    private void BtnRefreshSites_Click(object? sender, EventArgs e)
    {
        LoadSites();
        LoadArticles();
    }

    private void ListBoxSites_DoubleClick(object? sender, EventArgs e)
    {
        EditSelectedSite();
    }

    private void EditSelectedSite()
    {
        if (listBoxSites.SelectedItem is SiteInfo site)
        {
            // Get all sites for navigation
            var allSites = listBoxSites.DataSource as List<SiteInfo>;
            var currentIndex = allSites?.IndexOf(site) ?? -1;

            using var form = new SiteEditForm(site, allSites, currentIndex);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                LoadSites();
            }
        }
    }

    private void BtnManageSites_Click(object? sender, EventArgs e)
    {
        if (!ServiceContainer.Database.IsConnected)
        {
            MessageBox.Show("Please configure database connection first.", "Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var form = new SiteManagementForm();
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            LoadSites();
            LoadArticles();
            UpdateArticleCount();
        }
    }

    #endregion

    #region Article Management

    private void LoadArticles()
    {
        _articles = ServiceContainer.Database.GetRecentNews(MaxDisplayedArticles);
        olvArticles.SetObjects(_articles);
    }

    private void UpdateArticleCount()
    {
        var count = ServiceContainer.Database.GetNewsCount();
        statusArticleCount.Text = $"{count} articles";
    }

    private void AddArticleToTop(NewsInfo article)
    {
        if (InvokeRequired)
        {
            Invoke(() => AddArticleToTop(article));
            return;
        }

        _articles.Insert(0, article);

        // Keep only the last 100 items in the display list
        if (_articles.Count > MaxDisplayedArticles)
        {
            _articles.RemoveAt(_articles.Count - 1);
        }

        olvArticles.SetObjects(_articles);
        olvArticles.EnsureModelVisible(article);
        UpdateArticleCount();
    }

    private NewsInfo? GetSelectedArticle()
    {
        return olvArticles.SelectedObject as NewsInfo;
    }

    private void OlvArticles_DoubleClick(object? sender, EventArgs e)
    {
        OpenSelectedArticleUrl();
    }

    private void MenuItemOpenUrl_Click(object? sender, EventArgs e)
    {
        OpenSelectedArticleUrl();
    }

    private void MenuItemCopyUrl_Click(object? sender, EventArgs e)
    {
        var article = GetSelectedArticle();
        if (article != null)
        {
            Clipboard.SetText(article.NewsUrl);
            UpdateStatus("URL copied to clipboard");
        }
    }

    private void MenuItemCopyTitle_Click(object? sender, EventArgs e)
    {
        var article = GetSelectedArticle();
        if (article != null)
        {
            Clipboard.SetText(article.NewsTitle);
            UpdateStatus("Title copied to clipboard");
        }
    }

    private void MenuItemViewBody_Click(object? sender, EventArgs e)
    {
        var article = GetSelectedArticle();
        if (article != null)
        {
            using var form = new ArticleViewForm(article);
            form.ShowDialog(this);
        }
    }

    private void MenuItemDelete_Click(object? sender, EventArgs e)
    {
        var article = GetSelectedArticle();
        if (article != null)
        {
            ServiceContainer.Database.DeleteNews(article.Serial);
            _articles.Remove(article);
            olvArticles.RemoveObject(article);
            UpdateArticleCount();
        }
    }

    private void OpenSelectedArticleUrl()
    {
        var article = GetSelectedArticle();
        if (article != null)
        {
            try
            {
                Process.Start(new ProcessStartInfo(article.NewsUrl) { UseShellExecute = true });
                ServiceContainer.Database.MarkNewsAsRead(article.Serial);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open URL: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    #endregion

    #region Scraping Controls

    private void BtnStartStop_Click(object? sender, EventArgs e)
    {
        if (_backgroundScraper == null) return;

        if (!ServiceContainer.Database.IsConnected)
        {
            MessageBox.Show("Please configure database connection first.", "Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_backgroundScraper.IsRunning)
        {
            _backgroundScraper.Stop();
        }
        else
        {
            _backgroundScraper.Start();
        }
    }

    private async void BtnScrapeNow_Click(object? sender, EventArgs e)
    {
        if (_backgroundScraper == null) return;

        if (!ServiceContainer.Database.IsConnected)
        {
            MessageBox.Show("Please configure database connection first.", "Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Select a site to scrape
        if (listBoxSites.SelectedItem is not SiteInfo selectedSite)
        {
            MessageBox.Show("Please select a site to scrape.", "No Site Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        LogDebug($">> Starting scrape for: {selectedSite.SiteName}", "INFO");
        LogDebug($"   URL: {selectedSite.SiteLink}", "INFO");
        LogDebug($" Article Selector: {selectedSite.ArticleLinkSelector}", "INFO");

        btnScrapeNow.Enabled = false;
        statusProgress.Visible = true;
        statusProgress.Value = 0;

        try
        {
            // Run scraper for selected site only
            await _backgroundScraper.RunSiteOnceAsync(selectedSite);
            LogDebug($"[OK] Scrape completed for: {selectedSite.SiteName}", "SUCCESS");
        }
        catch (Exception ex)
        {
            LogDebug($"[FAIL] Scrape failed: {ex.Message}", "ERROR");
        }
        finally
        {
            btnScrapeNow.Enabled = true;
            statusProgress.Visible = false;
        }
    }

    private void BtnSettings_Click(object? sender, EventArgs e)
    {
        using var form = new SettingsForm();
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            _backgroundScraper?.UpdateInterval();

            // Reload if database connection changed
            if (ServiceContainer.Database.IsConnected)
            {
                LoadSites();
                LoadArticles();
                UpdateArticleCount();
                UpdateStatus("Ready");
                lblStatus.ForeColor = SystemColors.ControlText;
            }
        }
    }

    #endregion

    #region Background Scraper Events

    private void BackgroundScraper_ArticleScraped(object? sender, NewsInfo article)
    {
        AddArticleToTop(article);
    }

    private void BackgroundScraper_StatusChanged(object? sender, string status)
    {
        UpdateStatus(status);
    }

    private void BackgroundScraper_ProgressChanged(object? sender, (int current, int total) progress)
    {
        if (InvokeRequired)
        {
            Invoke(() => BackgroundScraper_ProgressChanged(sender, progress));
            return;
        }

        statusProgress.Visible = true;
        statusProgress.Maximum = progress.total;
        statusProgress.Value = progress.current;

        if (progress.current >= progress.total)
        {
            statusProgress.Visible = false;
        }
    }

    private void BackgroundScraper_RunningStateChanged(object? sender, bool isRunning)
    {
        if (InvokeRequired)
        {
            Invoke(() => BackgroundScraper_RunningStateChanged(sender, isRunning));
            return;
        }

        btnStartStop.Text = isRunning ? "Stop" : "Start";
        btnStartStop.ForeColor = isRunning ? Color.Red : Color.Green;
    }

    #endregion

    #region Debug Log

    private void LogDebug(string message, string level = "INFO")
    {
        if (InvokeRequired)
        {
            Invoke(() => LogDebug(message, level));
            return;
        }

        var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
        var logLine = $"[{timestamp}] [{level}] {message}{Environment.NewLine}";

        txtDebugLog.AppendText(logLine);

        // Also log errors and warnings to the error log
        if (level == "ERROR" || level == "WARN")
        {
            txtErrorLog.AppendText(logLine);

            // Limit error log lines
            var errorLines = txtErrorLog.Lines;
            if (errorLines.Length > MaxLogLines)
            {
                var newErrorLines = errorLines.Skip(errorLines.Length - MaxLogLines).ToArray();
                txtErrorLog.Lines = newErrorLines;
            }

            txtErrorLog.SelectionStart = txtErrorLog.Text.Length;
            txtErrorLog.ScrollToCaret();
        }

        // Limit main debug log lines
        var lines = txtDebugLog.Lines;
        if (lines.Length > MaxLogLines)
        {
            var newLines = lines.Skip(lines.Length - MaxLogLines).ToArray();
            txtDebugLog.Lines = newLines;
        }

        txtDebugLog.SelectionStart = txtDebugLog.Text.Length;
        txtDebugLog.ScrollToCaret();
    }

    private void BtnClearLog_Click(object? sender, EventArgs e)
    {
        txtDebugLog.Clear();
        LogDebug("Log cleared", "INFO");
    }

    private void BtnClearErrorLog_Click(object? sender, EventArgs e)
    {
        txtErrorLog.Clear();
    }

    #endregion

    #region Helpers

    private void UpdateStatus(string message)
    {
        if (InvokeRequired)
        {
            Invoke(() => UpdateStatus(message));
            return;
        }

        statusLabel.Text = message;
        lblStatus.Text = message;

        // Determine log level based on message content
        string level = "INFO";
        if (message.Contains("Error", StringComparison.OrdinalIgnoreCase) || 
            message.Contains("Failed", StringComparison.OrdinalIgnoreCase))
        {
            level = "ERROR";
        }
        else if (message.Contains("Warning", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("No new", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("Skipped", StringComparison.OrdinalIgnoreCase))
        {
            level = "WARN";
        }
        else if (message.Contains("New:", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("Saved", StringComparison.OrdinalIgnoreCase))
        {
            level = "SUCCESS";
        }

        LogDebug(message, level);
    }

    #endregion
}
