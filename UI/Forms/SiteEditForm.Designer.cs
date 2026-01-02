namespace nRun.UI.Forms;

partial class SiteEditForm
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
        // Labels
        this.lblName = new System.Windows.Forms.Label();
        this.lblUrl = new System.Windows.Forms.Label();
        this.lblCategory = new System.Windows.Forms.Label();
        this.lblCountry = new System.Windows.Forms.Label();
        this.lblArticleSelector = new System.Windows.Forms.Label();
        this.lblTitleSelector = new System.Windows.Forms.Label();
        this.lblBodySelector = new System.Windows.Forms.Label();
        this.lblSelectorInfo = new System.Windows.Forms.Label();

        // TextBoxes
        this.txtName = new System.Windows.Forms.TextBox();
        this.txtUrl = new System.Windows.Forms.TextBox();
        this.txtCategory = new System.Windows.Forms.TextBox();
        this.txtCountry = new System.Windows.Forms.TextBox();
        this.txtArticleSelector = new System.Windows.Forms.TextBox();
        this.txtTitleSelector = new System.Windows.Forms.TextBox();
        this.txtBodySelector = new System.Windows.Forms.TextBox();

        // Checkbox
        this.chkIsActive = new System.Windows.Forms.CheckBox();

        // Buttons
        this.btnTestSelectors = new System.Windows.Forms.Button();
        this.btnSave = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.btnPrevious = new System.Windows.Forms.Button();
        this.btnNext = new System.Windows.Forms.Button();

        // Test Results
        this.groupBoxTestResults = new System.Windows.Forms.GroupBox();
        this.txtTestResults = new System.Windows.Forms.TextBox();

        // Panels
        this.panelButtons = new System.Windows.Forms.Panel();
        this.panelMain = new System.Windows.Forms.Panel();
        this.panelNavigation = new System.Windows.Forms.Panel();
        this.lblNavInfo = new System.Windows.Forms.Label();

        this.groupBoxTestResults.SuspendLayout();
        this.panelButtons.SuspendLayout();
        this.panelMain.SuspendLayout();
        this.panelNavigation.SuspendLayout();
        this.SuspendLayout();

        //
        // panelNavigation
        //
        this.panelNavigation.Controls.Add(this.btnPrevious);
        this.panelNavigation.Controls.Add(this.lblNavInfo);
        this.panelNavigation.Controls.Add(this.btnNext);
        this.panelNavigation.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelNavigation.Location = new System.Drawing.Point(0, 0);
        this.panelNavigation.Name = "panelNavigation";
        this.panelNavigation.Size = new System.Drawing.Size(580, 35);
        this.panelNavigation.TabIndex = 0;
        this.panelNavigation.Visible = false;

        //
        // btnPrevious
        //
        this.btnPrevious.Location = new System.Drawing.Point(15, 5);
        this.btnPrevious.Name = "btnPrevious";
        this.btnPrevious.Size = new System.Drawing.Size(100, 25);
        this.btnPrevious.TabIndex = 0;
        this.btnPrevious.Text = "<< Previous";
        this.btnPrevious.UseVisualStyleBackColor = true;

        //
        // lblNavInfo
        //
        this.lblNavInfo.Anchor = System.Windows.Forms.AnchorStyles.Top;
        this.lblNavInfo.Location = new System.Drawing.Point(150, 8);
        this.lblNavInfo.Name = "lblNavInfo";
        this.lblNavInfo.Size = new System.Drawing.Size(280, 20);
        this.lblNavInfo.TabIndex = 1;
        this.lblNavInfo.Text = "Site 1 of 10";
        this.lblNavInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

        //
        // btnNext
        //
        this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnNext.Location = new System.Drawing.Point(465, 5);
        this.btnNext.Name = "btnNext";
        this.btnNext.Size = new System.Drawing.Size(100, 25);
        this.btnNext.TabIndex = 2;
        this.btnNext.Text = "Next >>";
        this.btnNext.UseVisualStyleBackColor = true;

        //
        // panelMain
        //
        this.panelMain.Controls.Add(this.lblName);
        this.panelMain.Controls.Add(this.txtName);
        this.panelMain.Controls.Add(this.lblUrl);
        this.panelMain.Controls.Add(this.txtUrl);
        this.panelMain.Controls.Add(this.lblCategory);
        this.panelMain.Controls.Add(this.txtCategory);
        this.panelMain.Controls.Add(this.lblCountry);
        this.panelMain.Controls.Add(this.txtCountry);
        this.panelMain.Controls.Add(this.chkIsActive);
        this.panelMain.Controls.Add(this.lblSelectorInfo);
        this.panelMain.Controls.Add(this.lblArticleSelector);
        this.panelMain.Controls.Add(this.txtArticleSelector);
        this.panelMain.Controls.Add(this.lblTitleSelector);
        this.panelMain.Controls.Add(this.txtTitleSelector);
        this.panelMain.Controls.Add(this.lblBodySelector);
        this.panelMain.Controls.Add(this.txtBodySelector);
        this.panelMain.Controls.Add(this.btnTestSelectors);
        this.panelMain.Controls.Add(this.groupBoxTestResults);
        this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
        this.panelMain.Location = new System.Drawing.Point(0, 35);
        this.panelMain.Name = "panelMain";
        this.panelMain.Padding = new System.Windows.Forms.Padding(15);
        this.panelMain.Size = new System.Drawing.Size(580, 515);
        this.panelMain.TabIndex = 1;

        //
        // lblName
        //
        this.lblName.AutoSize = true;
        this.lblName.Location = new System.Drawing.Point(15, 15);
        this.lblName.Name = "lblName";
        this.lblName.Size = new System.Drawing.Size(66, 15);
        this.lblName.TabIndex = 0;
        this.lblName.Text = "Site Name:";

        //
        // txtName
        //
        this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.txtName.Location = new System.Drawing.Point(130, 12);
        this.txtName.Name = "txtName";
        this.txtName.Size = new System.Drawing.Size(435, 23);
        this.txtName.TabIndex = 1;

        //
        // lblUrl
        //
        this.lblUrl.AutoSize = true;
        this.lblUrl.Location = new System.Drawing.Point(15, 45);
        this.lblUrl.Name = "lblUrl";
        this.lblUrl.Size = new System.Drawing.Size(55, 15);
        this.lblUrl.TabIndex = 2;
        this.lblUrl.Text = "Site URL:";

        //
        // txtUrl
        //
        this.txtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.txtUrl.Location = new System.Drawing.Point(130, 42);
        this.txtUrl.Name = "txtUrl";
        this.txtUrl.Size = new System.Drawing.Size(435, 23);
        this.txtUrl.TabIndex = 3;

        //
        // lblCategory
        //
        this.lblCategory.AutoSize = true;
        this.lblCategory.Location = new System.Drawing.Point(15, 75);
        this.lblCategory.Name = "lblCategory";
        this.lblCategory.Size = new System.Drawing.Size(58, 15);
        this.lblCategory.TabIndex = 4;
        this.lblCategory.Text = "Category:";

        //
        // txtCategory
        //
        this.txtCategory.Location = new System.Drawing.Point(130, 72);
        this.txtCategory.Name = "txtCategory";
        this.txtCategory.Size = new System.Drawing.Size(150, 23);
        this.txtCategory.TabIndex = 5;
        this.txtCategory.PlaceholderText = "e.g., Technology, Sports";

        //
        // lblCountry
        //
        this.lblCountry.AutoSize = true;
        this.lblCountry.Location = new System.Drawing.Point(295, 75);
        this.lblCountry.Name = "lblCountry";
        this.lblCountry.Size = new System.Drawing.Size(53, 15);
        this.lblCountry.TabIndex = 6;
        this.lblCountry.Text = "Country:";

        //
        // txtCountry
        //
        this.txtCountry.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.txtCountry.Location = new System.Drawing.Point(355, 72);
        this.txtCountry.Name = "txtCountry";
        this.txtCountry.Size = new System.Drawing.Size(210, 23);
        this.txtCountry.TabIndex = 7;
        this.txtCountry.PlaceholderText = "e.g., USA, Bangladesh";

        //
        // chkIsActive
        //
        this.chkIsActive.AutoSize = true;
        this.chkIsActive.Checked = true;
        this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkIsActive.Location = new System.Drawing.Point(130, 102);
        this.chkIsActive.Name = "chkIsActive";
        this.chkIsActive.Size = new System.Drawing.Size(59, 19);
        this.chkIsActive.TabIndex = 8;
        this.chkIsActive.Text = "Active";

        //
        // lblSelectorInfo
        //
        this.lblSelectorInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.lblSelectorInfo.ForeColor = System.Drawing.Color.Gray;
        this.lblSelectorInfo.Location = new System.Drawing.Point(15, 128);
        this.lblSelectorInfo.Name = "lblSelectorInfo";
        this.lblSelectorInfo.Size = new System.Drawing.Size(550, 30);
        this.lblSelectorInfo.TabIndex = 9;
        this.lblSelectorInfo.Text = "CSS Selectors are used to find elements on the page. Examples: .article-link, #main-title, article h1";

        //
        // lblArticleSelector
        //
        this.lblArticleSelector.AutoSize = true;
        this.lblArticleSelector.Location = new System.Drawing.Point(15, 168);
        this.lblArticleSelector.Name = "lblArticleSelector";
        this.lblArticleSelector.Size = new System.Drawing.Size(108, 15);
        this.lblArticleSelector.TabIndex = 10;
        this.lblArticleSelector.Text = "Article Link Selector:";

        //
        // txtArticleSelector
        //
        this.txtArticleSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.txtArticleSelector.Location = new System.Drawing.Point(130, 165);
        this.txtArticleSelector.Name = "txtArticleSelector";
        this.txtArticleSelector.Size = new System.Drawing.Size(435, 23);
        this.txtArticleSelector.TabIndex = 11;
        this.txtArticleSelector.PlaceholderText = "e.g., .article-list a, article a.title-link";

        //
        // lblTitleSelector
        //
        this.lblTitleSelector.AutoSize = true;
        this.lblTitleSelector.Location = new System.Drawing.Point(15, 198);
        this.lblTitleSelector.Name = "lblTitleSelector";
        this.lblTitleSelector.Size = new System.Drawing.Size(78, 15);
        this.lblTitleSelector.TabIndex = 12;
        this.lblTitleSelector.Text = "Title Selector:";

        //
        // txtTitleSelector
        //
        this.txtTitleSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.txtTitleSelector.Location = new System.Drawing.Point(130, 195);
        this.txtTitleSelector.Name = "txtTitleSelector";
        this.txtTitleSelector.Size = new System.Drawing.Size(435, 23);
        this.txtTitleSelector.TabIndex = 13;
        this.txtTitleSelector.PlaceholderText = "e.g., h1.title, article h1, .headline";

        //
        // lblBodySelector
        //
        this.lblBodySelector.AutoSize = true;
        this.lblBodySelector.Location = new System.Drawing.Point(15, 228);
        this.lblBodySelector.Name = "lblBodySelector";
        this.lblBodySelector.Size = new System.Drawing.Size(80, 15);
        this.lblBodySelector.TabIndex = 14;
        this.lblBodySelector.Text = "Body Selector:";

        //
        // txtBodySelector
        //
        this.txtBodySelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.txtBodySelector.Location = new System.Drawing.Point(130, 225);
        this.txtBodySelector.Name = "txtBodySelector";
        this.txtBodySelector.Size = new System.Drawing.Size(435, 23);
        this.txtBodySelector.TabIndex = 15;
        this.txtBodySelector.PlaceholderText = "e.g., .article-content, div.body-text, article p";

        //
        // btnTestSelectors
        //
        this.btnTestSelectors.Location = new System.Drawing.Point(130, 258);
        this.btnTestSelectors.Name = "btnTestSelectors";
        this.btnTestSelectors.Size = new System.Drawing.Size(120, 28);
        this.btnTestSelectors.TabIndex = 16;
        this.btnTestSelectors.Text = "Test Selectors";
        this.btnTestSelectors.UseVisualStyleBackColor = true;

        //
        // groupBoxTestResults
        //
        this.groupBoxTestResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.groupBoxTestResults.Controls.Add(this.txtTestResults);
        this.groupBoxTestResults.Location = new System.Drawing.Point(15, 295);
        this.groupBoxTestResults.Name = "groupBoxTestResults";
        this.groupBoxTestResults.Size = new System.Drawing.Size(550, 170);
        this.groupBoxTestResults.TabIndex = 17;
        this.groupBoxTestResults.TabStop = false;
        this.groupBoxTestResults.Text = "Test Results / Log";

        //
        // txtTestResults
        //
        this.txtTestResults.BackColor = System.Drawing.SystemColors.Window;
        this.txtTestResults.Dock = System.Windows.Forms.DockStyle.Fill;
        this.txtTestResults.Location = new System.Drawing.Point(3, 19);
        this.txtTestResults.Multiline = true;
        this.txtTestResults.Name = "txtTestResults";
        this.txtTestResults.ReadOnly = true;
        this.txtTestResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.txtTestResults.Size = new System.Drawing.Size(544, 148);
        this.txtTestResults.TabIndex = 0;

        //
        // panelButtons
        //
        this.panelButtons.Controls.Add(this.btnSave);
        this.panelButtons.Controls.Add(this.btnCancel);
        this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.panelButtons.Location = new System.Drawing.Point(0, 550);
        this.panelButtons.Name = "panelButtons";
        this.panelButtons.Padding = new System.Windows.Forms.Padding(10);
        this.panelButtons.Size = new System.Drawing.Size(580, 50);
        this.panelButtons.TabIndex = 2;

        //
        // btnSave
        //
        this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnSave.Location = new System.Drawing.Point(395, 12);
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
        this.btnCancel.Location = new System.Drawing.Point(485, 12);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(80, 28);
        this.btnCancel.TabIndex = 1;
        this.btnCancel.Text = "Close";
        this.btnCancel.UseVisualStyleBackColor = true;

        //
        // SiteEditForm
        //
        this.AcceptButton = this.btnSave;
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.btnCancel;
        this.ClientSize = new System.Drawing.Size(580, 600);
        this.Controls.Add(this.panelMain);
        this.Controls.Add(this.panelNavigation);
        this.Controls.Add(this.panelButtons);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "SiteEditForm";
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Add News Source";

        this.groupBoxTestResults.ResumeLayout(false);
        this.groupBoxTestResults.PerformLayout();
        this.panelButtons.ResumeLayout(false);
        this.panelMain.ResumeLayout(false);
        this.panelMain.PerformLayout();
        this.panelNavigation.ResumeLayout(false);
        this.ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Panel panelMain;
    private System.Windows.Forms.Panel panelButtons;
    private System.Windows.Forms.Panel panelNavigation;
    private System.Windows.Forms.Label lblName;
    private System.Windows.Forms.Label lblUrl;
    private System.Windows.Forms.Label lblCategory;
    private System.Windows.Forms.Label lblCountry;
    private System.Windows.Forms.Label lblArticleSelector;
    private System.Windows.Forms.Label lblTitleSelector;
    private System.Windows.Forms.Label lblBodySelector;
    private System.Windows.Forms.Label lblSelectorInfo;
    private System.Windows.Forms.Label lblNavInfo;
    private System.Windows.Forms.TextBox txtName;
    private System.Windows.Forms.TextBox txtUrl;
    private System.Windows.Forms.TextBox txtCategory;
    private System.Windows.Forms.TextBox txtCountry;
    private System.Windows.Forms.TextBox txtArticleSelector;
    private System.Windows.Forms.TextBox txtTitleSelector;
    private System.Windows.Forms.TextBox txtBodySelector;
    private System.Windows.Forms.CheckBox chkIsActive;
    private System.Windows.Forms.Button btnTestSelectors;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnPrevious;
    private System.Windows.Forms.Button btnNext;
    private System.Windows.Forms.GroupBox groupBoxTestResults;
    private System.Windows.Forms.TextBox txtTestResults;
}
