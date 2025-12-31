namespace nRun.UI.Forms;

partial class ArticleViewForm
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
        this.lblTitle = new System.Windows.Forms.Label();
        this.lblSource = new System.Windows.Forms.Label();
        this.lblDate = new System.Windows.Forms.Label();
        this.txtBody = new System.Windows.Forms.TextBox();
        this.btnOpenUrl = new System.Windows.Forms.Button();
        this.btnCopyBody = new System.Windows.Forms.Button();
        this.btnClose = new System.Windows.Forms.Button();
        this.panelButtons = new System.Windows.Forms.Panel();
        this.panelHeader = new System.Windows.Forms.Panel();
        this.linkUrl = new System.Windows.Forms.LinkLabel();

        this.panelButtons.SuspendLayout();
        this.panelHeader.SuspendLayout();
        this.SuspendLayout();

        //
        // panelHeader
        //
        this.panelHeader.Controls.Add(this.lblTitle);
        this.panelHeader.Controls.Add(this.lblSource);
        this.panelHeader.Controls.Add(this.lblDate);
        this.panelHeader.Controls.Add(this.linkUrl);
        this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelHeader.Location = new System.Drawing.Point(0, 0);
        this.panelHeader.Name = "panelHeader";
        this.panelHeader.Padding = new System.Windows.Forms.Padding(15);
        this.panelHeader.Size = new System.Drawing.Size(600, 100);
        this.panelHeader.TabIndex = 0;

        //
        // lblTitle
        //
        this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
        this.lblTitle.Location = new System.Drawing.Point(15, 10);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(570, 45);
        this.lblTitle.TabIndex = 0;
        this.lblTitle.Text = "Article Title";

        //
        // lblSource
        //
        this.lblSource.AutoSize = true;
        this.lblSource.ForeColor = System.Drawing.Color.Gray;
        this.lblSource.Location = new System.Drawing.Point(15, 58);
        this.lblSource.Name = "lblSource";
        this.lblSource.Size = new System.Drawing.Size(45, 15);
        this.lblSource.TabIndex = 1;
        this.lblSource.Text = "Source:";

        //
        // lblDate
        //
        this.lblDate.AutoSize = true;
        this.lblDate.ForeColor = System.Drawing.Color.Gray;
        this.lblDate.Location = new System.Drawing.Point(200, 58);
        this.lblDate.Name = "lblDate";
        this.lblDate.Size = new System.Drawing.Size(34, 15);
        this.lblDate.TabIndex = 2;
        this.lblDate.Text = "Date:";

        //
        // linkUrl
        //
        this.linkUrl.AutoSize = true;
        this.linkUrl.Location = new System.Drawing.Point(15, 78);
        this.linkUrl.Name = "linkUrl";
        this.linkUrl.Size = new System.Drawing.Size(28, 15);
        this.linkUrl.TabIndex = 3;
        this.linkUrl.TabStop = true;
        this.linkUrl.Text = "URL";

        //
        // txtBody
        //
        this.txtBody.BackColor = System.Drawing.SystemColors.Window;
        this.txtBody.Dock = System.Windows.Forms.DockStyle.Fill;
        this.txtBody.Font = new System.Drawing.Font("Segoe UI", 10F);
        this.txtBody.Location = new System.Drawing.Point(0, 100);
        this.txtBody.Multiline = true;
        this.txtBody.Name = "txtBody";
        this.txtBody.ReadOnly = true;
        this.txtBody.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.txtBody.Size = new System.Drawing.Size(600, 300);
        this.txtBody.TabIndex = 1;

        //
        // panelButtons
        //
        this.panelButtons.Controls.Add(this.btnOpenUrl);
        this.panelButtons.Controls.Add(this.btnCopyBody);
        this.panelButtons.Controls.Add(this.btnClose);
        this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.panelButtons.Location = new System.Drawing.Point(0, 400);
        this.panelButtons.Name = "panelButtons";
        this.panelButtons.Padding = new System.Windows.Forms.Padding(10);
        this.panelButtons.Size = new System.Drawing.Size(600, 50);
        this.panelButtons.TabIndex = 2;

        //
        // btnOpenUrl
        //
        this.btnOpenUrl.Location = new System.Drawing.Point(15, 12);
        this.btnOpenUrl.Name = "btnOpenUrl";
        this.btnOpenUrl.Size = new System.Drawing.Size(100, 28);
        this.btnOpenUrl.TabIndex = 0;
        this.btnOpenUrl.Text = "Open in Browser";
        this.btnOpenUrl.UseVisualStyleBackColor = true;

        //
        // btnCopyBody
        //
        this.btnCopyBody.Location = new System.Drawing.Point(125, 12);
        this.btnCopyBody.Name = "btnCopyBody";
        this.btnCopyBody.Size = new System.Drawing.Size(100, 28);
        this.btnCopyBody.TabIndex = 1;
        this.btnCopyBody.Text = "Copy Text";
        this.btnCopyBody.UseVisualStyleBackColor = true;

        //
        // btnClose
        //
        this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.btnClose.Location = new System.Drawing.Point(505, 12);
        this.btnClose.Name = "btnClose";
        this.btnClose.Size = new System.Drawing.Size(80, 28);
        this.btnClose.TabIndex = 2;
        this.btnClose.Text = "Close";
        this.btnClose.UseVisualStyleBackColor = true;

        //
        // ArticleViewForm
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.btnClose;
        this.ClientSize = new System.Drawing.Size(600, 450);
        this.Controls.Add(this.txtBody);
        this.Controls.Add(this.panelHeader);
        this.Controls.Add(this.panelButtons);
        this.MinimumSize = new System.Drawing.Size(450, 350);
        this.Name = "ArticleViewForm";
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Article Details";

        this.panelButtons.ResumeLayout(false);
        this.panelHeader.ResumeLayout(false);
        this.panelHeader.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Panel panelHeader;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Label lblSource;
    private System.Windows.Forms.Label lblDate;
    private System.Windows.Forms.LinkLabel linkUrl;
    private System.Windows.Forms.TextBox txtBody;
    private System.Windows.Forms.Panel panelButtons;
    private System.Windows.Forms.Button btnOpenUrl;
    private System.Windows.Forms.Button btnCopyBody;
    private System.Windows.Forms.Button btnClose;
}
