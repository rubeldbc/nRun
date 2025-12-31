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
        this.components = new System.ComponentModel.Container();

        // Main split container
        this.splitContainerMain = new System.Windows.Forms.SplitContainer();

        // Left panel components
        this.panelLeft = new System.Windows.Forms.Panel();
        this.listBoxSites = new System.Windows.Forms.ListBox();
        this.toolStripSites = new System.Windows.Forms.ToolStrip();
        this.btnAddSite = new System.Windows.Forms.ToolStripButton();
        this.btnEditSite = new System.Windows.Forms.ToolStripButton();
        this.btnDeleteSite = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        this.btnRefreshSites = new System.Windows.Forms.ToolStripButton();
        this.lblSitesHeader = new System.Windows.Forms.Label();

        // Right panel components
        this.panelRight = new System.Windows.Forms.Panel();
        this.splitContainerRight = new System.Windows.Forms.SplitContainer();
        this.olvArticles = new BrightIdeasSoftware.ObjectListView();
        this.olvColTitle = new BrightIdeasSoftware.OLVColumn();
        this.olvColSite = new BrightIdeasSoftware.OLVColumn();
        this.olvColDate = new BrightIdeasSoftware.OLVColumn();

        // Debug log panel
        this.panelDebug = new System.Windows.Forms.Panel();
        this.txtDebugLog = new System.Windows.Forms.TextBox();
        this.lblDebugHeader = new System.Windows.Forms.Label();
        this.btnClearLog = new System.Windows.Forms.Button();

        // Top toolbar
        this.toolStripMain = new System.Windows.Forms.ToolStrip();
        this.btnStartStop = new System.Windows.Forms.ToolStripButton();
        this.btnScrapeNow = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
        this.btnSettings = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
        this.lblStatus = new System.Windows.Forms.ToolStripLabel();

        // Status bar
        this.statusStrip = new System.Windows.Forms.StatusStrip();
        this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
        this.statusProgress = new System.Windows.Forms.ToolStripProgressBar();
        this.statusArticleCount = new System.Windows.Forms.ToolStripStatusLabel();

        // Context menu for articles
        this.contextMenuArticles = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.menuItemOpenUrl = new System.Windows.Forms.ToolStripMenuItem();
        this.menuItemCopyUrl = new System.Windows.Forms.ToolStripMenuItem();
        this.menuItemCopyTitle = new System.Windows.Forms.ToolStripMenuItem();
        this.menuItemSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        this.menuItemViewBody = new System.Windows.Forms.ToolStripMenuItem();
        this.menuItemSeparator2 = new System.Windows.Forms.ToolStripSeparator();
        this.menuItemDelete = new System.Windows.Forms.ToolStripMenuItem();

        ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
        this.splitContainerMain.Panel1.SuspendLayout();
        this.splitContainerMain.Panel2.SuspendLayout();
        this.splitContainerMain.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.splitContainerRight)).BeginInit();
        this.splitContainerRight.Panel1.SuspendLayout();
        this.splitContainerRight.Panel2.SuspendLayout();
        this.splitContainerRight.SuspendLayout();
        this.panelLeft.SuspendLayout();
        this.toolStripSites.SuspendLayout();
        this.panelRight.SuspendLayout();
        this.panelDebug.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.olvArticles)).BeginInit();
        this.toolStripMain.SuspendLayout();
        this.statusStrip.SuspendLayout();
        this.contextMenuArticles.SuspendLayout();
        this.SuspendLayout();

        //
        // splitContainerMain
        //
        this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
        this.splitContainerMain.Location = new System.Drawing.Point(0, 27);
        this.splitContainerMain.Name = "splitContainerMain";
        this.splitContainerMain.Panel1.Controls.Add(this.panelLeft);
        this.splitContainerMain.Panel2.Controls.Add(this.panelRight);
        this.splitContainerMain.Size = new System.Drawing.Size(1000, 500);
        this.splitContainerMain.SplitterDistance = 250;
        this.splitContainerMain.TabIndex = 0;

        //
        // panelLeft
        //
        this.panelLeft.Controls.Add(this.listBoxSites);
        this.panelLeft.Controls.Add(this.toolStripSites);
        this.panelLeft.Controls.Add(this.lblSitesHeader);
        this.panelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelLeft.Location = new System.Drawing.Point(0, 0);
        this.panelLeft.Name = "panelLeft";
        this.panelLeft.Size = new System.Drawing.Size(250, 500);
        this.panelLeft.TabIndex = 0;

        //
        // lblSitesHeader
        //
        this.lblSitesHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.lblSitesHeader.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        this.lblSitesHeader.Location = new System.Drawing.Point(0, 0);
        this.lblSitesHeader.Name = "lblSitesHeader";
        this.lblSitesHeader.Padding = new System.Windows.Forms.Padding(5, 5, 0, 0);
        this.lblSitesHeader.Size = new System.Drawing.Size(250, 25);
        this.lblSitesHeader.TabIndex = 0;
        this.lblSitesHeader.Text = "News Sources";

        //
        // toolStripSites
        //
        this.toolStripSites.Dock = System.Windows.Forms.DockStyle.Top;
        this.toolStripSites.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
        this.toolStripSites.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddSite,
            this.btnEditSite,
            this.btnDeleteSite,
            this.toolStripSeparator1,
            this.btnRefreshSites});
        this.toolStripSites.Location = new System.Drawing.Point(0, 25);
        this.toolStripSites.Name = "toolStripSites";
        this.toolStripSites.Size = new System.Drawing.Size(250, 25);
        this.toolStripSites.TabIndex = 1;

        //
        // btnAddSite
        //
        this.btnAddSite.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnAddSite.Name = "btnAddSite";
        this.btnAddSite.Size = new System.Drawing.Size(33, 22);
        this.btnAddSite.Text = "Add";
        this.btnAddSite.ToolTipText = "Add new news source";

        //
        // btnEditSite
        //
        this.btnEditSite.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnEditSite.Name = "btnEditSite";
        this.btnEditSite.Size = new System.Drawing.Size(31, 22);
        this.btnEditSite.Text = "Edit";
        this.btnEditSite.ToolTipText = "Edit selected source";

        //
        // btnDeleteSite
        //
        this.btnDeleteSite.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnDeleteSite.Name = "btnDeleteSite";
        this.btnDeleteSite.Size = new System.Drawing.Size(44, 22);
        this.btnDeleteSite.Text = "Delete";
        this.btnDeleteSite.ToolTipText = "Delete selected source";

        //
        // toolStripSeparator1
        //
        this.toolStripSeparator1.Name = "toolStripSeparator1";
        this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);

        //
        // btnRefreshSites
        //
        this.btnRefreshSites.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnRefreshSites.Name = "btnRefreshSites";
        this.btnRefreshSites.Size = new System.Drawing.Size(50, 22);
        this.btnRefreshSites.Text = "Refresh";
        this.btnRefreshSites.ToolTipText = "Refresh site list";

        //
        // listBoxSites
        //
        this.listBoxSites.Dock = System.Windows.Forms.DockStyle.Fill;
        this.listBoxSites.FormattingEnabled = true;
        this.listBoxSites.ItemHeight = 15;
        this.listBoxSites.Location = new System.Drawing.Point(0, 50);
        this.listBoxSites.Name = "listBoxSites";
        this.listBoxSites.Size = new System.Drawing.Size(250, 450);
        this.listBoxSites.TabIndex = 2;

        //
        // panelRight
        //
        this.panelRight.Controls.Add(this.splitContainerRight);
        this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelRight.Location = new System.Drawing.Point(0, 0);
        this.panelRight.Name = "panelRight";
        this.panelRight.Size = new System.Drawing.Size(746, 500);
        this.panelRight.TabIndex = 0;

        //
        // splitContainerRight
        //
        this.splitContainerRight.Dock = System.Windows.Forms.DockStyle.Fill;
        this.splitContainerRight.Location = new System.Drawing.Point(0, 0);
        this.splitContainerRight.Name = "splitContainerRight";
        this.splitContainerRight.Orientation = System.Windows.Forms.Orientation.Horizontal;
        this.splitContainerRight.Panel1.Controls.Add(this.olvArticles);
        this.splitContainerRight.Panel2.Controls.Add(this.panelDebug);
        this.splitContainerRight.Size = new System.Drawing.Size(746, 500);
        this.splitContainerRight.SplitterDistance = 350;
        this.splitContainerRight.TabIndex = 0;

        //
        // olvArticles
        //
        this.olvArticles.AllColumns.Add(this.olvColTitle);
        this.olvArticles.AllColumns.Add(this.olvColSite);
        this.olvArticles.AllColumns.Add(this.olvColDate);
        this.olvArticles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColTitle,
            this.olvColSite,
            this.olvColDate});
        this.olvArticles.ContextMenuStrip = this.contextMenuArticles;
        this.olvArticles.Dock = System.Windows.Forms.DockStyle.Fill;
        this.olvArticles.FullRowSelect = true;
        this.olvArticles.GridLines = true;
        this.olvArticles.Location = new System.Drawing.Point(0, 0);
        this.olvArticles.Name = "olvArticles";
        this.olvArticles.ShowGroups = false;
        this.olvArticles.Size = new System.Drawing.Size(746, 350);
        this.olvArticles.TabIndex = 0;
        this.olvArticles.UseCompatibleStateImageBehavior = false;
        this.olvArticles.View = System.Windows.Forms.View.Details;

        //
        // olvColTitle
        //
        this.olvColTitle.AspectName = "NewsTitle";
        this.olvColTitle.Text = "Title";
        this.olvColTitle.Width = 450;

        //
        // olvColSite
        //
        this.olvColSite.AspectName = "SiteName";
        this.olvColSite.Text = "Source";
        this.olvColSite.Width = 150;

        //
        // olvColDate
        //
        this.olvColDate.AspectName = "CreatedAtDisplay";
        this.olvColDate.Text = "Date";
        this.olvColDate.Width = 130;

        //
        // panelDebug
        //
        this.panelDebug.Controls.Add(this.txtDebugLog);
        this.panelDebug.Controls.Add(this.lblDebugHeader);
        this.panelDebug.Controls.Add(this.btnClearLog);
        this.panelDebug.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelDebug.Location = new System.Drawing.Point(0, 0);
        this.panelDebug.Name = "panelDebug";
        this.panelDebug.Size = new System.Drawing.Size(746, 146);
        this.panelDebug.TabIndex = 0;

        //
        // lblDebugHeader
        //
        this.lblDebugHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
        this.lblDebugHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.lblDebugHeader.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        this.lblDebugHeader.ForeColor = System.Drawing.Color.White;
        this.lblDebugHeader.Location = new System.Drawing.Point(0, 0);
        this.lblDebugHeader.Name = "lblDebugHeader";
        this.lblDebugHeader.Padding = new System.Windows.Forms.Padding(5, 3, 0, 0);
        this.lblDebugHeader.Size = new System.Drawing.Size(746, 22);
        this.lblDebugHeader.TabIndex = 0;
        this.lblDebugHeader.Text = "?? Debug Log";

        //
        // btnClearLog
        //
        this.btnClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnClearLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnClearLog.Font = new System.Drawing.Font("Segoe UI", 8F);
        this.btnClearLog.ForeColor = System.Drawing.Color.White;
        this.btnClearLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
        this.btnClearLog.Location = new System.Drawing.Point(696, 0);
        this.btnClearLog.Name = "btnClearLog";
        this.btnClearLog.Size = new System.Drawing.Size(50, 22);
        this.btnClearLog.TabIndex = 1;
        this.btnClearLog.Text = "Clear";

        //
        // txtDebugLog
        //
        this.txtDebugLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
        this.txtDebugLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
        this.txtDebugLog.Dock = System.Windows.Forms.DockStyle.Fill;
        this.txtDebugLog.Font = new System.Drawing.Font("Consolas", 9F);
        this.txtDebugLog.ForeColor = System.Drawing.Color.LightGreen;
        this.txtDebugLog.Location = new System.Drawing.Point(0, 22);
        this.txtDebugLog.Multiline = true;
        this.txtDebugLog.Name = "txtDebugLog";
        this.txtDebugLog.ReadOnly = true;
        this.txtDebugLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.txtDebugLog.Size = new System.Drawing.Size(746, 124);
        this.txtDebugLog.TabIndex = 2;

        //
        // toolStripMain
        //
        this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnStartStop,
            this.btnScrapeNow,
            this.toolStripSeparator2,
            this.btnSettings,
            this.toolStripSeparator3,
            this.lblStatus});
        this.toolStripMain.Location = new System.Drawing.Point(0, 0);
        this.toolStripMain.Name = "toolStripMain";
        this.toolStripMain.Size = new System.Drawing.Size(1000, 27);
        this.toolStripMain.TabIndex = 1;

        //
        // btnStartStop
        //
        this.btnStartStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnStartStop.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        this.btnStartStop.Name = "btnStartStop";
        this.btnStartStop.Size = new System.Drawing.Size(37, 24);
        this.btnStartStop.Text = "Start";
        this.btnStartStop.ToolTipText = "Start/Stop automatic scraping";

        //
        // btnScrapeNow
        //
        this.btnScrapeNow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnScrapeNow.Name = "btnScrapeNow";
        this.btnScrapeNow.Size = new System.Drawing.Size(72, 24);
        this.btnScrapeNow.Text = "Scrape Now";
        this.btnScrapeNow.ToolTipText = "Run scraper once now";

        //
        // toolStripSeparator2
        //
        this.toolStripSeparator2.Name = "toolStripSeparator2";
        this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);

        //
        // btnSettings
        //
        this.btnSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnSettings.Name = "btnSettings";
        this.btnSettings.Size = new System.Drawing.Size(53, 24);
        this.btnSettings.Text = "Settings";
        this.btnSettings.ToolTipText = "Open settings";

        //
        // toolStripSeparator3
        //
        this.toolStripSeparator3.Name = "toolStripSeparator3";
        this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);

        //
        // lblStatus
        //
        this.lblStatus.Name = "lblStatus";
        this.lblStatus.Size = new System.Drawing.Size(39, 24);
        this.lblStatus.Text = "Ready";

        //
        // statusStrip
        //
        this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.statusProgress,
            this.statusArticleCount});
        this.statusStrip.Location = new System.Drawing.Point(0, 527);
        this.statusStrip.Name = "statusStrip";
        this.statusStrip.Size = new System.Drawing.Size(1000, 22);
        this.statusStrip.TabIndex = 2;

        //
        // statusLabel
        //
        this.statusLabel.Name = "statusLabel";
        this.statusLabel.Size = new System.Drawing.Size(39, 17);
        this.statusLabel.Text = "Ready";
        this.statusLabel.Spring = true;
        this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

        //
        // statusProgress
        //
        this.statusProgress.Name = "statusProgress";
        this.statusProgress.Size = new System.Drawing.Size(100, 16);
        this.statusProgress.Visible = false;

        //
        // statusArticleCount
        //
        this.statusArticleCount.Name = "statusArticleCount";
        this.statusArticleCount.Size = new System.Drawing.Size(60, 17);
        this.statusArticleCount.Text = "0 articles";

        //
        // contextMenuArticles
        //
        this.contextMenuArticles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemOpenUrl,
            this.menuItemCopyUrl,
            this.menuItemCopyTitle,
            this.menuItemSeparator1,
            this.menuItemViewBody,
            this.menuItemSeparator2,
            this.menuItemDelete});
        this.contextMenuArticles.Name = "contextMenuArticles";
        this.contextMenuArticles.Size = new System.Drawing.Size(150, 126);

        //
        // menuItemOpenUrl
        //
        this.menuItemOpenUrl.Name = "menuItemOpenUrl";
        this.menuItemOpenUrl.Size = new System.Drawing.Size(149, 22);
        this.menuItemOpenUrl.Text = "Open URL";

        //
        // menuItemCopyUrl
        //
        this.menuItemCopyUrl.Name = "menuItemCopyUrl";
        this.menuItemCopyUrl.Size = new System.Drawing.Size(149, 22);
        this.menuItemCopyUrl.Text = "Copy URL";

        //
        // menuItemCopyTitle
        //
        this.menuItemCopyTitle.Name = "menuItemCopyTitle";
        this.menuItemCopyTitle.Size = new System.Drawing.Size(149, 22);
        this.menuItemCopyTitle.Text = "Copy Title";

        //
        // menuItemSeparator1
        //
        this.menuItemSeparator1.Name = "menuItemSeparator1";
        this.menuItemSeparator1.Size = new System.Drawing.Size(146, 6);

        //
        // menuItemViewBody
        //
        this.menuItemViewBody.Name = "menuItemViewBody";
        this.menuItemViewBody.Size = new System.Drawing.Size(149, 22);
        this.menuItemViewBody.Text = "View Content";

        //
        // menuItemSeparator2
        //
        this.menuItemSeparator2.Name = "menuItemSeparator2";
        this.menuItemSeparator2.Size = new System.Drawing.Size(146, 6);

        //
        // menuItemDelete
        //
        this.menuItemDelete.Name = "menuItemDelete";
        this.menuItemDelete.Size = new System.Drawing.Size(149, 22);
        this.menuItemDelete.Text = "Delete";

        //
        // MainForm
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1000, 549);
        this.Controls.Add(this.splitContainerMain);
        this.Controls.Add(this.toolStripMain);
        this.Controls.Add(this.statusStrip);
        this.MinimumSize = new System.Drawing.Size(800, 500);
        this.Name = "MainForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "nRun - News Scraper";

        ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
        this.splitContainerMain.Panel1.ResumeLayout(false);
        this.splitContainerMain.Panel2.ResumeLayout(false);
        this.splitContainerMain.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.splitContainerRight)).EndInit();
        this.splitContainerRight.Panel1.ResumeLayout(false);
        this.splitContainerRight.Panel2.ResumeLayout(false);
        this.splitContainerRight.ResumeLayout(false);
        this.panelLeft.ResumeLayout(false);
        this.panelLeft.PerformLayout();
        this.toolStripSites.ResumeLayout(false);
        this.toolStripSites.PerformLayout();
        this.panelRight.ResumeLayout(false);
        this.panelDebug.ResumeLayout(false);
        this.panelDebug.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.olvArticles)).EndInit();
        this.toolStripMain.ResumeLayout(false);
        this.toolStripMain.PerformLayout();
        this.statusStrip.ResumeLayout(false);
        this.statusStrip.PerformLayout();
        this.contextMenuArticles.ResumeLayout(false);
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

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
    private System.Windows.Forms.Label lblSitesHeader;
    private System.Windows.Forms.Label lblDebugHeader;
    private System.Windows.Forms.Button btnClearLog;
    private System.Windows.Forms.TextBox txtDebugLog;
    private BrightIdeasSoftware.ObjectListView olvArticles;
    private BrightIdeasSoftware.OLVColumn olvColTitle;
    private BrightIdeasSoftware.OLVColumn olvColSite;
    private BrightIdeasSoftware.OLVColumn olvColDate;
    private System.Windows.Forms.ToolStrip toolStripMain;
    private System.Windows.Forms.ToolStripButton btnStartStop;
    private System.Windows.Forms.ToolStripButton btnScrapeNow;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripButton btnSettings;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
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
}
