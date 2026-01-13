namespace nRun.UI.Forms;

partial class LogoExportForm
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
        lblSelectedCount = new Label();
        lblFormat = new Label();
        cboFormat = new ComboBox();
        lblSaveTo = new Label();
        txtSavePath = new TextBox();
        btnBrowse = new Button();
        grpSize = new GroupBox();
        rbOriginalSize = new RadioButton();
        rbCustomSize = new RadioButton();
        lblWidth = new Label();
        numWidth = new NumericUpDown();
        lblHeight = new Label();
        numHeight = new NumericUpDown();
        lblDelay = new Label();
        numDelay = new NumericUpDown();
        lblDelayUnit = new Label();
        progressBar = new ProgressBar();
        lblProgress = new Label();
        lblStatus = new Label();
        btnExport = new Button();
        btnCancel = new Button();
        grpSize.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numWidth).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numHeight).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numDelay).BeginInit();
        SuspendLayout();
        //
        // lblSelectedCount
        //
        lblSelectedCount.AutoSize = true;
        lblSelectedCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblSelectedCount.Location = new Point(12, 15);
        lblSelectedCount.Name = "lblSelectedCount";
        lblSelectedCount.Size = new Size(118, 15);
        lblSelectedCount.TabIndex = 0;
        lblSelectedCount.Text = "Selected: 0 profiles";
        //
        // lblFormat
        //
        lblFormat.AutoSize = true;
        lblFormat.Location = new Point(12, 45);
        lblFormat.Name = "lblFormat";
        lblFormat.Size = new Size(68, 15);
        lblFormat.TabIndex = 1;
        lblFormat.Text = "File Format:";
        //
        // cboFormat
        //
        cboFormat.DropDownStyle = ComboBoxStyle.DropDownList;
        cboFormat.FormattingEnabled = true;
        cboFormat.Items.AddRange(new object[] { "PNG", "WebP", "JPG" });
        cboFormat.Location = new Point(100, 42);
        cboFormat.Name = "cboFormat";
        cboFormat.Size = new Size(100, 23);
        cboFormat.TabIndex = 2;
        //
        // lblSaveTo
        //
        lblSaveTo.AutoSize = true;
        lblSaveTo.Location = new Point(12, 78);
        lblSaveTo.Name = "lblSaveTo";
        lblSaveTo.Size = new Size(49, 15);
        lblSaveTo.TabIndex = 3;
        lblSaveTo.Text = "Save To:";
        //
        // txtSavePath
        //
        txtSavePath.Location = new Point(100, 75);
        txtSavePath.Name = "txtSavePath";
        txtSavePath.Size = new Size(230, 23);
        txtSavePath.TabIndex = 4;
        //
        // btnBrowse
        //
        btnBrowse.Location = new Point(336, 74);
        btnBrowse.Name = "btnBrowse";
        btnBrowse.Size = new Size(32, 25);
        btnBrowse.TabIndex = 5;
        btnBrowse.Text = "...";
        btnBrowse.UseVisualStyleBackColor = true;
        //
        // grpSize
        //
        grpSize.Controls.Add(rbOriginalSize);
        grpSize.Controls.Add(rbCustomSize);
        grpSize.Controls.Add(lblWidth);
        grpSize.Controls.Add(numWidth);
        grpSize.Controls.Add(lblHeight);
        grpSize.Controls.Add(numHeight);
        grpSize.Location = new Point(12, 110);
        grpSize.Name = "grpSize";
        grpSize.Size = new Size(356, 95);
        grpSize.TabIndex = 6;
        grpSize.TabStop = false;
        grpSize.Text = "Size";
        //
        // rbOriginalSize
        //
        rbOriginalSize.AutoSize = true;
        rbOriginalSize.Location = new Point(15, 25);
        rbOriginalSize.Name = "rbOriginalSize";
        rbOriginalSize.Size = new Size(92, 19);
        rbOriginalSize.TabIndex = 0;
        rbOriginalSize.Text = "Original Size";
        rbOriginalSize.UseVisualStyleBackColor = true;
        //
        // rbCustomSize
        //
        rbCustomSize.AutoSize = true;
        rbCustomSize.Checked = true;
        rbCustomSize.Location = new Point(15, 55);
        rbCustomSize.Name = "rbCustomSize";
        rbCustomSize.Size = new Size(91, 19);
        rbCustomSize.TabIndex = 1;
        rbCustomSize.TabStop = true;
        rbCustomSize.Text = "Custom Size";
        rbCustomSize.UseVisualStyleBackColor = true;
        //
        // lblWidth
        //
        lblWidth.AutoSize = true;
        lblWidth.Location = new Point(130, 57);
        lblWidth.Name = "lblWidth";
        lblWidth.Size = new Size(42, 15);
        lblWidth.TabIndex = 2;
        lblWidth.Text = "Width:";
        //
        // numWidth
        //
        numWidth.Location = new Point(178, 54);
        numWidth.Maximum = new decimal(new int[] { 2000, 0, 0, 0 });
        numWidth.Minimum = new decimal(new int[] { 16, 0, 0, 0 });
        numWidth.Name = "numWidth";
        numWidth.Size = new Size(60, 23);
        numWidth.TabIndex = 3;
        numWidth.Value = new decimal(new int[] { 88, 0, 0, 0 });
        //
        // lblHeight
        //
        lblHeight.AutoSize = true;
        lblHeight.Location = new Point(250, 57);
        lblHeight.Name = "lblHeight";
        lblHeight.Size = new Size(46, 15);
        lblHeight.TabIndex = 4;
        lblHeight.Text = "Height:";
        //
        // numHeight
        //
        numHeight.Location = new Point(298, 54);
        numHeight.Maximum = new decimal(new int[] { 2000, 0, 0, 0 });
        numHeight.Minimum = new decimal(new int[] { 16, 0, 0, 0 });
        numHeight.Name = "numHeight";
        numHeight.Size = new Size(60, 23);
        numHeight.TabIndex = 5;
        numHeight.Value = new decimal(new int[] { 88, 0, 0, 0 });
        //
        // lblDelay
        //
        lblDelay.AutoSize = true;
        lblDelay.Location = new Point(12, 218);
        lblDelay.Name = "lblDelay";
        lblDelay.Size = new Size(80, 15);
        lblDelay.TabIndex = 12;
        lblDelay.Text = "Delay (fetch):";
        //
        // numDelay
        //
        numDelay.Location = new Point(100, 215);
        numDelay.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
        numDelay.Minimum = new decimal(new int[] { 3, 0, 0, 0 });
        numDelay.Name = "numDelay";
        numDelay.Size = new Size(60, 23);
        numDelay.TabIndex = 13;
        numDelay.Value = new decimal(new int[] { 10, 0, 0, 0 });
        //
        // lblDelayUnit
        //
        lblDelayUnit.AutoSize = true;
        lblDelayUnit.ForeColor = Color.DimGray;
        lblDelayUnit.Location = new Point(166, 218);
        lblDelayUnit.Name = "lblDelayUnit";
        lblDelayUnit.Size = new Size(50, 15);
        lblDelayUnit.TabIndex = 14;
        lblDelayUnit.Text = "seconds";
        //
        // progressBar
        //
        progressBar.Location = new Point(12, 250);
        progressBar.Name = "progressBar";
        progressBar.Size = new Size(300, 23);
        progressBar.TabIndex = 7;
        //
        // lblProgress
        //
        lblProgress.AutoSize = true;
        lblProgress.Location = new Point(318, 254);
        lblProgress.Name = "lblProgress";
        lblProgress.Size = new Size(24, 15);
        lblProgress.TabIndex = 8;
        lblProgress.Text = "0/0";
        //
        // lblStatus
        //
        lblStatus.AutoSize = true;
        lblStatus.ForeColor = Color.DimGray;
        lblStatus.Location = new Point(12, 280);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(39, 15);
        lblStatus.TabIndex = 9;
        lblStatus.Text = "Ready";
        //
        // btnExport
        //
        btnExport.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnExport.Location = new Point(193, 310);
        btnExport.Name = "btnExport";
        btnExport.Size = new Size(85, 30);
        btnExport.TabIndex = 10;
        btnExport.Text = "Export";
        btnExport.UseVisualStyleBackColor = true;
        //
        // btnCancel
        //
        btnCancel.Location = new Point(284, 310);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(85, 30);
        btnCancel.TabIndex = 11;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        //
        // LogoExportForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(384, 351);
        Controls.Add(lblSelectedCount);
        Controls.Add(lblFormat);
        Controls.Add(cboFormat);
        Controls.Add(lblSaveTo);
        Controls.Add(txtSavePath);
        Controls.Add(btnBrowse);
        Controls.Add(grpSize);
        Controls.Add(lblDelay);
        Controls.Add(numDelay);
        Controls.Add(lblDelayUnit);
        Controls.Add(progressBar);
        Controls.Add(lblProgress);
        Controls.Add(lblStatus);
        Controls.Add(btnExport);
        Controls.Add(btnCancel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "LogoExportForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Export Logos";
        grpSize.ResumeLayout(false);
        grpSize.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numWidth).EndInit();
        ((System.ComponentModel.ISupportInitialize)numHeight).EndInit();
        ((System.ComponentModel.ISupportInitialize)numDelay).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblSelectedCount;
    private Label lblFormat;
    private ComboBox cboFormat;
    private Label lblSaveTo;
    private TextBox txtSavePath;
    private Button btnBrowse;
    private GroupBox grpSize;
    private RadioButton rbOriginalSize;
    private RadioButton rbCustomSize;
    private Label lblWidth;
    private NumericUpDown numWidth;
    private Label lblHeight;
    private NumericUpDown numHeight;
    private Label lblDelay;
    private NumericUpDown numDelay;
    private Label lblDelayUnit;
    private ProgressBar progressBar;
    private Label lblProgress;
    private Label lblStatus;
    private Button btnExport;
    private Button btnCancel;
}
