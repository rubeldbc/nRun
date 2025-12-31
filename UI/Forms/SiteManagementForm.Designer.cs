namespace nRun.UI.Forms;

partial class SiteManagementForm
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
        // Main components
        this.panelTop = new System.Windows.Forms.Panel();
        this.toolStrip = new System.Windows.Forms.ToolStrip();
        this.btnImportCsv = new System.Windows.Forms.ToolStripButton();
        this.btnExportCsv = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
        this.btnAddRow = new System.Windows.Forms.ToolStripButton();
        this.btnDeleteSelected = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
        this.btnRefresh = new System.Windows.Forms.ToolStripButton();
        this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
        this.lblInfo = new System.Windows.Forms.ToolStripLabel();

        // ObjectListView
        this.olvSites = new BrightIdeasSoftware.ObjectListView();
        this.olvColSiteName = new BrightIdeasSoftware.OLVColumn();
        this.olvColSiteUrl = new BrightIdeasSoftware.OLVColumn();
        this.olvColArticleSelector = new BrightIdeasSoftware.OLVColumn();
        this.olvColTitleSelector = new BrightIdeasSoftware.OLVColumn();
        this.olvColBodySelector = new BrightIdeasSoftware.OLVColumn();
        this.olvColCategory = new BrightIdeasSoftware.OLVColumn();
        this.olvColCountry = new BrightIdeasSoftware.OLVColumn();
        this.olvColIsActive = new BrightIdeasSoftware.OLVColumn();

        // Bottom panel
        this.panelBottom = new System.Windows.Forms.Panel();
        this.btnSaveAll = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.lblStatus = new System.Windows.Forms.Label();
        this.progressLogos = new System.Windows.Forms.ProgressBar();
        this.lblLogoProgress = new System.Windows.Forms.Label();

        // Log panel
        this.panelLog = new System.Windows.Forms.Panel();
        this.splitContainerLog = new System.Windows.Forms.SplitContainer();
        this.txtLog = new System.Windows.Forms.TextBox();
        this.txtErrorLog = new System.Windows.Forms.TextBox();

        this.panelTop.SuspendLayout();
        this.toolStrip.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.olvSites)).BeginInit();
        this.panelBottom.SuspendLayout();
        this.panelLog.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.splitContainerLog)).BeginInit();
        this.splitContainerLog.Panel1.SuspendLayout();
        this.splitContainerLog.Panel2.SuspendLayout();
        this.splitContainerLog.SuspendLayout();
        this.SuspendLayout();

        //
        // panelTop
        //
        this.panelTop.Controls.Add(this.toolStrip);
        this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelTop.Location = new System.Drawing.Point(0, 0);
        this.panelTop.Name = "panelTop";
        this.panelTop.Size = new System.Drawing.Size(1100, 30);
        this.panelTop.TabIndex = 0;

        //
        // toolStrip
        //
        this.toolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
        this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
        this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnImportCsv,
            this.btnExportCsv,
            this.toolStripSeparator1,
            this.btnAddRow,
            this.btnDeleteSelected,
            this.toolStripSeparator2,
            this.btnRefresh,
            this.toolStripSeparator3,
            this.lblInfo});
        this.toolStrip.Location = new System.Drawing.Point(0, 0);
        this.toolStrip.Name = "toolStrip";
        this.toolStrip.Size = new System.Drawing.Size(1100, 30);
        this.toolStrip.TabIndex = 0;

        //
        // btnImportCsv
        //
        this.btnImportCsv.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnImportCsv.Name = "btnImportCsv";
        this.btnImportCsv.Size = new System.Drawing.Size(75, 27);
        this.btnImportCsv.Text = "Import CSV";
        this.btnImportCsv.ToolTipText = "Import sites from CSV file";

        //
        // btnExportCsv
        //
        this.btnExportCsv.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnExportCsv.Name = "btnExportCsv";
        this.btnExportCsv.Size = new System.Drawing.Size(73, 27);
        this.btnExportCsv.Text = "Export CSV";
        this.btnExportCsv.ToolTipText = "Export sites to CSV file";

        //
        // toolStripSeparator1
        //
        this.toolStripSeparator1.Name = "toolStripSeparator1";
        this.toolStripSeparator1.Size = new System.Drawing.Size(6, 30);

        //
        // btnAddRow
        //
        this.btnAddRow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnAddRow.Name = "btnAddRow";
        this.btnAddRow.Size = new System.Drawing.Size(60, 27);
        this.btnAddRow.Text = "Add New";
        this.btnAddRow.ToolTipText = "Add a new empty row";

        //
        // btnDeleteSelected
        //
        this.btnDeleteSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnDeleteSelected.Name = "btnDeleteSelected";
        this.btnDeleteSelected.Size = new System.Drawing.Size(95, 27);
        this.btnDeleteSelected.Text = "Delete Selected";
        this.btnDeleteSelected.ToolTipText = "Delete selected rows";

        //
        // toolStripSeparator2
        //
        this.toolStripSeparator2.Name = "toolStripSeparator2";
        this.toolStripSeparator2.Size = new System.Drawing.Size(6, 30);

        //
        // btnRefresh
        //
        this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.btnRefresh.Name = "btnRefresh";
        this.btnRefresh.Size = new System.Drawing.Size(50, 27);
        this.btnRefresh.Text = "Refresh";
        this.btnRefresh.ToolTipText = "Reload sites from database";

        //
        // toolStripSeparator3
        //
        this.toolStripSeparator3.Name = "toolStripSeparator3";
        this.toolStripSeparator3.Size = new System.Drawing.Size(6, 30);

        //
        // lblInfo
        //
        this.lblInfo.ForeColor = System.Drawing.Color.Gray;
        this.lblInfo.Name = "lblInfo";
        this.lblInfo.Size = new System.Drawing.Size(200, 27);
        this.lblInfo.Text = "Double-click cells to edit. Press Enter to confirm.";

        //
        // olvSites
        //
        this.olvSites.AllColumns.Add(this.olvColSiteName);
        this.olvSites.AllColumns.Add(this.olvColSiteUrl);
        this.olvSites.AllColumns.Add(this.olvColArticleSelector);
        this.olvSites.AllColumns.Add(this.olvColTitleSelector);
        this.olvSites.AllColumns.Add(this.olvColBodySelector);
        this.olvSites.AllColumns.Add(this.olvColCategory);
        this.olvSites.AllColumns.Add(this.olvColCountry);
        this.olvSites.AllColumns.Add(this.olvColIsActive);
        this.olvSites.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
        this.olvSites.CellEditUseWholeCell = true;
        this.olvSites.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColSiteName,
            this.olvColSiteUrl,
            this.olvColArticleSelector,
            this.olvColTitleSelector,
            this.olvColBodySelector,
            this.olvColCategory,
            this.olvColCountry,
            this.olvColIsActive});
        this.olvSites.Dock = System.Windows.Forms.DockStyle.Fill;
        this.olvSites.FullRowSelect = true;
        this.olvSites.GridLines = true;
        this.olvSites.Location = new System.Drawing.Point(0, 30);
        this.olvSites.Name = "olvSites";
        this.olvSites.ShowGroups = false;
        this.olvSites.Size = new System.Drawing.Size(1100, 470);
        this.olvSites.TabIndex = 1;
        this.olvSites.UseCompatibleStateImageBehavior = false;
        this.olvSites.View = System.Windows.Forms.View.Details;

        //
        // olvColSiteName
        //
        this.olvColSiteName.AspectName = "SiteName";
        this.olvColSiteName.Text = "Site Name";
        this.olvColSiteName.Width = 150;
        this.olvColSiteName.IsEditable = true;

        //
        // olvColSiteUrl
        //
        this.olvColSiteUrl.AspectName = "SiteLink";
        this.olvColSiteUrl.Text = "Site URL";
        this.olvColSiteUrl.Width = 200;
        this.olvColSiteUrl.IsEditable = true;

        //
        // olvColArticleSelector
        //
        this.olvColArticleSelector.AspectName = "ArticleLinkSelector";
        this.olvColArticleSelector.Text = "Article Link Selector";
        this.olvColArticleSelector.Width = 150;
        this.olvColArticleSelector.IsEditable = true;

        //
        // olvColTitleSelector
        //
        this.olvColTitleSelector.AspectName = "TitleSelector";
        this.olvColTitleSelector.Text = "Title Selector";
        this.olvColTitleSelector.Width = 120;
        this.olvColTitleSelector.IsEditable = true;

        //
        // olvColBodySelector
        //
        this.olvColBodySelector.AspectName = "BodySelector";
        this.olvColBodySelector.Text = "Body Selector";
        this.olvColBodySelector.Width = 120;
        this.olvColBodySelector.IsEditable = true;

        //
        // olvColCategory
        //
        this.olvColCategory.AspectName = "SiteCategory";
        this.olvColCategory.Text = "Category";
        this.olvColCategory.Width = 100;
        this.olvColCategory.IsEditable = true;

        //
        // olvColCountry
        //
        this.olvColCountry.AspectName = "SiteCountry";
        this.olvColCountry.Text = "Country";
        this.olvColCountry.Width = 80;
        this.olvColCountry.IsEditable = true;

        //
        // olvColIsActive
        //
        this.olvColIsActive.AspectName = "IsActive";
        this.olvColIsActive.Text = "Active";
        this.olvColIsActive.Width = 60;
        this.olvColIsActive.IsEditable = true;
        this.olvColIsActive.CheckBoxes = true;

        //
        // panelBottom
        //
        this.panelBottom.Controls.Add(this.lblStatus);
        this.panelBottom.Controls.Add(this.progressLogos);
        this.panelBottom.Controls.Add(this.lblLogoProgress);
        this.panelBottom.Controls.Add(this.btnSaveAll);
        this.panelBottom.Controls.Add(this.btnCancel);
        this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.panelBottom.Location = new System.Drawing.Point(0, 500);
        this.panelBottom.Name = "panelBottom";
        this.panelBottom.Padding = new System.Windows.Forms.Padding(10);
        this.panelBottom.Size = new System.Drawing.Size(1100, 50);
        this.panelBottom.TabIndex = 2;

        //
        // lblStatus
        //
        this.lblStatus.AutoSize = true;
        this.lblStatus.Location = new System.Drawing.Point(15, 18);
        this.lblStatus.Name = "lblStatus";
        this.lblStatus.Size = new System.Drawing.Size(100, 15);
        this.lblStatus.TabIndex = 0;
        this.lblStatus.Text = "";

        //
        // btnSaveAll
        //
        this.btnSaveAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnSaveAll.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        this.btnSaveAll.Location = new System.Drawing.Point(900, 12);
        this.btnSaveAll.Name = "btnSaveAll";
        this.btnSaveAll.Size = new System.Drawing.Size(90, 28);
        this.btnSaveAll.TabIndex = 1;
        this.btnSaveAll.Text = "Save All";
        this.btnSaveAll.UseVisualStyleBackColor = true;

        //
        // btnCancel
        //
        this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.btnCancel.Location = new System.Drawing.Point(1000, 12);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(80, 28);
        this.btnCancel.TabIndex = 2;
        this.btnCancel.Text = "Close";
        this.btnCancel.UseVisualStyleBackColor = true;

        //
        // progressLogos
        //
        this.progressLogos.Location = new System.Drawing.Point(300, 16);
        this.progressLogos.Name = "progressLogos";
        this.progressLogos.Size = new System.Drawing.Size(200, 18);
        this.progressLogos.TabIndex = 3;
        this.progressLogos.Visible = false;

        //
        // lblLogoProgress
        //
        this.lblLogoProgress.AutoSize = true;
        this.lblLogoProgress.ForeColor = System.Drawing.Color.Blue;
        this.lblLogoProgress.Location = new System.Drawing.Point(510, 18);
        this.lblLogoProgress.Name = "lblLogoProgress";
        this.lblLogoProgress.Size = new System.Drawing.Size(150, 15);
        this.lblLogoProgress.TabIndex = 4;
        this.lblLogoProgress.Text = "";
        this.lblLogoProgress.Visible = false;

        //
        // panelLog
        //
        this.panelLog.Controls.Add(this.splitContainerLog);
        this.panelLog.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.panelLog.Location = new System.Drawing.Point(0, 400);
        this.panelLog.Name = "panelLog";
        this.panelLog.Size = new System.Drawing.Size(1100, 100);
        this.panelLog.TabIndex = 3;

        //
        // splitContainerLog
        //
        this.splitContainerLog.Dock = System.Windows.Forms.DockStyle.Fill;
        this.splitContainerLog.Location = new System.Drawing.Point(0, 0);
        this.splitContainerLog.Name = "splitContainerLog";
        this.splitContainerLog.Panel1.Controls.Add(this.txtLog);
        this.splitContainerLog.Panel2.Controls.Add(this.txtErrorLog);
        this.splitContainerLog.Size = new System.Drawing.Size(1100, 100);
        this.splitContainerLog.SplitterDistance = 700;
        this.splitContainerLog.TabIndex = 0;

        //
        // txtLog
        //
        this.txtLog.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
        this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
        this.txtLog.Font = new System.Drawing.Font("Consolas", 8.25F);
        this.txtLog.ForeColor = System.Drawing.Color.LightGreen;
        this.txtLog.Location = new System.Drawing.Point(0, 0);
        this.txtLog.Multiline = true;
        this.txtLog.Name = "txtLog";
        this.txtLog.ReadOnly = true;
        this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.txtLog.Size = new System.Drawing.Size(700, 100);
        this.txtLog.TabIndex = 0;

        //
        // txtErrorLog
        //
        this.txtErrorLog.BackColor = System.Drawing.Color.FromArgb(50, 20, 20);
        this.txtErrorLog.Dock = System.Windows.Forms.DockStyle.Fill;
        this.txtErrorLog.Font = new System.Drawing.Font("Consolas", 8.25F);
        this.txtErrorLog.ForeColor = System.Drawing.Color.OrangeRed;
        this.txtErrorLog.Location = new System.Drawing.Point(0, 0);
        this.txtErrorLog.Multiline = true;
        this.txtErrorLog.Name = "txtErrorLog";
        this.txtErrorLog.ReadOnly = true;
        this.txtErrorLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.txtErrorLog.Size = new System.Drawing.Size(396, 100);
        this.txtErrorLog.TabIndex = 0;

        //
        // SiteManagementForm
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.btnCancel;
        this.ClientSize = new System.Drawing.Size(1100, 580);
        this.Controls.Add(this.olvSites);
        this.Controls.Add(this.panelLog);
        this.Controls.Add(this.panelTop);
        this.Controls.Add(this.panelBottom);
        this.MinimumSize = new System.Drawing.Size(900, 500);
        this.Name = "SiteManagementForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Site Management - Batch Edit & Import";

        this.panelTop.ResumeLayout(false);
        this.panelTop.PerformLayout();
        this.toolStrip.ResumeLayout(false);
        this.toolStrip.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.olvSites)).EndInit();
        this.panelBottom.ResumeLayout(false);
        this.panelBottom.PerformLayout();
        this.splitContainerLog.Panel1.ResumeLayout(false);
        this.splitContainerLog.Panel1.PerformLayout();
        this.splitContainerLog.Panel2.ResumeLayout(false);
        this.splitContainerLog.Panel2.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.splitContainerLog)).EndInit();
        this.splitContainerLog.ResumeLayout(false);
        this.panelLog.ResumeLayout(false);
        this.ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Panel panelTop;
    private System.Windows.Forms.ToolStrip toolStrip;
    private System.Windows.Forms.ToolStripButton btnImportCsv;
    private System.Windows.Forms.ToolStripButton btnExportCsv;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton btnAddRow;
    private System.Windows.Forms.ToolStripButton btnDeleteSelected;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripButton btnRefresh;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripLabel lblInfo;
    private BrightIdeasSoftware.ObjectListView olvSites;
    private BrightIdeasSoftware.OLVColumn olvColSiteName;
    private BrightIdeasSoftware.OLVColumn olvColSiteUrl;
    private BrightIdeasSoftware.OLVColumn olvColArticleSelector;
    private BrightIdeasSoftware.OLVColumn olvColTitleSelector;
    private BrightIdeasSoftware.OLVColumn olvColBodySelector;
    private BrightIdeasSoftware.OLVColumn olvColCategory;
    private BrightIdeasSoftware.OLVColumn olvColCountry;
    private BrightIdeasSoftware.OLVColumn olvColIsActive;
    private System.Windows.Forms.Panel panelBottom;
    private System.Windows.Forms.Button btnSaveAll;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.ProgressBar progressLogos;
    private System.Windows.Forms.Label lblLogoProgress;
    private System.Windows.Forms.Panel panelLog;
    private System.Windows.Forms.SplitContainer splitContainerLog;
    private System.Windows.Forms.TextBox txtLog;
    private System.Windows.Forms.TextBox txtErrorLog;
}
