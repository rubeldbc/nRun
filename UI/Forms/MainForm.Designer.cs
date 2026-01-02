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
        tabPageTiktok = new TabPage();
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
        // TikTok Tab Controls - must be instantiated before SuspendLayout
        splitContainerTiktok = new SplitContainer();
        panelTiktokLeft = new Panel();
        panelTiktokSchedule = new Panel();
        panelTiktokControls = new Panel();
        olvTiktokID = new BrightIdeasSoftware.ObjectListView();
        olvColTkUsername = new BrightIdeasSoftware.OLVColumn();
        olvColTkNickname = new BrightIdeasSoftware.OLVColumn();
        olvTiktokSchedule = new BrightIdeasSoftware.ObjectListView();
        olvColScheduleSL = new BrightIdeasSoftware.OLVColumn();
        olvColScheduleTiming = new BrightIdeasSoftware.OLVColumn();
        olvColScheduleStatus = new BrightIdeasSoftware.OLVColumn();
        btnTkAddSchedule = new Button();
        btnTkEditSchedule = new Button();
        btnTkDeleteSchedule = new Button();
        lblTkFrequency = new Label();
        numTkFrequency = new NumericUpDown();
        btnTkStartStop = new Button();
        btnTkAddId = new Button();
        btnTkDeleteId = new Button();
        lblTkIdHeader = new Label();
        lblTkScheduleHeader = new Label();
        panelTiktokRight = new Panel();
        olvTiktokData = new BrightIdeasSoftware.ObjectListView();
        olvColDataId = new BrightIdeasSoftware.OLVColumn();
        olvColDataUsername = new BrightIdeasSoftware.OLVColumn();
        olvColDataFollowers = new BrightIdeasSoftware.OLVColumn();
        olvColDataHearts = new BrightIdeasSoftware.OLVColumn();
        olvColDataVideos = new BrightIdeasSoftware.OLVColumn();
        olvColDataRecordedAt = new BrightIdeasSoftware.OLVColumn();
        panelTiktokStatus = new Panel();
        lblTkStatus = new Label();
        progressBarTk = new ProgressBar();
        // Now SuspendLayout after all controls are instantiated
        tabControlMain.SuspendLayout();
        tabPageNewsScrp.SuspendLayout();
        tabPageTiktok.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainerTiktok).BeginInit();
        splitContainerTiktok.Panel1.SuspendLayout();
        splitContainerTiktok.Panel2.SuspendLayout();
        splitContainerTiktok.SuspendLayout();
        panelTiktokLeft.SuspendLayout();
        panelTiktokSchedule.SuspendLayout();
        panelTiktokControls.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)olvTiktokID).BeginInit();
        ((System.ComponentModel.ISupportInitialize)olvTiktokSchedule).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numTkFrequency).BeginInit();
        panelTiktokRight.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)olvTiktokData).BeginInit();
        panelTiktokStatus.SuspendLayout();
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
        tabControlMain.Size = new Size(1000, 502);
        tabControlMain.TabIndex = 0;
        //
        // tabPageNewsScrp
        //
        tabPageNewsScrp.Controls.Add(splitContainerMain);
        tabPageNewsScrp.Location = new Point(4, 24);
        tabPageNewsScrp.Name = "tabPageNewsScrp";
        tabPageNewsScrp.Padding = new Padding(3);
        tabPageNewsScrp.Size = new Size(992, 474);
        tabPageNewsScrp.TabIndex = 0;
        tabPageNewsScrp.Text = "News Scrp";
        tabPageNewsScrp.UseVisualStyleBackColor = true;
        //
        // tabPageTiktok
        //
        tabPageTiktok.Controls.Add(splitContainerTiktok);
        tabPageTiktok.Location = new Point(4, 24);
        tabPageTiktok.Name = "tabPageTiktok";
        tabPageTiktok.Padding = new Padding(3);
        tabPageTiktok.Size = new Size(992, 474);
        tabPageTiktok.TabIndex = 1;
        tabPageTiktok.Text = "Tiktok";
        tabPageTiktok.UseVisualStyleBackColor = true;
        //
        // splitContainerTiktok
        //
        splitContainerTiktok.Dock = DockStyle.Fill;
        splitContainerTiktok.Location = new Point(3, 3);
        splitContainerTiktok.Name = "splitContainerTiktok";
        splitContainerTiktok.Panel1.Controls.Add(panelTiktokLeft);
        splitContainerTiktok.Panel2.Controls.Add(panelTiktokRight);
        splitContainerTiktok.Size = new Size(986, 468);
        splitContainerTiktok.SplitterDistance = 320;
        splitContainerTiktok.TabIndex = 0;
        //
        // panelTiktokLeft
        //
        panelTiktokLeft.Controls.Add(panelTiktokSchedule);
        panelTiktokLeft.Controls.Add(btnTkAddId);
        panelTiktokLeft.Controls.Add(btnTkDeleteId);
        panelTiktokLeft.Controls.Add(olvTiktokID);
        panelTiktokLeft.Controls.Add(lblTkIdHeader);
        panelTiktokLeft.Dock = DockStyle.Fill;
        panelTiktokLeft.Location = new Point(0, 0);
        panelTiktokLeft.Name = "panelTiktokLeft";
        panelTiktokLeft.Size = new Size(320, 468);
        panelTiktokLeft.TabIndex = 0;
        //
        // lblTkIdHeader
        //
        lblTkIdHeader.BackColor = Color.FromArgb(45, 45, 48);
        lblTkIdHeader.Dock = DockStyle.Top;
        lblTkIdHeader.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblTkIdHeader.ForeColor = Color.White;
        lblTkIdHeader.Location = new Point(0, 0);
        lblTkIdHeader.Name = "lblTkIdHeader";
        lblTkIdHeader.Padding = new Padding(5, 5, 0, 0);
        lblTkIdHeader.Size = new Size(320, 25);
        lblTkIdHeader.TabIndex = 0;
        lblTkIdHeader.Text = "TikTok IDs";
        //
        // olvTiktokID
        //
        olvTiktokID.AllColumns.Add(olvColTkUsername);
        olvTiktokID.AllColumns.Add(olvColTkNickname);
        olvTiktokID.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        olvTiktokID.CellEditUseWholeCell = false;
        olvTiktokID.Columns.AddRange(new ColumnHeader[] { olvColTkUsername, olvColTkNickname });
        olvTiktokID.FullRowSelect = true;
        olvTiktokID.GridLines = true;
        olvTiktokID.Location = new Point(0, 25);
        olvTiktokID.Name = "olvTiktokID";
        olvTiktokID.ShowGroups = false;
        olvTiktokID.Size = new Size(320, 120);
        olvTiktokID.TabIndex = 1;
        olvTiktokID.UseCompatibleStateImageBehavior = false;
        olvTiktokID.View = View.Details;
        //
        // btnTkAddId
        //
        btnTkAddId.Location = new Point(5, 148);
        btnTkAddId.Name = "btnTkAddId";
        btnTkAddId.Size = new Size(75, 25);
        btnTkAddId.TabIndex = 2;
        btnTkAddId.Text = "Add ID";
        btnTkAddId.UseVisualStyleBackColor = true;
        //
        // btnTkDeleteId
        //
        btnTkDeleteId.Location = new Point(85, 148);
        btnTkDeleteId.Name = "btnTkDeleteId";
        btnTkDeleteId.Size = new Size(75, 25);
        btnTkDeleteId.TabIndex = 3;
        btnTkDeleteId.Text = "Delete ID";
        btnTkDeleteId.UseVisualStyleBackColor = true;
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
        // panelTiktokSchedule
        //
        panelTiktokSchedule.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        panelTiktokSchedule.Controls.Add(panelTiktokControls);
        panelTiktokSchedule.Controls.Add(olvTiktokSchedule);
        panelTiktokSchedule.Controls.Add(lblTkScheduleHeader);
        panelTiktokSchedule.Location = new Point(0, 178);
        panelTiktokSchedule.Name = "panelTiktokSchedule";
        panelTiktokSchedule.Size = new Size(320, 290);
        panelTiktokSchedule.TabIndex = 4;
        //
        // lblTkScheduleHeader
        //
        lblTkScheduleHeader.BackColor = Color.FromArgb(45, 45, 48);
        lblTkScheduleHeader.Dock = DockStyle.Top;
        lblTkScheduleHeader.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblTkScheduleHeader.ForeColor = Color.White;
        lblTkScheduleHeader.Location = new Point(0, 0);
        lblTkScheduleHeader.Name = "lblTkScheduleHeader";
        lblTkScheduleHeader.Padding = new Padding(5, 5, 0, 0);
        lblTkScheduleHeader.Size = new Size(320, 28);
        lblTkScheduleHeader.TabIndex = 2;
        lblTkScheduleHeader.Text = "Schedule";
        //
        // olvTiktokSchedule
        //
        olvTiktokSchedule.AllColumns.Add(olvColScheduleSL);
        olvTiktokSchedule.AllColumns.Add(olvColScheduleTiming);
        olvTiktokSchedule.AllColumns.Add(olvColScheduleStatus);
        olvTiktokSchedule.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        olvTiktokSchedule.CellEditUseWholeCell = false;
        olvTiktokSchedule.Columns.AddRange(new ColumnHeader[] { olvColScheduleSL, olvColScheduleTiming, olvColScheduleStatus });
        olvTiktokSchedule.FullRowSelect = true;
        olvTiktokSchedule.GridLines = true;
        olvTiktokSchedule.Location = new Point(0, 28);
        olvTiktokSchedule.Name = "olvTiktokSchedule";
        olvTiktokSchedule.ShowGroups = false;
        olvTiktokSchedule.Size = new Size(320, 120);
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
        olvColScheduleStatus.AspectName = "StatusDisplay";
        olvColScheduleStatus.Text = "Status";
        olvColScheduleStatus.Width = 80;
        //
        // panelTiktokControls
        //
        panelTiktokControls.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        panelTiktokControls.Controls.Add(btnTkAddSchedule);
        panelTiktokControls.Controls.Add(btnTkEditSchedule);
        panelTiktokControls.Controls.Add(btnTkDeleteSchedule);
        panelTiktokControls.Controls.Add(lblTkFrequency);
        panelTiktokControls.Controls.Add(numTkFrequency);
        panelTiktokControls.Controls.Add(btnTkStartStop);
        panelTiktokControls.Location = new Point(0, 148);
        panelTiktokControls.Name = "panelTiktokControls";
        panelTiktokControls.Size = new Size(320, 140);
        panelTiktokControls.TabIndex = 4;
        //
        // btnTkAddSchedule
        //
        btnTkAddSchedule.Location = new Point(5, 5);
        btnTkAddSchedule.Name = "btnTkAddSchedule";
        btnTkAddSchedule.Size = new Size(60, 25);
        btnTkAddSchedule.TabIndex = 0;
        btnTkAddSchedule.Text = "Add";
        btnTkAddSchedule.UseVisualStyleBackColor = true;
        //
        // btnTkEditSchedule
        //
        btnTkEditSchedule.Location = new Point(70, 5);
        btnTkEditSchedule.Name = "btnTkEditSchedule";
        btnTkEditSchedule.Size = new Size(60, 25);
        btnTkEditSchedule.TabIndex = 1;
        btnTkEditSchedule.Text = "Edit";
        btnTkEditSchedule.UseVisualStyleBackColor = true;
        //
        // btnTkDeleteSchedule
        //
        btnTkDeleteSchedule.Location = new Point(135, 5);
        btnTkDeleteSchedule.Name = "btnTkDeleteSchedule";
        btnTkDeleteSchedule.Size = new Size(60, 25);
        btnTkDeleteSchedule.TabIndex = 2;
        btnTkDeleteSchedule.Text = "Delete";
        btnTkDeleteSchedule.UseVisualStyleBackColor = true;
        //
        // lblTkFrequency
        //
        lblTkFrequency.AutoSize = true;
        lblTkFrequency.Location = new Point(5, 40);
        lblTkFrequency.Name = "lblTkFrequency";
        lblTkFrequency.Size = new Size(140, 15);
        lblTkFrequency.TabIndex = 3;
        lblTkFrequency.Text = "Delay between IDs (sec):";
        //
        // numTkFrequency
        //
        numTkFrequency.Location = new Point(160, 38);
        numTkFrequency.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
        numTkFrequency.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numTkFrequency.Name = "numTkFrequency";
        numTkFrequency.Size = new Size(60, 23);
        numTkFrequency.TabIndex = 4;
        numTkFrequency.Value = new decimal(new int[] { 10, 0, 0, 0 });
        //
        // btnTkStartStop
        //
        btnTkStartStop.BackColor = Color.FromArgb(0, 120, 0);
        btnTkStartStop.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnTkStartStop.ForeColor = Color.White;
        btnTkStartStop.Location = new Point(5, 70);
        btnTkStartStop.Name = "btnTkStartStop";
        btnTkStartStop.Size = new Size(215, 35);
        btnTkStartStop.TabIndex = 5;
        btnTkStartStop.Text = "Start";
        btnTkStartStop.UseVisualStyleBackColor = false;
        //
        // panelTiktokRight
        //
        panelTiktokRight.Controls.Add(olvTiktokData);
        panelTiktokRight.Controls.Add(panelTiktokStatus);
        panelTiktokRight.Dock = DockStyle.Fill;
        panelTiktokRight.Location = new Point(0, 0);
        panelTiktokRight.Name = "panelTiktokRight";
        panelTiktokRight.Size = new Size(662, 468);
        panelTiktokRight.TabIndex = 0;
        //
        // olvTiktokData
        //
        olvTiktokData.AllColumns.Add(olvColDataId);
        olvTiktokData.AllColumns.Add(olvColDataUsername);
        olvTiktokData.AllColumns.Add(olvColDataFollowers);
        olvTiktokData.AllColumns.Add(olvColDataHearts);
        olvTiktokData.AllColumns.Add(olvColDataVideos);
        olvTiktokData.AllColumns.Add(olvColDataRecordedAt);
        olvTiktokData.CellEditUseWholeCell = false;
        olvTiktokData.Columns.AddRange(new ColumnHeader[] { olvColDataId, olvColDataUsername, olvColDataFollowers, olvColDataHearts, olvColDataVideos, olvColDataRecordedAt });
        olvTiktokData.Dock = DockStyle.Fill;
        olvTiktokData.FullRowSelect = true;
        olvTiktokData.GridLines = true;
        olvTiktokData.Location = new Point(0, 0);
        olvTiktokData.Name = "olvTiktokData";
        olvTiktokData.ShowGroups = false;
        olvTiktokData.Size = new Size(662, 428);
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
        olvColDataUsername.Width = 120;
        //
        // olvColDataFollowers
        //
        olvColDataFollowers.AspectName = "FollowerCountDisplay";
        olvColDataFollowers.Text = "Followers";
        olvColDataFollowers.Width = 100;
        //
        // olvColDataHearts
        //
        olvColDataHearts.AspectName = "HeartCountDisplay";
        olvColDataHearts.Text = "Hearts";
        olvColDataHearts.Width = 100;
        //
        // olvColDataVideos
        //
        olvColDataVideos.AspectName = "VideoCount";
        olvColDataVideos.Text = "Videos";
        olvColDataVideos.Width = 70;
        //
        // olvColDataRecordedAt
        //
        olvColDataRecordedAt.AspectName = "RecordedAtDisplay";
        olvColDataRecordedAt.Text = "Recorded At";
        olvColDataRecordedAt.Width = 150;
        //
        // panelTiktokStatus
        //
        panelTiktokStatus.BackColor = Color.FromArgb(45, 45, 48);
        panelTiktokStatus.Controls.Add(lblTkStatus);
        panelTiktokStatus.Controls.Add(progressBarTk);
        panelTiktokStatus.Dock = DockStyle.Bottom;
        panelTiktokStatus.Location = new Point(0, 428);
        panelTiktokStatus.Name = "panelTiktokStatus";
        panelTiktokStatus.Size = new Size(662, 40);
        panelTiktokStatus.TabIndex = 1;
        //
        // lblTkStatus
        //
        lblTkStatus.AutoSize = true;
        lblTkStatus.ForeColor = Color.White;
        lblTkStatus.Location = new Point(10, 12);
        lblTkStatus.Name = "lblTkStatus";
        lblTkStatus.Size = new Size(39, 15);
        lblTkStatus.TabIndex = 0;
        lblTkStatus.Text = "Ready";
        //
        // progressBarTk
        //
        progressBarTk.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        progressBarTk.Location = new Point(450, 8);
        progressBarTk.Name = "progressBarTk";
        progressBarTk.Size = new Size(200, 23);
        progressBarTk.TabIndex = 1;
        progressBarTk.Visible = false;
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
        splitContainerMain.Size = new Size(986, 468);
        splitContainerMain.SplitterDistance = 250;
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
        panelLeft.Size = new Size(250, 502);
        panelLeft.TabIndex = 0;
        // 
        // listBoxSites
        // 
        listBoxSites.Dock = DockStyle.Fill;
        listBoxSites.FormattingEnabled = true;
        listBoxSites.ItemHeight = 15;
        listBoxSites.Location = new Point(0, 50);
        listBoxSites.Name = "listBoxSites";
        listBoxSites.Size = new Size(250, 452);
        listBoxSites.TabIndex = 2;
        // 
        // toolStripSites
        // 
        toolStripSites.GripStyle = ToolStripGripStyle.Hidden;
        toolStripSites.Items.AddRange(new ToolStripItem[] { btnAddSite, btnEditSite, btnDeleteSite, toolStripSeparator1, btnRefreshSites, toolStripSeparatorManage, btnManageSites });
        toolStripSites.Location = new Point(0, 25);
        toolStripSites.Name = "toolStripSites";
        toolStripSites.Size = new Size(250, 25);
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
        lblSitesHeader.Size = new Size(250, 25);
        lblSitesHeader.TabIndex = 0;
        lblSitesHeader.Text = "News Sources";
        // 
        // panelRight
        // 
        panelRight.Controls.Add(splitContainerRight);
        panelRight.Dock = DockStyle.Fill;
        panelRight.Location = new Point(0, 0);
        panelRight.Name = "panelRight";
        panelRight.Size = new Size(746, 502);
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
        splitContainerRight.Size = new Size(746, 502);
        splitContainerRight.SplitterDistance = 351;
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
        olvArticles.Size = new Size(746, 351);
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
        panelDebug.Size = new Size(746, 147);
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
        splitContainerDebug.Size = new Size(746, 147);
        splitContainerDebug.SplitterDistance = 373;
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
        panelDebugLeft.Size = new Size(373, 147);
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
        txtDebugLog.Size = new Size(373, 125);
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
        lblDebugHeader.Size = new Size(373, 22);
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
        btnClearLog.Location = new Point(323, 0);
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
        panelDebugRight.Size = new Size(369, 147);
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
        txtErrorLog.Size = new Size(369, 125);
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
        lblErrorHeader.Size = new Size(369, 22);
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
        btnClearErrorLog.Location = new Point(319, 0);
        btnClearErrorLog.Name = "btnClearErrorLog";
        btnClearErrorLog.Size = new Size(50, 22);
        btnClearErrorLog.TabIndex = 1;
        btnClearErrorLog.Text = "Clear";
        btnClearErrorLog.UseVisualStyleBackColor = false;
        // 
        // toolStripMain
        // 
        toolStripMain.Items.AddRange(new ToolStripItem[] { btnStartStop, btnScrapeNow, toolStripSeparator2, btnSettings, toolStripSeparator3, btnMemuraiSync, btnMemuraiView, toolStripSeparator4, lblStatus });
        toolStripMain.Location = new Point(0, 0);
        toolStripMain.Name = "toolStripMain";
        toolStripMain.Size = new Size(1000, 25);
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
        statusStrip.Location = new Point(0, 527);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(1000, 22);
        statusStrip.TabIndex = 2;
        // 
        // statusLabel
        // 
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(932, 17);
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
        // MainForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 549);
        Controls.Add(tabControlMain);
        Controls.Add(toolStripMain);
        Controls.Add(statusStrip);
        MinimumSize = new Size(800, 500);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "nRun - News Scraper";
        tabControlMain.ResumeLayout(false);
        tabPageNewsScrp.ResumeLayout(false);
        tabPageTiktok.ResumeLayout(false);
        splitContainerTiktok.Panel1.ResumeLayout(false);
        splitContainerTiktok.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainerTiktok).EndInit();
        splitContainerTiktok.ResumeLayout(false);
        panelTiktokLeft.ResumeLayout(false);
        panelTiktokSchedule.ResumeLayout(false);
        panelTiktokControls.ResumeLayout(false);
        panelTiktokControls.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)olvTiktokID).EndInit();
        ((System.ComponentModel.ISupportInitialize)olvTiktokSchedule).EndInit();
        ((System.ComponentModel.ISupportInitialize)numTkFrequency).EndInit();
        panelTiktokRight.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)olvTiktokData).EndInit();
        panelTiktokStatus.ResumeLayout(false);
        panelTiktokStatus.PerformLayout();
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
    private System.Windows.Forms.Label lblTkFrequency;
    private System.Windows.Forms.NumericUpDown numTkFrequency;
    private System.Windows.Forms.Button btnTkStartStop;
    private System.Windows.Forms.Button btnTkAddId;
    private System.Windows.Forms.Button btnTkDeleteId;
    private System.Windows.Forms.Label lblTkIdHeader;
    private System.Windows.Forms.Label lblTkScheduleHeader;
    private System.Windows.Forms.Panel panelTiktokRight;
    private BrightIdeasSoftware.ObjectListView olvTiktokData;
    private BrightIdeasSoftware.OLVColumn olvColDataId;
    private BrightIdeasSoftware.OLVColumn olvColDataUsername;
    private BrightIdeasSoftware.OLVColumn olvColDataFollowers;
    private BrightIdeasSoftware.OLVColumn olvColDataHearts;
    private BrightIdeasSoftware.OLVColumn olvColDataVideos;
    private BrightIdeasSoftware.OLVColumn olvColDataRecordedAt;
    private System.Windows.Forms.Panel panelTiktokStatus;
    private System.Windows.Forms.Label lblTkStatus;
    private System.Windows.Forms.ProgressBar progressBarTk;
}
