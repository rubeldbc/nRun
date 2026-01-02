using System.ComponentModel;
using System.Diagnostics;
using nRun.Models;
using nRun.Services;

namespace nRun.UI.Forms;

public partial class MainForm : Form
{
    private BackgroundScraperService? _backgroundScraper;
    private List<NewsInfo> _articles = new();
    private const int MaxLogLines = 5000;

    // TikTok related fields
    private TikTokDataCollectionService? _tikTokCollector;
    private List<TkProfile> _tikTokProfiles = new();
    private List<TkSchedule> _tikTokSchedules = new();
    private List<TkData> _tikTokData = new();
    private int _nextScheduleId = 1;

    private int MaxDisplayedArticles => ServiceContainer.Settings.LoadSettings().MaxDisplayedArticles;

    public MainForm()
    {
        InitializeComponent();

        // Exit early in design mode
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;

        _backgroundScraper = new BackgroundScraperService();
        _tikTokCollector = new TikTokDataCollectionService();
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

        // Memurai buttons
        btnMemuraiSync.Click += BtnMemuraiSync_Click;
        btnMemuraiView.Click += BtnMemuraiView_Click;

        // Background scraper events
        if (_backgroundScraper != null)
        {
            _backgroundScraper.ArticleScraped += BackgroundScraper_ArticleScraped;
            _backgroundScraper.StatusChanged += BackgroundScraper_StatusChanged;
            _backgroundScraper.ProgressChanged += BackgroundScraper_ProgressChanged;
            _backgroundScraper.RunningStateChanged += BackgroundScraper_RunningStateChanged;
        }

        // Memurai service events
        ServiceContainer.Memurai.StatusChanged += Memurai_StatusChanged;
        ServiceContainer.Memurai.RunningStateChanged += Memurai_RunningStateChanged;

        // TikTok events
        btnTkAddId.Click += BtnTkAddId_Click;
        btnTkDeleteId.Click += BtnTkDeleteId_Click;
        btnTkRefreshId.Click += BtnTkRefreshId_Click;
        btnTkAddSchedule.Click += BtnTkAddSchedule_Click;
        btnTkEditSchedule.Click += BtnTkEditSchedule_Click;
        btnTkDeleteSchedule.Click += BtnTkDeleteSchedule_Click;
        btnTkSaveSettings.Click += BtnTkSaveSettings_Click;
        btnTkStartStop.Click += BtnTkStartStop_Click;
        numTkFrequency.ValueChanged += NumTkFrequency_ValueChanged;

        // Filter events
        btnApplyFilter.Click += BtnApplyFilter_Click;
        btnClearFilter.Click += BtnClearFilter_Click;

        // Schedule checkbox handling
        olvTiktokSchedule.SubItemChecking += OlvTiktokSchedule_SubItemChecking;

        // TikTok data list formatting for change columns
        olvTiktokData.FormatCell += OlvTiktokData_FormatCell;

        // Schedule countdown timer
        timerScheduleCountdown.Tick += TimerScheduleCountdown_Tick;

        // TikTok collector events
        if (_tikTokCollector != null)
        {
            _tikTokCollector.StatusChanged += TikTokCollector_StatusChanged;
            _tikTokCollector.ProgressChanged += TikTokCollector_ProgressChanged;
            _tikTokCollector.RunningStateChanged += TikTokCollector_RunningStateChanged;
            _tikTokCollector.DataCollected += TikTokCollector_DataCollected;
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

        LogDebug($"MaxDisplayedArticles setting: {MaxDisplayedArticles}", "INFO");
        LoadArticles();
        LogDebug($"Loaded {_articles.Count} articles", "INFO");
        UpdateArticleCount();

        // Check Memurai connection status
        CheckMemuraiConnection();

        // Auto-start if configured and connected
        var settings = ServiceContainer.Settings.LoadSettings();
        if (settings.AutoStartScraping && ServiceContainer.Database.IsConnected)
        {
            LogDebug("Auto-start enabled, starting background scraper", "INFO");
            _backgroundScraper?.Start();
        }

        // Load TikTok data and settings
        LoadTikTokSettings();
        LoadTikTokProfiles();
        InitializeFilterControls();
        ClearTikTokDataList(); // Clear on app launch
    }

    private void LoadTikTokSettings()
    {
        var settings = ServiceContainer.Settings.LoadSettings();

        // Load delay setting
        numTkFrequency.Value = Math.Clamp(settings.TikTokDelaySeconds, 1, 3600);

        // Load schedules
        _tikTokSchedules.Clear();
        foreach (var schedSetting in settings.TikTokSchedules)
        {
            var schedule = new TkSchedule
            {
                Id = _nextScheduleId++,
                Timing = new TimeSpan(schedSetting.Hour, schedSetting.Minute, 0),
                IsActive = schedSetting.IsEnabled
            };
            _tikTokSchedules.Add(schedule);
        }
        RefreshScheduleList();
    }

    private void SaveTikTokSettings()
    {
        var settings = ServiceContainer.Settings.LoadSettings();

        // Save delay setting
        settings.TikTokDelaySeconds = (int)numTkFrequency.Value;

        // Save schedules
        settings.TikTokSchedules.Clear();
        foreach (var schedule in _tikTokSchedules)
        {
            settings.TikTokSchedules.Add(new TikTokScheduleSettings
            {
                Hour = schedule.Timing.Hours,
                Minute = schedule.Timing.Minutes,
                IsEnabled = schedule.IsActive
            });
        }

        ServiceContainer.Settings.SaveSettings(settings);
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
            // Unsubscribe from background scraper events
            if (_backgroundScraper != null)
            {
                _backgroundScraper.ArticleScraped -= BackgroundScraper_ArticleScraped;
                _backgroundScraper.StatusChanged -= BackgroundScraper_StatusChanged;
                _backgroundScraper.ProgressChanged -= BackgroundScraper_ProgressChanged;
                _backgroundScraper.RunningStateChanged -= BackgroundScraper_RunningStateChanged;
            }

            // Unsubscribe from TikTok collector events
            if (_tikTokCollector != null)
            {
                _tikTokCollector.StatusChanged -= TikTokCollector_StatusChanged;
                _tikTokCollector.ProgressChanged -= TikTokCollector_ProgressChanged;
                _tikTokCollector.RunningStateChanged -= TikTokCollector_RunningStateChanged;
                _tikTokCollector.DataCollected -= TikTokCollector_DataCollected;
            }

            // Unsubscribe from Memurai service events
            ServiceContainer.Memurai.StatusChanged -= Memurai_StatusChanged;
            ServiceContainer.Memurai.RunningStateChanged -= Memurai_RunningStateChanged;

            // Stop and dispose timer
            timerScheduleCountdown.Stop();
            timerScheduleCountdown.Tick -= TimerScheduleCountdown_Tick;

            // Unsubscribe from list events
            olvTiktokData.FormatCell -= OlvTiktokData_FormatCell;
            olvTiktokSchedule.SubItemChecking -= OlvTiktokSchedule_SubItemChecking;

            // Stop and dispose background scraper
            _backgroundScraper?.Stop();
            _backgroundScraper?.Dispose();
            _backgroundScraper = null;

            // Stop and dispose TikTok collector
            _tikTokCollector?.Stop();
            _tikTokCollector?.Dispose();
            _tikTokCollector = null;

            // Stop and dispose Memurai service
            ServiceContainer.Memurai.Stop();
            ServiceContainer.Memurai.Dispose();

            // Clear collections to release memory
            _articles.Clear();
            _tikTokProfiles.Clear();
            _tikTokSchedules.Clear();
            _tikTokData.Clear();

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
        UpdateArticleCount();
        LogDebug($"Refreshed. {listBoxSites.Items.Count} sites loaded from database.", "INFO");
        UpdateStatus($"Synced with database. {listBoxSites.Items.Count} sites loaded.");
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
        var limit = MaxDisplayedArticles;
        LogDebug($"Loading articles with limit: {limit}", "INFO");
        _articles = ServiceContainer.Database.GetRecentNews(limit);
        olvArticles.SetObjects(_articles);
        UpdateMemuraiButtonState();
    }

    private void UpdateArticleCount()
    {
        var count = ServiceContainer.Database.GetNewsCount();
        statusArticleCount.Text = $"{count} articles";
    }

    private void UpdateMemuraiButtonState()
    {
        bool hasArticles = _articles.Count > 0;

        // Stop Memurai sync if all articles are removed
        if (!hasArticles && ServiceContainer.Memurai.IsRunning)
        {
            ServiceContainer.Memurai.Stop();
        }
    }

    private async void CheckMemuraiConnection()
    {
        var isConnected = await ServiceContainer.Memurai.TestConnectionAsync();

        if (isConnected)
        {
            LogDebug("Memurai server is reachable", "SUCCESS");
            btnMemuraiSync.ToolTipText = "Start/Stop syncing news to Memurai server";
        }
        else
        {
            LogDebug("Memurai server not reachable - check settings", "WARN");
            btnMemuraiSync.ToolTipText = "Memurai server not connected - check settings";
        }
    }

    private void AddArticleToTop(NewsInfo article)
    {
        if (InvokeRequired)
        {
            Invoke(() => AddArticleToTop(article));
            return;
        }

        _articles.Insert(0, article);

        // Keep only MaxDisplayedArticles items - remove oldest from bottom
        while (_articles.Count > MaxDisplayedArticles)
        {
            _articles.RemoveAt(_articles.Count - 1);
        }

        olvArticles.SetObjects(_articles);
        olvArticles.EnsureModelVisible(article);
        UpdateArticleCount();
        UpdateMemuraiButtonState();

        // Update Memurai service with current articles if running
        if (ServiceContainer.Memurai.IsRunning && ServiceContainer.Memurai is MemuraiService ms)
        {
            ms.UpdateArticles(_articles);
        }
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
            UpdateMemuraiButtonState();

            // Update Memurai service if running
            if (ServiceContainer.Memurai.IsRunning && ServiceContainer.Memurai is MemuraiService ms)
            {
                ms.UpdateArticles(_articles);
            }
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

    #region Memurai Sync

    private async void BtnMemuraiSync_Click(object? sender, EventArgs e)
    {
        // Check database connection first
        if (!ServiceContainer.Database.IsConnected)
        {
            LogDebug("Database not connected - cannot start Memurai sync", "WARN");
            return;
        }

        var memurai = ServiceContainer.Memurai;

        if (memurai.IsRunning)
        {
            memurai.Stop();
        }
        else
        {
            // Test connection first
            btnMemuraiSync.Enabled = false;
            var isConnected = await memurai.TestConnectionAsync();

            if (!isConnected)
            {
                MessageBox.Show(
                    "Cannot connect to Memurai server.\n\nPlease check your Memurai settings and ensure the server is running.",
                    "Connection Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                btnMemuraiSync.Enabled = true;
                return;
            }

            // Update the Memurai service with current articles before starting
            if (memurai is MemuraiService ms)
            {
                ms.UpdateArticles(_articles);
            }
            memurai.Start();

            // Do an immediate sync
            await memurai.SyncNowAsync(_articles);
            btnMemuraiSync.Enabled = true;
        }
    }

    private async void BtnMemuraiView_Click(object? sender, EventArgs e)
    {
        btnMemuraiView.Enabled = false;
        btnMemuraiView.Text = "Loading...";

        try
        {
            var data = await ServiceContainer.Memurai.GetStoredDataAsync();

            using var form = new Form
            {
                Text = "Memurai Data Viewer",
                Size = new Size(800, 600),
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = true,
                ShowInTaskbar = false
            };

            var txtData = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                Font = new Font("Consolas", 10F),
                ReadOnly = true,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.LightGreen,
                Text = data ?? "No data available",
                WordWrap = false
            };

            var btnClose = new Button
            {
                Text = "Close",
                Dock = DockStyle.Bottom,
                Height = 35
            };
            btnClose.Click += (s, args) => form.Close();

            var btnRefresh = new Button
            {
                Text = "Refresh",
                Dock = DockStyle.Bottom,
                Height = 35
            };
            btnRefresh.Click += async (s, args) =>
            {
                btnRefresh.Enabled = false;
                btnRefresh.Text = "Refreshing...";
                var newData = await ServiceContainer.Memurai.GetStoredDataAsync();
                txtData.Text = newData ?? "No data available";
                btnRefresh.Text = "Refresh";
                btnRefresh.Enabled = true;
            };

            form.Controls.Add(txtData);
            form.Controls.Add(btnRefresh);
            form.Controls.Add(btnClose);

            form.ShowDialog(this);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error viewing Memurai data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnMemuraiView.Text = "View Memurai";
            btnMemuraiView.Enabled = true;
        }
    }

    private void Memurai_StatusChanged(object? sender, string status)
    {
        if (InvokeRequired)
        {
            Invoke(() => Memurai_StatusChanged(sender, status));
            return;
        }

        LogDebug($"[Memurai] {status}", status.Contains("error", StringComparison.OrdinalIgnoreCase) ? "ERROR" : "INFO");
    }

    private void Memurai_RunningStateChanged(object? sender, bool isRunning)
    {
        if (InvokeRequired)
        {
            Invoke(() => Memurai_RunningStateChanged(sender, isRunning));
            return;
        }

        btnMemuraiSync.Text = isRunning ? "Stop Memurai" : "Memurai Sync";
        btnMemuraiSync.ForeColor = isRunning ? Color.Red : Color.Gray;
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

    #region TikTok Management

    private void LoadTikTokProfiles()
    {
        if (!ServiceContainer.Database.IsConnected) return;

        _tikTokProfiles = ServiceContainer.Database.GetAllTkProfiles();
        olvTiktokID.SetObjects(_tikTokProfiles);
        UpdateTkInfo();
    }

    private void LoadTikTokData(string? username = null, DateTime? fromDate = null, DateTime? toDate = null)
    {
        if (!ServiceContainer.Database.IsConnected) return;

        _tikTokData = ServiceContainer.Database.GetFilteredTkData(username, fromDate, toDate, 500);
        olvTiktokData.SetObjects(_tikTokData);
        UpdateTkInfo();
    }

    private void OlvTiktokData_FormatCell(object? sender, BrightIdeasSoftware.FormatCellEventArgs e)
    {
        if (e.Model is not TkData data) return;

        // Color the change columns based on positive/negative values
        if (e.Column == olvColDataFollowersChange)
        {
            FormatChangeCell(e, data.FollowersChange);
        }
        else if (e.Column == olvColDataHeartsChange)
        {
            FormatChangeCell(e, data.HeartsChange);
        }
        else if (e.Column == olvColDataVideosChange)
        {
            FormatChangeCell(e, data.VideosChange);
        }
    }

    private static void FormatChangeCell(BrightIdeasSoftware.FormatCellEventArgs e, long change)
    {
        if (change > 0)
        {
            e.SubItem.ForeColor = Color.LimeGreen;
        }
        else if (change < 0)
        {
            e.SubItem.ForeColor = Color.OrangeRed;
        }
        else
        {
            e.SubItem.ForeColor = Color.Gray;
        }
    }

    private void ClearTikTokDataList()
    {
        _tikTokData.Clear();
        olvTiktokData.SetObjects(_tikTokData);
        UpdateTkInfo();
    }

    private void InitializeFilterControls()
    {
        // Populate username dropdown
        cboFilterUsername.Items.Clear();
        cboFilterUsername.Items.Add("(All Users)");
        foreach (var profile in _tikTokProfiles)
        {
            cboFilterUsername.Items.Add(profile.Username);
        }
        cboFilterUsername.SelectedIndex = 0;

        // Set date/time defaults (last 30 days, from midnight to now)
        dtpFilterTo.Value = DateTime.Now;
        dtpFilterFrom.Value = DateTime.Today.AddDays(-30);
    }

    private void BtnApplyFilter_Click(object? sender, EventArgs e)
    {
        string? username = null;
        if (cboFilterUsername.SelectedIndex > 0)
        {
            username = cboFilterUsername.SelectedItem?.ToString();
        }

        // Use the exact datetime values from the pickers
        var fromDate = dtpFilterFrom.Value;
        var toDate = dtpFilterTo.Value;

        LoadTikTokData(username, fromDate, toDate);
        UpdateTkStatus($"Filter applied. Found {_tikTokData.Count} records.");
    }

    private void BtnClearFilter_Click(object? sender, EventArgs e)
    {
        cboFilterUsername.SelectedIndex = 0;
        dtpFilterTo.Value = DateTime.Now;
        dtpFilterFrom.Value = DateTime.Today.AddDays(-30);
        ClearTikTokDataList();
        UpdateTkStatus("Filter cleared.");
    }

    private void RefreshScheduleList()
    {
        // Update serial numbers
        for (int i = 0; i < _tikTokSchedules.Count; i++)
        {
            _tikTokSchedules[i].SerialNumber = i + 1;
        }
        olvTiktokSchedule.SetObjects(_tikTokSchedules);

        // Update collector
        _tikTokCollector?.UpdateSchedules(_tikTokSchedules);
    }

    private void OlvTiktokSchedule_SubItemChecking(object? sender, BrightIdeasSoftware.SubItemCheckingEventArgs e)
    {
        if (e.RowObject is TkSchedule schedule)
        {
            schedule.IsActive = e.NewValue == CheckState.Checked;
            SaveTikTokSettings();
            _tikTokCollector?.UpdateSchedules(_tikTokSchedules);
        }
    }

    private void BtnTkSaveSettings_Click(object? sender, EventArgs e)
    {
        SaveTikTokSettings();
        UpdateTkStatus("Settings saved successfully.");
    }

    private void BtnTkAddId_Click(object? sender, EventArgs e)
    {
        if (!ServiceContainer.Database.IsConnected)
        {
            MessageBox.Show("Please configure database connection first.", "Not Connected",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var form = new TikTokIdManagerForm();
        if (form.ShowDialog(this) == DialogResult.OK && form.ResultProfile != null)
        {
            LoadTikTokProfiles();
            UpdateTkStatus($"Added profile: @{form.ResultProfile.Username}");
        }
    }

    private void BtnTkDeleteId_Click(object? sender, EventArgs e)
    {
        if (olvTiktokID.SelectedObject is not TkProfile profile) return;

        var result = MessageBox.Show(
            $"Delete @{profile.Username} and all associated data?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result == DialogResult.Yes)
        {
            ServiceContainer.Database.DeleteTkProfile(profile.UserId);
            LoadTikTokProfiles();
            LoadTikTokData();
            UpdateTkStatus($"Deleted profile: @{profile.Username}");
        }
    }

    private void BtnTkRefreshId_Click(object? sender, EventArgs e)
    {
        LoadTikTokProfiles();
        InitializeFilterControls();
        UpdateTkStatus($"Refreshed. {_tikTokProfiles.Count} profiles loaded.");
    }

    private void BtnTkAddSchedule_Click(object? sender, EventArgs e)
    {
        using var form = new TikTokScheduleForm();
        if (form.ShowDialog(this) == DialogResult.OK && form.ResultSchedule != null)
        {
            form.ResultSchedule.Id = _nextScheduleId++;
            _tikTokSchedules.Add(form.ResultSchedule);
            RefreshScheduleList();
            SaveTikTokSettings();
            UpdateTkStatus($"Added schedule: {form.ResultSchedule.TimingDisplay}");
        }
    }

    private void BtnTkEditSchedule_Click(object? sender, EventArgs e)
    {
        if (olvTiktokSchedule.SelectedObject is not TkSchedule schedule) return;

        using var form = new TikTokScheduleForm(schedule);
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            RefreshScheduleList();
            SaveTikTokSettings();
            UpdateTkStatus($"Updated schedule: {schedule.TimingDisplay}");
        }
    }

    private void BtnTkDeleteSchedule_Click(object? sender, EventArgs e)
    {
        if (olvTiktokSchedule.SelectedObject is not TkSchedule schedule) return;

        _tikTokSchedules.Remove(schedule);
        RefreshScheduleList();
        SaveTikTokSettings();
        UpdateTkStatus("Schedule deleted");
    }

    private void NumTkFrequency_ValueChanged(object? sender, EventArgs e)
    {
        SaveTikTokSettings();
    }

    private void BtnTkStartStop_Click(object? sender, EventArgs e)
    {
        if (_tikTokCollector == null) return;

        if (!ServiceContainer.Database.IsConnected)
        {
            MessageBox.Show("Please configure database connection first.", "Not Connected",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_tikTokProfiles.Count == 0)
        {
            MessageBox.Show("Please add at least one TikTok ID first.", "No IDs",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_tikTokCollector.IsRunning)
        {
            _tikTokCollector.Stop();
        }
        else
        {
            // Clear data list when starting new fetch
            ClearTikTokDataList();

            // Update settings before starting
            _tikTokCollector.UpdateDelaySeconds((int)numTkFrequency.Value);
            _tikTokCollector.UpdateSchedules(_tikTokSchedules);
            _tikTokCollector.Start();
        }
    }

    private void TikTokCollector_StatusChanged(object? sender, string status)
    {
        UpdateTkStatus(status);
    }

    private void TikTokCollector_ProgressChanged(object? sender, (int current, int total) progress)
    {
        if (InvokeRequired)
        {
            Invoke(() => TikTokCollector_ProgressChanged(sender, progress));
            return;
        }

        progressBarTk.Visible = progress.current < progress.total;
        progressBarTk.Maximum = progress.total;
        progressBarTk.Value = progress.current;
    }

    private void TikTokCollector_RunningStateChanged(object? sender, bool isRunning)
    {
        if (InvokeRequired)
        {
            Invoke(() => TikTokCollector_RunningStateChanged(sender, isRunning));
            return;
        }

        btnTkStartStop.Text = isRunning ? "Stop" : "Start";
        btnTkStartStop.BackColor = isRunning ? Color.FromArgb(180, 0, 0) : Color.FromArgb(0, 120, 0);

        // Disable/enable filter controls during fetching
        SetTikTokFilterEnabled(!isRunning);

        // Start/stop countdown timer
        if (isRunning)
        {
            UpdateNextScheduleLabel();
            timerScheduleCountdown.Start();
        }
        else
        {
            timerScheduleCountdown.Stop();
            lblTkNextSchedule.Text = "Next: --:-- (stopped)";
        }
    }

    private void SetTikTokFilterEnabled(bool enabled)
    {
        cboFilterUsername.Enabled = enabled;
        dtpFilterFrom.Enabled = enabled;
        dtpFilterTo.Enabled = enabled;
        btnApplyFilter.Enabled = enabled;
        btnClearFilter.Enabled = enabled;
    }

    private void TikTokCollector_DataCollected(object? sender, TkData data)
    {
        if (InvokeRequired)
        {
            Invoke(() => TikTokCollector_DataCollected(sender, data));
            return;
        }

        // Add to the top of the list
        _tikTokData.Insert(0, data);

        // Limit rows to double the number of TikTok profiles
        int maxRows = Math.Max(10, _tikTokProfiles.Count * 2); // minimum 10 rows
        while (_tikTokData.Count > maxRows)
        {
            _tikTokData.RemoveAt(_tikTokData.Count - 1);
        }

        olvTiktokData.SetObjects(_tikTokData);
        UpdateTkInfo();
    }

    private void UpdateTkStatus(string message)
    {
        if (InvokeRequired)
        {
            Invoke(() => UpdateTkStatus(message));
            return;
        }

        lblTkStatus.Text = message;
    }

    private void UpdateTkInfo()
    {
        if (InvokeRequired)
        {
            Invoke(UpdateTkInfo);
            return;
        }

        lblTkInfo.Text = $"Id:{_tikTokProfiles.Count}, Sch:{_tikTokSchedules.Count}, Data:{_tikTokData.Count}";
    }

    private void TimerScheduleCountdown_Tick(object? sender, EventArgs e)
    {
        UpdateNextScheduleLabel();
    }

    private void UpdateNextScheduleLabel()
    {
        var now = DateTime.Now;
        var currentTime = now.TimeOfDay;

        // Get active schedules
        var activeSchedules = _tikTokSchedules.Where(s => s.IsActive).ToList();
        if (activeSchedules.Count == 0)
        {
            lblTkNextSchedule.Text = "Next: No active schedules";
            return;
        }

        // Find the next schedule time
        TimeSpan? nextScheduleTime = null;
        TimeSpan minDiff = TimeSpan.MaxValue;

        foreach (var schedule in activeSchedules)
        {
            var scheduleTime = schedule.Timing;
            TimeSpan diff;

            if (scheduleTime > currentTime)
            {
                // Schedule is later today
                diff = scheduleTime - currentTime;
            }
            else
            {
                // Schedule is tomorrow (already passed today)
                diff = TimeSpan.FromDays(1) - currentTime + scheduleTime;
            }

            if (diff < minDiff)
            {
                minDiff = diff;
                nextScheduleTime = scheduleTime;
            }
        }

        if (nextScheduleTime.HasValue)
        {
            var scheduleTimeStr = DateTime.Today.Add(nextScheduleTime.Value).ToString("HH:mm");
            var countdownStr = FormatCountdown(minDiff);
            lblTkNextSchedule.Text = $"Next: {scheduleTimeStr} ({countdownStr})";
        }
    }

    private static string FormatCountdown(TimeSpan diff)
    {
        if (diff.TotalHours >= 1)
        {
            return $"{(int)diff.TotalHours}h {diff.Minutes:D2}m";
        }
        else if (diff.TotalMinutes >= 1)
        {
            return $"{diff.Minutes}m {diff.Seconds:D2}s";
        }
        else
        {
            return $"{diff.Seconds}s";
        }
    }

    #endregion
}
