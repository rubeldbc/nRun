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
        tableLayoutConnection = new TableLayoutPanel();
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
        groupBoxActions = new GroupBox();
        btnTest = new Button();
        btnConnect = new Button();
        btnDatabaseStructure = new Button();
        lblStatus = new Label();
        groupBoxTableManagement = new GroupBox();
        groupBoxTikTok = new GroupBox();
        btnCreateTables = new Button();
        btnDeleteDatabase = new Button();
        groupBoxFacebook = new GroupBox();
        btnCreateFbTables = new Button();
        btnDeleteFbTables = new Button();
        groupBoxLog = new GroupBox();
        txtLog = new TextBox();
        panelButtons = new Panel();
        btnSave = new Button();
        btnCancel = new Button();
        groupBoxConnection.SuspendLayout();
        tableLayoutConnection.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numPort).BeginInit();
        groupBoxActions.SuspendLayout();
        groupBoxTableManagement.SuspendLayout();
        groupBoxTikTok.SuspendLayout();
        groupBoxFacebook.SuspendLayout();
        groupBoxLog.SuspendLayout();
        panelButtons.SuspendLayout();
        SuspendLayout();
        //
        // groupBoxConnection
        //
        groupBoxConnection.Controls.Add(tableLayoutConnection);
        groupBoxConnection.Location = new Point(12, 12);
        groupBoxConnection.Name = "groupBoxConnection";
        groupBoxConnection.Size = new Size(400, 155);
        groupBoxConnection.TabIndex = 0;
        groupBoxConnection.TabStop = false;
        groupBoxConnection.Text = "Connection Settings";
        //
        // tableLayoutConnection
        //
        tableLayoutConnection.ColumnCount = 4;
        tableLayoutConnection.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 75F));
        tableLayoutConnection.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
        tableLayoutConnection.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
        tableLayoutConnection.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
        tableLayoutConnection.Controls.Add(lblHost, 0, 0);
        tableLayoutConnection.Controls.Add(txtHost, 1, 0);
        tableLayoutConnection.Controls.Add(lblPort, 2, 0);
        tableLayoutConnection.Controls.Add(numPort, 3, 0);
        tableLayoutConnection.Controls.Add(lblDatabase, 0, 1);
        tableLayoutConnection.Controls.Add(txtDatabase, 1, 1);
        tableLayoutConnection.Controls.Add(lblUsername, 0, 2);
        tableLayoutConnection.Controls.Add(txtUsername, 1, 2);
        tableLayoutConnection.Controls.Add(lblPassword, 0, 3);
        tableLayoutConnection.Controls.Add(txtPassword, 1, 3);
        tableLayoutConnection.SetColumnSpan(txtDatabase, 3);
        tableLayoutConnection.SetColumnSpan(txtUsername, 3);
        tableLayoutConnection.SetColumnSpan(txtPassword, 3);
        tableLayoutConnection.Location = new Point(10, 22);
        tableLayoutConnection.Name = "tableLayoutConnection";
        tableLayoutConnection.RowCount = 4;
        tableLayoutConnection.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tableLayoutConnection.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tableLayoutConnection.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tableLayoutConnection.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tableLayoutConnection.Size = new Size(380, 120);
        tableLayoutConnection.TabIndex = 0;
        //
        // lblHost
        //
        lblHost.Anchor = AnchorStyles.Left;
        lblHost.AutoSize = true;
        lblHost.Location = new Point(3, 7);
        lblHost.Name = "lblHost";
        lblHost.Size = new Size(35, 15);
        lblHost.TabIndex = 0;
        lblHost.Text = "Host:";
        //
        // txtHost
        //
        txtHost.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        txtHost.Location = new Point(78, 3);
        txtHost.Name = "txtHost";
        txtHost.Size = new Size(153, 23);
        txtHost.TabIndex = 1;
        txtHost.Text = "localhost";
        //
        // lblPort
        //
        lblPort.Anchor = AnchorStyles.Left;
        lblPort.AutoSize = true;
        lblPort.Location = new Point(237, 7);
        lblPort.Name = "lblPort";
        lblPort.Size = new Size(32, 15);
        lblPort.TabIndex = 2;
        lblPort.Text = "Port:";
        //
        // numPort
        //
        numPort.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        numPort.Location = new Point(277, 3);
        numPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        numPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numPort.Name = "numPort";
        numPort.Size = new Size(100, 23);
        numPort.TabIndex = 3;
        numPort.Value = new decimal(new int[] { 5432, 0, 0, 0 });
        //
        // lblDatabase
        //
        lblDatabase.Anchor = AnchorStyles.Left;
        lblDatabase.AutoSize = true;
        lblDatabase.Location = new Point(3, 37);
        lblDatabase.Name = "lblDatabase";
        lblDatabase.Size = new Size(58, 15);
        lblDatabase.TabIndex = 4;
        lblDatabase.Text = "Database:";
        //
        // txtDatabase
        //
        txtDatabase.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        txtDatabase.Location = new Point(78, 33);
        txtDatabase.Name = "txtDatabase";
        txtDatabase.Size = new Size(299, 23);
        txtDatabase.TabIndex = 5;
        txtDatabase.Text = "nrun_db";
        //
        // lblUsername
        //
        lblUsername.Anchor = AnchorStyles.Left;
        lblUsername.AutoSize = true;
        lblUsername.Location = new Point(3, 67);
        lblUsername.Name = "lblUsername";
        lblUsername.Size = new Size(63, 15);
        lblUsername.TabIndex = 6;
        lblUsername.Text = "Username:";
        //
        // txtUsername
        //
        txtUsername.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        txtUsername.Location = new Point(78, 63);
        txtUsername.Name = "txtUsername";
        txtUsername.Size = new Size(299, 23);
        txtUsername.TabIndex = 7;
        txtUsername.Text = "postgres";
        //
        // lblPassword
        //
        lblPassword.Anchor = AnchorStyles.Left;
        lblPassword.AutoSize = true;
        lblPassword.Location = new Point(3, 97);
        lblPassword.Name = "lblPassword";
        lblPassword.Size = new Size(60, 15);
        lblPassword.TabIndex = 8;
        lblPassword.Text = "Password:";
        //
        // txtPassword
        //
        txtPassword.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        txtPassword.Location = new Point(78, 93);
        txtPassword.Name = "txtPassword";
        txtPassword.PasswordChar = '*';
        txtPassword.Size = new Size(299, 23);
        txtPassword.TabIndex = 9;
        //
        // groupBoxActions
        //
        groupBoxActions.Controls.Add(btnTest);
        groupBoxActions.Controls.Add(btnConnect);
        groupBoxActions.Controls.Add(btnDatabaseStructure);
        groupBoxActions.Controls.Add(lblStatus);
        groupBoxActions.Location = new Point(420, 12);
        groupBoxActions.Name = "groupBoxActions";
        groupBoxActions.Size = new Size(175, 155);
        groupBoxActions.TabIndex = 1;
        groupBoxActions.TabStop = false;
        groupBoxActions.Text = "Actions";
        //
        // btnTest
        //
        btnTest.Location = new Point(10, 25);
        btnTest.Name = "btnTest";
        btnTest.Size = new Size(155, 28);
        btnTest.TabIndex = 0;
        btnTest.Text = "Test Connection";
        btnTest.UseVisualStyleBackColor = true;
        //
        // btnConnect
        //
        btnConnect.Location = new Point(10, 58);
        btnConnect.Name = "btnConnect";
        btnConnect.Size = new Size(155, 28);
        btnConnect.TabIndex = 1;
        btnConnect.Text = "Connect";
        btnConnect.UseVisualStyleBackColor = true;
        //
        // btnDatabaseStructure
        //
        btnDatabaseStructure.Location = new Point(10, 91);
        btnDatabaseStructure.Name = "btnDatabaseStructure";
        btnDatabaseStructure.Size = new Size(155, 28);
        btnDatabaseStructure.TabIndex = 2;
        btnDatabaseStructure.Text = "Database Structure";
        btnDatabaseStructure.UseVisualStyleBackColor = true;
        //
        // lblStatus
        //
        lblStatus.Location = new Point(10, 125);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(155, 20);
        lblStatus.TabIndex = 3;
        lblStatus.Text = "Not connected";
        lblStatus.ForeColor = Color.Gray;
        lblStatus.TextAlign = ContentAlignment.MiddleCenter;
        //
        // groupBoxTableManagement
        //
        groupBoxTableManagement.Controls.Add(groupBoxTikTok);
        groupBoxTableManagement.Controls.Add(groupBoxFacebook);
        groupBoxTableManagement.Location = new Point(12, 175);
        groupBoxTableManagement.Name = "groupBoxTableManagement";
        groupBoxTableManagement.Size = new Size(583, 85);
        groupBoxTableManagement.TabIndex = 2;
        groupBoxTableManagement.TabStop = false;
        groupBoxTableManagement.Text = "Table Management";
        //
        // groupBoxTikTok
        //
        groupBoxTikTok.Controls.Add(btnCreateTables);
        groupBoxTikTok.Controls.Add(btnDeleteDatabase);
        groupBoxTikTok.Location = new Point(10, 22);
        groupBoxTikTok.Name = "groupBoxTikTok";
        groupBoxTikTok.Size = new Size(275, 55);
        groupBoxTikTok.TabIndex = 0;
        groupBoxTikTok.TabStop = false;
        groupBoxTikTok.Text = "TikTok";
        //
        // btnCreateTables
        //
        btnCreateTables.Location = new Point(10, 20);
        btnCreateTables.Name = "btnCreateTables";
        btnCreateTables.Size = new Size(120, 26);
        btnCreateTables.TabIndex = 0;
        btnCreateTables.Text = "Create Tables";
        btnCreateTables.UseVisualStyleBackColor = true;
        //
        // btnDeleteDatabase
        //
        btnDeleteDatabase.ForeColor = Color.DarkRed;
        btnDeleteDatabase.Location = new Point(140, 20);
        btnDeleteDatabase.Name = "btnDeleteDatabase";
        btnDeleteDatabase.Size = new Size(120, 26);
        btnDeleteDatabase.TabIndex = 1;
        btnDeleteDatabase.Text = "Delete Tables";
        btnDeleteDatabase.UseVisualStyleBackColor = true;
        //
        // groupBoxFacebook
        //
        groupBoxFacebook.Controls.Add(btnCreateFbTables);
        groupBoxFacebook.Controls.Add(btnDeleteFbTables);
        groupBoxFacebook.Location = new Point(295, 22);
        groupBoxFacebook.Name = "groupBoxFacebook";
        groupBoxFacebook.Size = new Size(275, 55);
        groupBoxFacebook.TabIndex = 1;
        groupBoxFacebook.TabStop = false;
        groupBoxFacebook.Text = "Facebook";
        //
        // btnCreateFbTables
        //
        btnCreateFbTables.Location = new Point(10, 20);
        btnCreateFbTables.Name = "btnCreateFbTables";
        btnCreateFbTables.Size = new Size(120, 26);
        btnCreateFbTables.TabIndex = 0;
        btnCreateFbTables.Text = "Create Tables";
        btnCreateFbTables.UseVisualStyleBackColor = true;
        //
        // btnDeleteFbTables
        //
        btnDeleteFbTables.ForeColor = Color.DarkRed;
        btnDeleteFbTables.Location = new Point(140, 20);
        btnDeleteFbTables.Name = "btnDeleteFbTables";
        btnDeleteFbTables.Size = new Size(120, 26);
        btnDeleteFbTables.TabIndex = 1;
        btnDeleteFbTables.Text = "Delete Tables";
        btnDeleteFbTables.UseVisualStyleBackColor = true;
        //
        // groupBoxLog
        //
        groupBoxLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        groupBoxLog.Controls.Add(txtLog);
        groupBoxLog.Location = new Point(12, 268);
        groupBoxLog.Name = "groupBoxLog";
        groupBoxLog.Size = new Size(583, 125);
        groupBoxLog.TabIndex = 3;
        groupBoxLog.TabStop = false;
        groupBoxLog.Text = "Log";
        //
        // txtLog
        //
        txtLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        txtLog.BackColor = Color.Black;
        txtLog.Font = new Font("Consolas", 9F);
        txtLog.ForeColor = Color.LightGreen;
        txtLog.Location = new Point(10, 22);
        txtLog.Multiline = true;
        txtLog.Name = "txtLog";
        txtLog.ReadOnly = true;
        txtLog.ScrollBars = ScrollBars.Vertical;
        txtLog.Size = new Size(563, 95);
        txtLog.TabIndex = 0;
        //
        // panelButtons
        //
        panelButtons.Controls.Add(btnSave);
        panelButtons.Controls.Add(btnCancel);
        panelButtons.Dock = DockStyle.Bottom;
        panelButtons.Location = new Point(0, 400);
        panelButtons.Name = "panelButtons";
        panelButtons.Size = new Size(607, 45);
        panelButtons.TabIndex = 4;
        //
        // btnSave
        //
        btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnSave.Location = new Point(415, 10);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(90, 28);
        btnSave.TabIndex = 0;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        //
        // btnCancel
        //
        btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Location = new Point(511, 10);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(90, 28);
        btnCancel.TabIndex = 1;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        //
        // DatabaseConnectionForm
        //
        AcceptButton = btnSave;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(607, 445);
        Controls.Add(groupBoxConnection);
        Controls.Add(groupBoxActions);
        Controls.Add(groupBoxTableManagement);
        Controls.Add(groupBoxLog);
        Controls.Add(panelButtons);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "DatabaseConnectionForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Database Connection";
        groupBoxConnection.ResumeLayout(false);
        tableLayoutConnection.ResumeLayout(false);
        tableLayoutConnection.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numPort).EndInit();
        groupBoxActions.ResumeLayout(false);
        groupBoxTableManagement.ResumeLayout(false);
        groupBoxTikTok.ResumeLayout(false);
        groupBoxFacebook.ResumeLayout(false);
        groupBoxLog.ResumeLayout(false);
        groupBoxLog.PerformLayout();
        panelButtons.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.GroupBox groupBoxConnection;
    private System.Windows.Forms.TableLayoutPanel tableLayoutConnection;
    private System.Windows.Forms.Label lblHost;
    private System.Windows.Forms.TextBox txtHost;
    private System.Windows.Forms.Label lblPort;
    private System.Windows.Forms.NumericUpDown numPort;
    private System.Windows.Forms.Label lblDatabase;
    private System.Windows.Forms.TextBox txtDatabase;
    private System.Windows.Forms.Label lblUsername;
    private System.Windows.Forms.TextBox txtUsername;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.GroupBox groupBoxActions;
    private System.Windows.Forms.Button btnTest;
    private System.Windows.Forms.Button btnConnect;
    private System.Windows.Forms.Button btnDatabaseStructure;
    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.GroupBox groupBoxTableManagement;
    private System.Windows.Forms.GroupBox groupBoxTikTok;
    private System.Windows.Forms.Button btnCreateTables;
    private System.Windows.Forms.Button btnDeleteDatabase;
    private System.Windows.Forms.GroupBox groupBoxFacebook;
    private System.Windows.Forms.Button btnCreateFbTables;
    private System.Windows.Forms.Button btnDeleteFbTables;
    private System.Windows.Forms.GroupBox groupBoxLog;
    private System.Windows.Forms.TextBox txtLog;
    private System.Windows.Forms.Panel panelButtons;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnCancel;
}
