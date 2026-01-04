namespace nRun.UI.Forms;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        tabControlMain = new TabControl();
        tabPageNewsScrp = new TabPage();
        splitContainerMain = new SplitContainer();
        panelLeft = new Panel();
        listBoxSites = new ListBox();
        panelNoScrap = new Panel();
        chkNoScrapEnabled = new CheckBox();
        lblNoScrapStart = new Label();
        dtpNoScrapStart = new DateTimePicker();
        lblNoScrapEnd = new Label();
        dtpNoScrapEnd = new DateTimePicker();
        toolStripSites = new ToolStrip();
        btnAddSite = new ToolStripButton();
        btnEditSite = new ToolStripButton();
        btnDeleteSite = new ToolStripButton();
        toolStripSeparator1 = new ToolStripSeparator();
        btnRefreshSites = new ToolStripButton();
        toolStripSeparatorManage = new ToolStripSeparator();
        btnManageSites = new ToolStripButton();
        lblSitesHeader = new Label();
        panelRight = new Panel();
        splitContainerRight = new SplitContainer();
        olvArticles = new BrightIdeasSoftware.ObjectListView();
        olvColTitle = new BrightIdeasSoftware.OLVColumn();
        olvColSite = new BrightIdeasSoftware.OLVColumn();
        olvColSiteLogo = new BrightIdeasSoftware.OLVColumn();
        olvColDate = new BrightIdeasSoftware.OLVColumn();
        contextMenuArticles = new ContextMenuStrip(components);
        menuItemOpenUrl = new ToolStripMenuItem();
        menuItemCopyUrl = new ToolStripMenuItem();
        menuItemCopyTitle = new ToolStripMenuItem();
        menuItemSeparator1 = new ToolStripSeparator();
        menuItemViewBody = new ToolStripMenuItem();
        menuItemSeparator2 = new ToolStripSeparator();
        menuItemDelete = new ToolStripMenuItem();
        panelDebug = new Panel();
        splitContainerDebug = new SplitContainer();
        panelDebugLeft = new Panel();
        txtDebugLog = new TextBox();
        lblDebugHeader = new Label();
        btnClearLog = new Button();
        panelDebugRight = new Panel();
        txtErrorLog = new TextBox();
        lblErrorHeader = new Label();
        btnClearErrorLog = new Button();
        tabPageTiktok = new TabPage();
        tableLayoutPanel1 = new TableLayoutPanel();
        tableLayoutPanel2 = new TableLayoutPanel();
        olvTiktokData = new BrightIdeasSoftware.ObjectListView();
        olvColDataId = new BrightIdeasSoftware.OLVColumn();
        olvColDataUsername = new BrightIdeasSoftware.OLVColumn();
        olvColDataFollowers = new BrightIdeasSoftware.OLVColumn();
        olvColDataFollowersChange = new BrightIdeasSoftware.OLVColumn();
        olvColDataHearts = new BrightIdeasSoftware.OLVColumn();
        olvColDataHeartsChange = new BrightIdeasSoftware.OLVColumn();
        olvColDataVideos = new BrightIdeasSoftware.OLVColumn();
        olvColDataVideosChange = new BrightIdeasSoftware.OLVColumn();
        olvColDataRecordedAt = new BrightIdeasSoftware.OLVColumn();
        tableLayoutPanel3 = new TableLayoutPanel();
        lblFilterUsername = new Label();
        cboFilterUsername = new ComboBox();
        lblFilterDateFrom = new Label();
        dtpFilterFrom = new DateTimePicker();
        lblFilterDateTo = new Label();
        dtpFilterTo = new DateTimePicker();
        btnClearFilter = new Button();
        btnApplyFilter = new Button();
        tableLayoutPanel4 = new TableLayoutPanel();
        lblTkStatus = new Label();
        lblTkInfo = new Label();
        progressBarTk = new ProgressBar();
        tableLayoutPanel5 = new TableLayoutPanel();
        olvTiktokID = new BrightIdeasSoftware.ObjectListView();
        olvColTkUsername = new BrightIdeasSoftware.OLVColumn();
        olvColTkNickname = new BrightIdeasSoftware.OLVColumn();
        olvColTkStatus = new BrightIdeasSoftware.OLVColumn();
        contextMenuTiktokID = new ContextMenuStrip(components);
        menuItemTkAddId = new ToolStripMenuItem();
        menuItemTkEditId = new ToolStripMenuItem();
        menuItemTkDeleteId = new ToolStripMenuItem();
        menuItemTkSeparator1 = new ToolStripSeparator();
        menuItemTkRefresh = new ToolStripMenuItem();
        menuItemTkFetch = new ToolStripMenuItem();
        lblTkNextSchedule = new Label();
        btnTkStartStop = new Button();
        olvTiktokSchedule = new BrightIdeasSoftware.ObjectListView();
        olvColScheduleSL = new BrightIdeasSoftware.OLVColumn();
        olvColScheduleTiming = new BrightIdeasSoftware.OLVColumn();
        olvColScheduleStatus = new BrightIdeasSoftware.OLVColumn();
        tableLayoutPanel6 = new TableLayoutPanel();
        btnTkRefreshId = new Button();
        btnTkDeleteId = new Button();
        btnTkAddId = new Button();
        tableLayoutPanel7 = new TableLayoutPanel();
        btnTkAddSchedule = new Button();
        btnTkSaveSettings = new Button();
        btnTkDeleteSchedule = new Button();
        btnTkEditSchedule = new Button();
        tableLayoutPanel8 = new TableLayoutPanel();
        numTkFrequency = new NumericUpDown();
        lblTkFrequency = new Label();
        tabPageFacebook = new TabPage();
        tableLayoutPanelFb1 = new TableLayoutPanel();
        tableLayoutPanelFb2 = new TableLayoutPanel();
        olvFacebookData = new BrightIdeasSoftware.ObjectListView();
        olvColFbDataId = new BrightIdeasSoftware.OLVColumn();
        olvColFbDataUsername = new BrightIdeasSoftware.OLVColumn();
        olvColFbDataFollowers = new BrightIdeasSoftware.OLVColumn();
        olvColFbFollowersChange = new BrightIdeasSoftware.OLVColumn();
        olvColFbDataTalkingAbout = new BrightIdeasSoftware.OLVColumn();
        olvColFbTalkingAboutChange = new BrightIdeasSoftware.OLVColumn();
        olvColFbDataRecordedAt = new BrightIdeasSoftware.OLVColumn();
        tableLayoutPanelFb3 = new TableLayoutPanel();
        lblFbFilterUsername = new Label();
        cboFbFilterUsername = new ComboBox();
        lblFbFilterDateFrom = new Label();
        dtpFbFilterFrom = new DateTimePicker();
        lblFbFilterDateTo = new Label();
        dtpFbFilterTo = new DateTimePicker();
        btnFbClearFilter = new Button();
        btnFbApplyFilter = new Button();
        tableLayoutPanelFb4 = new TableLayoutPanel();
        lblFbStatus = new Label();
        lblFbInfo = new Label();
        progressBarFb = new ProgressBar();
        tableLayoutPanelFb5 = new TableLayoutPanel();
        olvFacebookID = new BrightIdeasSoftware.ObjectListView();
        olvColFbUsername = new BrightIdeasSoftware.OLVColumn();
        olvColFbNickname = new BrightIdeasSoftware.OLVColumn();
        olvColFbStatus = new BrightIdeasSoftware.OLVColumn();
        contextMenuFacebookID = new ContextMenuStrip(components);
        menuItemFbAddId = new ToolStripMenuItem();
        menuItemFbEditId = new ToolStripMenuItem();
        menuItemFbDeleteId = new ToolStripMenuItem();
        menuItemFbSeparator1 = new ToolStripSeparator();
        menuItemFbRefresh = new ToolStripMenuItem();
        menuItemFbFetch = new ToolStripMenuItem();
        lblFbNextSchedule = new Label();
        btnFbStartStop = new Button();
        olvFacebookSchedule = new BrightIdeasSoftware.ObjectListView();
        olvColFbScheduleSL = new BrightIdeasSoftware.OLVColumn();
        olvColFbScheduleTiming = new BrightIdeasSoftware.OLVColumn();
        olvColFbScheduleStatus = new BrightIdeasSoftware.OLVColumn();
        tableLayoutPanelFb6 = new TableLayoutPanel();
        btnFbRefreshId = new Button();
        btnFbDeleteId = new Button();
        btnFbAddId = new Button();
        tableLayoutPanelFb7 = new TableLayoutPanel();
        btnFbAddSchedule = new Button();
        btnFbSaveSettings = new Button();
        btnFbDeleteSchedule = new Button();
        btnFbEditSchedule = new Button();
        tableLayoutPanelFb8 = new TableLayoutPanel();
        lblFbFrequency = new Label();
        numFbFrequency = new NumericUpDown();
        lblFbChunkSize = new Label();
        numFbChunkSize = new NumericUpDown();
        lblFbChunkDelay = new Label();
        numFbChunkDelay = new NumericUpDown();
        toolStripMain = new ToolStrip();
        btnStartStop = new ToolStripButton();
        btnScrapeNow = new ToolStripButton();
        toolStripSeparator2 = new ToolStripSeparator();
        btnSettings = new ToolStripButton();
        toolStripSeparator3 = new ToolStripSeparator();
        btnMemuraiSync = new ToolStripButton();
        btnMemuraiView = new ToolStripButton();
        toolStripSeparator4 = new ToolStripSeparator();
        lblStatus = new ToolStripLabel();
        toolStripSeparator5 = new ToolStripSeparator();
        btnAbout = new ToolStripButton();
        statusStrip = new StatusStrip();
        statusNewsLabel = new ToolStripStatusLabel();
        statusNewsProgress = new ToolStripProgressBar();
        statusNewsCount = new ToolStripStatusLabel();
        statusSeparator1 = new ToolStripStatusLabel();
        statusTikTokLabel = new ToolStripStatusLabel();
        statusTikTokSchedule = new ToolStripStatusLabel();
        statusSeparator2 = new ToolStripStatusLabel();
        statusFacebookLabel = new ToolStripStatusLabel();
        statusFacebookSchedule = new ToolStripStatusLabel();
        timerScheduleCountdown = new System.Windows.Forms.Timer(components);
        tabControlMain.SuspendLayout();
        tabPageNewsScrp.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
        splitContainerMain.Panel1.SuspendLayout();
        splitContainerMain.Panel2.SuspendLayout();
        splitContainerMain.SuspendLayout();
        panelLeft.SuspendLayout();
        panelNoScrap.SuspendLayout();
        toolStripSites.SuspendLayout();
        panelRight.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainerRight).BeginInit();
        splitContainerRight.Panel1.SuspendLayout();
        splitContainerRight.Panel2.SuspendLayout();
        splitContainerRight.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)olvArticles).BeginInit();
        contextMenuArticles.SuspendLayout();
        panelDebug.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainerDebug).BeginInit();
        splitContainerDebug.Panel1.SuspendLayout();
        splitContainerDebug.Panel2.SuspendLayout();
        splitContainerDebug.SuspendLayout();
        panelDebugLeft.SuspendLayout();
        panelDebugRight.SuspendLayout();
        tabPageTiktok.SuspendLayout();
        tableLayoutPanel1.SuspendLayout();
        tableLayoutPanel2.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)olvTiktokData).BeginInit();
        tableLayoutPanel3.SuspendLayout();
        tableLayoutPanel4.SuspendLayout();
        tableLayoutPanel5.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)olvTiktokID).BeginInit();
        contextMenuTiktokID.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)olvTiktokSchedule).BeginInit();
        tableLayoutPanel6.SuspendLayout();
        tableLayoutPanel7.SuspendLayout();
        tableLayoutPanel8.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numTkFrequency).BeginInit();
        tabPageFacebook.SuspendLayout();
        tableLayoutPanelFb1.SuspendLayout();
        tableLayoutPanelFb2.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)olvFacebookData).BeginInit();
        tableLayoutPanelFb3.SuspendLayout();
        tableLayoutPanelFb4.SuspendLayout();
        tableLayoutPanelFb5.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)olvFacebookID).BeginInit();
        contextMenuFacebookID.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)olvFacebookSchedule).BeginInit();
        tableLayoutPanelFb6.SuspendLayout();
        tableLayoutPanelFb7.SuspendLayout();
        tableLayoutPanelFb8.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numFbFrequency).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numFbChunkSize).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numFbChunkDelay).BeginInit();
        toolStripMain.SuspendLayout();
        statusStrip.SuspendLayout();
        SuspendLayout();
        // 
        // tabControlMain
        // 
        tabControlMain.Controls.Add(tabPageNewsScrp);
        tabControlMain.Controls.Add(tabPageTiktok);
        tabControlMain.Controls.Add(tabPageFacebook);
        tabControlMain.Dock = DockStyle.Fill;
        tabControlMain.Location = new Point(0, 25);
        tabControlMain.Name = "tabControlMain";
        tabControlMain.SelectedIndex = 0;
        tabControlMain.Size = new Size(1133, 634);
        tabControlMain.TabIndex = 0;
        // 
        // tabPageNewsScrp
        // 
        tabPageNewsScrp.Controls.Add(splitContainerMain);
        tabPageNewsScrp.Location = new Point(4, 24);
        tabPageNewsScrp.Name = "tabPageNewsScrp";
        tabPageNewsScrp.Padding = new Padding(3);
        tabPageNewsScrp.Size = new Size(1125, 606);
        tabPageNewsScrp.TabIndex = 0;
        tabPageNewsScrp.Text = "News Scrp";
        tabPageNewsScrp.UseVisualStyleBackColor = true;
        // 
        // splitContainerMain
        // 
        splitContainerMain.Dock = DockStyle.Fill;
        splitContainerMain.Location = new Point(3, 3);
        splitContainerMain.Name = "splitContainerMain";
        // 
        // splitContainerMain.Panel1
        // 
        splitContainerMain.Panel1.Controls.Add(panelLeft);
        // 
        // splitContainerMain.Panel2
        // 
        splitContainerMain.Panel2.Controls.Add(panelRight);
        splitContainerMain.Size = new Size(1119, 600);
        splitContainerMain.SplitterDistance = 283;
        splitContainerMain.TabIndex = 0;
        // 
        // panelLeft
        // 
        panelLeft.Controls.Add(listBoxSites);
        panelLeft.Controls.Add(panelNoScrap);
        panelLeft.Controls.Add(toolStripSites);
        panelLeft.Controls.Add(lblSitesHeader);
        panelLeft.Dock = DockStyle.Fill;
        panelLeft.Location = new Point(0, 0);
        panelLeft.Name = "panelLeft";
        panelLeft.Size = new Size(283, 600);
        panelLeft.TabIndex = 0;
        // 
        // listBoxSites
        // 
        listBoxSites.Dock = DockStyle.Fill;
        listBoxSites.FormattingEnabled = true;
        listBoxSites.ItemHeight = 15;
        listBoxSites.Location = new Point(0, 50);
        listBoxSites.Name = "listBoxSites";
        listBoxSites.Size = new Size(283, 485);
        listBoxSites.TabIndex = 2;
        // 
        // panelNoScrap
        // 
        panelNoScrap.Controls.Add(chkNoScrapEnabled);
        panelNoScrap.Controls.Add(lblNoScrapStart);
        panelNoScrap.Controls.Add(dtpNoScrapStart);
        panelNoScrap.Controls.Add(lblNoScrapEnd);
        panelNoScrap.Controls.Add(dtpNoScrapEnd);
        panelNoScrap.Dock = DockStyle.Bottom;
        panelNoScrap.Location = new Point(0, 535);
        panelNoScrap.Name = "panelNoScrap";
        panelNoScrap.Size = new Size(283, 65);
        panelNoScrap.TabIndex = 3;
        // 
        // chkNoScrapEnabled
        // 
        chkNoScrapEnabled.AutoSize = true;
        chkNoScrapEnabled.Location = new Point(10, 5);
        chkNoScrapEnabled.Name = "chkNoScrapEnabled";
        chkNoScrapEnabled.Size = new Size(149, 19);
        chkNoScrapEnabled.TabIndex = 0;
        chkNoScrapEnabled.Text = "Enable No-Scrape Time";
        chkNoScrapEnabled.UseVisualStyleBackColor = true;
        // 
        // lblNoScrapStart
        // 
        lblNoScrapStart.AutoSize = true;
        lblNoScrapStart.Location = new Point(10, 35);
        lblNoScrapStart.Name = "lblNoScrapStart";
        lblNoScrapStart.Size = new Size(34, 15);
        lblNoScrapStart.TabIndex = 1;
        lblNoScrapStart.Text = "Start:";
        // 
        // dtpNoScrapStart
        // 
        dtpNoScrapStart.CustomFormat = "HH:mm";
        dtpNoScrapStart.Format = DateTimePickerFormat.Custom;
        dtpNoScrapStart.Location = new Point(50, 32);
        dtpNoScrapStart.Name = "dtpNoScrapStart";
        dtpNoScrapStart.ShowUpDown = true;
        dtpNoScrapStart.Size = new Size(70, 23);
        dtpNoScrapStart.TabIndex = 2;
        // 
        // lblNoScrapEnd
        // 
        lblNoScrapEnd.AutoSize = true;
        lblNoScrapEnd.Location = new Point(135, 35);
        lblNoScrapEnd.Name = "lblNoScrapEnd";
        lblNoScrapEnd.Size = new Size(30, 15);
        lblNoScrapEnd.TabIndex = 3;
        lblNoScrapEnd.Text = "End:";
        // 
        // dtpNoScrapEnd
        // 
        dtpNoScrapEnd.CustomFormat = "HH:mm";
        dtpNoScrapEnd.Format = DateTimePickerFormat.Custom;
        dtpNoScrapEnd.Location = new Point(170, 32);
        dtpNoScrapEnd.Name = "dtpNoScrapEnd";
        dtpNoScrapEnd.ShowUpDown = true;
        dtpNoScrapEnd.Size = new Size(70, 23);
        dtpNoScrapEnd.TabIndex = 4;
        // 
        // toolStripSites
        // 
        toolStripSites.GripStyle = ToolStripGripStyle.Hidden;
        toolStripSites.Items.AddRange(new ToolStripItem[] { btnAddSite, btnEditSite, btnDeleteSite, toolStripSeparator1, btnRefreshSites, toolStripSeparatorManage, btnManageSites });
        toolStripSites.Location = new Point(0, 25);
        toolStripSites.Name = "toolStripSites";
        toolStripSites.Size = new Size(283, 25);
        toolStripSites.TabIndex = 1;
        // 
        // btnAddSite
        // 
        btnAddSite.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnAddSite.Name = "btnAddSite";
        btnAddSite.Size = new Size(33, 22);
        btnAddSite.Text = "Add";
        btnAddSite.ToolTipText = "Add new news source";
        // 
        // btnEditSite
        // 
        btnEditSite.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnEditSite.Name = "btnEditSite";
        btnEditSite.Size = new Size(31, 22);
        btnEditSite.Text = "Edit";
        btnEditSite.ToolTipText = "Edit selected source";
        // 
        // btnDeleteSite
        // 
        btnDeleteSite.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnDeleteSite.Name = "btnDeleteSite";
        btnDeleteSite.Size = new Size(44, 22);
        btnDeleteSite.Text = "Delete";
        btnDeleteSite.ToolTipText = "Delete selected source";
        // 
        // toolStripSeparator1
        // 
        toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new Size(6, 25);
        // 
        // btnRefreshSites
        // 
        btnRefreshSites.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnRefreshSites.Name = "btnRefreshSites";
        btnRefreshSites.Size = new Size(50, 22);
        btnRefreshSites.Text = "Refresh";
        btnRefreshSites.ToolTipText = "Refresh site list";
        // 
        // toolStripSeparatorManage
        // 
        toolStripSeparatorManage.Name = "toolStripSeparatorManage";
        toolStripSeparatorManage.Size = new Size(6, 25);
        // 
        // btnManageSites
        // 
        btnManageSites.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnManageSites.Name = "btnManageSites";
        btnManageSites.Size = new Size(54, 22);
        btnManageSites.Text = "Manage";
        btnManageSites.ToolTipText = "Batch manage sites (Import CSV, Edit All)";
        // 
        // lblSitesHeader
        // 
        lblSitesHeader.Dock = DockStyle.Top;
        lblSitesHeader.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblSitesHeader.Location = new Point(0, 0);
        lblSitesHeader.Name = "lblSitesHeader";
        lblSitesHeader.Padding = new Padding(5, 5, 0, 0);
        lblSitesHeader.Size = new Size(283, 25);
        lblSitesHeader.TabIndex = 0;
        lblSitesHeader.Text = "News Sources";
        // 
        // panelRight
        // 
        panelRight.Controls.Add(splitContainerRight);
        panelRight.Dock = DockStyle.Fill;
        panelRight.Location = new Point(0, 0);
        panelRight.Name = "panelRight";
        panelRight.Size = new Size(832, 600);
        panelRight.TabIndex = 0;
        // 
        // splitContainerRight
        // 
        splitContainerRight.Dock = DockStyle.Fill;
        splitContainerRight.Location = new Point(0, 0);
        splitContainerRight.Name = "splitContainerRight";
        splitContainerRight.Orientation = Orientation.Horizontal;
        // 
        // splitContainerRight.Panel1
        // 
        splitContainerRight.Panel1.Controls.Add(olvArticles);
        // 
        // splitContainerRight.Panel2
        // 
        splitContainerRight.Panel2.Controls.Add(panelDebug);
        splitContainerRight.Size = new Size(832, 600);
        splitContainerRight.SplitterDistance = 418;
        splitContainerRight.TabIndex = 0;
        // 
        // olvArticles
        // 
        olvArticles.AllColumns.Add(olvColTitle);
        olvArticles.AllColumns.Add(olvColSite);
        olvArticles.AllColumns.Add(olvColSiteLogo);
        olvArticles.AllColumns.Add(olvColDate);
        olvArticles.CellEditUseWholeCell = false;
        olvArticles.Columns.AddRange(new ColumnHeader[] { olvColTitle, olvColSite, olvColSiteLogo, olvColDate });
        olvArticles.ContextMenuStrip = contextMenuArticles;
        olvArticles.Dock = DockStyle.Fill;
        olvArticles.FullRowSelect = true;
        olvArticles.GridLines = true;
        olvArticles.Location = new Point(0, 0);
        olvArticles.Name = "olvArticles";
        olvArticles.ShowGroups = false;
        olvArticles.Size = new Size(832, 418);
        olvArticles.TabIndex = 0;
        olvArticles.UseCompatibleStateImageBehavior = false;
        olvArticles.View = View.Details;
        // 
        // olvColTitle
        // 
        olvColTitle.AspectName = "NewsTitle";
        olvColTitle.Text = "Title";
        olvColTitle.Width = 450;
        // 
        // olvColSite
        // 
        olvColSite.AspectName = "SiteName";
        olvColSite.Text = "Source";
        olvColSite.Width = 150;
        // 
        // olvColSiteLogo
        // 
        olvColSiteLogo.AspectName = "SiteLogo";
        olvColSiteLogo.Text = "Logo";
        olvColSiteLogo.Width = 150;
        // 
        // olvColDate
        // 
        olvColDate.AspectName = "CreatedAtDisplay";
        olvColDate.Text = "Date";
        olvColDate.Width = 130;
        // 
        // contextMenuArticles
        // 
        contextMenuArticles.Items.AddRange(new ToolStripItem[] { menuItemOpenUrl, menuItemCopyUrl, menuItemCopyTitle, menuItemSeparator1, menuItemViewBody, menuItemSeparator2, menuItemDelete });
        contextMenuArticles.Name = "contextMenuArticles";
        contextMenuArticles.Size = new Size(146, 126);
        // 
        // menuItemOpenUrl
        // 
        menuItemOpenUrl.Name = "menuItemOpenUrl";
        menuItemOpenUrl.Size = new Size(145, 22);
        menuItemOpenUrl.Text = "Open URL";
        // 
        // menuItemCopyUrl
        // 
        menuItemCopyUrl.Name = "menuItemCopyUrl";
        menuItemCopyUrl.Size = new Size(145, 22);
        menuItemCopyUrl.Text = "Copy URL";
        // 
        // menuItemCopyTitle
        // 
        menuItemCopyTitle.Name = "menuItemCopyTitle";
        menuItemCopyTitle.Size = new Size(145, 22);
        menuItemCopyTitle.Text = "Copy Title";
        // 
        // menuItemSeparator1
        // 
        menuItemSeparator1.Name = "menuItemSeparator1";
        menuItemSeparator1.Size = new Size(142, 6);
        // 
        // menuItemViewBody
        // 
        menuItemViewBody.Name = "menuItemViewBody";
        menuItemViewBody.Size = new Size(145, 22);
        menuItemViewBody.Text = "View Content";
        // 
        // menuItemSeparator2
        // 
        menuItemSeparator2.Name = "menuItemSeparator2";
        menuItemSeparator2.Size = new Size(142, 6);
        // 
        // menuItemDelete
        // 
        menuItemDelete.Name = "menuItemDelete";
        menuItemDelete.Size = new Size(145, 22);
        menuItemDelete.Text = "Delete";
        // 
        // panelDebug
        // 
        panelDebug.Controls.Add(splitContainerDebug);
        panelDebug.Dock = DockStyle.Fill;
        panelDebug.Location = new Point(0, 0);
        panelDebug.Name = "panelDebug";
        panelDebug.Size = new Size(832, 178);
        panelDebug.TabIndex = 0;
        // 
        // splitContainerDebug
        // 
        splitContainerDebug.Dock = DockStyle.Fill;
        splitContainerDebug.Location = new Point(0, 0);
        splitContainerDebug.Name = "splitContainerDebug";
        // 
        // splitContainerDebug.Panel1
        // 
        splitContainerDebug.Panel1.Controls.Add(panelDebugLeft);
        // 
        // splitContainerDebug.Panel2
        // 
        splitContainerDebug.Panel2.Controls.Add(panelDebugRight);
        splitContainerDebug.Size = new Size(832, 178);
        splitContainerDebug.SplitterDistance = 414;
        splitContainerDebug.TabIndex = 0;
        // 
        // panelDebugLeft
        // 
        panelDebugLeft.Controls.Add(txtDebugLog);
        panelDebugLeft.Controls.Add(lblDebugHeader);
        panelDebugLeft.Controls.Add(btnClearLog);
        panelDebugLeft.Dock = DockStyle.Fill;
        panelDebugLeft.Location = new Point(0, 0);
        panelDebugLeft.Name = "panelDebugLeft";
        panelDebugLeft.Size = new Size(414, 178);
        panelDebugLeft.TabIndex = 0;
        // 
        // txtDebugLog
        // 
        txtDebugLog.BackColor = Color.FromArgb(30, 30, 30);
        txtDebugLog.BorderStyle = BorderStyle.None;
        txtDebugLog.Dock = DockStyle.Fill;
        txtDebugLog.Font = new Font("Consolas", 9F);
        txtDebugLog.ForeColor = Color.LightGreen;
        txtDebugLog.Location = new Point(0, 22);
        txtDebugLog.Multiline = true;
        txtDebugLog.Name = "txtDebugLog";
        txtDebugLog.ReadOnly = true;
        txtDebugLog.ScrollBars = ScrollBars.Vertical;
        txtDebugLog.Size = new Size(414, 156);
        txtDebugLog.TabIndex = 2;
        // 
        // lblDebugHeader
        // 
        lblDebugHeader.BackColor = Color.FromArgb(45, 45, 48);
        lblDebugHeader.Dock = DockStyle.Top;
        lblDebugHeader.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDebugHeader.ForeColor = Color.White;
        lblDebugHeader.Location = new Point(0, 0);
        lblDebugHeader.Name = "lblDebugHeader";
        lblDebugHeader.Padding = new Padding(5, 3, 0, 0);
        lblDebugHeader.Size = new Size(414, 22);
        lblDebugHeader.TabIndex = 0;
        lblDebugHeader.Text = "Debug Log";
        // 
        // btnClearLog
        // 
        btnClearLog.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnClearLog.BackColor = Color.FromArgb(45, 45, 48);
        btnClearLog.FlatStyle = FlatStyle.Flat;
        btnClearLog.Font = new Font("Segoe UI", 8F);
        btnClearLog.ForeColor = Color.White;
        btnClearLog.Location = new Point(364, 0);
        btnClearLog.Name = "btnClearLog";
        btnClearLog.Size = new Size(50, 22);
        btnClearLog.TabIndex = 1;
        btnClearLog.Text = "Clear";
        btnClearLog.UseVisualStyleBackColor = false;
        // 
        // panelDebugRight
        // 
        panelDebugRight.Controls.Add(txtErrorLog);
        panelDebugRight.Controls.Add(lblErrorHeader);
        panelDebugRight.Controls.Add(btnClearErrorLog);
        panelDebugRight.Dock = DockStyle.Fill;
        panelDebugRight.Location = new Point(0, 0);
        panelDebugRight.Name = "panelDebugRight";
        panelDebugRight.Size = new Size(414, 178);
        panelDebugRight.TabIndex = 0;
        // 
        // txtErrorLog
        // 
        txtErrorLog.BackColor = Color.FromArgb(40, 20, 20);
        txtErrorLog.BorderStyle = BorderStyle.None;
        txtErrorLog.Dock = DockStyle.Fill;
        txtErrorLog.Font = new Font("Consolas", 9F);
        txtErrorLog.ForeColor = Color.OrangeRed;
        txtErrorLog.Location = new Point(0, 22);
        txtErrorLog.Multiline = true;
        txtErrorLog.Name = "txtErrorLog";
        txtErrorLog.ReadOnly = true;
        txtErrorLog.ScrollBars = ScrollBars.Vertical;
        txtErrorLog.Size = new Size(414, 156);
        txtErrorLog.TabIndex = 2;
        // 
        // lblErrorHeader
        // 
        lblErrorHeader.BackColor = Color.FromArgb(80, 40, 40);
        lblErrorHeader.Dock = DockStyle.Top;
        lblErrorHeader.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblErrorHeader.ForeColor = Color.White;
        lblErrorHeader.Location = new Point(0, 0);
        lblErrorHeader.Name = "lblErrorHeader";
        lblErrorHeader.Padding = new Padding(5, 3, 0, 0);
        lblErrorHeader.Size = new Size(414, 22);
        lblErrorHeader.TabIndex = 0;
        lblErrorHeader.Text = "Errors / Warnings";
        // 
        // btnClearErrorLog
        // 
        btnClearErrorLog.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnClearErrorLog.BackColor = Color.FromArgb(80, 40, 40);
        btnClearErrorLog.FlatStyle = FlatStyle.Flat;
        btnClearErrorLog.Font = new Font("Segoe UI", 8F);
        btnClearErrorLog.ForeColor = Color.White;
        btnClearErrorLog.Location = new Point(364, 0);
        btnClearErrorLog.Name = "btnClearErrorLog";
        btnClearErrorLog.Size = new Size(50, 22);
        btnClearErrorLog.TabIndex = 1;
        btnClearErrorLog.Text = "Clear";
        btnClearErrorLog.UseVisualStyleBackColor = false;
        // 
        // tabPageTiktok
        // 
        tabPageTiktok.Controls.Add(tableLayoutPanel1);
        tabPageTiktok.Location = new Point(4, 24);
        tabPageTiktok.Name = "tabPageTiktok";
        tabPageTiktok.Padding = new Padding(3);
        tabPageTiktok.Size = new Size(1125, 606);
        tabPageTiktok.TabIndex = 1;
        tabPageTiktok.Text = "Tiktok";
        tabPageTiktok.UseVisualStyleBackColor = true;
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 2;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 302F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 0);
        tableLayoutPanel1.Controls.Add(tableLayoutPanel5, 0, 0);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Location = new Point(3, 3);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 1;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.Size = new Size(1119, 600);
        tableLayoutPanel1.TabIndex = 8;
        // 
        // tableLayoutPanel2
        // 
        tableLayoutPanel2.ColumnCount = 1;
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel2.Controls.Add(olvTiktokData, 0, 1);
        tableLayoutPanel2.Controls.Add(tableLayoutPanel3, 0, 0);
        tableLayoutPanel2.Controls.Add(tableLayoutPanel4, 0, 2);
        tableLayoutPanel2.Dock = DockStyle.Fill;
        tableLayoutPanel2.Location = new Point(305, 3);
        tableLayoutPanel2.Name = "tableLayoutPanel2";
        tableLayoutPanel2.RowCount = 3;
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
        tableLayoutPanel2.Size = new Size(811, 594);
        tableLayoutPanel2.TabIndex = 0;
        // 
        // olvTiktokData
        // 
        olvTiktokData.AllColumns.Add(olvColDataId);
        olvTiktokData.AllColumns.Add(olvColDataUsername);
        olvTiktokData.AllColumns.Add(olvColDataFollowers);
        olvTiktokData.AllColumns.Add(olvColDataFollowersChange);
        olvTiktokData.AllColumns.Add(olvColDataHearts);
        olvTiktokData.AllColumns.Add(olvColDataHeartsChange);
        olvTiktokData.AllColumns.Add(olvColDataVideos);
        olvTiktokData.AllColumns.Add(olvColDataVideosChange);
        olvTiktokData.AllColumns.Add(olvColDataRecordedAt);
        olvTiktokData.CellEditUseWholeCell = false;
        olvTiktokData.Columns.AddRange(new ColumnHeader[] { olvColDataId, olvColDataUsername, olvColDataFollowers, olvColDataFollowersChange, olvColDataHearts, olvColDataHeartsChange, olvColDataVideos, olvColDataVideosChange, olvColDataRecordedAt });
        olvTiktokData.Dock = DockStyle.Fill;
        olvTiktokData.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        olvTiktokData.FullRowSelect = true;
        olvTiktokData.GridLines = true;
        olvTiktokData.Location = new Point(3, 38);
        olvTiktokData.Name = "olvTiktokData";
        olvTiktokData.ShowGroups = false;
        olvTiktokData.Size = new Size(805, 518);
        olvTiktokData.TabIndex = 0;
        olvTiktokData.UseCompatibleStateImageBehavior = false;
        olvTiktokData.View = View.Details;
        // 
        // olvColDataId
        // 
        olvColDataId.AspectName = "DataId";
        olvColDataId.Text = "ID";
        olvColDataId.Width = 50;
        // 
        // olvColDataUsername
        // 
        olvColDataUsername.AspectName = "Username";
        olvColDataUsername.Text = "Username";
        olvColDataUsername.Width = 200;
        // 
        // olvColDataFollowers
        // 
        olvColDataFollowers.AspectName = "FollowerCountDisplay";
        olvColDataFollowers.Text = "Followers";
        olvColDataFollowers.Width = 120;
        // 
        // olvColDataFollowersChange
        // 
        olvColDataFollowersChange.AspectName = "FollowersChangeDisplay";
        olvColDataFollowersChange.Text = "Change";
        olvColDataFollowersChange.Width = 120;
        // 
        // olvColDataHearts
        // 
        olvColDataHearts.AspectName = "HeartCountDisplay";
        olvColDataHearts.Text = "Hearts";
        olvColDataHearts.Width = 120;
        // 
        // olvColDataHeartsChange
        // 
        olvColDataHeartsChange.AspectName = "HeartsChangeDisplay";
        olvColDataHeartsChange.Text = "Change";
        olvColDataHeartsChange.Width = 120;
        // 
        // olvColDataVideos
        // 
        olvColDataVideos.AspectName = "VideoCount";
        olvColDataVideos.Text = "Videos";
        olvColDataVideos.Width = 120;
        // 
        // olvColDataVideosChange
        // 
        olvColDataVideosChange.AspectName = "VideosChangeDisplay";
        olvColDataVideosChange.Text = "Change";
        olvColDataVideosChange.Width = 120;
        // 
        // olvColDataRecordedAt
        // 
        olvColDataRecordedAt.AspectName = "RecordedAtDisplay";
        olvColDataRecordedAt.Text = "Recorded At";
        olvColDataRecordedAt.Width = 160;
        // 
        // tableLayoutPanel3
        // 
        tableLayoutPanel3.ColumnCount = 8;
        tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
        tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
        tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
        tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
        tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
        tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        tableLayoutPanel3.Controls.Add(lblFilterUsername, 0, 0);
        tableLayoutPanel3.Controls.Add(cboFilterUsername, 1, 0);
        tableLayoutPanel3.Controls.Add(lblFilterDateFrom, 2, 0);
        tableLayoutPanel3.Controls.Add(dtpFilterFrom, 3, 0);
        tableLayoutPanel3.Controls.Add(lblFilterDateTo, 4, 0);
        tableLayoutPanel3.Controls.Add(dtpFilterTo, 5, 0);
        tableLayoutPanel3.Controls.Add(btnClearFilter, 7, 0);
        tableLayoutPanel3.Controls.Add(btnApplyFilter, 6, 0);
        tableLayoutPanel3.Dock = DockStyle.Fill;
        tableLayoutPanel3.Location = new Point(3, 3);
        tableLayoutPanel3.Name = "tableLayoutPanel3";
        tableLayoutPanel3.RowCount = 1;
        tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanel3.Size = new Size(805, 29);
        tableLayoutPanel3.TabIndex = 1;
        // 
        // lblFilterUsername
        // 
        lblFilterUsername.AutoSize = true;
        lblFilterUsername.Dock = DockStyle.Fill;
        lblFilterUsername.Location = new Point(3, 0);
        lblFilterUsername.Name = "lblFilterUsername";
        lblFilterUsername.Size = new Size(4, 29);
        lblFilterUsername.TabIndex = 0;
        lblFilterUsername.Text = "User:";
        lblFilterUsername.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cboFilterUsername
        // 
        cboFilterUsername.Dock = DockStyle.Fill;
        cboFilterUsername.DropDownStyle = ComboBoxStyle.DropDownList;
        cboFilterUsername.Location = new Point(13, 3);
        cboFilterUsername.Name = "cboFilterUsername";
        cboFilterUsername.Size = new Size(139, 23);
        cboFilterUsername.TabIndex = 1;
        // 
        // lblFilterDateFrom
        // 
        lblFilterDateFrom.AutoSize = true;
        lblFilterDateFrom.Dock = DockStyle.Fill;
        lblFilterDateFrom.Location = new Point(158, 0);
        lblFilterDateFrom.Name = "lblFilterDateFrom";
        lblFilterDateFrom.Size = new Size(54, 29);
        lblFilterDateFrom.TabIndex = 2;
        lblFilterDateFrom.Text = "From:";
        lblFilterDateFrom.TextAlign = ContentAlignment.MiddleRight;
        // 
        // dtpFilterFrom
        // 
        dtpFilterFrom.CustomFormat = "yyyy-MM-dd HH:mm";
        dtpFilterFrom.Dock = DockStyle.Fill;
        dtpFilterFrom.Format = DateTimePickerFormat.Custom;
        dtpFilterFrom.Location = new Point(218, 3);
        dtpFilterFrom.Name = "dtpFilterFrom";
        dtpFilterFrom.Size = new Size(139, 23);
        dtpFilterFrom.TabIndex = 3;
        // 
        // lblFilterDateTo
        // 
        lblFilterDateTo.AutoSize = true;
        lblFilterDateTo.Dock = DockStyle.Fill;
        lblFilterDateTo.Location = new Point(363, 0);
        lblFilterDateTo.Name = "lblFilterDateTo";
        lblFilterDateTo.Size = new Size(54, 29);
        lblFilterDateTo.TabIndex = 4;
        lblFilterDateTo.Text = "To:";
        lblFilterDateTo.TextAlign = ContentAlignment.MiddleRight;
        // 
        // dtpFilterTo
        // 
        dtpFilterTo.CustomFormat = "yyyy-MM-dd HH:mm";
        dtpFilterTo.Dock = DockStyle.Fill;
        dtpFilterTo.Format = DateTimePickerFormat.Custom;
        dtpFilterTo.Location = new Point(423, 3);
        dtpFilterTo.Name = "dtpFilterTo";
        dtpFilterTo.Size = new Size(139, 23);
        dtpFilterTo.TabIndex = 5;
        // 
        // btnClearFilter
        // 
        btnClearFilter.Dock = DockStyle.Fill;
        btnClearFilter.Location = new Point(685, 0);
        btnClearFilter.Margin = new Padding(0);
        btnClearFilter.Name = "btnClearFilter";
        btnClearFilter.Size = new Size(120, 29);
        btnClearFilter.TabIndex = 7;
        btnClearFilter.Text = "Clear";
        btnClearFilter.UseVisualStyleBackColor = true;
        // 
        // btnApplyFilter
        // 
        btnApplyFilter.Dock = DockStyle.Fill;
        btnApplyFilter.Location = new Point(565, 0);
        btnApplyFilter.Margin = new Padding(0);
        btnApplyFilter.Name = "btnApplyFilter";
        btnApplyFilter.Size = new Size(120, 29);
        btnApplyFilter.TabIndex = 6;
        btnApplyFilter.Text = "Filter";
        btnApplyFilter.UseVisualStyleBackColor = true;
        // 
        // tableLayoutPanel4
        // 
        tableLayoutPanel4.ColumnCount = 3;
        tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
        tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel4.Controls.Add(lblTkStatus, 0, 0);
        tableLayoutPanel4.Controls.Add(lblTkInfo, 1, 0);
        tableLayoutPanel4.Controls.Add(progressBarTk, 2, 0);
        tableLayoutPanel4.Dock = DockStyle.Fill;
        tableLayoutPanel4.Location = new Point(3, 562);
        tableLayoutPanel4.Name = "tableLayoutPanel4";
        tableLayoutPanel4.RowCount = 1;
        tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanel4.Size = new Size(805, 29);
        tableLayoutPanel4.TabIndex = 2;
        // 
        // lblTkStatus
        // 
        lblTkStatus.AutoSize = true;
        lblTkStatus.Dock = DockStyle.Fill;
        lblTkStatus.Location = new Point(3, 0);
        lblTkStatus.Name = "lblTkStatus";
        lblTkStatus.Size = new Size(296, 29);
        lblTkStatus.TabIndex = 0;
        lblTkStatus.Text = "Ready";
        lblTkStatus.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblTkInfo
        // 
        lblTkInfo.AutoSize = true;
        lblTkInfo.Dock = DockStyle.Fill;
        lblTkInfo.Location = new Point(305, 0);
        lblTkInfo.Name = "lblTkInfo";
        lblTkInfo.Size = new Size(194, 29);
        lblTkInfo.TabIndex = 2;
        lblTkInfo.Text = "Id:0, Sch:0, Data:0";
        lblTkInfo.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // progressBarTk
        // 
        progressBarTk.Dock = DockStyle.Fill;
        progressBarTk.Location = new Point(502, 0);
        progressBarTk.Margin = new Padding(0);
        progressBarTk.Name = "progressBarTk";
        progressBarTk.Size = new Size(303, 29);
        progressBarTk.TabIndex = 1;
        progressBarTk.Visible = false;
        // 
        // tableLayoutPanel5
        // 
        tableLayoutPanel5.ColumnCount = 1;
        tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel5.Controls.Add(olvTiktokID, 0, 0);
        tableLayoutPanel5.Controls.Add(lblTkNextSchedule, 0, 6);
        tableLayoutPanel5.Controls.Add(btnTkStartStop, 0, 5);
        tableLayoutPanel5.Controls.Add(olvTiktokSchedule, 0, 2);
        tableLayoutPanel5.Controls.Add(tableLayoutPanel6, 0, 1);
        tableLayoutPanel5.Controls.Add(tableLayoutPanel7, 0, 3);
        tableLayoutPanel5.Controls.Add(tableLayoutPanel8, 0, 4);
        tableLayoutPanel5.Dock = DockStyle.Fill;
        tableLayoutPanel5.Location = new Point(3, 3);
        tableLayoutPanel5.Name = "tableLayoutPanel5";
        tableLayoutPanel5.RowCount = 7;
        tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 75F));
        tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
        tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
        tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
        tableLayoutPanel5.Size = new Size(296, 594);
        tableLayoutPanel5.TabIndex = 1;
        // 
        // olvTiktokID
        // 
        olvTiktokID.AllColumns.Add(olvColTkUsername);
        olvTiktokID.AllColumns.Add(olvColTkNickname);
        olvTiktokID.AllColumns.Add(olvColTkStatus);
        olvTiktokID.CellEditUseWholeCell = false;
        olvTiktokID.Columns.AddRange(new ColumnHeader[] { olvColTkUsername, olvColTkNickname, olvColTkStatus });
        olvTiktokID.ContextMenuStrip = contextMenuTiktokID;
        olvTiktokID.Dock = DockStyle.Fill;
        olvTiktokID.FullRowSelect = true;
        olvTiktokID.GridLines = true;
        olvTiktokID.Location = new Point(3, 3);
        olvTiktokID.Name = "olvTiktokID";
        olvTiktokID.ShowGroups = false;
        olvTiktokID.Size = new Size(290, 297);
        olvTiktokID.TabIndex = 1;
        olvTiktokID.UseCompatibleStateImageBehavior = false;
        olvTiktokID.View = View.Details;
        // 
        // olvColTkUsername
        // 
        olvColTkUsername.AspectName = "Username";
        olvColTkUsername.Text = "Username";
        olvColTkUsername.Width = 120;
        // 
        // olvColTkNickname
        // 
        olvColTkNickname.AspectName = "Nickname";
        olvColTkNickname.Text = "Nickname";
        olvColTkNickname.Width = 120;
        // 
        // olvColTkStatus
        // 
        olvColTkStatus.AspectName = "Status";
        olvColTkStatus.CheckBoxes = true;
        olvColTkStatus.HeaderTextAlign = HorizontalAlignment.Center;
        olvColTkStatus.Text = "Active";
        olvColTkStatus.TextAlign = HorizontalAlignment.Center;
        olvColTkStatus.Width = 50;
        // 
        // contextMenuTiktokID
        // 
        contextMenuTiktokID.Items.AddRange(new ToolStripItem[] { menuItemTkAddId, menuItemTkEditId, menuItemTkDeleteId, menuItemTkSeparator1, menuItemTkRefresh, menuItemTkFetch });
        contextMenuTiktokID.Name = "contextMenuTiktokID";
        contextMenuTiktokID.Size = new Size(151, 120);
        // 
        // menuItemTkAddId
        // 
        menuItemTkAddId.Name = "menuItemTkAddId";
        menuItemTkAddId.Size = new Size(150, 22);
        menuItemTkAddId.Text = "Add ID";
        // 
        // menuItemTkEditId
        // 
        menuItemTkEditId.Name = "menuItemTkEditId";
        menuItemTkEditId.Size = new Size(150, 22);
        menuItemTkEditId.Text = "Edit ID";
        // 
        // menuItemTkDeleteId
        // 
        menuItemTkDeleteId.Name = "menuItemTkDeleteId";
        menuItemTkDeleteId.Size = new Size(150, 22);
        menuItemTkDeleteId.Text = "Delete ID";
        // 
        // menuItemTkSeparator1
        // 
        menuItemTkSeparator1.Name = "menuItemTkSeparator1";
        menuItemTkSeparator1.Size = new Size(147, 6);
        // 
        // menuItemTkRefresh
        // 
        menuItemTkRefresh.Name = "menuItemTkRefresh";
        menuItemTkRefresh.Size = new Size(150, 22);
        menuItemTkRefresh.Text = "Refresh";
        // 
        // menuItemTkFetch
        // 
        menuItemTkFetch.Name = "menuItemTkFetch";
        menuItemTkFetch.Size = new Size(150, 22);
        menuItemTkFetch.Text = "Fetch Selected";
        // 
        // lblTkNextSchedule
        // 
        lblTkNextSchedule.AutoSize = true;
        lblTkNextSchedule.Dock = DockStyle.Fill;
        lblTkNextSchedule.ForeColor = Color.OliveDrab;
        lblTkNextSchedule.Location = new Point(3, 567);
        lblTkNextSchedule.Name = "lblTkNextSchedule";
        lblTkNextSchedule.Size = new Size(290, 27);
        lblTkNextSchedule.TabIndex = 7;
        lblTkNextSchedule.Text = "Next: --:-- (--:--)";
        lblTkNextSchedule.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // btnTkStartStop
        // 
        btnTkStartStop.BackColor = Color.FromArgb(0, 120, 0);
        btnTkStartStop.Dock = DockStyle.Fill;
        btnTkStartStop.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnTkStartStop.ForeColor = Color.White;
        btnTkStartStop.Location = new Point(0, 512);
        btnTkStartStop.Margin = new Padding(0);
        btnTkStartStop.Name = "btnTkStartStop";
        btnTkStartStop.Size = new Size(296, 55);
        btnTkStartStop.TabIndex = 5;
        btnTkStartStop.Text = "Start";
        btnTkStartStop.UseVisualStyleBackColor = false;
        // 
        // olvTiktokSchedule
        // 
        olvTiktokSchedule.AllColumns.Add(olvColScheduleSL);
        olvTiktokSchedule.AllColumns.Add(olvColScheduleTiming);
        olvTiktokSchedule.AllColumns.Add(olvColScheduleStatus);
        olvTiktokSchedule.CellEditUseWholeCell = false;
        olvTiktokSchedule.Columns.AddRange(new ColumnHeader[] { olvColScheduleSL, olvColScheduleTiming, olvColScheduleStatus });
        olvTiktokSchedule.Dock = DockStyle.Fill;
        olvTiktokSchedule.FullRowSelect = true;
        olvTiktokSchedule.GridLines = true;
        olvTiktokSchedule.Location = new Point(3, 344);
        olvTiktokSchedule.Name = "olvTiktokSchedule";
        olvTiktokSchedule.ShowGroups = false;
        olvTiktokSchedule.Size = new Size(290, 95);
        olvTiktokSchedule.TabIndex = 3;
        olvTiktokSchedule.UseCompatibleStateImageBehavior = false;
        olvTiktokSchedule.View = View.Details;
        // 
        // olvColScheduleSL
        // 
        olvColScheduleSL.AspectName = "SerialNumber";
        olvColScheduleSL.Text = "SL";
        olvColScheduleSL.Width = 40;
        // 
        // olvColScheduleTiming
        // 
        olvColScheduleTiming.AspectName = "TimingDisplay";
        olvColScheduleTiming.Text = "Timing";
        olvColScheduleTiming.Width = 100;
        // 
        // olvColScheduleStatus
        // 
        olvColScheduleStatus.AspectName = "IsActive";
        olvColScheduleStatus.CheckBoxes = true;
        olvColScheduleStatus.Text = "Active";
        // 
        // tableLayoutPanel6
        // 
        tableLayoutPanel6.ColumnCount = 3;
        tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 104F));
        tableLayoutPanel6.Controls.Add(btnTkRefreshId, 2, 0);
        tableLayoutPanel6.Controls.Add(btnTkDeleteId, 1, 0);
        tableLayoutPanel6.Controls.Add(btnTkAddId, 0, 0);
        tableLayoutPanel6.Dock = DockStyle.Fill;
        tableLayoutPanel6.Location = new Point(3, 306);
        tableLayoutPanel6.Name = "tableLayoutPanel6";
        tableLayoutPanel6.RowCount = 1;
        tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel6.Size = new Size(290, 32);
        tableLayoutPanel6.TabIndex = 2;
        // 
        // btnTkRefreshId
        // 
        btnTkRefreshId.Dock = DockStyle.Fill;
        btnTkRefreshId.Location = new Point(186, 0);
        btnTkRefreshId.Margin = new Padding(0);
        btnTkRefreshId.Name = "btnTkRefreshId";
        btnTkRefreshId.Size = new Size(104, 32);
        btnTkRefreshId.TabIndex = 4;
        btnTkRefreshId.Text = "Refresh";
        btnTkRefreshId.UseVisualStyleBackColor = true;
        // 
        // btnTkDeleteId
        // 
        btnTkDeleteId.Dock = DockStyle.Fill;
        btnTkDeleteId.Location = new Point(93, 0);
        btnTkDeleteId.Margin = new Padding(0);
        btnTkDeleteId.Name = "btnTkDeleteId";
        btnTkDeleteId.Size = new Size(93, 32);
        btnTkDeleteId.TabIndex = 3;
        btnTkDeleteId.Text = "Delete ID";
        btnTkDeleteId.UseVisualStyleBackColor = true;
        // 
        // btnTkAddId
        // 
        btnTkAddId.Dock = DockStyle.Fill;
        btnTkAddId.Location = new Point(0, 0);
        btnTkAddId.Margin = new Padding(0);
        btnTkAddId.Name = "btnTkAddId";
        btnTkAddId.Size = new Size(93, 32);
        btnTkAddId.TabIndex = 2;
        btnTkAddId.Text = "Add ID";
        btnTkAddId.UseVisualStyleBackColor = true;
        // 
        // tableLayoutPanel7
        // 
        tableLayoutPanel7.ColumnCount = 4;
        tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tableLayoutPanel7.Controls.Add(btnTkAddSchedule, 0, 0);
        tableLayoutPanel7.Controls.Add(btnTkSaveSettings, 3, 0);
        tableLayoutPanel7.Controls.Add(btnTkDeleteSchedule, 2, 0);
        tableLayoutPanel7.Controls.Add(btnTkEditSchedule, 1, 0);
        tableLayoutPanel7.Dock = DockStyle.Fill;
        tableLayoutPanel7.Location = new Point(3, 445);
        tableLayoutPanel7.Name = "tableLayoutPanel7";
        tableLayoutPanel7.RowCount = 1;
        tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanel7.Size = new Size(290, 32);
        tableLayoutPanel7.TabIndex = 4;
        // 
        // btnTkAddSchedule
        // 
        btnTkAddSchedule.Dock = DockStyle.Fill;
        btnTkAddSchedule.Location = new Point(0, 0);
        btnTkAddSchedule.Margin = new Padding(0);
        btnTkAddSchedule.Name = "btnTkAddSchedule";
        btnTkAddSchedule.Size = new Size(72, 32);
        btnTkAddSchedule.TabIndex = 0;
        btnTkAddSchedule.Text = "Add";
        btnTkAddSchedule.UseVisualStyleBackColor = true;
        // 
        // btnTkSaveSettings
        // 
        btnTkSaveSettings.Dock = DockStyle.Fill;
        btnTkSaveSettings.Location = new Point(216, 0);
        btnTkSaveSettings.Margin = new Padding(0);
        btnTkSaveSettings.Name = "btnTkSaveSettings";
        btnTkSaveSettings.Size = new Size(74, 32);
        btnTkSaveSettings.TabIndex = 6;
        btnTkSaveSettings.Text = "Save";
        btnTkSaveSettings.UseVisualStyleBackColor = true;
        // 
        // btnTkDeleteSchedule
        // 
        btnTkDeleteSchedule.Dock = DockStyle.Fill;
        btnTkDeleteSchedule.Location = new Point(144, 0);
        btnTkDeleteSchedule.Margin = new Padding(0);
        btnTkDeleteSchedule.Name = "btnTkDeleteSchedule";
        btnTkDeleteSchedule.Size = new Size(72, 32);
        btnTkDeleteSchedule.TabIndex = 2;
        btnTkDeleteSchedule.Text = "Delete";
        btnTkDeleteSchedule.UseVisualStyleBackColor = true;
        // 
        // btnTkEditSchedule
        // 
        btnTkEditSchedule.Dock = DockStyle.Fill;
        btnTkEditSchedule.Location = new Point(72, 0);
        btnTkEditSchedule.Margin = new Padding(0);
        btnTkEditSchedule.Name = "btnTkEditSchedule";
        btnTkEditSchedule.Size = new Size(72, 32);
        btnTkEditSchedule.TabIndex = 1;
        btnTkEditSchedule.Text = "Edit";
        btnTkEditSchedule.UseVisualStyleBackColor = true;
        // 
        // tableLayoutPanel8
        // 
        tableLayoutPanel8.ColumnCount = 2;
        tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 74.13793F));
        tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25.8620682F));
        tableLayoutPanel8.Controls.Add(numTkFrequency, 1, 0);
        tableLayoutPanel8.Controls.Add(lblTkFrequency, 0, 0);
        tableLayoutPanel8.Dock = DockStyle.Fill;
        tableLayoutPanel8.Location = new Point(3, 483);
        tableLayoutPanel8.Name = "tableLayoutPanel8";
        tableLayoutPanel8.RowCount = 1;
        tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanel8.Size = new Size(290, 26);
        tableLayoutPanel8.TabIndex = 5;
        // 
        // numTkFrequency
        // 
        numTkFrequency.Dock = DockStyle.Fill;
        numTkFrequency.Location = new Point(218, 3);
        numTkFrequency.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
        numTkFrequency.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numTkFrequency.Name = "numTkFrequency";
        numTkFrequency.Size = new Size(69, 23);
        numTkFrequency.TabIndex = 4;
        numTkFrequency.Value = new decimal(new int[] { 10, 0, 0, 0 });
        // 
        // lblTkFrequency
        // 
        lblTkFrequency.AutoSize = true;
        lblTkFrequency.Dock = DockStyle.Fill;
        lblTkFrequency.Location = new Point(3, 0);
        lblTkFrequency.Name = "lblTkFrequency";
        lblTkFrequency.Size = new Size(209, 26);
        lblTkFrequency.TabIndex = 3;
        lblTkFrequency.Text = "Delay between IDs (sec):";
        lblTkFrequency.TextAlign = ContentAlignment.MiddleRight;
        // 
        // tabPageFacebook
        // 
        tabPageFacebook.Controls.Add(tableLayoutPanelFb1);
        tabPageFacebook.Location = new Point(4, 24);
        tabPageFacebook.Name = "tabPageFacebook";
        tabPageFacebook.Padding = new Padding(3);
        tabPageFacebook.Size = new Size(1125, 606);
        tabPageFacebook.TabIndex = 2;
        tabPageFacebook.Text = "Facebook";
        tabPageFacebook.UseVisualStyleBackColor = true;
        // 
        // tableLayoutPanelFb1
        // 
        tableLayoutPanelFb1.ColumnCount = 2;
        tableLayoutPanelFb1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 302F));
        tableLayoutPanelFb1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanelFb1.Controls.Add(tableLayoutPanelFb2, 1, 0);
        tableLayoutPanelFb1.Controls.Add(tableLayoutPanelFb5, 0, 0);
        tableLayoutPanelFb1.Dock = DockStyle.Fill;
        tableLayoutPanelFb1.Location = new Point(3, 3);
        tableLayoutPanelFb1.Name = "tableLayoutPanelFb1";
        tableLayoutPanelFb1.RowCount = 1;
        tableLayoutPanelFb1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanelFb1.Size = new Size(1119, 600);
        tableLayoutPanelFb1.TabIndex = 0;
        // 
        // tableLayoutPanelFb2
        // 
        tableLayoutPanelFb2.ColumnCount = 1;
        tableLayoutPanelFb2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanelFb2.Controls.Add(olvFacebookData, 0, 1);
        tableLayoutPanelFb2.Controls.Add(tableLayoutPanelFb3, 0, 0);
        tableLayoutPanelFb2.Controls.Add(tableLayoutPanelFb4, 0, 2);
        tableLayoutPanelFb2.Dock = DockStyle.Fill;
        tableLayoutPanelFb2.Location = new Point(305, 3);
        tableLayoutPanelFb2.Name = "tableLayoutPanelFb2";
        tableLayoutPanelFb2.RowCount = 3;
        tableLayoutPanelFb2.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
        tableLayoutPanelFb2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanelFb2.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
        tableLayoutPanelFb2.Size = new Size(811, 594);
        tableLayoutPanelFb2.TabIndex = 0;
        // 
        // olvFacebookData
        // 
        olvFacebookData.AllColumns.Add(olvColFbDataId);
        olvFacebookData.AllColumns.Add(olvColFbDataUsername);
        olvFacebookData.AllColumns.Add(olvColFbDataFollowers);
        olvFacebookData.AllColumns.Add(olvColFbFollowersChange);
        olvFacebookData.AllColumns.Add(olvColFbDataTalkingAbout);
        olvFacebookData.AllColumns.Add(olvColFbTalkingAboutChange);
        olvFacebookData.AllColumns.Add(olvColFbDataRecordedAt);
        olvFacebookData.CellEditUseWholeCell = false;
        olvFacebookData.Columns.AddRange(new ColumnHeader[] { olvColFbDataId, olvColFbDataUsername, olvColFbDataFollowers, olvColFbFollowersChange, olvColFbDataTalkingAbout, olvColFbTalkingAboutChange, olvColFbDataRecordedAt });
        olvFacebookData.Dock = DockStyle.Fill;
        olvFacebookData.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
        olvFacebookData.FullRowSelect = true;
        olvFacebookData.GridLines = true;
        olvFacebookData.Location = new Point(3, 38);
        olvFacebookData.Name = "olvFacebookData";
        olvFacebookData.ShowGroups = false;
        olvFacebookData.Size = new Size(805, 518);
        olvFacebookData.TabIndex = 0;
        olvFacebookData.UseCompatibleStateImageBehavior = false;
        olvFacebookData.View = View.Details;
        // 
        // olvColFbDataId
        // 
        olvColFbDataId.AspectName = "DataId";
        olvColFbDataId.Text = "ID";
        olvColFbDataId.Width = 50;
        // 
        // olvColFbDataUsername
        // 
        olvColFbDataUsername.AspectName = "Username";
        olvColFbDataUsername.Text = "Username";
        olvColFbDataUsername.Width = 200;
        // 
        // olvColFbDataFollowers
        // 
        olvColFbDataFollowers.AspectName = "FollowersCountDisplay";
        olvColFbDataFollowers.Text = "Followers";
        olvColFbDataFollowers.Width = 120;
        // 
        // olvColFbFollowersChange
        // 
        olvColFbFollowersChange.AspectName = "FollowersChangeDisplay";
        olvColFbFollowersChange.Text = "Change";
        olvColFbFollowersChange.Width = 100;
        // 
        // olvColFbDataTalkingAbout
        // 
        olvColFbDataTalkingAbout.AspectName = "TalkingAboutDisplay";
        olvColFbDataTalkingAbout.Text = "Talking About";
        olvColFbDataTalkingAbout.Width = 120;
        // 
        // olvColFbTalkingAboutChange
        // 
        olvColFbTalkingAboutChange.AspectName = "TalkingAboutChangeDisplay";
        olvColFbTalkingAboutChange.Text = "Change";
        olvColFbTalkingAboutChange.Width = 100;
        // 
        // olvColFbDataRecordedAt
        // 
        olvColFbDataRecordedAt.AspectName = "RecordedAtDisplay";
        olvColFbDataRecordedAt.Text = "Recorded At";
        olvColFbDataRecordedAt.Width = 160;
        // 
        // tableLayoutPanelFb3
        // 
        tableLayoutPanelFb3.ColumnCount = 8;
        tableLayoutPanelFb3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanelFb3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
        tableLayoutPanelFb3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
        tableLayoutPanelFb3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
        tableLayoutPanelFb3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
        tableLayoutPanelFb3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
        tableLayoutPanelFb3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        tableLayoutPanelFb3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        tableLayoutPanelFb3.Controls.Add(lblFbFilterUsername, 0, 0);
        tableLayoutPanelFb3.Controls.Add(cboFbFilterUsername, 1, 0);
        tableLayoutPanelFb3.Controls.Add(lblFbFilterDateFrom, 2, 0);
        tableLayoutPanelFb3.Controls.Add(dtpFbFilterFrom, 3, 0);
        tableLayoutPanelFb3.Controls.Add(lblFbFilterDateTo, 4, 0);
        tableLayoutPanelFb3.Controls.Add(dtpFbFilterTo, 5, 0);
        tableLayoutPanelFb3.Controls.Add(btnFbClearFilter, 7, 0);
        tableLayoutPanelFb3.Controls.Add(btnFbApplyFilter, 6, 0);
        tableLayoutPanelFb3.Dock = DockStyle.Fill;
        tableLayoutPanelFb3.Location = new Point(3, 3);
        tableLayoutPanelFb3.Name = "tableLayoutPanelFb3";
        tableLayoutPanelFb3.RowCount = 1;
        tableLayoutPanelFb3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanelFb3.Size = new Size(805, 29);
        tableLayoutPanelFb3.TabIndex = 1;
        // 
        // lblFbFilterUsername
        // 
        lblFbFilterUsername.AutoSize = true;
        lblFbFilterUsername.Dock = DockStyle.Fill;
        lblFbFilterUsername.Location = new Point(3, 0);
        lblFbFilterUsername.Name = "lblFbFilterUsername";
        lblFbFilterUsername.Size = new Size(4, 29);
        lblFbFilterUsername.TabIndex = 0;
        lblFbFilterUsername.Text = "User:";
        lblFbFilterUsername.TextAlign = ContentAlignment.MiddleRight;
        // 
        // cboFbFilterUsername
        // 
        cboFbFilterUsername.Dock = DockStyle.Fill;
        cboFbFilterUsername.DropDownStyle = ComboBoxStyle.DropDownList;
        cboFbFilterUsername.Location = new Point(13, 3);
        cboFbFilterUsername.Name = "cboFbFilterUsername";
        cboFbFilterUsername.Size = new Size(139, 23);
        cboFbFilterUsername.TabIndex = 1;
        // 
        // lblFbFilterDateFrom
        // 
        lblFbFilterDateFrom.AutoSize = true;
        lblFbFilterDateFrom.Dock = DockStyle.Fill;
        lblFbFilterDateFrom.Location = new Point(158, 0);
        lblFbFilterDateFrom.Name = "lblFbFilterDateFrom";
        lblFbFilterDateFrom.Size = new Size(54, 29);
        lblFbFilterDateFrom.TabIndex = 2;
        lblFbFilterDateFrom.Text = "From:";
        lblFbFilterDateFrom.TextAlign = ContentAlignment.MiddleRight;
        // 
        // dtpFbFilterFrom
        // 
        dtpFbFilterFrom.CustomFormat = "yyyy-MM-dd HH:mm";
        dtpFbFilterFrom.Dock = DockStyle.Fill;
        dtpFbFilterFrom.Format = DateTimePickerFormat.Custom;
        dtpFbFilterFrom.Location = new Point(218, 3);
        dtpFbFilterFrom.Name = "dtpFbFilterFrom";
        dtpFbFilterFrom.Size = new Size(139, 23);
        dtpFbFilterFrom.TabIndex = 3;
        // 
        // lblFbFilterDateTo
        // 
        lblFbFilterDateTo.AutoSize = true;
        lblFbFilterDateTo.Dock = DockStyle.Fill;
        lblFbFilterDateTo.Location = new Point(363, 0);
        lblFbFilterDateTo.Name = "lblFbFilterDateTo";
        lblFbFilterDateTo.Size = new Size(54, 29);
        lblFbFilterDateTo.TabIndex = 4;
        lblFbFilterDateTo.Text = "To:";
        lblFbFilterDateTo.TextAlign = ContentAlignment.MiddleRight;
        // 
        // dtpFbFilterTo
        // 
        dtpFbFilterTo.CustomFormat = "yyyy-MM-dd HH:mm";
        dtpFbFilterTo.Dock = DockStyle.Fill;
        dtpFbFilterTo.Format = DateTimePickerFormat.Custom;
        dtpFbFilterTo.Location = new Point(423, 3);
        dtpFbFilterTo.Name = "dtpFbFilterTo";
        dtpFbFilterTo.Size = new Size(139, 23);
        dtpFbFilterTo.TabIndex = 5;
        // 
        // btnFbClearFilter
        // 
        btnFbClearFilter.Dock = DockStyle.Fill;
        btnFbClearFilter.Location = new Point(685, 0);
        btnFbClearFilter.Margin = new Padding(0);
        btnFbClearFilter.Name = "btnFbClearFilter";
        btnFbClearFilter.Size = new Size(120, 29);
        btnFbClearFilter.TabIndex = 7;
        btnFbClearFilter.Text = "Clear";
        btnFbClearFilter.UseVisualStyleBackColor = true;
        // 
        // btnFbApplyFilter
        // 
        btnFbApplyFilter.Dock = DockStyle.Fill;
        btnFbApplyFilter.Location = new Point(565, 0);
        btnFbApplyFilter.Margin = new Padding(0);
        btnFbApplyFilter.Name = "btnFbApplyFilter";
        btnFbApplyFilter.Size = new Size(120, 29);
        btnFbApplyFilter.TabIndex = 6;
        btnFbApplyFilter.Text = "Filter";
        btnFbApplyFilter.UseVisualStyleBackColor = true;
        // 
        // tableLayoutPanelFb4
        // 
        tableLayoutPanelFb4.ColumnCount = 3;
        tableLayoutPanelFb4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanelFb4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
        tableLayoutPanelFb4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanelFb4.Controls.Add(lblFbStatus, 0, 0);
        tableLayoutPanelFb4.Controls.Add(lblFbInfo, 1, 0);
        tableLayoutPanelFb4.Controls.Add(progressBarFb, 2, 0);
        tableLayoutPanelFb4.Dock = DockStyle.Fill;
        tableLayoutPanelFb4.Location = new Point(3, 562);
        tableLayoutPanelFb4.Name = "tableLayoutPanelFb4";
        tableLayoutPanelFb4.RowCount = 1;
        tableLayoutPanelFb4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanelFb4.Size = new Size(805, 29);
        tableLayoutPanelFb4.TabIndex = 2;
        // 
        // lblFbStatus
        // 
        lblFbStatus.AutoSize = true;
        lblFbStatus.Dock = DockStyle.Fill;
        lblFbStatus.Location = new Point(3, 0);
        lblFbStatus.Name = "lblFbStatus";
        lblFbStatus.Size = new Size(296, 29);
        lblFbStatus.TabIndex = 0;
        lblFbStatus.Text = "Ready";
        lblFbStatus.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblFbInfo
        // 
        lblFbInfo.AutoSize = true;
        lblFbInfo.Dock = DockStyle.Fill;
        lblFbInfo.Location = new Point(305, 0);
        lblFbInfo.Name = "lblFbInfo";
        lblFbInfo.Size = new Size(194, 29);
        lblFbInfo.TabIndex = 2;
        lblFbInfo.Text = "Id:0, Sch:0, Data:0";
        lblFbInfo.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // progressBarFb
        // 
        progressBarFb.Dock = DockStyle.Fill;
        progressBarFb.Location = new Point(502, 0);
        progressBarFb.Margin = new Padding(0);
        progressBarFb.Name = "progressBarFb";
        progressBarFb.Size = new Size(303, 29);
        progressBarFb.TabIndex = 1;
        progressBarFb.Visible = false;
        // 
        // tableLayoutPanelFb5
        // 
        tableLayoutPanelFb5.ColumnCount = 1;
        tableLayoutPanelFb5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanelFb5.Controls.Add(olvFacebookID, 0, 0);
        tableLayoutPanelFb5.Controls.Add(lblFbNextSchedule, 0, 6);
        tableLayoutPanelFb5.Controls.Add(btnFbStartStop, 0, 5);
        tableLayoutPanelFb5.Controls.Add(olvFacebookSchedule, 0, 2);
        tableLayoutPanelFb5.Controls.Add(tableLayoutPanelFb6, 0, 1);
        tableLayoutPanelFb5.Controls.Add(tableLayoutPanelFb7, 0, 3);
        tableLayoutPanelFb5.Controls.Add(tableLayoutPanelFb8, 0, 4);
        tableLayoutPanelFb5.Dock = DockStyle.Fill;
        tableLayoutPanelFb5.Location = new Point(3, 3);
        tableLayoutPanelFb5.Name = "tableLayoutPanelFb5";
        tableLayoutPanelFb5.RowCount = 7;
        tableLayoutPanelFb5.RowStyles.Add(new RowStyle(SizeType.Percent, 75F));
        tableLayoutPanelFb5.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        tableLayoutPanelFb5.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
        tableLayoutPanelFb5.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        tableLayoutPanelFb5.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
        tableLayoutPanelFb5.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
        tableLayoutPanelFb5.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
        tableLayoutPanelFb5.Size = new Size(296, 594);
        tableLayoutPanelFb5.TabIndex = 1;
        // 
        // olvFacebookID
        // 
        olvFacebookID.AllColumns.Add(olvColFbUsername);
        olvFacebookID.AllColumns.Add(olvColFbNickname);
        olvFacebookID.AllColumns.Add(olvColFbStatus);
        olvFacebookID.CellEditUseWholeCell = false;
        olvFacebookID.Columns.AddRange(new ColumnHeader[] { olvColFbUsername, olvColFbNickname, olvColFbStatus });
        olvFacebookID.ContextMenuStrip = contextMenuFacebookID;
        olvFacebookID.Dock = DockStyle.Fill;
        olvFacebookID.FullRowSelect = true;
        olvFacebookID.GridLines = true;
        olvFacebookID.Location = new Point(3, 3);
        olvFacebookID.Name = "olvFacebookID";
        olvFacebookID.ShowGroups = false;
        olvFacebookID.Size = new Size(290, 261);
        olvFacebookID.TabIndex = 1;
        olvFacebookID.UseCompatibleStateImageBehavior = false;
        olvFacebookID.View = View.Details;
        // 
        // olvColFbUsername
        // 
        olvColFbUsername.AspectName = "Username";
        olvColFbUsername.Text = "Username";
        olvColFbUsername.Width = 80;
        // 
        // olvColFbNickname
        // 
        olvColFbNickname.AspectName = "Nickname";
        olvColFbNickname.Text = "Nickname";
        olvColFbNickname.Width = 120;
        // 
        // olvColFbStatus
        // 
        olvColFbStatus.AspectName = "Status";
        olvColFbStatus.CheckBoxes = true;
        olvColFbStatus.HeaderTextAlign = HorizontalAlignment.Center;
        olvColFbStatus.Text = "Active";
        olvColFbStatus.TextAlign = HorizontalAlignment.Center;
        olvColFbStatus.Width = 50;
        // 
        // contextMenuFacebookID
        // 
        contextMenuFacebookID.Items.AddRange(new ToolStripItem[] { menuItemFbAddId, menuItemFbEditId, menuItemFbDeleteId, menuItemFbSeparator1, menuItemFbRefresh, menuItemFbFetch });
        contextMenuFacebookID.Name = "contextMenuFacebookID";
        contextMenuFacebookID.Size = new Size(151, 120);
        // 
        // menuItemFbAddId
        // 
        menuItemFbAddId.Name = "menuItemFbAddId";
        menuItemFbAddId.Size = new Size(150, 22);
        menuItemFbAddId.Text = "Add ID";
        // 
        // menuItemFbEditId
        // 
        menuItemFbEditId.Name = "menuItemFbEditId";
        menuItemFbEditId.Size = new Size(150, 22);
        menuItemFbEditId.Text = "Edit ID";
        // 
        // menuItemFbDeleteId
        // 
        menuItemFbDeleteId.Name = "menuItemFbDeleteId";
        menuItemFbDeleteId.Size = new Size(150, 22);
        menuItemFbDeleteId.Text = "Delete ID";
        // 
        // menuItemFbSeparator1
        // 
        menuItemFbSeparator1.Name = "menuItemFbSeparator1";
        menuItemFbSeparator1.Size = new Size(147, 6);
        // 
        // menuItemFbRefresh
        // 
        menuItemFbRefresh.Name = "menuItemFbRefresh";
        menuItemFbRefresh.Size = new Size(150, 22);
        menuItemFbRefresh.Text = "Refresh";
        // 
        // menuItemFbFetch
        // 
        menuItemFbFetch.Name = "menuItemFbFetch";
        menuItemFbFetch.Size = new Size(150, 22);
        menuItemFbFetch.Text = "Fetch Selected";
        // 
        // lblFbNextSchedule
        // 
        lblFbNextSchedule.AutoSize = true;
        lblFbNextSchedule.Dock = DockStyle.Fill;
        lblFbNextSchedule.ForeColor = Color.OliveDrab;
        lblFbNextSchedule.Location = new Point(3, 567);
        lblFbNextSchedule.Name = "lblFbNextSchedule";
        lblFbNextSchedule.Size = new Size(290, 27);
        lblFbNextSchedule.TabIndex = 7;
        lblFbNextSchedule.Text = "Next: --:-- (--:--)";
        lblFbNextSchedule.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // btnFbStartStop
        // 
        btnFbStartStop.BackColor = Color.FromArgb(0, 120, 0);
        btnFbStartStop.Dock = DockStyle.Fill;
        btnFbStartStop.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnFbStartStop.ForeColor = Color.White;
        btnFbStartStop.Location = new Point(0, 512);
        btnFbStartStop.Margin = new Padding(0);
        btnFbStartStop.Name = "btnFbStartStop";
        btnFbStartStop.Size = new Size(296, 55);
        btnFbStartStop.TabIndex = 5;
        btnFbStartStop.Text = "Start";
        btnFbStartStop.UseVisualStyleBackColor = false;
        // 
        // olvFacebookSchedule
        // 
        olvFacebookSchedule.AllColumns.Add(olvColFbScheduleSL);
        olvFacebookSchedule.AllColumns.Add(olvColFbScheduleTiming);
        olvFacebookSchedule.AllColumns.Add(olvColFbScheduleStatus);
        olvFacebookSchedule.CellEditUseWholeCell = false;
        olvFacebookSchedule.Columns.AddRange(new ColumnHeader[] { olvColFbScheduleSL, olvColFbScheduleTiming, olvColFbScheduleStatus });
        olvFacebookSchedule.Dock = DockStyle.Fill;
        olvFacebookSchedule.FullRowSelect = true;
        olvFacebookSchedule.GridLines = true;
        olvFacebookSchedule.Location = new Point(3, 308);
        olvFacebookSchedule.Name = "olvFacebookSchedule";
        olvFacebookSchedule.ShowGroups = false;
        olvFacebookSchedule.Size = new Size(290, 83);
        olvFacebookSchedule.TabIndex = 3;
        olvFacebookSchedule.UseCompatibleStateImageBehavior = false;
        olvFacebookSchedule.View = View.Details;
        // 
        // olvColFbScheduleSL
        // 
        olvColFbScheduleSL.AspectName = "SerialNumber";
        olvColFbScheduleSL.Text = "SL";
        olvColFbScheduleSL.Width = 40;
        // 
        // olvColFbScheduleTiming
        // 
        olvColFbScheduleTiming.AspectName = "TimingDisplay";
        olvColFbScheduleTiming.Text = "Timing";
        olvColFbScheduleTiming.Width = 100;
        // 
        // olvColFbScheduleStatus
        // 
        olvColFbScheduleStatus.AspectName = "IsActive";
        olvColFbScheduleStatus.CheckBoxes = true;
        olvColFbScheduleStatus.Text = "Active";
        // 
        // tableLayoutPanelFb6
        // 
        tableLayoutPanelFb6.ColumnCount = 3;
        tableLayoutPanelFb6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanelFb6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanelFb6.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 104F));
        tableLayoutPanelFb6.Controls.Add(btnFbRefreshId, 2, 0);
        tableLayoutPanelFb6.Controls.Add(btnFbDeleteId, 1, 0);
        tableLayoutPanelFb6.Controls.Add(btnFbAddId, 0, 0);
        tableLayoutPanelFb6.Dock = DockStyle.Fill;
        tableLayoutPanelFb6.Location = new Point(3, 270);
        tableLayoutPanelFb6.Name = "tableLayoutPanelFb6";
        tableLayoutPanelFb6.RowCount = 1;
        tableLayoutPanelFb6.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanelFb6.Size = new Size(290, 32);
        tableLayoutPanelFb6.TabIndex = 2;
        // 
        // btnFbRefreshId
        // 
        btnFbRefreshId.Dock = DockStyle.Fill;
        btnFbRefreshId.Location = new Point(186, 0);
        btnFbRefreshId.Margin = new Padding(0);
        btnFbRefreshId.Name = "btnFbRefreshId";
        btnFbRefreshId.Size = new Size(104, 32);
        btnFbRefreshId.TabIndex = 4;
        btnFbRefreshId.Text = "Refresh";
        btnFbRefreshId.UseVisualStyleBackColor = true;
        // 
        // btnFbDeleteId
        // 
        btnFbDeleteId.Dock = DockStyle.Fill;
        btnFbDeleteId.Location = new Point(93, 0);
        btnFbDeleteId.Margin = new Padding(0);
        btnFbDeleteId.Name = "btnFbDeleteId";
        btnFbDeleteId.Size = new Size(93, 32);
        btnFbDeleteId.TabIndex = 3;
        btnFbDeleteId.Text = "Delete ID";
        btnFbDeleteId.UseVisualStyleBackColor = true;
        // 
        // btnFbAddId
        // 
        btnFbAddId.Dock = DockStyle.Fill;
        btnFbAddId.Location = new Point(0, 0);
        btnFbAddId.Margin = new Padding(0);
        btnFbAddId.Name = "btnFbAddId";
        btnFbAddId.Size = new Size(93, 32);
        btnFbAddId.TabIndex = 2;
        btnFbAddId.Text = "Add ID";
        btnFbAddId.UseVisualStyleBackColor = true;
        // 
        // tableLayoutPanelFb7
        // 
        tableLayoutPanelFb7.ColumnCount = 4;
        tableLayoutPanelFb7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tableLayoutPanelFb7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tableLayoutPanelFb7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tableLayoutPanelFb7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tableLayoutPanelFb7.Controls.Add(btnFbAddSchedule, 0, 0);
        tableLayoutPanelFb7.Controls.Add(btnFbSaveSettings, 3, 0);
        tableLayoutPanelFb7.Controls.Add(btnFbDeleteSchedule, 2, 0);
        tableLayoutPanelFb7.Controls.Add(btnFbEditSchedule, 1, 0);
        tableLayoutPanelFb7.Dock = DockStyle.Fill;
        tableLayoutPanelFb7.Location = new Point(3, 397);
        tableLayoutPanelFb7.Name = "tableLayoutPanelFb7";
        tableLayoutPanelFb7.RowCount = 1;
        tableLayoutPanelFb7.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanelFb7.Size = new Size(290, 32);
        tableLayoutPanelFb7.TabIndex = 4;
        // 
        // btnFbAddSchedule
        // 
        btnFbAddSchedule.Dock = DockStyle.Fill;
        btnFbAddSchedule.Location = new Point(0, 0);
        btnFbAddSchedule.Margin = new Padding(0);
        btnFbAddSchedule.Name = "btnFbAddSchedule";
        btnFbAddSchedule.Size = new Size(72, 32);
        btnFbAddSchedule.TabIndex = 0;
        btnFbAddSchedule.Text = "Add";
        btnFbAddSchedule.UseVisualStyleBackColor = true;
        // 
        // btnFbSaveSettings
        // 
        btnFbSaveSettings.Dock = DockStyle.Fill;
        btnFbSaveSettings.Location = new Point(216, 0);
        btnFbSaveSettings.Margin = new Padding(0);
        btnFbSaveSettings.Name = "btnFbSaveSettings";
        btnFbSaveSettings.Size = new Size(74, 32);
        btnFbSaveSettings.TabIndex = 6;
        btnFbSaveSettings.Text = "Save";
        btnFbSaveSettings.UseVisualStyleBackColor = true;
        // 
        // btnFbDeleteSchedule
        // 
        btnFbDeleteSchedule.Dock = DockStyle.Fill;
        btnFbDeleteSchedule.Location = new Point(144, 0);
        btnFbDeleteSchedule.Margin = new Padding(0);
        btnFbDeleteSchedule.Name = "btnFbDeleteSchedule";
        btnFbDeleteSchedule.Size = new Size(72, 32);
        btnFbDeleteSchedule.TabIndex = 2;
        btnFbDeleteSchedule.Text = "Delete";
        btnFbDeleteSchedule.UseVisualStyleBackColor = true;
        // 
        // btnFbEditSchedule
        // 
        btnFbEditSchedule.Dock = DockStyle.Fill;
        btnFbEditSchedule.Location = new Point(72, 0);
        btnFbEditSchedule.Margin = new Padding(0);
        btnFbEditSchedule.Name = "btnFbEditSchedule";
        btnFbEditSchedule.Size = new Size(72, 32);
        btnFbEditSchedule.TabIndex = 1;
        btnFbEditSchedule.Text = "Edit";
        btnFbEditSchedule.UseVisualStyleBackColor = true;
        // 
        // tableLayoutPanelFb8
        // 
        tableLayoutPanelFb8.ColumnCount = 2;
        tableLayoutPanelFb8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
        tableLayoutPanelFb8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
        tableLayoutPanelFb8.Controls.Add(lblFbFrequency, 0, 0);
        tableLayoutPanelFb8.Controls.Add(numFbFrequency, 1, 0);
        tableLayoutPanelFb8.Controls.Add(lblFbChunkSize, 0, 1);
        tableLayoutPanelFb8.Controls.Add(numFbChunkSize, 1, 1);
        tableLayoutPanelFb8.Controls.Add(lblFbChunkDelay, 0, 2);
        tableLayoutPanelFb8.Controls.Add(numFbChunkDelay, 1, 2);
        tableLayoutPanelFb8.Dock = DockStyle.Fill;
        tableLayoutPanelFb8.Location = new Point(3, 435);
        tableLayoutPanelFb8.Name = "tableLayoutPanelFb8";
        tableLayoutPanelFb8.RowCount = 3;
        tableLayoutPanelFb8.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
        tableLayoutPanelFb8.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
        tableLayoutPanelFb8.RowStyles.Add(new RowStyle(SizeType.Percent, 33.34F));
        tableLayoutPanelFb8.Size = new Size(290, 74);
        tableLayoutPanelFb8.TabIndex = 5;
        // 
        // lblFbFrequency
        // 
        lblFbFrequency.AutoSize = true;
        lblFbFrequency.Dock = DockStyle.Fill;
        lblFbFrequency.Location = new Point(3, 0);
        lblFbFrequency.Name = "lblFbFrequency";
        lblFbFrequency.Size = new Size(197, 24);
        lblFbFrequency.TabIndex = 3;
        lblFbFrequency.Text = "Delay (sec):";
        lblFbFrequency.TextAlign = ContentAlignment.MiddleRight;
        // 
        // numFbFrequency
        // 
        numFbFrequency.Dock = DockStyle.Fill;
        numFbFrequency.Location = new Point(206, 3);
        numFbFrequency.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
        numFbFrequency.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numFbFrequency.Name = "numFbFrequency";
        numFbFrequency.Size = new Size(81, 23);
        numFbFrequency.TabIndex = 4;
        numFbFrequency.Value = new decimal(new int[] { 10, 0, 0, 0 });
        // 
        // lblFbChunkSize
        // 
        lblFbChunkSize.AutoSize = true;
        lblFbChunkSize.Dock = DockStyle.Fill;
        lblFbChunkSize.Location = new Point(3, 24);
        lblFbChunkSize.Name = "lblFbChunkSize";
        lblFbChunkSize.Size = new Size(197, 24);
        lblFbChunkSize.TabIndex = 6;
        lblFbChunkSize.Text = "Chunk Size:";
        lblFbChunkSize.TextAlign = ContentAlignment.MiddleRight;
        // 
        // numFbChunkSize
        // 
        numFbChunkSize.Dock = DockStyle.Fill;
        numFbChunkSize.Location = new Point(206, 27);
        numFbChunkSize.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        numFbChunkSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numFbChunkSize.Name = "numFbChunkSize";
        numFbChunkSize.Size = new Size(81, 23);
        numFbChunkSize.TabIndex = 5;
        numFbChunkSize.Value = new decimal(new int[] { 10, 0, 0, 0 });
        // 
        // lblFbChunkDelay
        // 
        lblFbChunkDelay.AutoSize = true;
        lblFbChunkDelay.Dock = DockStyle.Fill;
        lblFbChunkDelay.Location = new Point(3, 48);
        lblFbChunkDelay.Name = "lblFbChunkDelay";
        lblFbChunkDelay.Size = new Size(197, 26);
        lblFbChunkDelay.TabIndex = 8;
        lblFbChunkDelay.Text = "Chunk Delay (min):";
        lblFbChunkDelay.TextAlign = ContentAlignment.MiddleRight;
        // 
        // numFbChunkDelay
        // 
        numFbChunkDelay.Dock = DockStyle.Fill;
        numFbChunkDelay.Location = new Point(206, 51);
        numFbChunkDelay.Maximum = new decimal(new int[] { 1440, 0, 0, 0 });
        numFbChunkDelay.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numFbChunkDelay.Name = "numFbChunkDelay";
        numFbChunkDelay.Size = new Size(81, 23);
        numFbChunkDelay.TabIndex = 7;
        numFbChunkDelay.Value = new decimal(new int[] { 5, 0, 0, 0 });
        // 
        // toolStripMain
        // 
        toolStripMain.Items.AddRange(new ToolStripItem[] { btnStartStop, btnScrapeNow, toolStripSeparator2, btnSettings, toolStripSeparator3, btnMemuraiSync, btnMemuraiView, toolStripSeparator4, lblStatus, toolStripSeparator5, btnAbout });
        toolStripMain.Location = new Point(0, 0);
        toolStripMain.Name = "toolStripMain";
        toolStripMain.Size = new Size(1133, 25);
        toolStripMain.TabIndex = 1;
        // 
        // btnStartStop
        // 
        btnStartStop.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnStartStop.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnStartStop.Name = "btnStartStop";
        btnStartStop.Size = new Size(39, 22);
        btnStartStop.Text = "Start";
        btnStartStop.ToolTipText = "Start/Stop automatic scraping";
        // 
        // btnScrapeNow
        // 
        btnScrapeNow.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnScrapeNow.Name = "btnScrapeNow";
        btnScrapeNow.Size = new Size(74, 22);
        btnScrapeNow.Text = "Scrape Now";
        btnScrapeNow.ToolTipText = "Run scraper once now";
        // 
        // toolStripSeparator2
        // 
        toolStripSeparator2.Name = "toolStripSeparator2";
        toolStripSeparator2.Size = new Size(6, 25);
        // 
        // btnSettings
        // 
        btnSettings.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnSettings.Name = "btnSettings";
        btnSettings.Size = new Size(53, 22);
        btnSettings.Text = "Settings";
        btnSettings.ToolTipText = "Open settings";
        // 
        // toolStripSeparator3
        // 
        toolStripSeparator3.Name = "toolStripSeparator3";
        toolStripSeparator3.Size = new Size(6, 25);
        // 
        // btnMemuraiSync
        // 
        btnMemuraiSync.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnMemuraiSync.ForeColor = Color.Black;
        btnMemuraiSync.Name = "btnMemuraiSync";
        btnMemuraiSync.Size = new Size(87, 22);
        btnMemuraiSync.Text = "Memurai Sync";
        btnMemuraiSync.ToolTipText = "Start/Stop syncing news to Memurai server";
        // 
        // btnMemuraiView
        // 
        btnMemuraiView.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnMemuraiView.ForeColor = Color.DarkCyan;
        btnMemuraiView.Name = "btnMemuraiView";
        btnMemuraiView.Size = new Size(87, 22);
        btnMemuraiView.Text = "View Memurai";
        btnMemuraiView.ToolTipText = "View data stored in Memurai server";
        // 
        // toolStripSeparator4
        // 
        toolStripSeparator4.Name = "toolStripSeparator4";
        toolStripSeparator4.Size = new Size(6, 25);
        // 
        // lblStatus
        // 
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(39, 22);
        lblStatus.Text = "Ready";
        // 
        // toolStripSeparator5
        // 
        toolStripSeparator5.Name = "toolStripSeparator5";
        toolStripSeparator5.Size = new Size(6, 25);
        // 
        // btnAbout
        // 
        btnAbout.DisplayStyle = ToolStripItemDisplayStyle.Text;
        btnAbout.Name = "btnAbout";
        btnAbout.Size = new Size(44, 22);
        btnAbout.Text = "About";
        btnAbout.ToolTipText = "About nRun";
        // 
        // statusStrip
        // 
        statusStrip.Items.AddRange(new ToolStripItem[] { statusNewsLabel, statusNewsProgress, statusNewsCount, statusSeparator1, statusTikTokLabel, statusTikTokSchedule, statusSeparator2, statusFacebookLabel, statusFacebookSchedule });
        statusStrip.Location = new Point(0, 659);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(1133, 24);
        statusStrip.TabIndex = 2;
        // 
        // statusNewsLabel
        // 
        statusNewsLabel.BackColor = Color.LightCyan;
        statusNewsLabel.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
        statusNewsLabel.BorderStyle = Border3DStyle.SunkenOuter;
        statusNewsLabel.Name = "statusNewsLabel";
        statusNewsLabel.Size = new Size(269, 19);
        statusNewsLabel.Spring = true;
        statusNewsLabel.Text = "News: Ready";
        statusNewsLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // statusNewsProgress
        // 
        statusNewsProgress.BackColor = Color.LightCyan;
        statusNewsProgress.Name = "statusNewsProgress";
        statusNewsProgress.Size = new Size(80, 18);
        statusNewsProgress.Visible = false;
        // 
        // statusNewsCount
        // 
        statusNewsCount.AutoSize = false;
        statusNewsCount.BackColor = Color.LightCyan;
        statusNewsCount.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
        statusNewsCount.BorderStyle = Border3DStyle.SunkenOuter;
        statusNewsCount.Name = "statusNewsCount";
        statusNewsCount.Size = new Size(70, 19);
        statusNewsCount.Text = "0 articles";
        // 
        // statusSeparator1
        // 
        statusSeparator1.Name = "statusSeparator1";
        statusSeparator1.Size = new Size(0, 19);
        // 
        // statusTikTokLabel
        // 
        statusTikTokLabel.BackColor = Color.Honeydew;
        statusTikTokLabel.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
        statusTikTokLabel.BorderStyle = Border3DStyle.SunkenOuter;
        statusTikTokLabel.Name = "statusTikTokLabel";
        statusTikTokLabel.Size = new Size(269, 19);
        statusTikTokLabel.Spring = true;
        statusTikTokLabel.Text = "TikTok: Ready";
        statusTikTokLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // statusTikTokSchedule
        // 
        statusTikTokSchedule.AutoSize = false;
        statusTikTokSchedule.BackColor = Color.Honeydew;
        statusTikTokSchedule.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
        statusTikTokSchedule.BorderStyle = Border3DStyle.SunkenOuter;
        statusTikTokSchedule.Name = "statusTikTokSchedule";
        statusTikTokSchedule.Size = new Size(120, 19);
        statusTikTokSchedule.Text = "Next: --:--";
        // 
        // statusSeparator2
        // 
        statusSeparator2.Name = "statusSeparator2";
        statusSeparator2.Size = new Size(0, 19);
        // 
        // statusFacebookLabel
        // 
        statusFacebookLabel.BackColor = Color.MistyRose;
        statusFacebookLabel.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
        statusFacebookLabel.BorderStyle = Border3DStyle.SunkenOuter;
        statusFacebookLabel.Name = "statusFacebookLabel";
        statusFacebookLabel.Size = new Size(269, 19);
        statusFacebookLabel.Spring = true;
        statusFacebookLabel.Text = "Facebook: Ready";
        statusFacebookLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // statusFacebookSchedule
        // 
        statusFacebookSchedule.AutoSize = false;
        statusFacebookSchedule.BackColor = Color.MistyRose;
        statusFacebookSchedule.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
        statusFacebookSchedule.BorderStyle = Border3DStyle.SunkenOuter;
        statusFacebookSchedule.Name = "statusFacebookSchedule";
        statusFacebookSchedule.Size = new Size(120, 19);
        statusFacebookSchedule.Text = "Next: --:--";
        // 
        // timerScheduleCountdown
        // 
        timerScheduleCountdown.Interval = 1000;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1133, 683);
        Controls.Add(tabControlMain);
        Controls.Add(toolStripMain);
        Controls.Add(statusStrip);
        MinimumSize = new Size(800, 500);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "nRun - News Scraper";
        WindowState = FormWindowState.Maximized;
        tabControlMain.ResumeLayout(false);
        tabPageNewsScrp.ResumeLayout(false);
        splitContainerMain.Panel1.ResumeLayout(false);
        splitContainerMain.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
        splitContainerMain.ResumeLayout(false);
        panelLeft.ResumeLayout(false);
        panelLeft.PerformLayout();
        panelNoScrap.ResumeLayout(false);
        panelNoScrap.PerformLayout();
        toolStripSites.ResumeLayout(false);
        toolStripSites.PerformLayout();
        panelRight.ResumeLayout(false);
        splitContainerRight.Panel1.ResumeLayout(false);
        splitContainerRight.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainerRight).EndInit();
        splitContainerRight.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)olvArticles).EndInit();
        contextMenuArticles.ResumeLayout(false);
        panelDebug.ResumeLayout(false);
        splitContainerDebug.Panel1.ResumeLayout(false);
        splitContainerDebug.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainerDebug).EndInit();
        splitContainerDebug.ResumeLayout(false);
        panelDebugLeft.ResumeLayout(false);
        panelDebugLeft.PerformLayout();
        panelDebugRight.ResumeLayout(false);
        panelDebugRight.PerformLayout();
        tabPageTiktok.ResumeLayout(false);
        tableLayoutPanel1.ResumeLayout(false);
        tableLayoutPanel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)olvTiktokData).EndInit();
        tableLayoutPanel3.ResumeLayout(false);
        tableLayoutPanel3.PerformLayout();
        tableLayoutPanel4.ResumeLayout(false);
        tableLayoutPanel4.PerformLayout();
        tableLayoutPanel5.ResumeLayout(false);
        tableLayoutPanel5.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)olvTiktokID).EndInit();
        contextMenuTiktokID.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)olvTiktokSchedule).EndInit();
        tableLayoutPanel6.ResumeLayout(false);
        tableLayoutPanel7.ResumeLayout(false);
        tableLayoutPanel8.ResumeLayout(false);
        tableLayoutPanel8.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numTkFrequency).EndInit();
        tabPageFacebook.ResumeLayout(false);
        tableLayoutPanelFb1.ResumeLayout(false);
        tableLayoutPanelFb2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)olvFacebookData).EndInit();
        tableLayoutPanelFb3.ResumeLayout(false);
        tableLayoutPanelFb3.PerformLayout();
        tableLayoutPanelFb4.ResumeLayout(false);
        tableLayoutPanelFb4.PerformLayout();
        tableLayoutPanelFb5.ResumeLayout(false);
        tableLayoutPanelFb5.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)olvFacebookID).EndInit();
        contextMenuFacebookID.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)olvFacebookSchedule).EndInit();
        tableLayoutPanelFb6.ResumeLayout(false);
        tableLayoutPanelFb7.ResumeLayout(false);
        tableLayoutPanelFb8.ResumeLayout(false);
        tableLayoutPanelFb8.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numFbFrequency).EndInit();
        ((System.ComponentModel.ISupportInitialize)numFbChunkSize).EndInit();
        ((System.ComponentModel.ISupportInitialize)numFbChunkDelay).EndInit();
        toolStripMain.ResumeLayout(false);
        toolStripMain.PerformLayout();
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.TabControl tabControlMain;
    private System.Windows.Forms.TabPage tabPageNewsScrp;
    private System.Windows.Forms.TabPage tabPageTiktok;
    private System.Windows.Forms.SplitContainer splitContainerMain;
    private System.Windows.Forms.SplitContainer splitContainerRight;
    private System.Windows.Forms.Panel panelLeft;
    private System.Windows.Forms.Panel panelRight;
    private System.Windows.Forms.Panel panelDebug;
    private System.Windows.Forms.Panel panelNoScrap;
    private System.Windows.Forms.CheckBox chkNoScrapEnabled;
    private System.Windows.Forms.Label lblNoScrapStart;
    private System.Windows.Forms.DateTimePicker dtpNoScrapStart;
    private System.Windows.Forms.Label lblNoScrapEnd;
    private System.Windows.Forms.DateTimePicker dtpNoScrapEnd;
    private System.Windows.Forms.ListBox listBoxSites;
    private System.Windows.Forms.ToolStrip toolStripSites;
    private System.Windows.Forms.ToolStripButton btnAddSite;
    private System.Windows.Forms.ToolStripButton btnEditSite;
    private System.Windows.Forms.ToolStripButton btnDeleteSite;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton btnRefreshSites;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparatorManage;
    private System.Windows.Forms.ToolStripButton btnManageSites;
    private System.Windows.Forms.Label lblSitesHeader;
    private System.Windows.Forms.Label lblDebugHeader;
    private System.Windows.Forms.Button btnClearLog;
    private System.Windows.Forms.TextBox txtDebugLog;
    private System.Windows.Forms.SplitContainer splitContainerDebug;
    private System.Windows.Forms.Panel panelDebugLeft;
    private System.Windows.Forms.Panel panelDebugRight;
    private System.Windows.Forms.TextBox txtErrorLog;
    private System.Windows.Forms.Label lblErrorHeader;
    private System.Windows.Forms.Button btnClearErrorLog;
    private BrightIdeasSoftware.ObjectListView olvArticles;
    private BrightIdeasSoftware.OLVColumn olvColTitle;
    private BrightIdeasSoftware.OLVColumn olvColSite;
    private BrightIdeasSoftware.OLVColumn olvColSiteLogo;
    private BrightIdeasSoftware.OLVColumn olvColDate;
    private System.Windows.Forms.ToolStrip toolStripMain;
    private System.Windows.Forms.ToolStripButton btnStartStop;
    private System.Windows.Forms.ToolStripButton btnScrapeNow;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripButton btnSettings;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripButton btnMemuraiSync;
    private System.Windows.Forms.ToolStripButton btnMemuraiView;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.ToolStripLabel lblStatus;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    private System.Windows.Forms.ToolStripButton btnAbout;
    private System.Windows.Forms.StatusStrip statusStrip;
    // News Scraping status bar items
    private System.Windows.Forms.ToolStripStatusLabel statusNewsLabel;
    private System.Windows.Forms.ToolStripProgressBar statusNewsProgress;
    private System.Windows.Forms.ToolStripStatusLabel statusNewsCount;
    private System.Windows.Forms.ToolStripStatusLabel statusSeparator1;
    // TikTok status bar items
    private System.Windows.Forms.ToolStripStatusLabel statusTikTokLabel;
    private System.Windows.Forms.ToolStripStatusLabel statusTikTokSchedule;
    private System.Windows.Forms.ToolStripStatusLabel statusSeparator2;
    // Facebook status bar items
    private System.Windows.Forms.ToolStripStatusLabel statusFacebookLabel;
    private System.Windows.Forms.ToolStripStatusLabel statusFacebookSchedule;
    private System.Windows.Forms.ContextMenuStrip contextMenuArticles;
    private System.Windows.Forms.ToolStripMenuItem menuItemOpenUrl;
    private System.Windows.Forms.ToolStripMenuItem menuItemCopyUrl;
    private System.Windows.Forms.ToolStripMenuItem menuItemCopyTitle;
    private System.Windows.Forms.ToolStripSeparator menuItemSeparator1;
    private System.Windows.Forms.ToolStripMenuItem menuItemViewBody;
    private System.Windows.Forms.ToolStripSeparator menuItemSeparator2;
    private System.Windows.Forms.ToolStripMenuItem menuItemDelete;

    // TikTok Tab Controls
    private BrightIdeasSoftware.ObjectListView olvTiktokID;
    private BrightIdeasSoftware.OLVColumn olvColTkStatus;
    private BrightIdeasSoftware.OLVColumn olvColTkUsername;
    private BrightIdeasSoftware.OLVColumn olvColTkNickname;
    private System.Windows.Forms.ContextMenuStrip contextMenuTiktokID;
    private System.Windows.Forms.ToolStripMenuItem menuItemTkAddId;
    private System.Windows.Forms.ToolStripMenuItem menuItemTkEditId;
    private System.Windows.Forms.ToolStripMenuItem menuItemTkDeleteId;
    private System.Windows.Forms.ToolStripSeparator menuItemTkSeparator1;
    private System.Windows.Forms.ToolStripMenuItem menuItemTkRefresh;
    private System.Windows.Forms.ToolStripMenuItem menuItemTkFetch;
    private BrightIdeasSoftware.ObjectListView olvTiktokSchedule;
    private BrightIdeasSoftware.OLVColumn olvColScheduleSL;
    private BrightIdeasSoftware.OLVColumn olvColScheduleTiming;
    private BrightIdeasSoftware.OLVColumn olvColScheduleStatus;
    private System.Windows.Forms.Button btnTkAddSchedule;
    private System.Windows.Forms.Button btnTkEditSchedule;
    private System.Windows.Forms.Button btnTkDeleteSchedule;
    private System.Windows.Forms.Button btnTkSaveSettings;
    private System.Windows.Forms.Label lblTkFrequency;
    private System.Windows.Forms.NumericUpDown numTkFrequency;
    private System.Windows.Forms.Button btnTkStartStop;
    private System.Windows.Forms.Label lblTkNextSchedule;
    private System.Windows.Forms.Timer timerScheduleCountdown;
    private System.Windows.Forms.Button btnTkAddId;
    private System.Windows.Forms.Button btnTkDeleteId;
    private System.Windows.Forms.Button btnTkRefreshId;
    private BrightIdeasSoftware.ObjectListView olvTiktokData;
    private BrightIdeasSoftware.OLVColumn olvColDataId;
    private BrightIdeasSoftware.OLVColumn olvColDataUsername;
    private BrightIdeasSoftware.OLVColumn olvColDataFollowers;
    private BrightIdeasSoftware.OLVColumn olvColDataFollowersChange;
    private BrightIdeasSoftware.OLVColumn olvColDataHearts;
    private BrightIdeasSoftware.OLVColumn olvColDataHeartsChange;
    private BrightIdeasSoftware.OLVColumn olvColDataVideos;
    private BrightIdeasSoftware.OLVColumn olvColDataVideosChange;
    private BrightIdeasSoftware.OLVColumn olvColDataRecordedAt;
    private System.Windows.Forms.Label lblTkStatus;
    private System.Windows.Forms.Label lblTkInfo;
    private System.Windows.Forms.ProgressBar progressBarTk;
    // Filter controls
    private System.Windows.Forms.Label lblFilterUsername;
    private System.Windows.Forms.ComboBox cboFilterUsername;
    private System.Windows.Forms.Label lblFilterDateFrom;
    private System.Windows.Forms.DateTimePicker dtpFilterFrom;
    private System.Windows.Forms.Label lblFilterDateTo;
    private System.Windows.Forms.DateTimePicker dtpFilterTo;
    private System.Windows.Forms.Button btnApplyFilter;
    private System.Windows.Forms.Button btnClearFilter;
    private TableLayoutPanel tableLayoutPanel1;
    private TableLayoutPanel tableLayoutPanel2;
    private TableLayoutPanel tableLayoutPanel3;
    private TableLayoutPanel tableLayoutPanel4;
    private TableLayoutPanel tableLayoutPanel5;
    private TableLayoutPanel tableLayoutPanel6;
    private TableLayoutPanel tableLayoutPanel7;
    private TableLayoutPanel tableLayoutPanel8;

    // Facebook Tab Controls
    private System.Windows.Forms.TabPage tabPageFacebook;
    private TableLayoutPanel tableLayoutPanelFb1;
    private TableLayoutPanel tableLayoutPanelFb2;
    private TableLayoutPanel tableLayoutPanelFb3;
    private TableLayoutPanel tableLayoutPanelFb4;
    private TableLayoutPanel tableLayoutPanelFb5;
    private TableLayoutPanel tableLayoutPanelFb6;
    private TableLayoutPanel tableLayoutPanelFb7;
    private TableLayoutPanel tableLayoutPanelFb8;
    private BrightIdeasSoftware.ObjectListView olvFacebookID;
    private BrightIdeasSoftware.OLVColumn olvColFbStatus;
    private BrightIdeasSoftware.OLVColumn olvColFbUsername;
    private BrightIdeasSoftware.OLVColumn olvColFbNickname;
    private System.Windows.Forms.ContextMenuStrip contextMenuFacebookID;
    private System.Windows.Forms.ToolStripMenuItem menuItemFbAddId;
    private System.Windows.Forms.ToolStripMenuItem menuItemFbEditId;
    private System.Windows.Forms.ToolStripMenuItem menuItemFbDeleteId;
    private System.Windows.Forms.ToolStripSeparator menuItemFbSeparator1;
    private System.Windows.Forms.ToolStripMenuItem menuItemFbRefresh;
    private System.Windows.Forms.ToolStripMenuItem menuItemFbFetch;
    private BrightIdeasSoftware.ObjectListView olvFacebookSchedule;
    private BrightIdeasSoftware.OLVColumn olvColFbScheduleSL;
    private BrightIdeasSoftware.OLVColumn olvColFbScheduleTiming;
    private BrightIdeasSoftware.OLVColumn olvColFbScheduleStatus;
    private System.Windows.Forms.Button btnFbAddSchedule;
    private System.Windows.Forms.Button btnFbEditSchedule;
    private System.Windows.Forms.Button btnFbDeleteSchedule;
    private System.Windows.Forms.Button btnFbSaveSettings;
    private System.Windows.Forms.Label lblFbFrequency;
    private System.Windows.Forms.NumericUpDown numFbFrequency;
    private System.Windows.Forms.Label lblFbChunkSize;
    private System.Windows.Forms.NumericUpDown numFbChunkSize;
    private System.Windows.Forms.Label lblFbChunkDelay;
    private System.Windows.Forms.NumericUpDown numFbChunkDelay;
    private System.Windows.Forms.Button btnFbStartStop;
    private System.Windows.Forms.Label lblFbNextSchedule;
    private System.Windows.Forms.Button btnFbAddId;
    private System.Windows.Forms.Button btnFbDeleteId;
    private System.Windows.Forms.Button btnFbRefreshId;
    private BrightIdeasSoftware.ObjectListView olvFacebookData;
    private BrightIdeasSoftware.OLVColumn olvColFbDataId;
    private BrightIdeasSoftware.OLVColumn olvColFbDataUsername;
    private BrightIdeasSoftware.OLVColumn olvColFbDataFollowers;
    private BrightIdeasSoftware.OLVColumn olvColFbFollowersChange;
    private BrightIdeasSoftware.OLVColumn olvColFbDataTalkingAbout;
    private BrightIdeasSoftware.OLVColumn olvColFbTalkingAboutChange;
    private BrightIdeasSoftware.OLVColumn olvColFbDataRecordedAt;
    private System.Windows.Forms.Label lblFbStatus;
    private System.Windows.Forms.Label lblFbInfo;
    private System.Windows.Forms.ProgressBar progressBarFb;
    private System.Windows.Forms.Label lblFbFilterUsername;
    private System.Windows.Forms.ComboBox cboFbFilterUsername;
    private System.Windows.Forms.Label lblFbFilterDateFrom;
    private System.Windows.Forms.DateTimePicker dtpFbFilterFrom;
    private System.Windows.Forms.Label lblFbFilterDateTo;
    private System.Windows.Forms.DateTimePicker dtpFbFilterTo;
    private System.Windows.Forms.Button btnFbApplyFilter;
    private System.Windows.Forms.Button btnFbClearFilter;
}
