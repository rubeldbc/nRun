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
        groupBoxConnection = new GroupBox();
        lblHost = new Label();
        txtHost = new TextBox();
        lblPort = new Label();
        numPort = new NumericUpDown();
        lblDatabase = new Label();
        txtDatabase = new TextBox();
        lblUsername = new Label();
        txtUsername = new TextBox();
        lblPassword = new Label();
        txtPassword = new TextBox();
        btnTest = new Button();
        btnConnect = new Button();
        btnCreateTables = new Button();
        btnDeleteDatabase = new Button();
        btnSave = new Button();
        btnCancel = new Button();
        lblStatus = new Label();
        txtLog = new TextBox();
        panelButtons = new Panel();
        panelActions = new Panel();
        groupBoxConnection.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numPort).BeginInit();
        panelButtons.SuspendLayout();
        panelActions.SuspendLayout();
        SuspendLayout();
        // 
        // groupBoxConnection
        // 
        groupBoxConnection.Controls.Add(lblHost);
        groupBoxConnection.Controls.Add(txtHost);
        groupBoxConnection.Controls.Add(lblPort);
        groupBoxConnection.Controls.Add(numPort);
        groupBoxConnection.Controls.Add(lblDatabase);
        groupBoxConnection.Controls.Add(txtDatabase);
        groupBoxConnection.Controls.Add(lblUsername);
        groupBoxConnection.Controls.Add(txtUsername);
        groupBoxConnection.Controls.Add(lblPassword);
        groupBoxConnection.Controls.Add(txtPassword);
        groupBoxConnection.Location = new Point(15, 15);
        groupBoxConnection.Name = "groupBoxConnection";
        groupBoxConnection.Size = new Size(440, 180);
        groupBoxConnection.TabIndex = 0;
        groupBoxConnection.TabStop = false;
        groupBoxConnection.Text = "PostgreSQL Connection Settings";
        // 
        // lblHost
        // 
        lblHost.AutoSize = true;
        lblHost.Location = new Point(15, 30);
        lblHost.Name = "lblHost";
        lblHost.Size = new Size(35, 15);
        lblHost.TabIndex = 0;
        lblHost.Text = "Host:";
        // 
        // txtHost
        // 
        txtHost.Location = new Point(100, 27);
        txtHost.Name = "txtHost";
        txtHost.Size = new Size(200, 23);
        txtHost.TabIndex = 1;
        txtHost.Text = "localhost";
        // 
        // lblPort
        // 
        lblPort.AutoSize = true;
        lblPort.Location = new Point(310, 30);
        lblPort.Name = "lblPort";
        lblPort.Size = new Size(32, 15);
        lblPort.TabIndex = 2;
        lblPort.Text = "Port:";
        // 
        // numPort
        // 
        numPort.Location = new Point(350, 27);
        numPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        numPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numPort.Name = "numPort";
        numPort.Size = new Size(55, 23);
        numPort.TabIndex = 3;
        numPort.Value = new decimal(new int[] { 5432, 0, 0, 0 });
        // 
        // lblDatabase
        // 
        lblDatabase.AutoSize = true;
        lblDatabase.Location = new Point(15, 60);
        lblDatabase.Name = "lblDatabase";
        lblDatabase.Size = new Size(58, 15);
        lblDatabase.TabIndex = 4;
        lblDatabase.Text = "Database:";
        // 
        // txtDatabase
        // 
        txtDatabase.Location = new Point(100, 57);
        txtDatabase.Name = "txtDatabase";
        txtDatabase.Size = new Size(325, 23);
        txtDatabase.TabIndex = 5;
        txtDatabase.Text = "nrun_db";
        // 
        // lblUsername
        // 
        lblUsername.AutoSize = true;
        lblUsername.Location = new Point(15, 90);
        lblUsername.Name = "lblUsername";
        lblUsername.Size = new Size(63, 15);
        lblUsername.TabIndex = 6;
        lblUsername.Text = "Username:";
        // 
        // txtUsername
        // 
        txtUsername.Location = new Point(100, 87);
        txtUsername.Name = "txtUsername";
        txtUsername.Size = new Size(325, 23);
        txtUsername.TabIndex = 7;
        txtUsername.Text = "postgres";
        // 
        // lblPassword
        // 
        lblPassword.AutoSize = true;
        lblPassword.Location = new Point(15, 120);
        lblPassword.Name = "lblPassword";
        lblPassword.Size = new Size(60, 15);
        lblPassword.TabIndex = 8;
        lblPassword.Text = "Password:";
        // 
        // txtPassword
        // 
        txtPassword.Location = new Point(100, 117);
        txtPassword.Name = "txtPassword";
        txtPassword.PasswordChar = '*';
        txtPassword.Size = new Size(325, 23);
        txtPassword.TabIndex = 9;
        // 
        // btnTest
        // 
        btnTest.Location = new Point(0, 5);
        btnTest.Name = "btnTest";
        btnTest.Size = new Size(104, 28);
        btnTest.TabIndex = 0;
        btnTest.Text = "Test Connection";
        btnTest.UseVisualStyleBackColor = true;
        // 
        // btnConnect
        // 
        btnConnect.Location = new Point(110, 5);
        btnConnect.Name = "btnConnect";
        btnConnect.Size = new Size(80, 28);
        btnConnect.TabIndex = 1;
        btnConnect.Text = "Connect";
        btnConnect.UseVisualStyleBackColor = true;
        // 
        // btnCreateTables
        // 
        btnCreateTables.Location = new Point(196, 5);
        btnCreateTables.Name = "btnCreateTables";
        btnCreateTables.Size = new Size(161, 28);
        btnCreateTables.TabIndex = 2;
        btnCreateTables.Text = "Create Tables";
        btnCreateTables.UseVisualStyleBackColor = true;
        // 
        // btnDeleteDatabase
        // 
        btnDeleteDatabase.ForeColor = Color.DarkRed;
        btnDeleteDatabase.Location = new Point(363, 5);
        btnDeleteDatabase.Name = "btnDeleteDatabase";
        btnDeleteDatabase.Size = new Size(132, 28);
        btnDeleteDatabase.TabIndex = 3;
        btnDeleteDatabase.Text = "Delete Tables";
        btnDeleteDatabase.UseVisualStyleBackColor = true;
        // 
        // btnSave
        // 
        btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSave.Location = new Point(528, 12);
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
        btnCancel.Location = new Point(618, 12);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(80, 28);
        btnCancel.TabIndex = 1;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.ForeColor = Color.Gray;
        lblStatus.Location = new Point(15, 40);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(86, 15);
        lblStatus.TabIndex = 4;
        lblStatus.Text = "Not connected";
        // 
        // txtLog
        // 
        txtLog.BackColor = Color.Black;
        txtLog.Font = new Font("Consolas", 9F);
        txtLog.ForeColor = Color.LightGreen;
        txtLog.Location = new Point(15, 270);
        txtLog.Multiline = true;
        txtLog.Name = "txtLog";
        txtLog.ReadOnly = true;
        txtLog.ScrollBars = ScrollBars.Vertical;
        txtLog.Size = new Size(440, 130);
        txtLog.TabIndex = 2;
        // 
        // panelButtons
        // 
        panelButtons.Controls.Add(btnSave);
        panelButtons.Controls.Add(btnCancel);
        panelButtons.Dock = DockStyle.Bottom;
        panelButtons.Location = new Point(0, 410);
        panelButtons.Name = "panelButtons";
        panelButtons.Padding = new Padding(10);
        panelButtons.Size = new Size(739, 50);
        panelButtons.TabIndex = 3;
        // 
        // panelActions
        // 
        panelActions.Controls.Add(btnTest);
        panelActions.Controls.Add(btnConnect);
        panelActions.Controls.Add(btnCreateTables);
        panelActions.Controls.Add(btnDeleteDatabase);
        panelActions.Controls.Add(lblStatus);
        panelActions.Location = new Point(15, 200);
        panelActions.Name = "panelActions";
        panelActions.Size = new Size(692, 60);
        panelActions.TabIndex = 1;
        // 
        // DatabaseConnectionForm
        // 
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(739, 460);
        Controls.Add(groupBoxConnection);
        Controls.Add(panelActions);
        Controls.Add(txtLog);
        Controls.Add(panelButtons);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "DatabaseConnectionForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Database Connection";
        groupBoxConnection.ResumeLayout(false);
        groupBoxConnection.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numPort).EndInit();
        panelButtons.ResumeLayout(false);
        panelActions.ResumeLayout(false);
        panelActions.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
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
