using System.Diagnostics;
using nRun.Models;

namespace nRun.UI.Forms;

public partial class ArticleViewForm : Form
{
    private readonly NewsInfo _article;

    public ArticleViewForm(NewsInfo article)
    {
        InitializeComponent();

        _article = article;
        SetupEventHandlers();
        LoadArticle();
    }

    private void SetupEventHandlers()
    {
        btnOpenUrl.Click += BtnOpenUrl_Click;
        btnCopyBody.Click += BtnCopyBody_Click;
        linkUrl.LinkClicked += LinkUrl_LinkClicked;
    }

    private void LoadArticle()
    {
        lblTitle.Text = _article.NewsTitle;
        lblSource.Text = $"Source: {_article.SiteName}";
        lblDate.Text = $"Fetched: {_article.CreatedAtDisplay}";
        linkUrl.Text = _article.NewsUrl;
        txtBody.Text = _article.NewsText;

        var shortTitle = _article.NewsTitle.Length > 50
            ? _article.NewsTitle[..50] + "..."
            : _article.NewsTitle;
        this.Text = $"Article - {shortTitle}";
    }

    private void BtnOpenUrl_Click(object? sender, EventArgs e)
    {
        OpenUrl();
    }

    private void BtnCopyBody_Click(object? sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(_article.NewsText))
        {
            Clipboard.SetText(_article.NewsText);
            MessageBox.Show("Article text copied to clipboard.", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void LinkUrl_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
    {
        OpenUrl();
    }

    private void OpenUrl()
    {
        try
        {
            Process.Start(new ProcessStartInfo(_article.NewsUrl) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to open URL: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
