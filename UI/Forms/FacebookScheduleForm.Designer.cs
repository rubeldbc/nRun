namespace nRun.UI.Forms;

partial class FacebookScheduleForm
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
        lblTime = new Label();
        dtpTime = new DateTimePicker();
        chkActive = new CheckBox();
        btnSave = new Button();
        btnCancel = new Button();
        SuspendLayout();
        //
        // lblTime
        //
        lblTime.AutoSize = true;
        lblTime.Location = new Point(20, 25);
        lblTime.Name = "lblTime";
        lblTime.Size = new Size(36, 15);
        lblTime.TabIndex = 0;
        lblTime.Text = "Time:";
        //
        // dtpTime
        //
        dtpTime.CustomFormat = "HH:mm";
        dtpTime.Format = DateTimePickerFormat.Custom;
        dtpTime.Location = new Point(70, 22);
        dtpTime.Name = "dtpTime";
        dtpTime.ShowUpDown = true;
        dtpTime.Size = new Size(80, 23);
        dtpTime.TabIndex = 1;
        //
        // chkActive
        //
        chkActive.AutoSize = true;
        chkActive.Checked = true;
        chkActive.CheckState = CheckState.Checked;
        chkActive.Location = new Point(20, 60);
        chkActive.Name = "chkActive";
        chkActive.Size = new Size(59, 19);
        chkActive.TabIndex = 2;
        chkActive.Text = "Active";
        chkActive.UseVisualStyleBackColor = true;
        //
        // btnSave
        //
        btnSave.Location = new Point(50, 100);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(75, 28);
        btnSave.TabIndex = 3;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        //
        // btnCancel
        //
        btnCancel.Location = new Point(135, 100);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(75, 28);
        btnCancel.TabIndex = 4;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        //
        // FacebookScheduleForm
        //
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(260, 145);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        Controls.Add(chkActive);
        Controls.Add(dtpTime);
        Controls.Add(lblTime);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FacebookScheduleForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Schedule";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblTime;
    private DateTimePicker dtpTime;
    private CheckBox chkActive;
    private Button btnSave;
    private Button btnCancel;
}
