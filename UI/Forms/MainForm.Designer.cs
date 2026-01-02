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
        statusStrip = new StatusStrip();
        statusLabel = new ToolStripStatusLabel();
        statusProgress = new ToolStripProgressBar();
        statusArticleCount = new ToolStripStatusLabel();
        timerScheduleCountdown = new System.Windows.Forms.Timer(components);
        tabControlMain.SuspendLayout();
        tabPageNewsScrp.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
        splitContainerMain.Panel1.SuspendLayout();
        splitContainerMain.Panel2.SuspendLayout();
        splitContainerMain.SuspendLayout();
        panelLeft.SuspendLayout();
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
        ((System.ComponentModel.ISupportInitialize)olvTiktokSchedule).BeginInit();
        tableLayoutPanel6.SuspendLayout();
        tableLayoutPanel7.SuspendLayout();
        tableLayoutPanel8.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numTkFrequency).BeginInit();
        toolStripMain.SuspendLayout();
        statusStrip.SuspendLayout();
        SuspendLayout();
        // 
        // tabControlMain
        // 
        tabControlMain.Controls.Add(tabPageNewsScrp);
        tabControlMain.Controls.Add(tabPageTiktok);
        tabControlMain.Dock = DockStyle.Fill;
        tabControlMain.Location = new Point(0, 25);
        tabControlMain.Name = "tabControlMain";
        tabControlMain.SelectedIndex = 0;
        tabControlMain.Size = new Size(1133, 636);
        tabControlMain.TabIndex = 0;
        // 
        // tabPageNewsScrp
        // 
        tabPageNewsScrp.Controls.Add(splitContainerMain);
        tabPageNewsScrp.Location = new Point(4, 24);
        tabPageNewsScrp.Name = "tabPageNewsScrp";
        tabPageNewsScrp.Padding = new Padding(3);
        tabPageNewsScrp.Size = new Size(1125, 608);
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
        splitContainerMain.Size = new Size(1119, 602);
        splitContainerMain.SplitterDistance = 283;
        splitContainerMain.TabIndex = 0;
        // 
        // panelLeft
        // 
        panelLeft.Controls.Add(listBoxSites);
        panelLeft.Controls.Add(toolStripSites);
        panelLeft.Controls.Add(lblSitesHeader);
        panelLeft.Dock = DockStyle.Fill;
        panelLeft.Location = new Point(0, 0);
        panelLeft.Name = "panelLeft";
        panelLeft.Size = new Size(283, 602);
        panelLeft.TabIndex = 0;
        // 
        // listBoxSites
        // 
        listBoxSites.Dock = DockStyle.Fill;
        listBoxSites.FormattingEnabled = true;
        listBoxSites.ItemHeight = 15;
        listBoxSites.Location = new Point(0, 50);
        listBoxSites.Name = "listBoxSites";
        listBoxSites.Size = new Size(283, 552);
        listBoxSites.TabIndex = 2;
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
        panelRight.Size = new Size(832, 602);
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
        splitContainerRight.Size = new Size(832, 602);
        splitContainerRight.SplitterDistance = 420;
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
        olvArticles.Size = new Size(832, 420);
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
        tabPageTiktok.Size = new Size(1125, 608);
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
        tableLayoutPanel1.Size = new Size(1119, 602);
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
        tableLayoutPanel2.Size = new Size(811, 596);
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
        olvTiktokData.Size = new Size(805, 520);
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
        tableLayoutPanel4.Location = new Point(3, 564);
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
        tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
        tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
        tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
        tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
        tableLayoutPanel5.Size = new Size(296, 596);
        tableLayoutPanel5.TabIndex = 1;
        // 
        // olvTiktokID
        // 
        olvTiktokID.AllColumns.Add(olvColTkUsername);
        olvTiktokID.AllColumns.Add(olvColTkNickname);
        olvTiktokID.CellEditUseWholeCell = false;
        olvTiktokID.Columns.AddRange(new ColumnHeader[] { olvColTkUsername, olvColTkNickname });
        olvTiktokID.Dock = DockStyle.Fill;
        olvTiktokID.FullRowSelect = true;
        olvTiktokID.GridLines = true;
        olvTiktokID.Location = new Point(3, 3);
        olvTiktokID.Name = "olvTiktokID";
        olvTiktokID.ShowGroups = false;
        olvTiktokID.Size = new Size(290, 236);
        olvTiktokID.TabIndex = 1;
        olvTiktokID.UseCompatibleStateImageBehavior = false;
        olvTiktokID.View = View.Details;
        // 
        // olvColTkUsername
        // 
        olvColTkUsername.AspectName = "Username";
        olvColTkUsername.Text = "Username";
        olvColTkUsername.Width = 130;
        // 
        // olvColTkNickname
        // 
        olvColTkNickname.AspectName = "Nickname";
        olvColTkNickname.Text = "Nickname";
        olvColTkNickname.Width = 150;
        // 
        // lblTkNextSchedule
        // 
        lblTkNextSchedule.AutoSize = true;
        lblTkNextSchedule.Dock = DockStyle.Fill;
        lblTkNextSchedule.ForeColor = Color.OliveDrab;
        lblTkNextSchedule.Location = new Point(3, 568);
        lblTkNextSchedule.Name = "lblTkNextSchedule";
        lblTkNextSchedule.Size = new Size(290, 28);
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
        btnTkStartStop.Location = new Point(0, 513);
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
        olvTiktokSchedule.Location = new Point(3, 285);
        olvTiktokSchedule.Name = "olvTiktokSchedule";
        olvTiktokSchedule.ShowGroups = false;
        olvTiktokSchedule.Size = new Size(290, 155);
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
        tableLayoutPanel6.Location = new Point(3, 245);
        tableLayoutPanel6.Name = "tableLayoutPanel6";
        tableLayoutPanel6.RowCount = 1;
        tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel6.Size = new Size(290, 34);
        tableLayoutPanel6.TabIndex = 2;
        // 
        // btnTkRefreshId
        // 
        btnTkRefreshId.Dock = DockStyle.Fill;
        btnTkRefreshId.Location = new Point(186, 0);
        btnTkRefreshId.Margin = new Padding(0);
        btnTkRefreshId.Name = "btnTkRefreshId";
        btnTkRefreshId.Size = new Size(104, 34);
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
        btnTkDeleteId.Size = new Size(93, 34);
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
        btnTkAddId.Size = new Size(93, 34);
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
        tableLayoutPanel7.Location = new Point(3, 446);
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
        tableLayoutPanel8.Location = new Point(3, 484);
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
        // toolStripMain
        // 
        toolStripMain.Items.AddRange(new ToolStripItem[] { btnStartStop, btnScrapeNow, toolStripSeparator2, btnSettings, toolStripSeparator3, btnMemuraiSync, btnMemuraiView, toolStripSeparator4, lblStatus });
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
        // statusStrip
        // 
        statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel, statusProgress, statusArticleCount });
        statusStrip.Location = new Point(0, 661);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(1133, 22);
        statusStrip.TabIndex = 2;
        // 
        // statusLabel
        // 
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(1065, 17);
        statusLabel.Spring = true;
        statusLabel.Text = "Ready";
        statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // statusProgress
        // 
        statusProgress.Name = "statusProgress";
        statusProgress.Size = new Size(100, 16);
        statusProgress.Visible = false;
        // 
        // statusArticleCount
        // 
        statusArticleCount.Name = "statusArticleCount";
        statusArticleCount.Size = new Size(53, 17);
        statusArticleCount.Text = "0 articles";
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
        ((System.ComponentModel.ISupportInitialize)olvTiktokSchedule).EndInit();
        tableLayoutPanel6.ResumeLayout(false);
        tableLayoutPanel7.ResumeLayout(false);
        tableLayoutPanel8.ResumeLayout(false);
        tableLayoutPanel8.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numTkFrequency).EndInit();
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
    private System.Windows.Forms.StatusStrip statusStrip;
    private System.Windows.Forms.ToolStripStatusLabel statusLabel;
    private System.Windows.Forms.ToolStripProgressBar statusProgress;
    private System.Windows.Forms.ToolStripStatusLabel statusArticleCount;
    private System.Windows.Forms.ContextMenuStrip contextMenuArticles;
    private System.Windows.Forms.ToolStripMenuItem menuItemOpenUrl;
    private System.Windows.Forms.ToolStripMenuItem menuItemCopyUrl;
    private System.Windows.Forms.ToolStripMenuItem menuItemCopyTitle;
    private System.Windows.Forms.ToolStripSeparator menuItemSeparator1;
    private System.Windows.Forms.ToolStripMenuItem menuItemViewBody;
    private System.Windows.Forms.ToolStripSeparator menuItemSeparator2;
    private System.Windows.Forms.ToolStripMenuItem menuItemDelete;

    // TikTok Tab Controls
    private System.Windows.Forms.SplitContainer splitContainerTiktok;
    private System.Windows.Forms.Panel panelTiktokLeft;
    private System.Windows.Forms.Panel panelTiktokSchedule;
    private System.Windows.Forms.Panel panelTiktokControls;
    private BrightIdeasSoftware.ObjectListView olvTiktokID;
    private BrightIdeasSoftware.OLVColumn olvColTkUsername;
    private BrightIdeasSoftware.OLVColumn olvColTkNickname;
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
    private System.Windows.Forms.Panel panelTiktokRight;
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
    private System.Windows.Forms.Panel panelTiktokStatus;
    private System.Windows.Forms.Label lblTkStatus;
    private System.Windows.Forms.Label lblTkInfo;
    private System.Windows.Forms.ProgressBar progressBarTk;
    // Filter controls
    private System.Windows.Forms.Panel panelTiktokFilter;
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
}
