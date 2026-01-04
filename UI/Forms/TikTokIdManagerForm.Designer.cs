namespace nRun.UI.Forms;

partial class TikTokIdManagerForm
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
        lblUrlPrompt = new Label();
        txtProfileUrl = new TextBox();
        btnFetch = new Button();
        grpProfileInfo = new GroupBox();
        lblUserId = new Label();
        txtUserId = new TextBox();
        lblUsername = new Label();
        txtUsername = new TextBox();
        lblNickname = new Label();
        txtNickname = new TextBox();
        lblRegion = new Label();
        txtRegion = new TextBox();
        lblCreatedAt = new Label();
        txtCreatedAt = new TextBox();
        btnSave = new Button();
        lblStatus = new Label();
        progressBar = new ProgressBar();
        btnCancel = new Button();
        grpBulkImport = new GroupBox();
        olvBulkList = new BrightIdeasSoftware.ObjectListView();
        olvColBulkUsername = new BrightIdeasSoftware.OLVColumn();
        olvColBulkNickname = new BrightIdeasSoftware.OLVColumn();
        olvColBulkStatus = new BrightIdeasSoftware.OLVColumn();
        btnImport = new Button();
        btnExport = new Button();
        btnStartBulk = new Button();
        btnStopBulk = new Button();
        lblDelay = new Label();
        numDelay = new NumericUpDown();
        progressBarBulk = new ProgressBar();
        lblBulkStatus = new Label();
        grpProfileInfo.SuspendLayout();
        grpBulkImport.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)olvBulkList).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numDelay).BeginInit();
        SuspendLayout();
        // 
        // lblUrlPrompt
        // 
        lblUrlPrompt.AutoSize = true;
        lblUrlPrompt.Location = new Point(12, 15);
        lblUrlPrompt.Name = "lblUrlPrompt";
        lblUrlPrompt.Size = new Size(104, 15);
        lblUrlPrompt.TabIndex = 0;
        lblUrlPrompt.Text = "TikTok Profile URL:";
        // 
        // txtProfileUrl
        // 
        txtProfileUrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtProfileUrl.Location = new Point(12, 33);
        txtProfileUrl.Name = "txtProfileUrl";
        txtProfileUrl.PlaceholderText = "https://www.tiktok.com/@username";
        txtProfileUrl.Size = new Size(440, 23);
        txtProfileUrl.TabIndex = 1;
        // 
        // btnFetch
        // 
        btnFetch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnFetch.Location = new Point(458, 32);
        btnFetch.Name = "btnFetch";
        btnFetch.Size = new Size(75, 25);
        btnFetch.TabIndex = 2;
        btnFetch.Text = "Fetch";
        btnFetch.UseVisualStyleBackColor = true;
        // 
        // grpProfileInfo
        // 
        grpProfileInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grpProfileInfo.Controls.Add(lblUserId);
        grpProfileInfo.Controls.Add(txtUserId);
        grpProfileInfo.Controls.Add(lblUsername);
        grpProfileInfo.Controls.Add(txtUsername);
        grpProfileInfo.Controls.Add(lblNickname);
        grpProfileInfo.Controls.Add(txtNickname);
        grpProfileInfo.Controls.Add(lblRegion);
        grpProfileInfo.Controls.Add(txtRegion);
        grpProfileInfo.Controls.Add(lblCreatedAt);
        grpProfileInfo.Controls.Add(txtCreatedAt);
        grpProfileInfo.Controls.Add(btnSave);
        grpProfileInfo.Controls.Add(lblStatus);
        grpProfileInfo.Controls.Add(progressBar);
        grpProfileInfo.Location = new Point(12, 65);
        grpProfileInfo.Name = "grpProfileInfo";
        grpProfileInfo.Size = new Size(521, 175);
        grpProfileInfo.TabIndex = 3;
        grpProfileInfo.TabStop = false;
        grpProfileInfo.Text = "Single Profile";
        // 
        // lblUserId
        // 
        lblUserId.AutoSize = true;
        lblUserId.Location = new Point(15, 25);
        lblUserId.Name = "lblUserId";
        lblUserId.Size = new Size(47, 15);
        lblUserId.TabIndex = 0;
        lblUserId.Text = "User ID:";
        // 
        // txtUserId
        // 
        txtUserId.Location = new Point(100, 22);
        txtUserId.Name = "txtUserId";
        txtUserId.ReadOnly = true;
        txtUserId.Size = new Size(150, 23);
        txtUserId.TabIndex = 1;
        // 
        // lblUsername
        // 
        lblUsername.AutoSize = true;
        lblUsername.Location = new Point(15, 55);
        lblUsername.Name = "lblUsername";
        lblUsername.Size = new Size(63, 15);
        lblUsername.TabIndex = 2;
        lblUsername.Text = "Username:";
        // 
        // txtUsername
        // 
        txtUsername.Location = new Point(100, 52);
        txtUsername.Name = "txtUsername";
        txtUsername.Size = new Size(150, 23);
        txtUsername.TabIndex = 3;
        // 
        // lblNickname
        // 
        lblNickname.AutoSize = true;
        lblNickname.Location = new Point(270, 25);
        lblNickname.Name = "lblNickname";
        lblNickname.Size = new Size(64, 15);
        lblNickname.TabIndex = 4;
        lblNickname.Text = "Nickname:";
        // 
        // txtNickname
        // 
        txtNickname.Location = new Point(340, 22);
        txtNickname.Name = "txtNickname";
        txtNickname.Size = new Size(150, 23);
        txtNickname.TabIndex = 5;
        // 
        // lblRegion
        // 
        lblRegion.AutoSize = true;
        lblRegion.Location = new Point(270, 55);
        lblRegion.Name = "lblRegion";
        lblRegion.Size = new Size(47, 15);
        lblRegion.TabIndex = 6;
        lblRegion.Text = "Region:";
        // 
        // txtRegion
        // 
        txtRegion.Location = new Point(340, 52);
        txtRegion.Name = "txtRegion";
        txtRegion.Size = new Size(150, 23);
        txtRegion.TabIndex = 7;
        // 
        // lblCreatedAt
        // 
        lblCreatedAt.AutoSize = true;
        lblCreatedAt.Location = new Point(15, 85);
        lblCreatedAt.Name = "lblCreatedAt";
        lblCreatedAt.Size = new Size(86, 15);
        lblCreatedAt.TabIndex = 8;
        lblCreatedAt.Text = "Account Since:";
        // 
        // txtCreatedAt
        // 
        txtCreatedAt.Location = new Point(100, 82);
        txtCreatedAt.Name = "txtCreatedAt";
        txtCreatedAt.ReadOnly = true;
        txtCreatedAt.Size = new Size(150, 23);
        txtCreatedAt.TabIndex = 9;
        // 
        // btnSave
        // 
        btnSave.Enabled = false;
        btnSave.Location = new Point(340, 80);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(150, 28);
        btnSave.TabIndex = 10;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.ForeColor = Color.Gray;
        lblStatus.Location = new Point(15, 120);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(185, 15);
        lblStatus.TabIndex = 11;
        lblStatus.Text = "Enter a TikTok URL and click Fetch";
        // 
        // progressBar
        // 
        progressBar.Location = new Point(15, 145);
        progressBar.MarqueeAnimationSpeed = 30;
        progressBar.Name = "progressBar";
        progressBar.Size = new Size(475, 18);
        progressBar.Style = ProgressBarStyle.Marquee;
        progressBar.TabIndex = 12;
        progressBar.Visible = false;
        // 
        // btnCancel
        // 
        btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCancel.Location = new Point(458, 532);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(75, 28);
        btnCancel.TabIndex = 5;
        btnCancel.Text = "Close";
        btnCancel.UseVisualStyleBackColor = true;
        // 
        // grpBulkImport
        // 
        grpBulkImport.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        grpBulkImport.Controls.Add(olvBulkList);
        grpBulkImport.Controls.Add(btnImport);
        grpBulkImport.Controls.Add(btnExport);
        grpBulkImport.Controls.Add(btnStartBulk);
        grpBulkImport.Controls.Add(btnStopBulk);
        grpBulkImport.Controls.Add(lblDelay);
        grpBulkImport.Controls.Add(numDelay);
        grpBulkImport.Controls.Add(progressBarBulk);
        grpBulkImport.Controls.Add(lblBulkStatus);
        grpBulkImport.Location = new Point(12, 246);
        grpBulkImport.Name = "grpBulkImport";
        grpBulkImport.Size = new Size(521, 280);
        grpBulkImport.TabIndex = 4;
        grpBulkImport.TabStop = false;
        grpBulkImport.Text = "Bulk Import (one username per line)";
        // 
        // olvBulkList
        // 
        olvBulkList.AllColumns.Add(olvColBulkUsername);
        olvBulkList.AllColumns.Add(olvColBulkNickname);
        olvBulkList.AllColumns.Add(olvColBulkStatus);
        olvBulkList.AllowDrop = true;
        olvBulkList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        olvBulkList.CellEditUseWholeCell = false;
        olvBulkList.Columns.AddRange(new ColumnHeader[] { olvColBulkUsername, olvColBulkNickname, olvColBulkStatus });
        olvBulkList.FullRowSelect = true;
        olvBulkList.GridLines = true;
        olvBulkList.Location = new Point(15, 55);
        olvBulkList.Name = "olvBulkList";
        olvBulkList.ShowGroups = false;
        olvBulkList.Size = new Size(490, 150);
        olvBulkList.TabIndex = 0;
        olvBulkList.UseCompatibleStateImageBehavior = false;
        olvBulkList.View = View.Details;
        // 
        // olvColBulkUsername
        // 
        olvColBulkUsername.AspectName = "Username";
        olvColBulkUsername.Text = "Username";
        olvColBulkUsername.Width = 130;
        // 
        // olvColBulkNickname
        // 
        olvColBulkNickname.AspectName = "Nickname";
        olvColBulkNickname.Text = "Nickname";
        olvColBulkNickname.Width = 150;
        // 
        // olvColBulkStatus
        // 
        olvColBulkStatus.AspectName = "Status";
        olvColBulkStatus.Text = "Status";
        olvColBulkStatus.Width = 170;
        // 
        // btnImport
        // 
        btnImport.Location = new Point(15, 22);
        btnImport.Name = "btnImport";
        btnImport.Size = new Size(80, 25);
        btnImport.TabIndex = 1;
        btnImport.Text = "Import...";
        btnImport.UseVisualStyleBackColor = true;
        // 
        // btnExport
        // 
        btnExport.Location = new Point(100, 22);
        btnExport.Name = "btnExport";
        btnExport.Size = new Size(80, 25);
        btnExport.TabIndex = 2;
        btnExport.Text = "Export...";
        btnExport.UseVisualStyleBackColor = true;
        // 
        // btnStartBulk
        // 
        btnStartBulk.BackColor = Color.FromArgb(0, 120, 0);
        btnStartBulk.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnStartBulk.ForeColor = Color.White;
        btnStartBulk.Location = new Point(350, 20);
        btnStartBulk.Name = "btnStartBulk";
        btnStartBulk.Size = new Size(75, 28);
        btnStartBulk.TabIndex = 5;
        btnStartBulk.Text = "Start";
        btnStartBulk.UseVisualStyleBackColor = false;
        // 
        // btnStopBulk
        // 
        btnStopBulk.BackColor = Color.FromArgb(180, 0, 0);
        btnStopBulk.Enabled = false;
        btnStopBulk.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnStopBulk.ForeColor = Color.White;
        btnStopBulk.Location = new Point(430, 20);
        btnStopBulk.Name = "btnStopBulk";
        btnStopBulk.Size = new Size(75, 28);
        btnStopBulk.TabIndex = 6;
        btnStopBulk.Text = "Stop";
        btnStopBulk.UseVisualStyleBackColor = false;
        // 
        // lblDelay
        // 
        lblDelay.AutoSize = true;
        lblDelay.Location = new Point(200, 27);
        lblDelay.Name = "lblDelay";
        lblDelay.Size = new Size(67, 15);
        lblDelay.TabIndex = 3;
        lblDelay.Text = "Delay (sec):";
        // 
        // numDelay
        // 
        numDelay.Location = new Point(270, 24);
        numDelay.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
        numDelay.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numDelay.Name = "numDelay";
        numDelay.Size = new Size(60, 23);
        numDelay.TabIndex = 4;
        numDelay.Value = new decimal(new int[] { 10, 0, 0, 0 });
        // 
        // progressBarBulk
        // 
        progressBarBulk.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        progressBarBulk.Location = new Point(15, 215);
        progressBarBulk.Name = "progressBarBulk";
        progressBarBulk.Size = new Size(490, 20);
        progressBarBulk.TabIndex = 7;
        // 
        // lblBulkStatus
        // 
        lblBulkStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lblBulkStatus.ForeColor = Color.Gray;
        lblBulkStatus.Location = new Point(15, 245);
        lblBulkStatus.Name = "lblBulkStatus";
        lblBulkStatus.Size = new Size(490, 25);
        lblBulkStatus.TabIndex = 8;
        lblBulkStatus.Text = "Import a text file with usernames to begin bulk fetch";
        // 
        // TikTokIdManagerForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(545, 572);
        Controls.Add(grpBulkImport);
        Controls.Add(btnCancel);
        Controls.Add(grpProfileInfo);
        Controls.Add(btnFetch);
        Controls.Add(txtProfileUrl);
        Controls.Add(lblUrlPrompt);
        MinimizeBox = false;
        MinimumSize = new Size(561, 611);
        Name = "TikTokIdManagerForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "TikTok ID Manager";
        grpProfileInfo.ResumeLayout(false);
        grpProfileInfo.PerformLayout();
        grpBulkImport.ResumeLayout(false);
        grpBulkImport.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)olvBulkList).EndInit();
        ((System.ComponentModel.ISupportInitialize)numDelay).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblUrlPrompt;
    private TextBox txtProfileUrl;
    private Button btnFetch;
    private GroupBox grpProfileInfo;
    private Label lblUserId;
    private TextBox txtUserId;
    private Label lblUsername;
    private TextBox txtUsername;
    private Label lblNickname;
    private TextBox txtNickname;
    private Label lblRegion;
    private TextBox txtRegion;
    private Label lblCreatedAt;
    private TextBox txtCreatedAt;
    private Button btnSave;
    private Button btnCancel;
    private Label lblStatus;
    private ProgressBar progressBar;
    private GroupBox grpBulkImport;
    private BrightIdeasSoftware.ObjectListView olvBulkList;
    private BrightIdeasSoftware.OLVColumn olvColBulkUsername;
    private BrightIdeasSoftware.OLVColumn olvColBulkNickname;
    private BrightIdeasSoftware.OLVColumn olvColBulkStatus;
    private Button btnImport;
    private Button btnExport;
    private Button btnStartBulk;
    private Button btnStopBulk;
    private Label lblDelay;
    private NumericUpDown numDelay;
    private ProgressBar progressBarBulk;
    private Label lblBulkStatus;
}
