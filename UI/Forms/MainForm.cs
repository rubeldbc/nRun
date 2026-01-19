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
    private bool _noScrapAutoStopped = false; // Track if scraping was auto-stopped due to no-scrap window
    private bool _userWantsScraping = false; // Track if user wants scraping to be active
    private bool _waitingForNoScrapEnd = false; // Track if waiting for no-scrap window to end

    // TikTok related fields
    private TikTokDataCollectionService? _tikTokCollector;
    private List<TkProfile> _tikTokProfiles = new();
    private List<TkSchedule> _tikTokSchedules = new();
    private List<TkData> _tikTokData = new();
    private int _nextScheduleId = 1;

    // Facebook related fields
    private FacebookDataCollectionService? _facebookCollector;
    private List<FbProfile> _facebookProfiles = new();
    private List<FbSchedule> _facebookSchedules = new();
    private List<FbData> _facebookData = new();
    private int _nextFbScheduleId = 1;

    private int MaxDisplayedArticles => ServiceContainer.Settings.LoadSettings().MaxDisplayedArticles;

    public MainForm()
    {
        InitializeComponent();

        // Exit early in design mode
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;

        _backgroundScraper = new BackgroundScraperService();
        _tikTokCollector = new TikTokDataCollectionService();
        _facebookCollector = new FacebookDataCollectionService();
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
        btnAbout.Click += BtnAbout_Click;

        // Site list events
        btnAddSite.Click += BtnAddSite_Click;
        btnEditSite.Click += BtnEditSite_Click;
        btnDeleteSite.Click += BtnDeleteSite_Click;
        btnRefreshSites.Click += BtnRefreshSites_Click;
        btnManageSites.Click += BtnManageSites_Click;
        listBoxSites.DoubleClick += ListBoxSites_DoubleClick;
        listBoxSites.DrawItem += ListBoxSites_DrawItem;
        listBoxSites.MouseDown += ListBoxSites_MouseDown;
        contextMenuSites.Opening += ContextMenuSites_Opening;
        menuItemAllowCloudflareBypass.Click += MenuItemAllowCloudflareBypass_Click;

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

        // No-Scrape time window events
        chkNoScrapEnabled.CheckedChanged += NoScrapSettings_Changed;
        dtpNoScrapStart.ValueChanged += NoScrapSettings_Changed;
        dtpNoScrapEnd.ValueChanged += NoScrapSettings_Changed;

        // Schedule checkbox handling
        olvTiktokSchedule.SubItemChecking += OlvTiktokSchedule_SubItemChecking;

        // TikTok ID status checkbox and context menu
        olvTiktokID.SubItemChecking += OlvTiktokID_SubItemChecking;
        menuItemTkAddId.Click += MenuItemTkAddId_Click;
        menuItemTkEditId.Click += MenuItemTkEditId_Click;
        menuItemTkDeleteId.Click += MenuItemTkDeleteId_Click;
        menuItemTkRefresh.Click += MenuItemTkRefresh_Click;
        menuItemTkFetch.Click += MenuItemTkFetch_Click;
        menuItemTkExportLogos.Click += MenuItemTkExportLogos_Click;

        // Facebook ID status checkbox and context menu
        olvFacebookID.SubItemChecking += OlvFacebookID_SubItemChecking;
        menuItemFbAddId.Click += MenuItemFbAddId_Click;
        menuItemFbEditId.Click += MenuItemFbEditId_Click;
        menuItemFbDeleteId.Click += MenuItemFbDeleteId_Click;
        menuItemFbRefresh.Click += MenuItemFbRefresh_Click;
        menuItemFbFetch.Click += MenuItemFbFetch_Click;
        menuItemFbExportLogos.Click += MenuItemFbExportLogos_Click;

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

        // Facebook events
        btnFbAddId.Click += BtnFbAddId_Click;
        btnFbDeleteId.Click += BtnFbDeleteId_Click;
        btnFbRefreshId.Click += BtnFbRefreshId_Click;
        btnFbAddSchedule.Click += BtnFbAddSchedule_Click;
        btnFbEditSchedule.Click += BtnFbEditSchedule_Click;
        btnFbDeleteSchedule.Click += BtnFbDeleteSchedule_Click;
        btnFbSaveSettings.Click += BtnFbSaveSettings_Click;
        btnFbStartStop.Click += BtnFbStartStop_Click;
        numFbFrequency.ValueChanged += NumFbFrequency_ValueChanged;
        btnFbApplyFilter.Click += BtnFbApplyFilter_Click;
        btnFbClearFilter.Click += BtnFbClearFilter_Click;
        olvFacebookData.FormatCell += OlvFacebookData_FormatCell;

        // Facebook collector events
        if (_facebookCollector != null)
        {
            _facebookCollector.StatusChanged += FacebookCollector_StatusChanged;
            _facebookCollector.ProgressChanged += FacebookCollector_ProgressChanged;
            _facebookCollector.RunningStateChanged += FacebookCollector_RunningStateChanged;
            _facebookCollector.DataCollected += FacebookCollector_DataCollected;
        }

        // Database connection change event
        DatabaseConnectionForm.DatabaseChanged += OnDatabaseChanged;
    }

    private void OnDatabaseChanged()
    {
        if (InvokeRequired)
        {
            Invoke(OnDatabaseChanged);
            return;
        }

        // Update form title with new host/database names
        UpdateFormTitle();

        // Reload all data from the new database
        if (ServiceContainer.Database.IsConnected)
        {
            // Reload news scraper data
            LoadSites();
            LoadArticles();
            UpdateArticleCount();

            // Reload TikTok data
            LoadTikTokProfiles();
            InitializeFilterControls();
            ClearTikTokDataList();

            // Reload Facebook data
            LoadFacebookProfiles();
            InitializeFbFilterControls();
            ClearFacebookDataList();

            LogDebug("Database changed - all lists refreshed", "INFO");
            UpdateStatus("Database connected - lists refreshed");
            lblStatus.ForeColor = SystemColors.ControlText;
        }
    }

    private void MainForm_Load(object? sender, EventArgs e)
    {
        LogDebug("Application started", "INFO");

        // Update form title with host names
        UpdateFormTitle();

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

        // Load Facebook data and settings
        LoadFacebookSettings();
        LoadFacebookProfiles();
        InitializeFbFilterControls();

        // Load No-Scrape settings
        LoadNoScrapSettings();
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

            // Unsubscribe from Facebook collector events
            if (_facebookCollector != null)
            {
                _facebookCollector.StatusChanged -= FacebookCollector_StatusChanged;
                _facebookCollector.ProgressChanged -= FacebookCollector_ProgressChanged;
                _facebookCollector.RunningStateChanged -= FacebookCollector_RunningStateChanged;
                _facebookCollector.DataCollected -= FacebookCollector_DataCollected;
            }

            // Unsubscribe from Memurai service events
            ServiceContainer.Memurai.StatusChanged -= Memurai_StatusChanged;
            ServiceContainer.Memurai.RunningStateChanged -= Memurai_RunningStateChanged;

            // Unsubscribe from database connection change event
            DatabaseConnectionForm.DatabaseChanged -= OnDatabaseChanged;

            // Stop and dispose timer
            timerScheduleCountdown.Stop();
            timerScheduleCountdown.Tick -= TimerScheduleCountdown_Tick;

            // Unsubscribe from list events
            olvTiktokData.FormatCell -= OlvTiktokData_FormatCell;
            olvTiktokSchedule.SubItemChecking -= OlvTiktokSchedule_SubItemChecking;

            // Unsubscribe from no-scrap events
            chkNoScrapEnabled.CheckedChanged -= NoScrapSettings_Changed;
            dtpNoScrapStart.ValueChanged -= NoScrapSettings_Changed;
            dtpNoScrapEnd.ValueChanged -= NoScrapSettings_Changed;

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

    private async Task LoadSitesAsync()
    {
        var sites = await ServiceContainer.Database.GetAllSitesAsync().ConfigureAwait(true);
        listBoxSites.DataSource = null;
        listBoxSites.DataSource = sites;
        listBoxSites.DisplayMember = "SiteName";
    }

    private void LoadSites()
    {
        // Sync version for backwards compatibility - fires and forgets the async version
        _ = LoadSitesAsync();
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

    private void ListBoxSites_DrawItem(object? sender, DrawItemEventArgs e)
    {
        if (e.Index < 0) return;

        // Get the item
        var item = listBoxSites.Items[e.Index];
        if (item is not SiteInfo site) return;

        // Check if cloudflare bypass is enabled for this site
        bool isCloudflareBypassEnabled = ServiceContainer.CloudflareBypass.IsEnabled(site.SiteId);

        // Determine background color
        Color backColor;
        if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
        {
            // Selected item - use system highlight color
            backColor = SystemColors.Highlight;
        }
        else if (isCloudflareBypassEnabled)
        {
            // Cloudflare bypass enabled - use a light blue/cyan tint
            backColor = Color.FromArgb(220, 240, 255);
        }
        else
        {
            // Normal item
            backColor = SystemColors.Window;
        }

        // Draw background
        using (var brush = new SolidBrush(backColor))
        {
            e.Graphics.FillRectangle(brush, e.Bounds);
        }

        // Determine text color
        Color textColor = (e.State & DrawItemState.Selected) == DrawItemState.Selected
            ? SystemColors.HighlightText
            : SystemColors.WindowText;

        // Draw the text
        using (var brush = new SolidBrush(textColor))
        {
            var text = site.SiteName;
            e.Graphics.DrawString(text, e.Font ?? listBoxSites.Font, brush,
                new PointF(e.Bounds.X + 2, e.Bounds.Y + 1));
        }

        // Draw focus rectangle if needed
        e.DrawFocusRectangle();
    }

    private void ListBoxSites_MouseDown(object? sender, MouseEventArgs e)
    {
        // Select item on right-click for context menu
        if (e.Button == MouseButtons.Right)
        {
            int index = listBoxSites.IndexFromPoint(e.Location);
            if (index >= 0)
            {
                listBoxSites.SelectedIndex = index;
            }
        }
    }

    private void ContextMenuSites_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        // Update the check state based on the selected site
        if (listBoxSites.SelectedItem is SiteInfo site)
        {
            menuItemAllowCloudflareBypass.Checked = ServiceContainer.CloudflareBypass.IsEnabled(site.SiteId);
        }
        else
        {
            e.Cancel = true; // Don't show context menu if no item selected
        }
    }

    private void MenuItemAllowCloudflareBypass_Click(object? sender, EventArgs e)
    {
        if (listBoxSites.SelectedItem is SiteInfo site)
        {
            // Toggle the setting
            bool newValue = ServiceContainer.CloudflareBypass.Toggle(site.SiteId);

            // Refresh the list to update the background color
            listBoxSites.Invalidate();

            // Log the change
            LogDebug($"Cloudflare bypass {(newValue ? "enabled" : "disabled")} for: {site.SiteName}", "INFO");
        }
    }

    #endregion

    #region Article Management

    private async Task LoadArticlesAsync()
    {
        var limit = MaxDisplayedArticles;
        LogDebug($"Loading articles with limit: {limit}", "INFO");
        _articles = await ServiceContainer.Database.GetRecentNewsAsync(limit).ConfigureAwait(true);
        olvArticles.SetObjects(_articles);
        UpdateMemuraiButtonState();
    }

    private void LoadArticles()
    {
        // Sync version for backwards compatibility - fires and forgets the async version
        _ = LoadArticlesAsync();
    }

    private async Task UpdateArticleCountAsync()
    {
        var count = await ServiceContainer.Database.GetNewsCountAsync().ConfigureAwait(true);
        statusNewsCount.Text = $"{count} articles";
    }

    private void UpdateArticleCount()
    {
        // Sync version for backwards compatibility - fires and forgets the async version
        _ = UpdateArticleCountAsync();
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

        if (_userWantsScraping)
        {
            // User wants to stop
            _userWantsScraping = false;
            _waitingForNoScrapEnd = false;
            _noScrapAutoStopped = false;

            if (_backgroundScraper.IsRunning)
            {
                _backgroundScraper.Stop();
            }

            LogDebug("Scraping stopped by user", "INFO");
            UpdateNoScrapStatus();
            UpdateScraperButtonState(false);
        }
        else
        {
            // User wants to start
            _userWantsScraping = true;

            if (IsInNoScrapWindow())
            {
                // In no-scrap window - don't start, but set button to Stop state
                _waitingForNoScrapEnd = true;
                var remaining = GetNoScrapRemainingTime();
                var (start, end) = ServiceContainer.NoScrapWindow.GetWindowTimes();
                LogDebug($"═══════════════════════════════════════════════════", "WARNING");
                LogDebug($"NO-SCRAPE WINDOW ACTIVE", "WARNING");
                LogDebug($"Window: {DateTime.Today.Add(start):HH:mm} - {DateTime.Today.Add(end):HH:mm}", "WARNING");
                LogDebug($"Time remaining: {remaining:hh\\:mm\\:ss}", "WARNING");
                LogDebug($"Scraping will automatically start when window ends", "INFO");
                LogDebug($"WebDriver will NOT be initialized during this time", "WARNING");
                LogDebug($"═══════════════════════════════════════════════════", "WARNING");
                UpdateNoScrapStatus();
                UpdateScraperButtonState(true);
            }
            else
            {
                // Not in no-scrap window - start normally
                _waitingForNoScrapEnd = false;
                _backgroundScraper.Start();
                LogDebug("Scraping started", "INFO");
            }
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

        // Block during no-scrap window
        if (IsInNoScrapWindow())
        {
            var remaining = GetNoScrapRemainingTime();
            var (start, end) = ServiceContainer.NoScrapWindow.GetWindowTimes();
            MessageBox.Show(
                $"Scraping is disabled during No-Scrape time window.\n\n" +
                $"Window: {DateTime.Today.Add(start):HH:mm} - {DateTime.Today.Add(end):HH:mm}\n" +
                $"Time remaining: {remaining:hh\\:mm\\:ss}",
                "No-Scrape Active",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
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
        statusNewsProgress.Visible = true;
        statusNewsProgress.Value = 0;

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
            statusNewsProgress.Visible = false;
        }
    }

    private void BtnSettings_Click(object? sender, EventArgs e)
    {
        using var form = new SettingsForm();
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            _backgroundScraper?.UpdateInterval();

            // Always update title to reflect new host/database settings
            UpdateFormTitle();

            // Reload all data if database connection changed
            if (ServiceContainer.Database.IsConnected)
            {
                // Reload news scraper data
                LoadSites();
                LoadArticles();
                UpdateArticleCount();

                // Reload TikTok data
                LoadTikTokProfiles();
                InitializeFilterControls();
                ClearTikTokDataList();

                // Reload Facebook data
                LoadFacebookProfiles();
                InitializeFbFilterControls();
                ClearFacebookDataList();

                UpdateStatus("Ready");
                lblStatus.ForeColor = SystemColors.ControlText;
            }
        }
    }

    private void BtnAbout_Click(object? sender, EventArgs e)
    {
        using var form = new AboutForm();
        form.ShowDialog(this);
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

        statusNewsProgress.Visible = true;
        statusNewsProgress.Maximum = progress.total;
        statusNewsProgress.Value = progress.current;

        if (progress.current >= progress.total)
        {
            statusNewsProgress.Visible = false;
        }
    }

    private void BackgroundScraper_RunningStateChanged(object? sender, bool isRunning)
    {
        if (InvokeRequired)
        {
            Invoke(() => BackgroundScraper_RunningStateChanged(sender, isRunning));
            return;
        }

        // Use _userWantsScraping to determine button state
        // This ensures button shows "Stop" when waiting for no-scrap window to end
        UpdateScraperButtonState(_userWantsScraping);
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

    private void UpdateFormTitle()
    {
        var settings = ServiceContainer.Settings.LoadSettings();
        var dbHost = settings.DbHost;
        var dbName = settings.DbName;
        var memuraiHost = settings.MemuraiHost;
        this.Text = $"nRun - News Scraper | PostgreSQL: {dbHost}/{dbName} | Memurai: {memuraiHost}";
    }

    private void UpdateStatus(string message)
    {
        if (InvokeRequired)
        {
            Invoke(() => UpdateStatus(message));
            return;
        }

        statusNewsLabel.Text = $"News: {message}";
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

    private async Task LoadTikTokProfilesAsync()
    {
        if (!ServiceContainer.Database.IsConnected) return;

        _tikTokProfiles = await ServiceContainer.Database.GetAllTkProfilesAsync().ConfigureAwait(true);
        olvTiktokID.SetObjects(_tikTokProfiles);
        UpdateTkInfo();
    }

    private void LoadTikTokProfiles()
    {
        // Sync version for backwards compatibility - fires and forgets the async version
        _ = LoadTikTokProfilesAsync();
    }

    private async Task LoadTikTokDataAsync(string? username = null, DateTime? fromDate = null, DateTime? toDate = null)
    {
        if (!ServiceContainer.Database.IsConnected) return;

        _tikTokData = await ServiceContainer.Database.GetFilteredTkDataAsync(username, fromDate, toDate, 500).ConfigureAwait(true);
        olvTiktokData.SetObjects(_tikTokData);
        UpdateTkInfo();
    }

    private void LoadTikTokData(string? username = null, DateTime? fromDate = null, DateTime? toDate = null)
    {
        // Sync version for backwards compatibility - fires and forgets the async version
        _ = LoadTikTokDataAsync(username, fromDate, toDate);
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

        // Set date/time defaults (last 2 days)
        dtpFilterTo.Value = DateTime.Now;
        dtpFilterFrom.Value = DateTime.Today.AddDays(-2);
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
        dtpFilterFrom.Value = DateTime.Today.AddDays(-2);
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

    private void OlvTiktokID_SubItemChecking(object? sender, BrightIdeasSoftware.SubItemCheckingEventArgs e)
    {
        if (e.RowObject is TkProfile profile)
        {
            profile.Status = e.NewValue == CheckState.Checked;
            ServiceContainer.Database.UpdateTkProfileStatus(profile.UserId, profile.Status);
            UpdateTkStatus($"@{profile.Username} status: {(profile.Status ? "Active" : "Inactive")}");
        }
    }

    private void MenuItemTkAddId_Click(object? sender, EventArgs e)
    {
        BtnTkAddId_Click(sender, e);
    }

    private void MenuItemTkEditId_Click(object? sender, EventArgs e)
    {
        if (olvTiktokID.SelectedObject is not TkProfile profile)
        {
            MessageBox.Show("Please select a profile to edit.", "No Selection",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var form = new TikTokIdManagerForm(profile);
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            LoadTikTokProfiles();
            UpdateTkStatus($"Updated profile: @{profile.Username}");
        }
    }

    private void MenuItemTkDeleteId_Click(object? sender, EventArgs e)
    {
        BtnTkDeleteId_Click(sender, e);
    }

    private void MenuItemTkRefresh_Click(object? sender, EventArgs e)
    {
        BtnTkRefreshId_Click(sender, e);
    }

    private async void MenuItemTkFetch_Click(object? sender, EventArgs e)
    {
        if (olvTiktokID.SelectedObject is not TkProfile profile)
        {
            MessageBox.Show("Please select a profile to fetch.", "No Selection",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        await FetchTikTokProfileAsync(profile);
    }

    private void MenuItemTkExportLogos_Click(object? sender, EventArgs e)
    {
        var selectedProfiles = olvTiktokID.SelectedObjects.Cast<TkProfile>().ToList();
        if (selectedProfiles.Count == 0)
        {
            MessageBox.Show("Please select one or more profiles to export logos.", "No Selection",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var form = new LogoExportForm(selectedProfiles, Services.LogoExportPlatform.TikTok);
        form.ShowDialog(this);
    }

    private async Task FetchTikTokProfileAsync(TkProfile profile)
    {
        try
        {
            UpdateTkStatus($"Fetching @{profile.Username}...");
            progressBarTk.Visible = true;
            progressBarTk.Style = ProgressBarStyle.Marquee;

            var scraper = new TikTokScraperService();
            var data = await scraper.FetchStatsAsync(profile);

            if (data != null)
            {
                var dataId = ServiceContainer.Database.AddTkData(data);
                data.DataId = dataId;

                // Add to data list display
                _tikTokData.Insert(0, data);
                int maxRows = Math.Max(10, _tikTokProfiles.Count * 2);
                while (_tikTokData.Count > maxRows)
                {
                    _tikTokData.RemoveAt(_tikTokData.Count - 1);
                }
                olvTiktokData.SetObjects(_tikTokData);
                UpdateTkInfo();

                UpdateTkStatus($"Fetched @{profile.Username}: {data.FollowerCount:N0} followers");
            }
            else
            {
                UpdateTkStatus($"Failed to fetch @{profile.Username}");
            }

            scraper.Dispose();
        }
        catch (Exception ex)
        {
            UpdateTkStatus($"Error fetching @{profile.Username}: {ex.Message}");
        }
        finally
        {
            progressBarTk.Visible = false;
            progressBarTk.Style = ProgressBarStyle.Blocks;
        }
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
            // Check if there are any active schedules
            var activeSchedules = _tikTokSchedules.Where(s => s.IsActive).ToList();
            if (activeSchedules.Count == 0)
            {
                MessageBox.Show("Please add and enable at least one schedule first.", "No Active Schedules",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Clear data list when starting new fetch
            ClearTikTokDataList();

            // Update settings before starting
            _tikTokCollector.UpdateDelaySeconds((int)numTkFrequency.Value);
            _tikTokCollector.UpdateSchedules(_tikTokSchedules);

            // Set flag to skip immediate collection - wait for next schedule
            _tikTokCollector.SkipInitialCollection = true;
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
            // Start timer if not already running (Facebook might have it running)
            if (!timerScheduleCountdown.Enabled)
            {
                timerScheduleCountdown.Start();
            }
        }
        else
        {
            lblTkNextSchedule.Text = "Next: --:-- (stopped)";
            statusTikTokSchedule.Text = "(stopped)";
            statusTikTokLabel.Text = "TikTok: Stopped";
            // Only stop timer if Facebook is also not running
            if (_facebookCollector == null || !_facebookCollector.IsRunning)
            {
                timerScheduleCountdown.Stop();
            }
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
        statusTikTokLabel.Text = $"TikTok: {message}";
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
        // Update TikTok countdown if running
        if (_tikTokCollector != null && _tikTokCollector.IsRunning)
        {
            UpdateNextScheduleLabel();
        }

        // Update Facebook countdown if running
        if (_facebookCollector != null && _facebookCollector.IsRunning)
        {
            UpdateFbNextScheduleLabel();
        }

        // Check if we're in the no-scrape time window
        CheckNoScrapWindow();
    }

    private void UpdateNextScheduleLabel()
    {
        var activeSchedules = _tikTokSchedules.Where(s => s.IsActive).Select(s => s.Timing);
        var result = ServiceContainer.ScheduleCalculation.CalculateNextSchedule(activeSchedules);

        if (result.NextScheduleTime == null)
        {
            lblTkNextSchedule.Text = "Next: No active schedules";
            statusTikTokSchedule.Text = "No schedules";
        }
        else
        {
            lblTkNextSchedule.Text = $"Next: {result.FormattedTime} ({result.FormattedCountdown})";
            statusTikTokSchedule.Text = $"{result.FormattedTime} ({result.FormattedCountdown})";
        }
    }

    private void UpdateFbNextScheduleLabel()
    {
        var activeSchedules = _facebookSchedules.Where(s => s.IsActive).Select(s => s.Timing);
        var result = ServiceContainer.ScheduleCalculation.CalculateNextSchedule(activeSchedules);

        if (result.NextScheduleTime == null)
        {
            lblFbNextSchedule.Text = "Next: No active schedules";
            statusFacebookSchedule.Text = "No schedules";
        }
        else
        {
            lblFbNextSchedule.Text = $"Next: {result.FormattedTime} ({result.FormattedCountdown})";
            statusFacebookSchedule.Text = $"{result.FormattedTime} ({result.FormattedCountdown})";
        }
    }

    #endregion

    #region Facebook Management

    private void LoadFacebookSettings()
    {
        var settings = ServiceContainer.Settings.LoadSettings();

        // Load delay setting
        numFbFrequency.Value = Math.Clamp(settings.FacebookDelaySeconds, 1, 3600);

        // Load chunk settings
        numFbChunkSize.Value = Math.Clamp(settings.FacebookChunkSize, 1, 1000);
        numFbChunkDelay.Value = Math.Clamp(settings.FacebookChunkDelayMinutes, 1, 1440);

        // Load schedules
        _facebookSchedules.Clear();
        foreach (var schedSetting in settings.FacebookSchedules)
        {
            var schedule = new FbSchedule
            {
                Id = _nextFbScheduleId++,
                Timing = new TimeSpan(schedSetting.Hour, schedSetting.Minute, 0),
                IsActive = schedSetting.IsEnabled
            };
            _facebookSchedules.Add(schedule);
        }
        RefreshFbScheduleList();
    }

    private void SaveFacebookSettings()
    {
        var settings = ServiceContainer.Settings.LoadSettings();

        // Save delay setting
        settings.FacebookDelaySeconds = (int)numFbFrequency.Value;

        // Save chunk settings
        settings.FacebookChunkSize = (int)numFbChunkSize.Value;
        settings.FacebookChunkDelayMinutes = (int)numFbChunkDelay.Value;

        // Save schedules
        settings.FacebookSchedules.Clear();
        foreach (var schedule in _facebookSchedules)
        {
            settings.FacebookSchedules.Add(new FacebookScheduleSettings
            {
                Hour = schedule.Timing.Hours,
                Minute = schedule.Timing.Minutes,
                IsEnabled = schedule.IsActive
            });
        }

        ServiceContainer.Settings.SaveSettings(settings);
    }

    #region No-Scrape Time Window

    private void LoadNoScrapSettings()
    {
        var settings = ServiceContainer.Settings.LoadSettings();

        chkNoScrapEnabled.Checked = settings.NoScrapEnabled;
        dtpNoScrapStart.Value = DateTime.Today.AddHours(settings.NoScrapStartHour).AddMinutes(settings.NoScrapStartMinute);
        dtpNoScrapEnd.Value = DateTime.Today.AddHours(settings.NoScrapEndHour).AddMinutes(settings.NoScrapEndMinute);
    }

    private void SaveNoScrapSettings()
    {
        var settings = ServiceContainer.Settings.LoadSettings();

        settings.NoScrapEnabled = chkNoScrapEnabled.Checked;
        settings.NoScrapStartHour = dtpNoScrapStart.Value.Hour;
        settings.NoScrapStartMinute = dtpNoScrapStart.Value.Minute;
        settings.NoScrapEndHour = dtpNoScrapEnd.Value.Hour;
        settings.NoScrapEndMinute = dtpNoScrapEnd.Value.Minute;

        ServiceContainer.Settings.SaveSettings(settings);
    }

    private void NoScrapSettings_Changed(object? sender, EventArgs e)
    {
        SaveNoScrapSettings();

        // If no-scrap is disabled and user wants scraping but was waiting, start now
        if (!chkNoScrapEnabled.Checked && _userWantsScraping && _waitingForNoScrapEnd && _backgroundScraper != null)
        {
            _waitingForNoScrapEnd = false;
            _noScrapAutoStopped = false;
            _backgroundScraper.Start();
            LogDebug("No-Scrape disabled, starting scraping", "INFO");
        }
        // If no-scrap is disabled and scraping was auto-stopped, restart it
        else if (!chkNoScrapEnabled.Checked && _noScrapAutoStopped && _backgroundScraper != null)
        {
            _noScrapAutoStopped = false;
            _backgroundScraper.Start();
            LogDebug("No-Scrape disabled, resuming scraping", "INFO");
        }
    }

    private bool IsInNoScrapWindow()
    {
        return ServiceContainer.NoScrapWindow.IsInNoScrapWindow();
    }

    private void CheckNoScrapWindow()
    {
        if (_backgroundScraper == null) return;

        bool inNoScrapWindow = IsInNoScrapWindow();

        // If scraping is running and we enter no-scrap window
        if (inNoScrapWindow && _backgroundScraper.IsRunning && _userWantsScraping)
        {
            // Stop scraping during no-scrap window
            _backgroundScraper.Stop();
            _waitingForNoScrapEnd = true;
            _noScrapAutoStopped = true;
            var (start, end) = ServiceContainer.NoScrapWindow.GetWindowTimes();
            LogDebug($"═══════════════════════════════════════════════════", "WARNING");
            LogDebug($"ENTERING NO-SCRAPE WINDOW", "WARNING");
            LogDebug($"Window: {DateTime.Today.Add(start):HH:mm} - {DateTime.Today.Add(end):HH:mm}", "WARNING");
            LogDebug($"Scraping paused - WebDriver stopped", "WARNING");
            LogDebug($"═══════════════════════════════════════════════════", "WARNING");
        }
        // If user wants scraping and we're waiting for no-scrap to end
        else if (!inNoScrapWindow && _userWantsScraping && (_waitingForNoScrapEnd || _noScrapAutoStopped))
        {
            // Resume scraping after no-scrap window ends
            _waitingForNoScrapEnd = false;
            _noScrapAutoStopped = false;
            _backgroundScraper.Start();
            LogDebug($"═══════════════════════════════════════════════════", "INFO");
            LogDebug($"NO-SCRAPE WINDOW ENDED", "INFO");
            LogDebug($"Automatically starting scraping", "INFO");
            LogDebug($"═══════════════════════════════════════════════════", "INFO");
        }

        // Update status with countdown if waiting
        if (_userWantsScraping && _waitingForNoScrapEnd && inNoScrapWindow)
        {
            UpdateNoScrapStatus();
        }
    }

    private TimeSpan GetNoScrapRemainingTime()
    {
        return ServiceContainer.NoScrapWindow.GetRemainingTime();
    }

    private void UpdateNoScrapStatus()
    {
        if (InvokeRequired)
        {
            Invoke(() => UpdateNoScrapStatus());
            return;
        }

        if (_userWantsScraping && _waitingForNoScrapEnd && IsInNoScrapWindow())
        {
            var remaining = GetNoScrapRemainingTime();
            var statusText = $"No-Scrape: Resumes in {remaining:hh\\:mm\\:ss}";
            statusNewsLabel.Text = $"News: {statusText}";
            lblStatus.Text = statusText;
        }
    }

    private void UpdateScraperButtonState(bool showAsRunning)
    {
        if (InvokeRequired)
        {
            Invoke(() => UpdateScraperButtonState(showAsRunning));
            return;
        }

        if (showAsRunning)
        {
            btnStartStop.Text = "Stop";
            btnStartStop.BackColor = Color.IndianRed;
        }
        else
        {
            btnStartStop.Text = "Start";
            btnStartStop.BackColor = Color.MediumSeaGreen;
        }
    }

    #endregion

    private async Task LoadFacebookProfilesAsync()
    {
        if (!ServiceContainer.Database.IsConnected) return;

        _facebookProfiles = await ServiceContainer.Database.GetAllFbProfilesAsync().ConfigureAwait(true);
        olvFacebookID.SetObjects(_facebookProfiles);
        UpdateFbInfo();
    }

    private void LoadFacebookProfiles()
    {
        // Sync version for backwards compatibility - fires and forgets the async version
        _ = LoadFacebookProfilesAsync();
    }

    private async Task LoadFacebookDataAsync(string? username = null, DateTime? fromDate = null, DateTime? toDate = null)
    {
        if (!ServiceContainer.Database.IsConnected) return;

        _facebookData = await ServiceContainer.Database.GetFilteredFbDataAsync(username, fromDate, toDate, 500).ConfigureAwait(true);
        olvFacebookData.SetObjects(_facebookData);
        UpdateFbInfo();
    }

    private void LoadFacebookData(string? username = null, DateTime? fromDate = null, DateTime? toDate = null)
    {
        // Sync version for backwards compatibility - fires and forgets the async version
        _ = LoadFacebookDataAsync(username, fromDate, toDate);
    }

    private void RefreshFbScheduleList()
    {
        // Update serial numbers
        for (int i = 0; i < _facebookSchedules.Count; i++)
        {
            _facebookSchedules[i].SerialNumber = i + 1;
        }
        olvFacebookSchedule.SetObjects(_facebookSchedules);
    }

    private void BtnFbAddId_Click(object? sender, EventArgs e)
    {
        using var form = new FacebookIdManagerForm();
        if (form.ShowDialog() == DialogResult.OK)
        {
            LoadFacebookProfiles();
            InitializeFbFilterControls();
        }
    }

    private void BtnFbDeleteId_Click(object? sender, EventArgs e)
    {
        if (olvFacebookID.SelectedObject is not FbProfile profile)
        {
            MessageBox.Show("Please select a profile to delete.", "No Selection",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var result = MessageBox.Show(
            $"Are you sure you want to delete the profile '{profile.Username}'?\n\n" +
            "This will also delete all associated data records.",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result != DialogResult.Yes) return;

        ServiceContainer.Database.DeleteFbProfile(profile.UserId);
        LoadFacebookProfiles();
        LoadFacebookData();
        InitializeFbFilterControls();
    }

    private void BtnFbRefreshId_Click(object? sender, EventArgs e)
    {
        LoadFacebookProfiles();
        InitializeFbFilterControls();
    }

    private void OlvFacebookID_SubItemChecking(object? sender, BrightIdeasSoftware.SubItemCheckingEventArgs e)
    {
        if (e.RowObject is FbProfile profile)
        {
            profile.Status = e.NewValue == CheckState.Checked;
            ServiceContainer.Database.UpdateFbProfileStatus(profile.UserId, profile.Status);
            UpdateFbStatus($"{profile.Username} status: {(profile.Status ? "Active" : "Inactive")}");
        }
    }

    private void MenuItemFbAddId_Click(object? sender, EventArgs e)
    {
        BtnFbAddId_Click(sender, e);
    }

    private void MenuItemFbEditId_Click(object? sender, EventArgs e)
    {
        if (olvFacebookID.SelectedObject is not FbProfile profile)
        {
            MessageBox.Show("Please select a profile to edit.", "No Selection",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var form = new FacebookIdManagerForm(profile);
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            LoadFacebookProfiles();
            UpdateFbStatus($"Updated profile: {profile.Username}");
        }
    }

    private void MenuItemFbDeleteId_Click(object? sender, EventArgs e)
    {
        BtnFbDeleteId_Click(sender, e);
    }

    private void MenuItemFbRefresh_Click(object? sender, EventArgs e)
    {
        BtnFbRefreshId_Click(sender, e);
    }

    private async void MenuItemFbFetch_Click(object? sender, EventArgs e)
    {
        if (olvFacebookID.SelectedObject is not FbProfile profile)
        {
            MessageBox.Show("Please select a profile to fetch.", "No Selection",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        await FetchFacebookProfileAsync(profile);
    }

    private void MenuItemFbExportLogos_Click(object? sender, EventArgs e)
    {
        var selectedProfiles = olvFacebookID.SelectedObjects.Cast<FbProfile>().ToList();
        if (selectedProfiles.Count == 0)
        {
            MessageBox.Show("Please select one or more profiles to export logos.", "No Selection",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var form = new LogoExportForm(selectedProfiles, Services.LogoExportPlatform.Facebook);
        form.ShowDialog(this);
    }

    private async Task FetchFacebookProfileAsync(FbProfile profile)
    {
        try
        {
            UpdateFbStatus($"Fetching {profile.Username}...");
            progressBarFb.Visible = true;
            progressBarFb.Style = ProgressBarStyle.Marquee;

            var scraper = new FacebookScraperService();
            var data = await scraper.FetchStatsAsync(profile);

            if (data != null)
            {
                var dataId = ServiceContainer.Database.AddFbData(data);
                data.DataId = dataId;

                // Add to data list display
                _facebookData.Insert(0, data);
                while (_facebookData.Count > 500)
                {
                    _facebookData.RemoveAt(_facebookData.Count - 1);
                }
                olvFacebookData.SetObjects(_facebookData);
                UpdateFbInfo();

                UpdateFbStatus($"Fetched {profile.Username}: {data.FollowersCount:N0} followers");
            }
            else
            {
                UpdateFbStatus($"Failed to fetch {profile.Username}");
            }

            scraper.Dispose();
        }
        catch (Exception ex)
        {
            UpdateFbStatus($"Error fetching {profile.Username}: {ex.Message}");
        }
        finally
        {
            progressBarFb.Visible = false;
            progressBarFb.Style = ProgressBarStyle.Blocks;
        }
    }

    private void BtnFbAddSchedule_Click(object? sender, EventArgs e)
    {
        using var form = new FacebookScheduleForm();
        if (form.ShowDialog() == DialogResult.OK && form.ResultSchedule != null)
        {
            var newSchedule = form.ResultSchedule;
            newSchedule.Id = _nextFbScheduleId++;
            _facebookSchedules.Add(newSchedule);
            RefreshFbScheduleList();
            SaveFacebookSettings();

            // Update collector with new schedules
            _facebookCollector?.UpdateSchedules(_facebookSchedules);
        }
    }

    private void BtnFbEditSchedule_Click(object? sender, EventArgs e)
    {
        if (olvFacebookSchedule.SelectedObject is not FbSchedule schedule)
        {
            MessageBox.Show("Please select a schedule to edit.", "No Selection",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var form = new FacebookScheduleForm(schedule);
        if (form.ShowDialog() == DialogResult.OK)
        {
            RefreshFbScheduleList();
            SaveFacebookSettings();

            // Update collector with new schedules
            _facebookCollector?.UpdateSchedules(_facebookSchedules);
        }
    }

    private void BtnFbDeleteSchedule_Click(object? sender, EventArgs e)
    {
        if (olvFacebookSchedule.SelectedObject is not FbSchedule schedule)
        {
            MessageBox.Show("Please select a schedule to delete.", "No Selection",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _facebookSchedules.Remove(schedule);
        RefreshFbScheduleList();
        SaveFacebookSettings();

        // Update collector with new schedules
        _facebookCollector?.UpdateSchedules(_facebookSchedules);
    }

    private void BtnFbSaveSettings_Click(object? sender, EventArgs e)
    {
        SaveFacebookSettings();
        _facebookCollector?.UpdateDelaySeconds((int)numFbFrequency.Value);
        UpdateFbStatus("Settings saved");
    }

    private void BtnFbStartStop_Click(object? sender, EventArgs e)
    {
        if (_facebookCollector == null) return;

        if (!ServiceContainer.Database.IsConnected)
        {
            MessageBox.Show("Please configure database connection first.", "Not Connected",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_facebookProfiles.Count == 0)
        {
            MessageBox.Show("Please add at least one Facebook Page first.", "No Pages",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_facebookCollector.IsRunning)
        {
            _facebookCollector.Stop();
        }
        else
        {
            // Check if there are any active schedules
            var activeSchedules = _facebookSchedules.Where(s => s.IsActive).ToList();
            if (activeSchedules.Count == 0)
            {
                MessageBox.Show("Please add and enable at least one schedule first.", "No Active Schedules",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Clear data list when starting new fetch
            ClearFacebookDataList();

            // Update settings before starting
            _facebookCollector.UpdateDelaySeconds((int)numFbFrequency.Value);
            _facebookCollector.UpdateChunkSettings((int)numFbChunkSize.Value, (int)numFbChunkDelay.Value);
            _facebookCollector.UpdateSchedules(_facebookSchedules);

            // Set flag to skip immediate collection - wait for next schedule
            _facebookCollector.SkipInitialCollection = true;
            _facebookCollector.Start();
        }
    }

    private void NumFbFrequency_ValueChanged(object? sender, EventArgs e)
    {
        // Update collector delay in real-time
        _facebookCollector?.UpdateDelaySeconds((int)numFbFrequency.Value);
    }

    private void BtnFbApplyFilter_Click(object? sender, EventArgs e)
    {
        string? username = null;
        if (cboFbFilterUsername.SelectedIndex > 0)
        {
            username = cboFbFilterUsername.SelectedItem?.ToString();
        }

        var fromDate = dtpFbFilterFrom.Value;
        var toDate = dtpFbFilterTo.Value;

        LoadFacebookData(username, fromDate, toDate);
    }

    private void BtnFbClearFilter_Click(object? sender, EventArgs e)
    {
        cboFbFilterUsername.SelectedIndex = 0;
        dtpFbFilterTo.Value = DateTime.Now;
        dtpFbFilterFrom.Value = DateTime.Today.AddDays(-2);
        LoadFacebookData();
    }

    private void ClearFacebookDataList()
    {
        _facebookData.Clear();
        olvFacebookData.SetObjects(_facebookData);
        UpdateFbInfo();
    }

    private void InitializeFbFilterControls()
    {
        // Populate username dropdown
        cboFbFilterUsername.Items.Clear();
        cboFbFilterUsername.Items.Add("(All Pages)");
        foreach (var profile in _facebookProfiles)
        {
            cboFbFilterUsername.Items.Add(profile.Username);
        }
        cboFbFilterUsername.SelectedIndex = 0;

        // Set date/time defaults (last 2 days)
        dtpFbFilterTo.Value = DateTime.Now;
        dtpFbFilterFrom.Value = DateTime.Today.AddDays(-2);
    }

    private void OlvFacebookData_FormatCell(object? sender, BrightIdeasSoftware.FormatCellEventArgs e)
    {
        if (e.Model is not FbData data) return;

        // Color the change columns based on positive/negative values
        if (e.Column == olvColFbFollowersChange)
        {
            FormatChangeCell(e, data.FollowersChange);
        }
        else if (e.Column == olvColFbTalkingAboutChange)
        {
            FormatChangeCell(e, data.TalkingAboutChange);
        }
    }

    private void FacebookCollector_StatusChanged(object? sender, string e)
    {
        UpdateFbStatus(e);
    }

    private void FacebookCollector_ProgressChanged(object? sender, (int current, int total) e)
    {
        if (InvokeRequired)
        {
            Invoke(() => FacebookCollector_ProgressChanged(sender, e));
            return;
        }

        progressBarFb.Maximum = Math.Max(1, e.total);
        progressBarFb.Value = Math.Min(e.current, e.total);
    }

    private void FacebookCollector_RunningStateChanged(object? sender, bool isRunning)
    {
        if (InvokeRequired)
        {
            Invoke(() => FacebookCollector_RunningStateChanged(sender, isRunning));
            return;
        }

        btnFbStartStop.Text = isRunning ? "Stop" : "Start";
        btnFbStartStop.BackColor = isRunning ? Color.FromArgb(180, 0, 0) : Color.FromArgb(0, 120, 0);

        // Disable/enable filter controls during fetching
        SetFacebookFilterEnabled(!isRunning);

        // Start/stop countdown timer
        if (isRunning)
        {
            UpdateFbNextScheduleLabel();
            // Start timer if not already running (TikTok might have it running)
            if (!timerScheduleCountdown.Enabled)
            {
                timerScheduleCountdown.Start();
            }
        }
        else
        {
            progressBarFb.Value = 0;
            lblFbNextSchedule.Text = "Next: --:-- (stopped)";
            statusFacebookSchedule.Text = "(stopped)";
            statusFacebookLabel.Text = "Facebook: Stopped";
            // Only stop timer if TikTok is also not running
            if (_tikTokCollector == null || !_tikTokCollector.IsRunning)
            {
                timerScheduleCountdown.Stop();
            }
        }
    }

    private void SetFacebookFilterEnabled(bool enabled)
    {
        cboFbFilterUsername.Enabled = enabled;
        dtpFbFilterFrom.Enabled = enabled;
        dtpFbFilterTo.Enabled = enabled;
        btnFbApplyFilter.Enabled = enabled;
        btnFbClearFilter.Enabled = enabled;
    }

    private void FacebookCollector_DataCollected(object? sender, FbData data)
    {
        if (InvokeRequired)
        {
            Invoke(() => FacebookCollector_DataCollected(sender, data));
            return;
        }

        // Add to front of list and refresh
        _facebookData.Insert(0, data);

        // Keep list size manageable
        while (_facebookData.Count > 500)
        {
            _facebookData.RemoveAt(_facebookData.Count - 1);
        }

        olvFacebookData.SetObjects(_facebookData);
        UpdateFbInfo();
    }

    private void UpdateFbStatus(string message)
    {
        if (InvokeRequired)
        {
            Invoke(() => UpdateFbStatus(message));
            return;
        }

        lblFbStatus.Text = message;
        statusFacebookLabel.Text = $"Facebook: {message}";
    }

    private void UpdateFbInfo()
    {
        if (InvokeRequired)
        {
            Invoke(UpdateFbInfo);
            return;
        }

        lblFbInfo.Text = $"Pages:{_facebookProfiles.Count}, Sch:{_facebookSchedules.Count}, Data:{_facebookData.Count}";
    }

    #endregion
}
