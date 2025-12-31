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
        // GroupBox for scraping settings
        this.groupBoxScraping = new System.Windows.Forms.GroupBox();

        // Labels
        this.lblCheckInterval = new System.Windows.Forms.Label();
        this.lblDelayBetweenLinks = new System.Windows.Forms.Label();
        this.lblMaxArticles = new System.Windows.Forms.Label();
        this.lblBrowserTimeout = new System.Windows.Forms.Label();
        this.lblIntervalMinutes = new System.Windows.Forms.Label();
        this.lblDelaySeconds = new System.Windows.Forms.Label();
        this.lblTimeoutSeconds = new System.Windows.Forms.Label();

        // NumericUpDown controls
        this.numCheckInterval = new System.Windows.Forms.NumericUpDown();
        this.numDelayBetweenLinks = new System.Windows.Forms.NumericUpDown();
        this.numMaxArticles = new System.Windows.Forms.NumericUpDown();
        this.numBrowserTimeout = new System.Windows.Forms.NumericUpDown();

        // Checkboxes
        this.chkUseHeadless = new System.Windows.Forms.CheckBox();
        this.chkAutoStart = new System.Windows.Forms.CheckBox();

        // Database button
        this.btnDatabase = new System.Windows.Forms.Button();

        // Buttons
        this.btnSave = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();

        // Panel
        this.panelButtons = new System.Windows.Forms.Panel();

        this.groupBoxScraping.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.numCheckInterval)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.numDelayBetweenLinks)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.numMaxArticles)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.numBrowserTimeout)).BeginInit();
        this.panelButtons.SuspendLayout();
        this.SuspendLayout();

        //
        // groupBoxScraping
        //
        this.groupBoxScraping.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.groupBoxScraping.Controls.Add(this.lblCheckInterval);
        this.groupBoxScraping.Controls.Add(this.numCheckInterval);
        this.groupBoxScraping.Controls.Add(this.lblIntervalMinutes);
        this.groupBoxScraping.Controls.Add(this.lblDelayBetweenLinks);
        this.groupBoxScraping.Controls.Add(this.numDelayBetweenLinks);
        this.groupBoxScraping.Controls.Add(this.lblDelaySeconds);
        this.groupBoxScraping.Controls.Add(this.lblMaxArticles);
        this.groupBoxScraping.Controls.Add(this.numMaxArticles);
        this.groupBoxScraping.Controls.Add(this.lblBrowserTimeout);
        this.groupBoxScraping.Controls.Add(this.numBrowserTimeout);
        this.groupBoxScraping.Controls.Add(this.lblTimeoutSeconds);
        this.groupBoxScraping.Controls.Add(this.chkUseHeadless);
        this.groupBoxScraping.Controls.Add(this.chkAutoStart);
        this.groupBoxScraping.Controls.Add(this.btnDatabase);
        this.groupBoxScraping.Location = new System.Drawing.Point(15, 15);
        this.groupBoxScraping.Name = "groupBoxScraping";
        this.groupBoxScraping.Size = new System.Drawing.Size(400, 250);
        this.groupBoxScraping.TabIndex = 0;
        this.groupBoxScraping.TabStop = false;
        this.groupBoxScraping.Text = "Scraping Settings";

        //
        // lblCheckInterval
        //
        this.lblCheckInterval.AutoSize = true;
        this.lblCheckInterval.Location = new System.Drawing.Point(15, 30);
        this.lblCheckInterval.Name = "lblCheckInterval";
        this.lblCheckInterval.Size = new System.Drawing.Size(82, 15);
        this.lblCheckInterval.TabIndex = 0;
        this.lblCheckInterval.Text = "Check Interval:";

        //
        // numCheckInterval
        //
        this.numCheckInterval.Location = new System.Drawing.Point(180, 27);
        this.numCheckInterval.Maximum = new decimal(new int[] { 1440, 0, 0, 0 });
        this.numCheckInterval.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.numCheckInterval.Name = "numCheckInterval";
        this.numCheckInterval.Size = new System.Drawing.Size(80, 23);
        this.numCheckInterval.TabIndex = 1;
        this.numCheckInterval.Value = new decimal(new int[] { 5, 0, 0, 0 });

        //
        // lblIntervalMinutes
        //
        this.lblIntervalMinutes.AutoSize = true;
        this.lblIntervalMinutes.Location = new System.Drawing.Point(265, 30);
        this.lblIntervalMinutes.Name = "lblIntervalMinutes";
        this.lblIntervalMinutes.Size = new System.Drawing.Size(50, 15);
        this.lblIntervalMinutes.TabIndex = 2;
        this.lblIntervalMinutes.Text = "minutes";

        //
        // lblDelayBetweenLinks
        //
        this.lblDelayBetweenLinks.AutoSize = true;
        this.lblDelayBetweenLinks.Location = new System.Drawing.Point(15, 60);
        this.lblDelayBetweenLinks.Name = "lblDelayBetweenLinks";
        this.lblDelayBetweenLinks.Size = new System.Drawing.Size(118, 15);
        this.lblDelayBetweenLinks.TabIndex = 3;
        this.lblDelayBetweenLinks.Text = "Delay Between Links:";

        //
        // numDelayBetweenLinks
        //
        this.numDelayBetweenLinks.Location = new System.Drawing.Point(180, 57);
        this.numDelayBetweenLinks.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        this.numDelayBetweenLinks.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
        this.numDelayBetweenLinks.Name = "numDelayBetweenLinks";
        this.numDelayBetweenLinks.Size = new System.Drawing.Size(80, 23);
        this.numDelayBetweenLinks.TabIndex = 4;
        this.numDelayBetweenLinks.Value = new decimal(new int[] { 2, 0, 0, 0 });

        //
        // lblDelaySeconds
        //
        this.lblDelaySeconds.AutoSize = true;
        this.lblDelaySeconds.Location = new System.Drawing.Point(265, 60);
        this.lblDelaySeconds.Name = "lblDelaySeconds";
        this.lblDelaySeconds.Size = new System.Drawing.Size(93, 15);
        this.lblDelaySeconds.TabIndex = 5;
        this.lblDelaySeconds.Text = "seconds (0-10000)";

        //
        // lblMaxArticles
        //
        this.lblMaxArticles.AutoSize = true;
        this.lblMaxArticles.Location = new System.Drawing.Point(15, 90);
        this.lblMaxArticles.Name = "lblMaxArticles";
        this.lblMaxArticles.Size = new System.Drawing.Size(118, 15);
        this.lblMaxArticles.TabIndex = 6;
        this.lblMaxArticles.Text = "Max Articles per Site:";

        //
        // numMaxArticles
        //
        this.numMaxArticles.Location = new System.Drawing.Point(180, 87);
        this.numMaxArticles.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
        this.numMaxArticles.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.numMaxArticles.Name = "numMaxArticles";
        this.numMaxArticles.Size = new System.Drawing.Size(80, 23);
        this.numMaxArticles.TabIndex = 7;
        this.numMaxArticles.Value = new decimal(new int[] { 10, 0, 0, 0 });

        //
        // lblBrowserTimeout
        //
        this.lblBrowserTimeout.AutoSize = true;
        this.lblBrowserTimeout.Location = new System.Drawing.Point(15, 120);
        this.lblBrowserTimeout.Name = "lblBrowserTimeout";
        this.lblBrowserTimeout.Size = new System.Drawing.Size(98, 15);
        this.lblBrowserTimeout.TabIndex = 8;
        this.lblBrowserTimeout.Text = "Browser Timeout:";

        //
        // numBrowserTimeout
        //
        this.numBrowserTimeout.Location = new System.Drawing.Point(180, 117);
        this.numBrowserTimeout.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
        this.numBrowserTimeout.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
        this.numBrowserTimeout.Name = "numBrowserTimeout";
        this.numBrowserTimeout.Size = new System.Drawing.Size(80, 23);
        this.numBrowserTimeout.TabIndex = 9;
        this.numBrowserTimeout.Value = new decimal(new int[] { 30, 0, 0, 0 });

        //
        // lblTimeoutSeconds
        //
        this.lblTimeoutSeconds.AutoSize = true;
        this.lblTimeoutSeconds.Location = new System.Drawing.Point(265, 120);
        this.lblTimeoutSeconds.Name = "lblTimeoutSeconds";
        this.lblTimeoutSeconds.Size = new System.Drawing.Size(50, 15);
        this.lblTimeoutSeconds.TabIndex = 10;
        this.lblTimeoutSeconds.Text = "seconds";

        //
        // chkUseHeadless
        //
        this.chkUseHeadless.AutoSize = true;
        this.chkUseHeadless.Checked = true;
        this.chkUseHeadless.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkUseHeadless.Location = new System.Drawing.Point(15, 155);
        this.chkUseHeadless.Name = "chkUseHeadless";
        this.chkUseHeadless.Size = new System.Drawing.Size(206, 19);
        this.chkUseHeadless.TabIndex = 11;
        this.chkUseHeadless.Text = "Use Headless Browser (no window)";

        //
        // chkAutoStart
        //
        this.chkAutoStart.AutoSize = true;
        this.chkAutoStart.Location = new System.Drawing.Point(15, 180);
        this.chkAutoStart.Name = "chkAutoStart";
        this.chkAutoStart.Size = new System.Drawing.Size(218, 19);
        this.chkAutoStart.TabIndex = 12;
        this.chkAutoStart.Text = "Auto-start scraping when app opens";

        //
        // btnDatabase
        //
        this.btnDatabase.Location = new System.Drawing.Point(15, 210);
        this.btnDatabase.Name = "btnDatabase";
        this.btnDatabase.Size = new System.Drawing.Size(150, 28);
        this.btnDatabase.TabIndex = 13;
        this.btnDatabase.Text = "Database Connection...";
        this.btnDatabase.UseVisualStyleBackColor = true;

        //
        // panelButtons
        //
        this.panelButtons.Controls.Add(this.btnSave);
        this.panelButtons.Controls.Add(this.btnCancel);
        this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.panelButtons.Location = new System.Drawing.Point(0, 280);
        this.panelButtons.Name = "panelButtons";
        this.panelButtons.Padding = new System.Windows.Forms.Padding(10);
        this.panelButtons.Size = new System.Drawing.Size(430, 50);
        this.panelButtons.TabIndex = 1;

        //
        // btnSave
        //
        this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnSave.Location = new System.Drawing.Point(245, 12);
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new System.Drawing.Size(80, 28);
        this.btnSave.TabIndex = 0;
        this.btnSave.Text = "Save";
        this.btnSave.UseVisualStyleBackColor = true;

        //
        // btnCancel
        //
        this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.btnCancel.Location = new System.Drawing.Point(335, 12);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(80, 28);
        this.btnCancel.TabIndex = 1;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;

        //
        // SettingsForm
        //
        this.AcceptButton = this.btnSave;
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.btnCancel;
        this.ClientSize = new System.Drawing.Size(430, 330);
        this.Controls.Add(this.groupBoxScraping);
        this.Controls.Add(this.panelButtons);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "SettingsForm";
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Settings";

        this.groupBoxScraping.ResumeLayout(false);
        this.groupBoxScraping.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.numCheckInterval)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.numDelayBetweenLinks)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.numMaxArticles)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.numBrowserTimeout)).EndInit();
        this.panelButtons.ResumeLayout(false);
        this.ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.GroupBox groupBoxScraping;
    private System.Windows.Forms.Label lblCheckInterval;
    private System.Windows.Forms.Label lblDelayBetweenLinks;
    private System.Windows.Forms.Label lblMaxArticles;
    private System.Windows.Forms.Label lblBrowserTimeout;
    private System.Windows.Forms.Label lblIntervalMinutes;
    private System.Windows.Forms.Label lblDelaySeconds;
    private System.Windows.Forms.Label lblTimeoutSeconds;
    private System.Windows.Forms.NumericUpDown numCheckInterval;
    private System.Windows.Forms.NumericUpDown numDelayBetweenLinks;
    private System.Windows.Forms.NumericUpDown numMaxArticles;
    private System.Windows.Forms.NumericUpDown numBrowserTimeout;
    private System.Windows.Forms.CheckBox chkUseHeadless;
    private System.Windows.Forms.CheckBox chkAutoStart;
    private System.Windows.Forms.Button btnDatabase;
    private System.Windows.Forms.Panel panelButtons;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnCancel;
}
