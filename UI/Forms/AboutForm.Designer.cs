namespace nRun.UI.Forms;

partial class AboutForm
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
        panelHeader = new Panel();
        lblAppName = new Label();
        lblVersion = new Label();
        panelContent = new Panel();
        grpDeveloper = new GroupBox();
        tableLayoutDeveloper = new TableLayoutPanel();
        lblDevNameLabel = new Label();
        lblDevName = new Label();
        lblContactLabel = new Label();
        lnkPhone = new LinkLabel();
        lblSocialLabel = new Label();
        lnkFacebook = new LinkLabel();
        grpAbout = new GroupBox();
        txtDescription = new TextBox();
        grpTechnical = new GroupBox();
        txtTechnical = new TextBox();
        panelFooter = new Panel();
        btnClose = new Button();
        lblCopyright = new Label();
        panelHeader.SuspendLayout();
        panelContent.SuspendLayout();
        grpDeveloper.SuspendLayout();
        tableLayoutDeveloper.SuspendLayout();
        grpAbout.SuspendLayout();
        grpTechnical.SuspendLayout();
        panelFooter.SuspendLayout();
        SuspendLayout();
        //
        // panelHeader
        //
        panelHeader.BackColor = Color.FromArgb(45, 45, 48);
        panelHeader.Controls.Add(lblAppName);
        panelHeader.Controls.Add(lblVersion);
        panelHeader.Dock = DockStyle.Top;
        panelHeader.Location = new Point(0, 0);
        panelHeader.Name = "panelHeader";
        panelHeader.Size = new Size(500, 80);
        panelHeader.TabIndex = 0;
        //
        // lblAppName
        //
        lblAppName.AutoSize = true;
        lblAppName.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        lblAppName.ForeColor = Color.White;
        lblAppName.Location = new Point(20, 15);
        lblAppName.Name = "lblAppName";
        lblAppName.Size = new Size(85, 37);
        lblAppName.TabIndex = 0;
        lblAppName.Text = "nRun";
        //
        // lblVersion
        //
        lblVersion.AutoSize = true;
        lblVersion.Font = new Font("Segoe UI", 10F);
        lblVersion.ForeColor = Color.LightGray;
        lblVersion.Location = new Point(22, 52);
        lblVersion.Name = "lblVersion";
        lblVersion.Size = new Size(82, 19);
        lblVersion.TabIndex = 1;
        lblVersion.Text = "Version 1.0.0";
        //
        // panelContent
        //
        panelContent.AutoScroll = true;
        panelContent.Controls.Add(grpTechnical);
        panelContent.Controls.Add(grpAbout);
        panelContent.Controls.Add(grpDeveloper);
        panelContent.Dock = DockStyle.Fill;
        panelContent.Location = new Point(0, 80);
        panelContent.Name = "panelContent";
        panelContent.Padding = new Padding(15);
        panelContent.Size = new Size(500, 420);
        panelContent.TabIndex = 1;
        //
        // grpDeveloper
        //
        grpDeveloper.Controls.Add(tableLayoutDeveloper);
        grpDeveloper.Dock = DockStyle.Top;
        grpDeveloper.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        grpDeveloper.Location = new Point(15, 15);
        grpDeveloper.Name = "grpDeveloper";
        grpDeveloper.Padding = new Padding(10);
        grpDeveloper.Size = new Size(470, 105);
        grpDeveloper.TabIndex = 0;
        grpDeveloper.TabStop = false;
        grpDeveloper.Text = "Developer Information";
        //
        // tableLayoutDeveloper
        //
        tableLayoutDeveloper.ColumnCount = 2;
        tableLayoutDeveloper.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
        tableLayoutDeveloper.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutDeveloper.Controls.Add(lblDevNameLabel, 0, 0);
        tableLayoutDeveloper.Controls.Add(lblDevName, 1, 0);
        tableLayoutDeveloper.Controls.Add(lblContactLabel, 0, 1);
        tableLayoutDeveloper.Controls.Add(lnkPhone, 1, 1);
        tableLayoutDeveloper.Controls.Add(lblSocialLabel, 0, 2);
        tableLayoutDeveloper.Controls.Add(lnkFacebook, 1, 2);
        tableLayoutDeveloper.Dock = DockStyle.Fill;
        tableLayoutDeveloper.Location = new Point(10, 25);
        tableLayoutDeveloper.Name = "tableLayoutDeveloper";
        tableLayoutDeveloper.RowCount = 3;
        tableLayoutDeveloper.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
        tableLayoutDeveloper.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
        tableLayoutDeveloper.RowStyles.Add(new RowStyle(SizeType.Percent, 33.34F));
        tableLayoutDeveloper.Size = new Size(450, 70);
        tableLayoutDeveloper.TabIndex = 0;
        //
        // lblDevNameLabel
        //
        lblDevNameLabel.AutoSize = true;
        lblDevNameLabel.Dock = DockStyle.Fill;
        lblDevNameLabel.Font = new Font("Segoe UI", 9F);
        lblDevNameLabel.ForeColor = Color.Gray;
        lblDevNameLabel.Location = new Point(3, 0);
        lblDevNameLabel.Name = "lblDevNameLabel";
        lblDevNameLabel.Size = new Size(74, 23);
        lblDevNameLabel.TabIndex = 0;
        lblDevNameLabel.Text = "Name:";
        lblDevNameLabel.TextAlign = ContentAlignment.MiddleLeft;
        //
        // lblDevName
        //
        lblDevName.AutoSize = true;
        lblDevName.Dock = DockStyle.Fill;
        lblDevName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDevName.Location = new Point(83, 0);
        lblDevName.Name = "lblDevName";
        lblDevName.Size = new Size(364, 23);
        lblDevName.TabIndex = 1;
        lblDevName.Text = "Md. Kamrul Islam Rubel";
        lblDevName.TextAlign = ContentAlignment.MiddleLeft;
        //
        // lblContactLabel
        //
        lblContactLabel.AutoSize = true;
        lblContactLabel.Dock = DockStyle.Fill;
        lblContactLabel.Font = new Font("Segoe UI", 9F);
        lblContactLabel.ForeColor = Color.Gray;
        lblContactLabel.Location = new Point(3, 23);
        lblContactLabel.Name = "lblContactLabel";
        lblContactLabel.Size = new Size(74, 23);
        lblContactLabel.TabIndex = 2;
        lblContactLabel.Text = "Contact:";
        lblContactLabel.TextAlign = ContentAlignment.MiddleLeft;
        //
        // lnkPhone
        //
        lnkPhone.AutoSize = true;
        lnkPhone.Dock = DockStyle.Fill;
        lnkPhone.Font = new Font("Segoe UI", 9F);
        lnkPhone.Location = new Point(83, 23);
        lnkPhone.Name = "lnkPhone";
        lnkPhone.Size = new Size(364, 23);
        lnkPhone.TabIndex = 3;
        lnkPhone.TabStop = true;
        lnkPhone.Text = "+8801760002332";
        lnkPhone.TextAlign = ContentAlignment.MiddleLeft;
        lnkPhone.LinkClicked += LnkPhone_LinkClicked;
        //
        // lblSocialLabel
        //
        lblSocialLabel.AutoSize = true;
        lblSocialLabel.Dock = DockStyle.Fill;
        lblSocialLabel.Font = new Font("Segoe UI", 9F);
        lblSocialLabel.ForeColor = Color.Gray;
        lblSocialLabel.Location = new Point(3, 46);
        lblSocialLabel.Name = "lblSocialLabel";
        lblSocialLabel.Size = new Size(74, 24);
        lblSocialLabel.TabIndex = 4;
        lblSocialLabel.Text = "Social:";
        lblSocialLabel.TextAlign = ContentAlignment.MiddleLeft;
        //
        // lnkFacebook
        //
        lnkFacebook.AutoSize = true;
        lnkFacebook.Dock = DockStyle.Fill;
        lnkFacebook.Font = new Font("Segoe UI", 9F);
        lnkFacebook.Location = new Point(83, 46);
        lnkFacebook.Name = "lnkFacebook";
        lnkFacebook.Size = new Size(364, 24);
        lnkFacebook.TabIndex = 5;
        lnkFacebook.TabStop = true;
        lnkFacebook.Text = "fb.com/rubel.social";
        lnkFacebook.TextAlign = ContentAlignment.MiddleLeft;
        lnkFacebook.LinkClicked += LnkFacebook_LinkClicked;
        //
        // grpAbout
        //
        grpAbout.Controls.Add(txtDescription);
        grpAbout.Dock = DockStyle.Top;
        grpAbout.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        grpAbout.Location = new Point(15, 120);
        grpAbout.Name = "grpAbout";
        grpAbout.Padding = new Padding(10);
        grpAbout.Size = new Size(470, 120);
        grpAbout.TabIndex = 1;
        grpAbout.TabStop = false;
        grpAbout.Text = "About nRun";
        //
        // txtDescription
        //
        txtDescription.BackColor = SystemColors.Control;
        txtDescription.BorderStyle = BorderStyle.None;
        txtDescription.Dock = DockStyle.Fill;
        txtDescription.Font = new Font("Segoe UI", 9F);
        txtDescription.Location = new Point(10, 25);
        txtDescription.Multiline = true;
        txtDescription.Name = "txtDescription";
        txtDescription.ReadOnly = true;
        txtDescription.Size = new Size(450, 85);
        txtDescription.TabIndex = 0;
        txtDescription.Text = "nRun is a comprehensive social media data collection and monitoring application. " +
            "It enables automated tracking of TikTok and Facebook profiles, collecting follower counts, " +
            "engagement metrics, and profile information on customizable schedules. " +
            "The application also includes news article scraping capabilities from multiple sources.";
        //
        // grpTechnical
        //
        grpTechnical.Controls.Add(txtTechnical);
        grpTechnical.Dock = DockStyle.Top;
        grpTechnical.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        grpTechnical.Location = new Point(15, 240);
        grpTechnical.Name = "grpTechnical";
        grpTechnical.Padding = new Padding(10);
        grpTechnical.Size = new Size(470, 160);
        grpTechnical.TabIndex = 2;
        grpTechnical.TabStop = false;
        grpTechnical.Text = "Technical Architecture";
        //
        // txtTechnical
        //
        txtTechnical.BackColor = SystemColors.Control;
        txtTechnical.BorderStyle = BorderStyle.None;
        txtTechnical.Dock = DockStyle.Fill;
        txtTechnical.Font = new Font("Consolas", 8.5F);
        txtTechnical.Location = new Point(10, 25);
        txtTechnical.Multiline = true;
        txtTechnical.Name = "txtTechnical";
        txtTechnical.ReadOnly = true;
        txtTechnical.ScrollBars = ScrollBars.Vertical;
        txtTechnical.Size = new Size(450, 125);
        txtTechnical.TabIndex = 0;
        txtTechnical.Text = "Framework:     .NET 8.0 Windows Forms\r\n" +
            "Database:      PostgreSQL with Npgsql\r\n" +
            "Web Scraping:  Selenium WebDriver (Chrome)\r\n" +
            "Image Process: SkiaSharp\r\n" +
            "UI Components: BrightIdeasSoftware ObjectListView\r\n" +
            "Architecture:  Service-oriented with async patterns\r\n" +
            "Rate Limiting: Configurable delays with jitter\r\n" +
            "Scheduling:    Timer-based with customizable intervals";
        //
        // panelFooter
        //
        panelFooter.Controls.Add(btnClose);
        panelFooter.Controls.Add(lblCopyright);
        panelFooter.Dock = DockStyle.Bottom;
        panelFooter.Location = new Point(0, 500);
        panelFooter.Name = "panelFooter";
        panelFooter.Size = new Size(500, 50);
        panelFooter.TabIndex = 2;
        //
        // btnClose
        //
        btnClose.Anchor = AnchorStyles.Right;
        btnClose.Location = new Point(400, 12);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(85, 28);
        btnClose.TabIndex = 0;
        btnClose.Text = "Close";
        btnClose.UseVisualStyleBackColor = true;
        btnClose.Click += BtnClose_Click;
        //
        // lblCopyright
        //
        lblCopyright.AutoSize = true;
        lblCopyright.Font = new Font("Segoe UI", 8F);
        lblCopyright.ForeColor = Color.Gray;
        lblCopyright.Location = new Point(15, 18);
        lblCopyright.Name = "lblCopyright";
        lblCopyright.Size = new Size(200, 13);
        lblCopyright.TabIndex = 1;
        lblCopyright.Text = "\u00a9 2024 Md. Kamrul Islam Rubel. All rights reserved.";
        //
        // AboutForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(500, 550);
        Controls.Add(panelContent);
        Controls.Add(panelFooter);
        Controls.Add(panelHeader);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "AboutForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "About nRun";
        panelHeader.ResumeLayout(false);
        panelHeader.PerformLayout();
        panelContent.ResumeLayout(false);
        grpDeveloper.ResumeLayout(false);
        tableLayoutDeveloper.ResumeLayout(false);
        tableLayoutDeveloper.PerformLayout();
        grpAbout.ResumeLayout(false);
        grpAbout.PerformLayout();
        grpTechnical.ResumeLayout(false);
        grpTechnical.PerformLayout();
        panelFooter.ResumeLayout(false);
        panelFooter.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private Panel panelHeader;
    private Label lblAppName;
    private Label lblVersion;
    private Panel panelContent;
    private GroupBox grpDeveloper;
    private TableLayoutPanel tableLayoutDeveloper;
    private Label lblDevNameLabel;
    private Label lblDevName;
    private Label lblContactLabel;
    private LinkLabel lnkPhone;
    private Label lblSocialLabel;
    private LinkLabel lnkFacebook;
    private GroupBox grpAbout;
    private TextBox txtDescription;
    private GroupBox grpTechnical;
    private TextBox txtTechnical;
    private Panel panelFooter;
    private Button btnClose;
    private Label lblCopyright;
}
