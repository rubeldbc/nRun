namespace nRun.UI.Forms;

partial class SettingsForm
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
        groupBoxScraping = new GroupBox();
        lblCheckInterval = new Label();
        numCheckInterval = new NumericUpDown();
        lblIntervalMinutes = new Label();
        lblDelayBetweenLinks = new Label();
        numDelayBetweenLinks = new NumericUpDown();
        lblDelaySeconds = new Label();
        lblMaxArticles = new Label();
        numMaxArticles = new NumericUpDown();
        lblBrowserTimeout = new Label();
        numBrowserTimeout = new NumericUpDown();
        lblTimeoutSeconds = new Label();
        lblMaxDisplayed = new Label();
        numMaxDisplayed = new NumericUpDown();
        lblMaxDisplayedInfo = new Label();
        chkUseHeadless = new CheckBox();
        chkAutoStart = new CheckBox();
        btnDatabase = new Button();
        groupBoxMemurai = new GroupBox();
        lblMemuraiHost = new Label();
        txtMemuraiHost = new TextBox();
        lblMemuraiPort = new Label();
        numMemuraiPort = new NumericUpDown();
        lblMemuraiPassword = new Label();
        txtMemuraiPassword = new TextBox();
        lblMemuraiSyncInterval = new Label();
        numMemuraiSyncInterval = new NumericUpDown();
        lblMemuraiSyncSeconds = new Label();
        btnTestMemurai = new Button();
        lblMemuraiStatus = new Label();
        groupBoxChrome = new GroupBox();
        lblChromeLbl = new Label();
        lblChromeVersion = new Label();
        lblDriverLbl = new Label();
        lblDriverVersion = new Label();
        lblVersionStatus = new Label();
        btnCheckVersion = new Button();
        btnDownloadDriver = new Button();
        progressBarDownload = new ProgressBar();
        btnSave = new Button();
        btnCancel = new Button();
        panelButtons = new Panel();
        groupBoxScraping.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numCheckInterval).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numDelayBetweenLinks).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numMaxArticles).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numBrowserTimeout).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numMaxDisplayed).BeginInit();
        groupBoxMemurai.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numMemuraiPort).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numMemuraiSyncInterval).BeginInit();
        groupBoxChrome.SuspendLayout();
        panelButtons.SuspendLayout();
        SuspendLayout();
        // 
        // groupBoxScraping
        // 
        groupBoxScraping.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        groupBoxScraping.Controls.Add(lblCheckInterval);
        groupBoxScraping.Controls.Add(numCheckInterval);
        groupBoxScraping.Controls.Add(lblIntervalMinutes);
        groupBoxScraping.Controls.Add(lblDelayBetweenLinks);
        groupBoxScraping.Controls.Add(numDelayBetweenLinks);
        groupBoxScraping.Controls.Add(lblDelaySeconds);
        groupBoxScraping.Controls.Add(lblMaxArticles);
        groupBoxScraping.Controls.Add(numMaxArticles);
        groupBoxScraping.Controls.Add(lblBrowserTimeout);
        groupBoxScraping.Controls.Add(numBrowserTimeout);
        groupBoxScraping.Controls.Add(lblTimeoutSeconds);
        groupBoxScraping.Controls.Add(lblMaxDisplayed);
        groupBoxScraping.Controls.Add(numMaxDisplayed);
        groupBoxScraping.Controls.Add(lblMaxDisplayedInfo);
        groupBoxScraping.Controls.Add(chkUseHeadless);
        groupBoxScraping.Controls.Add(chkAutoStart);
        groupBoxScraping.Controls.Add(btnDatabase);
        groupBoxScraping.Location = new Point(15, 15);
        groupBoxScraping.Name = "groupBoxScraping";
        groupBoxScraping.Size = new Size(400, 280);
        groupBoxScraping.TabIndex = 0;
        groupBoxScraping.TabStop = false;
        groupBoxScraping.Text = "Scraping Settings";
        // 
        // lblCheckInterval
        // 
        lblCheckInterval.AutoSize = true;
        lblCheckInterval.Location = new Point(15, 30);
        lblCheckInterval.Name = "lblCheckInterval";
        lblCheckInterval.Size = new Size(85, 15);
        lblCheckInterval.TabIndex = 0;
        lblCheckInterval.Text = "Check Interval:";
        // 
        // numCheckInterval
        // 
        numCheckInterval.Location = new Point(180, 27);
        numCheckInterval.Maximum = new decimal(new int[] { 1440, 0, 0, 0 });
        numCheckInterval.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numCheckInterval.Name = "numCheckInterval";
        numCheckInterval.Size = new Size(80, 23);
        numCheckInterval.TabIndex = 1;
        numCheckInterval.Value = new decimal(new int[] { 5, 0, 0, 0 });
        // 
        // lblIntervalMinutes
        // 
        lblIntervalMinutes.AutoSize = true;
        lblIntervalMinutes.Location = new Point(265, 30);
        lblIntervalMinutes.Name = "lblIntervalMinutes";
        lblIntervalMinutes.Size = new Size(50, 15);
        lblIntervalMinutes.TabIndex = 2;
        lblIntervalMinutes.Text = "minutes";
        // 
        // lblDelayBetweenLinks
        // 
        lblDelayBetweenLinks.AutoSize = true;
        lblDelayBetweenLinks.Location = new Point(15, 60);
        lblDelayBetweenLinks.Name = "lblDelayBetweenLinks";
        lblDelayBetweenLinks.Size = new Size(117, 15);
        lblDelayBetweenLinks.TabIndex = 3;
        lblDelayBetweenLinks.Text = "Delay Between Links:";
        // 
        // numDelayBetweenLinks
        // 
        numDelayBetweenLinks.Location = new Point(180, 57);
        numDelayBetweenLinks.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        numDelayBetweenLinks.Name = "numDelayBetweenLinks";
        numDelayBetweenLinks.Size = new Size(80, 23);
        numDelayBetweenLinks.TabIndex = 4;
        numDelayBetweenLinks.Value = new decimal(new int[] { 2, 0, 0, 0 });
        // 
        // lblDelaySeconds
        // 
        lblDelaySeconds.AutoSize = true;
        lblDelaySeconds.Location = new Point(265, 60);
        lblDelaySeconds.Name = "lblDelaySeconds";
        lblDelaySeconds.Size = new Size(102, 15);
        lblDelaySeconds.TabIndex = 5;
        lblDelaySeconds.Text = "seconds (0-10000)";
        // 
        // lblMaxArticles
        // 
        lblMaxArticles.AutoSize = true;
        lblMaxArticles.Location = new Point(15, 90);
        lblMaxArticles.Name = "lblMaxArticles";
        lblMaxArticles.Size = new Size(116, 15);
        lblMaxArticles.TabIndex = 6;
        lblMaxArticles.Text = "Max Articles per Site:";
        // 
        // numMaxArticles
        // 
        numMaxArticles.Location = new Point(180, 87);
        numMaxArticles.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numMaxArticles.Name = "numMaxArticles";
        numMaxArticles.Size = new Size(80, 23);
        numMaxArticles.TabIndex = 7;
        numMaxArticles.Value = new decimal(new int[] { 10, 0, 0, 0 });
        // 
        // lblBrowserTimeout
        // 
        lblBrowserTimeout.AutoSize = true;
        lblBrowserTimeout.Location = new Point(15, 120);
        lblBrowserTimeout.Name = "lblBrowserTimeout";
        lblBrowserTimeout.Size = new Size(100, 15);
        lblBrowserTimeout.TabIndex = 8;
        lblBrowserTimeout.Text = "Browser Timeout:";
        // 
        // numBrowserTimeout
        // 
        numBrowserTimeout.Location = new Point(180, 117);
        numBrowserTimeout.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
        numBrowserTimeout.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
        numBrowserTimeout.Name = "numBrowserTimeout";
        numBrowserTimeout.Size = new Size(80, 23);
        numBrowserTimeout.TabIndex = 9;
        numBrowserTimeout.Value = new decimal(new int[] { 30, 0, 0, 0 });
        // 
        // lblTimeoutSeconds
        // 
        lblTimeoutSeconds.AutoSize = true;
        lblTimeoutSeconds.Location = new Point(265, 120);
        lblTimeoutSeconds.Name = "lblTimeoutSeconds";
        lblTimeoutSeconds.Size = new Size(50, 15);
        lblTimeoutSeconds.TabIndex = 10;
        lblTimeoutSeconds.Text = "seconds";
        // 
        // lblMaxDisplayed
        // 
        lblMaxDisplayed.AutoSize = true;
        lblMaxDisplayed.Location = new Point(15, 150);
        lblMaxDisplayed.Name = "lblMaxDisplayed";
        lblMaxDisplayed.Size = new Size(128, 15);
        lblMaxDisplayed.TabIndex = 11;
        lblMaxDisplayed.Text = "Max Displayed Articles:";
        // 
        // numMaxDisplayed
        // 
        numMaxDisplayed.Location = new Point(180, 147);
        numMaxDisplayed.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        numMaxDisplayed.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
        numMaxDisplayed.Name = "numMaxDisplayed";
        numMaxDisplayed.Size = new Size(80, 23);
        numMaxDisplayed.TabIndex = 12;
        numMaxDisplayed.Value = new decimal(new int[] { 100, 0, 0, 0 });
        // 
        // lblMaxDisplayedInfo
        // 
        lblMaxDisplayedInfo.AutoSize = true;
        lblMaxDisplayedInfo.Location = new Point(265, 150);
        lblMaxDisplayedInfo.Name = "lblMaxDisplayedInfo";
        lblMaxDisplayedInfo.Size = new Size(56, 15);
        lblMaxDisplayedInfo.TabIndex = 13;
        lblMaxDisplayedInfo.Text = "(10-1000)";
        // 
        // chkUseHeadless
        // 
        chkUseHeadless.AutoSize = true;
        chkUseHeadless.Checked = true;
        chkUseHeadless.CheckState = CheckState.Checked;
        chkUseHeadless.Location = new Point(15, 185);
        chkUseHeadless.Name = "chkUseHeadless";
        chkUseHeadless.Size = new Size(210, 19);
        chkUseHeadless.TabIndex = 14;
        chkUseHeadless.Text = "Use Headless Browser (no window)";
        // 
        // chkAutoStart
        // 
        chkAutoStart.AutoSize = true;
        chkAutoStart.Location = new Point(15, 210);
        chkAutoStart.Name = "chkAutoStart";
        chkAutoStart.Size = new Size(218, 19);
        chkAutoStart.TabIndex = 15;
        chkAutoStart.Text = "Auto-start scraping when app opens";
        // 
        // btnDatabase
        // 
        btnDatabase.Location = new Point(15, 240);
        btnDatabase.Name = "btnDatabase";
        btnDatabase.Size = new Size(150, 28);
        btnDatabase.TabIndex = 16;
        btnDatabase.Text = "Database Connection...";
        btnDatabase.UseVisualStyleBackColor = true;
        // 
        // groupBoxMemurai
        // 
        groupBoxMemurai.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        groupBoxMemurai.Controls.Add(lblMemuraiHost);
        groupBoxMemurai.Controls.Add(txtMemuraiHost);
        groupBoxMemurai.Controls.Add(lblMemuraiPort);
        groupBoxMemurai.Controls.Add(numMemuraiPort);
        groupBoxMemurai.Controls.Add(lblMemuraiPassword);
        groupBoxMemurai.Controls.Add(txtMemuraiPassword);
        groupBoxMemurai.Controls.Add(lblMemuraiSyncInterval);
        groupBoxMemurai.Controls.Add(numMemuraiSyncInterval);
        groupBoxMemurai.Controls.Add(lblMemuraiSyncSeconds);
        groupBoxMemurai.Controls.Add(btnTestMemurai);
        groupBoxMemurai.Controls.Add(lblMemuraiStatus);
        groupBoxMemurai.Location = new Point(15, 305);
        groupBoxMemurai.Name = "groupBoxMemurai";
        groupBoxMemurai.Size = new Size(400, 135);
        groupBoxMemurai.TabIndex = 2;
        groupBoxMemurai.TabStop = false;
        groupBoxMemurai.Text = "Memurai / Redis Server";
        // 
        // lblMemuraiHost
        // 
        lblMemuraiHost.AutoSize = true;
        lblMemuraiHost.Location = new Point(15, 28);
        lblMemuraiHost.Name = "lblMemuraiHost";
        lblMemuraiHost.Size = new Size(35, 15);
        lblMemuraiHost.TabIndex = 0;
        lblMemuraiHost.Text = "Host:";
        // 
        // txtMemuraiHost
        // 
        txtMemuraiHost.Location = new Point(80, 25);
        txtMemuraiHost.Name = "txtMemuraiHost";
        txtMemuraiHost.Size = new Size(130, 23);
        txtMemuraiHost.TabIndex = 1;
        txtMemuraiHost.Text = "localhost";
        // 
        // lblMemuraiPort
        // 
        lblMemuraiPort.AutoSize = true;
        lblMemuraiPort.Location = new Point(220, 28);
        lblMemuraiPort.Name = "lblMemuraiPort";
        lblMemuraiPort.Size = new Size(32, 15);
        lblMemuraiPort.TabIndex = 2;
        lblMemuraiPort.Text = "Port:";
        // 
        // numMemuraiPort
        // 
        numMemuraiPort.Location = new Point(260, 25);
        numMemuraiPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        numMemuraiPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numMemuraiPort.Name = "numMemuraiPort";
        numMemuraiPort.Size = new Size(70, 23);
        numMemuraiPort.TabIndex = 3;
        numMemuraiPort.Value = new decimal(new int[] { 6379, 0, 0, 0 });
        // 
        // lblMemuraiPassword
        // 
        lblMemuraiPassword.AutoSize = true;
        lblMemuraiPassword.Location = new Point(15, 58);
        lblMemuraiPassword.Name = "lblMemuraiPassword";
        lblMemuraiPassword.Size = new Size(60, 15);
        lblMemuraiPassword.TabIndex = 4;
        lblMemuraiPassword.Text = "Password:";
        // 
        // txtMemuraiPassword
        // 
        txtMemuraiPassword.Location = new Point(80, 55);
        txtMemuraiPassword.Name = "txtMemuraiPassword";
        txtMemuraiPassword.PasswordChar = '*';
        txtMemuraiPassword.Size = new Size(130, 23);
        txtMemuraiPassword.TabIndex = 5;
        // 
        // lblMemuraiSyncInterval
        // 
        lblMemuraiSyncInterval.AutoSize = true;
        lblMemuraiSyncInterval.Location = new Point(220, 58);
        lblMemuraiSyncInterval.Name = "lblMemuraiSyncInterval";
        lblMemuraiSyncInterval.Size = new Size(35, 15);
        lblMemuraiSyncInterval.TabIndex = 6;
        lblMemuraiSyncInterval.Text = "Sync:";
        // 
        // numMemuraiSyncInterval
        // 
        numMemuraiSyncInterval.Location = new Point(260, 55);
        numMemuraiSyncInterval.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
        numMemuraiSyncInterval.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
        numMemuraiSyncInterval.Name = "numMemuraiSyncInterval";
        numMemuraiSyncInterval.Size = new Size(70, 23);
        numMemuraiSyncInterval.TabIndex = 7;
        numMemuraiSyncInterval.Value = new decimal(new int[] { 30, 0, 0, 0 });
        // 
        // lblMemuraiSyncSeconds
        // 
        lblMemuraiSyncSeconds.AutoSize = true;
        lblMemuraiSyncSeconds.Location = new Point(335, 58);
        lblMemuraiSyncSeconds.Name = "lblMemuraiSyncSeconds";
        lblMemuraiSyncSeconds.Size = new Size(24, 15);
        lblMemuraiSyncSeconds.TabIndex = 8;
        lblMemuraiSyncSeconds.Text = "sec";
        //
        // btnTestMemurai
        //
        btnTestMemurai.Location = new Point(15, 90);
        btnTestMemurai.Name = "btnTestMemurai";
        btnTestMemurai.Size = new Size(120, 28);
        btnTestMemurai.TabIndex = 9;
        btnTestMemurai.Text = "Test Connection";
        btnTestMemurai.UseVisualStyleBackColor = true;
        //
        // lblMemuraiStatus
        //
        lblMemuraiStatus.Anchor = AnchorStyles.Left;
        lblMemuraiStatus.Location = new Point(145, 90);
        lblMemuraiStatus.Name = "lblMemuraiStatus";
        lblMemuraiStatus.Size = new Size(240, 28);
        lblMemuraiStatus.TabIndex = 10;
        lblMemuraiStatus.Text = "Not tested";
        lblMemuraiStatus.ForeColor = Color.Gray;
        lblMemuraiStatus.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // groupBoxChrome
        // 
        groupBoxChrome.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        groupBoxChrome.Controls.Add(lblChromeLbl);
        groupBoxChrome.Controls.Add(lblChromeVersion);
        groupBoxChrome.Controls.Add(lblDriverLbl);
        groupBoxChrome.Controls.Add(lblDriverVersion);
        groupBoxChrome.Controls.Add(lblVersionStatus);
        groupBoxChrome.Controls.Add(btnCheckVersion);
        groupBoxChrome.Controls.Add(btnDownloadDriver);
        groupBoxChrome.Controls.Add(progressBarDownload);
        groupBoxChrome.Location = new Point(15, 450);
        groupBoxChrome.Name = "groupBoxChrome";
        groupBoxChrome.Size = new Size(400, 145);
        groupBoxChrome.TabIndex = 3;
        groupBoxChrome.TabStop = false;
        groupBoxChrome.Text = "Chrome / ChromeDriver Version";
        // 
        // lblChromeLbl
        // 
        lblChromeLbl.AutoSize = true;
        lblChromeLbl.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblChromeLbl.Location = new Point(15, 28);
        lblChromeLbl.Name = "lblChromeLbl";
        lblChromeLbl.Size = new Size(98, 15);
        lblChromeLbl.TabIndex = 0;
        lblChromeLbl.Text = "Chrome Version:";
        // 
        // lblChromeVersion
        // 
        lblChromeVersion.AutoSize = true;
        lblChromeVersion.ForeColor = Color.DarkBlue;
        lblChromeVersion.Location = new Point(159, 28);
        lblChromeVersion.Name = "lblChromeVersion";
        lblChromeVersion.Size = new Size(66, 15);
        lblChromeVersion.TabIndex = 1;
        lblChromeVersion.Text = "Checking...";
        // 
        // lblDriverLbl
        // 
        lblDriverLbl.AutoSize = true;
        lblDriverLbl.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDriverLbl.Location = new Point(15, 53);
        lblDriverLbl.Name = "lblDriverLbl";
        lblDriverLbl.Size = new Size(134, 15);
        lblDriverLbl.TabIndex = 2;
        lblDriverLbl.Text = "ChromeDriver Version:";
        // 
        // lblDriverVersion
        // 
        lblDriverVersion.AutoSize = true;
        lblDriverVersion.ForeColor = Color.DarkBlue;
        lblDriverVersion.Location = new Point(159, 53);
        lblDriverVersion.Name = "lblDriverVersion";
        lblDriverVersion.Size = new Size(66, 15);
        lblDriverVersion.TabIndex = 3;
        lblDriverVersion.Text = "Checking...";
        // 
        // lblVersionStatus
        // 
        lblVersionStatus.AutoSize = true;
        lblVersionStatus.Location = new Point(15, 78);
        lblVersionStatus.Name = "lblVersionStatus";
        lblVersionStatus.Size = new Size(0, 15);
        lblVersionStatus.TabIndex = 4;
        // 
        // btnCheckVersion
        // 
        btnCheckVersion.Location = new Point(15, 105);
        btnCheckVersion.Name = "btnCheckVersion";
        btnCheckVersion.Size = new Size(100, 28);
        btnCheckVersion.TabIndex = 5;
        btnCheckVersion.Text = "Refresh";
        btnCheckVersion.UseVisualStyleBackColor = true;
        // 
        // btnDownloadDriver
        // 
        btnDownloadDriver.Location = new Point(125, 105);
        btnDownloadDriver.Name = "btnDownloadDriver";
        btnDownloadDriver.Size = new Size(130, 28);
        btnDownloadDriver.TabIndex = 6;
        btnDownloadDriver.Text = "Download";
        btnDownloadDriver.UseVisualStyleBackColor = true;
        // 
        // progressBarDownload
        // 
        progressBarDownload.Location = new Point(265, 105);
        progressBarDownload.Name = "progressBarDownload";
        progressBarDownload.Size = new Size(120, 28);
        progressBarDownload.TabIndex = 7;
        progressBarDownload.Visible = false;
        // 
        // btnSave
        // 
        btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSave.Location = new Point(245, 12);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(80, 28);
        btnSave.TabIndex = 0;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        // 
        // btnCancel
        // 
        btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Location = new Point(335, 12);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(80, 28);
        btnCancel.TabIndex = 1;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        // 
        // panelButtons
        // 
        panelButtons.Controls.Add(btnSave);
        panelButtons.Controls.Add(btnCancel);
        panelButtons.Dock = DockStyle.Bottom;
        panelButtons.Location = new Point(0, 605);
        panelButtons.Name = "panelButtons";
        panelButtons.Padding = new Padding(10);
        panelButtons.Size = new Size(430, 50);
        panelButtons.TabIndex = 4;
        // 
        // SettingsForm
        // 
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(430, 655);
        Controls.Add(groupBoxScraping);
        Controls.Add(groupBoxMemurai);
        Controls.Add(groupBoxChrome);
        Controls.Add(panelButtons);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "SettingsForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Settings";
        groupBoxScraping.ResumeLayout(false);
        groupBoxScraping.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numCheckInterval).EndInit();
        ((System.ComponentModel.ISupportInitialize)numDelayBetweenLinks).EndInit();
        ((System.ComponentModel.ISupportInitialize)numMaxArticles).EndInit();
        ((System.ComponentModel.ISupportInitialize)numBrowserTimeout).EndInit();
        ((System.ComponentModel.ISupportInitialize)numMaxDisplayed).EndInit();
        groupBoxMemurai.ResumeLayout(false);
        groupBoxMemurai.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numMemuraiPort).EndInit();
        ((System.ComponentModel.ISupportInitialize)numMemuraiSyncInterval).EndInit();
        groupBoxChrome.ResumeLayout(false);
        groupBoxChrome.PerformLayout();
        panelButtons.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.GroupBox groupBoxScraping;
    private System.Windows.Forms.GroupBox groupBoxChrome;
    private System.Windows.Forms.Label lblCheckInterval;
    private System.Windows.Forms.Label lblDelayBetweenLinks;
    private System.Windows.Forms.Label lblMaxArticles;
    private System.Windows.Forms.Label lblBrowserTimeout;
    private System.Windows.Forms.Label lblIntervalMinutes;
    private System.Windows.Forms.Label lblDelaySeconds;
    private System.Windows.Forms.Label lblTimeoutSeconds;
    private System.Windows.Forms.Label lblChromeLbl;
    private System.Windows.Forms.Label lblDriverLbl;
    private System.Windows.Forms.Label lblChromeVersion;
    private System.Windows.Forms.Label lblDriverVersion;
    private System.Windows.Forms.Label lblVersionStatus;
    private System.Windows.Forms.NumericUpDown numCheckInterval;
    private System.Windows.Forms.NumericUpDown numDelayBetweenLinks;
    private System.Windows.Forms.NumericUpDown numMaxArticles;
    private System.Windows.Forms.NumericUpDown numBrowserTimeout;
    private System.Windows.Forms.NumericUpDown numMaxDisplayed;
    private System.Windows.Forms.Label lblMaxDisplayed;
    private System.Windows.Forms.Label lblMaxDisplayedInfo;
    private System.Windows.Forms.CheckBox chkUseHeadless;
    private System.Windows.Forms.CheckBox chkAutoStart;
    private System.Windows.Forms.Button btnDatabase;
    private System.Windows.Forms.GroupBox groupBoxMemurai;
    private System.Windows.Forms.Label lblMemuraiHost;
    private System.Windows.Forms.TextBox txtMemuraiHost;
    private System.Windows.Forms.Label lblMemuraiPort;
    private System.Windows.Forms.NumericUpDown numMemuraiPort;
    private System.Windows.Forms.Label lblMemuraiPassword;
    private System.Windows.Forms.TextBox txtMemuraiPassword;
    private System.Windows.Forms.Label lblMemuraiSyncInterval;
    private System.Windows.Forms.NumericUpDown numMemuraiSyncInterval;
    private System.Windows.Forms.Label lblMemuraiSyncSeconds;
    private System.Windows.Forms.Button btnTestMemurai;
    private System.Windows.Forms.Label lblMemuraiStatus;
    private System.Windows.Forms.Button btnCheckVersion;
    private System.Windows.Forms.Button btnDownloadDriver;
    private System.Windows.Forms.ProgressBar progressBarDownload;
    private System.Windows.Forms.Panel panelButtons;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnCancel;
}
