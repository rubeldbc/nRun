namespace nRun.UI.Forms;

partial class DatabaseConnectionForm
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
        // GroupBox for connection settings
        this.groupBoxConnection = new System.Windows.Forms.GroupBox();

        // Labels
        this.lblHost = new System.Windows.Forms.Label();
        this.lblPort = new System.Windows.Forms.Label();
        this.lblDatabase = new System.Windows.Forms.Label();
        this.lblUsername = new System.Windows.Forms.Label();
        this.lblPassword = new System.Windows.Forms.Label();

        // TextBoxes
        this.txtHost = new System.Windows.Forms.TextBox();
        this.numPort = new System.Windows.Forms.NumericUpDown();
        this.txtDatabase = new System.Windows.Forms.TextBox();
        this.txtUsername = new System.Windows.Forms.TextBox();
        this.txtPassword = new System.Windows.Forms.TextBox();

        // Buttons
        this.btnTest = new System.Windows.Forms.Button();
        this.btnConnect = new System.Windows.Forms.Button();
        this.btnCreateTables = new System.Windows.Forms.Button();
        this.btnDeleteDatabase = new System.Windows.Forms.Button();
        this.btnSave = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();

        // Status
        this.lblStatus = new System.Windows.Forms.Label();
        this.txtLog = new System.Windows.Forms.TextBox();

        // Panels
        this.panelButtons = new System.Windows.Forms.Panel();
        this.panelActions = new System.Windows.Forms.Panel();

        this.groupBoxConnection.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
        this.panelButtons.SuspendLayout();
        this.panelActions.SuspendLayout();
        this.SuspendLayout();

        //
        // groupBoxConnection
        //
        this.groupBoxConnection.Controls.Add(this.lblHost);
        this.groupBoxConnection.Controls.Add(this.txtHost);
        this.groupBoxConnection.Controls.Add(this.lblPort);
        this.groupBoxConnection.Controls.Add(this.numPort);
        this.groupBoxConnection.Controls.Add(this.lblDatabase);
        this.groupBoxConnection.Controls.Add(this.txtDatabase);
        this.groupBoxConnection.Controls.Add(this.lblUsername);
        this.groupBoxConnection.Controls.Add(this.txtUsername);
        this.groupBoxConnection.Controls.Add(this.lblPassword);
        this.groupBoxConnection.Controls.Add(this.txtPassword);
        this.groupBoxConnection.Location = new System.Drawing.Point(15, 15);
        this.groupBoxConnection.Name = "groupBoxConnection";
        this.groupBoxConnection.Size = new System.Drawing.Size(420, 180);
        this.groupBoxConnection.TabIndex = 0;
        this.groupBoxConnection.TabStop = false;
        this.groupBoxConnection.Text = "PostgreSQL Connection Settings";

        //
        // lblHost
        //
        this.lblHost.AutoSize = true;
        this.lblHost.Location = new System.Drawing.Point(15, 30);
        this.lblHost.Name = "lblHost";
        this.lblHost.Size = new System.Drawing.Size(35, 15);
        this.lblHost.TabIndex = 0;
        this.lblHost.Text = "Host:";

        //
        // txtHost
        //
        this.txtHost.Location = new System.Drawing.Point(100, 27);
        this.txtHost.Name = "txtHost";
        this.txtHost.Size = new System.Drawing.Size(200, 23);
        this.txtHost.TabIndex = 1;
        this.txtHost.Text = "localhost";

        //
        // lblPort
        //
        this.lblPort.AutoSize = true;
        this.lblPort.Location = new System.Drawing.Point(310, 30);
        this.lblPort.Name = "lblPort";
        this.lblPort.Size = new System.Drawing.Size(32, 15);
        this.lblPort.TabIndex = 2;
        this.lblPort.Text = "Port:";

        //
        // numPort
        //
        this.numPort.Location = new System.Drawing.Point(350, 27);
        this.numPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        this.numPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        this.numPort.Name = "numPort";
        this.numPort.Size = new System.Drawing.Size(55, 23);
        this.numPort.TabIndex = 3;
        this.numPort.Value = new decimal(new int[] { 5432, 0, 0, 0 });

        //
        // lblDatabase
        //
        this.lblDatabase.AutoSize = true;
        this.lblDatabase.Location = new System.Drawing.Point(15, 60);
        this.lblDatabase.Name = "lblDatabase";
        this.lblDatabase.Size = new System.Drawing.Size(58, 15);
        this.lblDatabase.TabIndex = 4;
        this.lblDatabase.Text = "Database:";

        //
        // txtDatabase
        //
        this.txtDatabase.Location = new System.Drawing.Point(100, 57);
        this.txtDatabase.Name = "txtDatabase";
        this.txtDatabase.Size = new System.Drawing.Size(305, 23);
        this.txtDatabase.TabIndex = 5;
        this.txtDatabase.Text = "nrun_db";

        //
        // lblUsername
        //
        this.lblUsername.AutoSize = true;
        this.lblUsername.Location = new System.Drawing.Point(15, 90);
        this.lblUsername.Name = "lblUsername";
        this.lblUsername.Size = new System.Drawing.Size(63, 15);
        this.lblUsername.TabIndex = 6;
        this.lblUsername.Text = "Username:";

        //
        // txtUsername
        //
        this.txtUsername.Location = new System.Drawing.Point(100, 87);
        this.txtUsername.Name = "txtUsername";
        this.txtUsername.Size = new System.Drawing.Size(305, 23);
        this.txtUsername.TabIndex = 7;
        this.txtUsername.Text = "postgres";

        //
        // lblPassword
        //
        this.lblPassword.AutoSize = true;
        this.lblPassword.Location = new System.Drawing.Point(15, 120);
        this.lblPassword.Name = "lblPassword";
        this.lblPassword.Size = new System.Drawing.Size(60, 15);
        this.lblPassword.TabIndex = 8;
        this.lblPassword.Text = "Password:";

        //
        // txtPassword
        //
        this.txtPassword.Location = new System.Drawing.Point(100, 117);
        this.txtPassword.Name = "txtPassword";
        this.txtPassword.PasswordChar = '*';
        this.txtPassword.Size = new System.Drawing.Size(305, 23);
        this.txtPassword.TabIndex = 9;

        //
        // panelActions
        //
        this.panelActions.Controls.Add(this.btnTest);
        this.panelActions.Controls.Add(this.btnConnect);
        this.panelActions.Controls.Add(this.btnCreateTables);
        this.panelActions.Controls.Add(this.btnDeleteDatabase);
        this.panelActions.Controls.Add(this.lblStatus);
        this.panelActions.Location = new System.Drawing.Point(15, 200);
        this.panelActions.Name = "panelActions";
        this.panelActions.Size = new System.Drawing.Size(420, 60);
        this.panelActions.TabIndex = 1;

        //
        // btnTest
        //
        this.btnTest.Location = new System.Drawing.Point(0, 5);
        this.btnTest.Name = "btnTest";
        this.btnTest.Size = new System.Drawing.Size(100, 28);
        this.btnTest.TabIndex = 0;
        this.btnTest.Text = "Test Connection";
        this.btnTest.UseVisualStyleBackColor = true;

        //
        // btnConnect
        //
        this.btnConnect.Location = new System.Drawing.Point(110, 5);
        this.btnConnect.Name = "btnConnect";
        this.btnConnect.Size = new System.Drawing.Size(80, 28);
        this.btnConnect.TabIndex = 1;
        this.btnConnect.Text = "Connect";
        this.btnConnect.UseVisualStyleBackColor = true;

        //
        // btnCreateTables
        //
        this.btnCreateTables.Location = new System.Drawing.Point(200, 5);
        this.btnCreateTables.Name = "btnCreateTables";
        this.btnCreateTables.Size = new System.Drawing.Size(100, 28);
        this.btnCreateTables.TabIndex = 2;
        this.btnCreateTables.Text = "Create Tables";
        this.btnCreateTables.UseVisualStyleBackColor = true;

        //
        // btnDeleteDatabase
        //
        this.btnDeleteDatabase.ForeColor = System.Drawing.Color.DarkRed;
        this.btnDeleteDatabase.Location = new System.Drawing.Point(310, 5);
        this.btnDeleteDatabase.Name = "btnDeleteDatabase";
        this.btnDeleteDatabase.Size = new System.Drawing.Size(105, 28);
        this.btnDeleteDatabase.TabIndex = 3;
        this.btnDeleteDatabase.Text = "Delete Database";
        this.btnDeleteDatabase.UseVisualStyleBackColor = true;

        //
        // lblStatus
        //
        this.lblStatus.AutoSize = true;
        this.lblStatus.ForeColor = System.Drawing.Color.Gray;
        this.lblStatus.Location = new System.Drawing.Point(15, 40);
        this.lblStatus.Name = "lblStatus";
        this.lblStatus.Size = new System.Drawing.Size(84, 15);
        this.lblStatus.TabIndex = 4;
        this.lblStatus.Text = "Not connected";

        //
        // txtLog
        //
        this.txtLog.BackColor = System.Drawing.Color.Black;
        this.txtLog.ForeColor = System.Drawing.Color.LightGreen;
        this.txtLog.Location = new System.Drawing.Point(15, 270);
        this.txtLog.Multiline = true;
        this.txtLog.Name = "txtLog";
        this.txtLog.ReadOnly = true;
        this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.txtLog.Size = new System.Drawing.Size(420, 130);
        this.txtLog.TabIndex = 2;
        this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);

        //
        // panelButtons
        //
        this.panelButtons.Controls.Add(this.btnSave);
        this.panelButtons.Controls.Add(this.btnCancel);
        this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.panelButtons.Location = new System.Drawing.Point(0, 410);
        this.panelButtons.Name = "panelButtons";
        this.panelButtons.Padding = new System.Windows.Forms.Padding(10);
        this.panelButtons.Size = new System.Drawing.Size(450, 50);
        this.panelButtons.TabIndex = 3;

        //
        // btnSave
        //
        this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnSave.Location = new System.Drawing.Point(265, 12);
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
        this.btnCancel.Location = new System.Drawing.Point(355, 12);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(80, 28);
        this.btnCancel.TabIndex = 1;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = true;

        //
        // DatabaseConnectionForm
        //
        this.AcceptButton = this.btnSave;
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.btnCancel;
        this.ClientSize = new System.Drawing.Size(450, 460);
        this.Controls.Add(this.groupBoxConnection);
        this.Controls.Add(this.panelActions);
        this.Controls.Add(this.txtLog);
        this.Controls.Add(this.panelButtons);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "DatabaseConnectionForm";
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Database Connection";

        this.groupBoxConnection.ResumeLayout(false);
        this.groupBoxConnection.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
        this.panelButtons.ResumeLayout(false);
        this.panelActions.ResumeLayout(false);
        this.panelActions.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private System.Windows.Forms.GroupBox groupBoxConnection;
    private System.Windows.Forms.Label lblHost;
    private System.Windows.Forms.Label lblPort;
    private System.Windows.Forms.Label lblDatabase;
    private System.Windows.Forms.Label lblUsername;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.TextBox txtHost;
    private System.Windows.Forms.NumericUpDown numPort;
    private System.Windows.Forms.TextBox txtDatabase;
    private System.Windows.Forms.TextBox txtUsername;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Panel panelActions;
    private System.Windows.Forms.Button btnTest;
    private System.Windows.Forms.Button btnConnect;
    private System.Windows.Forms.Button btnCreateTables;
    private System.Windows.Forms.Button btnDeleteDatabase;
    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.TextBox txtLog;
    private System.Windows.Forms.Panel panelButtons;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnCancel;
}
