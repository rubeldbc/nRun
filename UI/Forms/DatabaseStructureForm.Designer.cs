namespace nRun.UI.Forms;

partial class DatabaseStructureForm
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
        tabControl = new TabControl();
        tabDatabase = new TabPage();
        tabQueries = new TabPage();
        txtDatabaseStructure = new TextBox();
        btnCopyStructure = new Button();
        cboObjects = new ComboBox();
        lblSelectObject = new Label();
        txtQuery = new TextBox();
        btnCopyQuery = new Button();
        btnExportAll = new Button();
        btnClose = new Button();
        panelBottom = new Panel();
        tabControl.SuspendLayout();
        tabDatabase.SuspendLayout();
        tabQueries.SuspendLayout();
        panelBottom.SuspendLayout();
        SuspendLayout();
        //
        // tabControl
        //
        tabControl.Controls.Add(tabDatabase);
        tabControl.Controls.Add(tabQueries);
        tabControl.Dock = DockStyle.Fill;
        tabControl.Location = new Point(0, 0);
        tabControl.Name = "tabControl";
        tabControl.SelectedIndex = 0;
        tabControl.Size = new Size(800, 500);
        tabControl.TabIndex = 0;
        //
        // tabDatabase
        //
        tabDatabase.Controls.Add(txtDatabaseStructure);
        tabDatabase.Controls.Add(btnCopyStructure);
        tabDatabase.Location = new Point(4, 24);
        tabDatabase.Name = "tabDatabase";
        tabDatabase.Padding = new Padding(10);
        tabDatabase.Size = new Size(792, 472);
        tabDatabase.TabIndex = 0;
        tabDatabase.Text = "Database";
        tabDatabase.UseVisualStyleBackColor = true;
        //
        // txtDatabaseStructure
        //
        txtDatabaseStructure.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        txtDatabaseStructure.BackColor = Color.White;
        txtDatabaseStructure.Font = new Font("Consolas", 9F);
        txtDatabaseStructure.Location = new Point(10, 10);
        txtDatabaseStructure.Multiline = true;
        txtDatabaseStructure.Name = "txtDatabaseStructure";
        txtDatabaseStructure.ReadOnly = true;
        txtDatabaseStructure.ScrollBars = ScrollBars.Both;
        txtDatabaseStructure.Size = new Size(772, 412);
        txtDatabaseStructure.TabIndex = 0;
        txtDatabaseStructure.WordWrap = false;
        //
        // btnCopyStructure
        //
        btnCopyStructure.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCopyStructure.Location = new Point(682, 430);
        btnCopyStructure.Name = "btnCopyStructure";
        btnCopyStructure.Size = new Size(100, 30);
        btnCopyStructure.TabIndex = 1;
        btnCopyStructure.Text = "Copy";
        btnCopyStructure.UseVisualStyleBackColor = true;
        //
        // tabQueries
        //
        tabQueries.Controls.Add(lblSelectObject);
        tabQueries.Controls.Add(cboObjects);
        tabQueries.Controls.Add(txtQuery);
        tabQueries.Controls.Add(btnCopyQuery);
        tabQueries.Controls.Add(btnExportAll);
        tabQueries.Location = new Point(4, 24);
        tabQueries.Name = "tabQueries";
        tabQueries.Padding = new Padding(10);
        tabQueries.Size = new Size(792, 472);
        tabQueries.TabIndex = 1;
        tabQueries.Text = "Queries";
        tabQueries.UseVisualStyleBackColor = true;
        //
        // lblSelectObject
        //
        lblSelectObject.AutoSize = true;
        lblSelectObject.Location = new Point(10, 15);
        lblSelectObject.Name = "lblSelectObject";
        lblSelectObject.Size = new Size(80, 15);
        lblSelectObject.TabIndex = 0;
        lblSelectObject.Text = "Select Object:";
        //
        // cboObjects
        //
        cboObjects.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        cboObjects.DropDownStyle = ComboBoxStyle.DropDownList;
        cboObjects.FormattingEnabled = true;
        cboObjects.Location = new Point(100, 12);
        cboObjects.Name = "cboObjects";
        cboObjects.Size = new Size(682, 23);
        cboObjects.TabIndex = 1;
        //
        // txtQuery
        //
        txtQuery.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        txtQuery.BackColor = Color.White;
        txtQuery.Font = new Font("Consolas", 9F);
        txtQuery.Location = new Point(10, 45);
        txtQuery.Multiline = true;
        txtQuery.Name = "txtQuery";
        txtQuery.ReadOnly = true;
        txtQuery.ScrollBars = ScrollBars.Both;
        txtQuery.Size = new Size(772, 377);
        txtQuery.TabIndex = 2;
        txtQuery.WordWrap = false;
        //
        // btnCopyQuery
        //
        btnCopyQuery.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCopyQuery.Location = new Point(572, 430);
        btnCopyQuery.Name = "btnCopyQuery";
        btnCopyQuery.Size = new Size(100, 30);
        btnCopyQuery.TabIndex = 3;
        btnCopyQuery.Text = "Copy";
        btnCopyQuery.UseVisualStyleBackColor = true;
        //
        // btnExportAll
        //
        btnExportAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnExportAll.Location = new Point(682, 430);
        btnExportAll.Name = "btnExportAll";
        btnExportAll.Size = new Size(100, 30);
        btnExportAll.TabIndex = 4;
        btnExportAll.Text = "Export All";
        btnExportAll.UseVisualStyleBackColor = true;
        //
        // btnClose
        //
        btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.Location = new Point(700, 10);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(90, 30);
        btnClose.TabIndex = 0;
        btnClose.Text = "Close";
        btnClose.UseVisualStyleBackColor = true;
        //
        // panelBottom
        //
        panelBottom.Controls.Add(btnClose);
        panelBottom.Dock = DockStyle.Bottom;
        panelBottom.Location = new Point(0, 500);
        panelBottom.Name = "panelBottom";
        panelBottom.Size = new Size(800, 50);
        panelBottom.TabIndex = 1;
        //
        // DatabaseStructureForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnClose;
        ClientSize = new Size(800, 550);
        Controls.Add(tabControl);
        Controls.Add(panelBottom);
        MinimizeBox = false;
        MinimumSize = new Size(600, 400);
        Name = "DatabaseStructureForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Database Structure";
        tabControl.ResumeLayout(false);
        tabDatabase.ResumeLayout(false);
        tabDatabase.PerformLayout();
        tabQueries.ResumeLayout(false);
        tabQueries.PerformLayout();
        panelBottom.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.TabControl tabControl;
    private System.Windows.Forms.TabPage tabDatabase;
    private System.Windows.Forms.TabPage tabQueries;
    private System.Windows.Forms.TextBox txtDatabaseStructure;
    private System.Windows.Forms.Button btnCopyStructure;
    private System.Windows.Forms.Label lblSelectObject;
    private System.Windows.Forms.ComboBox cboObjects;
    private System.Windows.Forms.TextBox txtQuery;
    private System.Windows.Forms.Button btnCopyQuery;
    private System.Windows.Forms.Button btnExportAll;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Panel panelBottom;
}
