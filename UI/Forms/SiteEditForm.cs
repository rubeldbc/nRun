using nRun.Data;
using nRun.Models;
using nRun.Services;

namespace nRun.UI.Forms;

public partial class SiteEditForm : Form
{
    private readonly SiteInfo? _existingSite;
    private readonly bool _isEditMode;

    public SiteEditForm() : this(null) { }

    public SiteEditForm(SiteInfo? site)
    {
        InitializeComponent();

        _existingSite = site;
        _isEditMode = site != null;

        SetupEventHandlers();
        LoadSiteData();
    }

    private void SetupEventHandlers()
    {
        btnSave.Click += BtnSave_Click;
        btnTestSelectors.Click += BtnTestSelectors_Click;
    }

    private void LoadSiteData()
    {
        if (_existingSite != null)
        {
            this.Text = "Edit News Source";
            txtName.Text = _existingSite.SiteName;
            txtUrl.Text = _existingSite.SiteLink;
            txtCategory.Text = _existingSite.SiteCategory ?? "";
            txtCountry.Text = _existingSite.SiteCountry ?? "";
            txtArticleSelector.Text = _existingSite.ArticleLinkSelector;
            txtTitleSelector.Text = _existingSite.TitleSelector;
            txtBodySelector.Text = _existingSite.BodySelector;
            chkIsActive.Checked = _existingSite.IsActive;
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        // Validate
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            MessageBox.Show("Please enter a site name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtName.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(txtUrl.Text))
        {
            MessageBox.Show("Please enter a site URL.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtUrl.Focus();
            return;
        }

        if (!Uri.TryCreate(txtUrl.Text, UriKind.Absolute, out _))
        {
            MessageBox.Show("Please enter a valid URL.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtUrl.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(txtArticleSelector.Text))
        {
            MessageBox.Show("Please enter an article link selector.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtArticleSelector.Focus();
            return;
        }

        try
        {
            if (_isEditMode && _existingSite != null)
            {
                // Update existing
                _existingSite.SiteName = txtName.Text.Trim();
                _existingSite.SiteLink = txtUrl.Text.Trim();
                _existingSite.SiteCategory = string.IsNullOrWhiteSpace(txtCategory.Text) ? null : txtCategory.Text.Trim();
                _existingSite.SiteCountry = string.IsNullOrWhiteSpace(txtCountry.Text) ? null : txtCountry.Text.Trim();
                _existingSite.ArticleLinkSelector = txtArticleSelector.Text.Trim();
                _existingSite.TitleSelector = txtTitleSelector.Text.Trim();
                _existingSite.BodySelector = txtBodySelector.Text.Trim();
                _existingSite.IsActive = chkIsActive.Checked;

                DatabaseService.UpdateSite(_existingSite);
            }
            else
            {
                // Create new
                var newSite = new SiteInfo
                {
                    SiteName = txtName.Text.Trim(),
                    SiteLink = txtUrl.Text.Trim(),
                    SiteCategory = string.IsNullOrWhiteSpace(txtCategory.Text) ? null : txtCategory.Text.Trim(),
                    SiteCountry = string.IsNullOrWhiteSpace(txtCountry.Text) ? null : txtCountry.Text.Trim(),
                    ArticleLinkSelector = txtArticleSelector.Text.Trim(),
                    TitleSelector = txtTitleSelector.Text.Trim(),
                    BodySelector = txtBodySelector.Text.Trim(),
                    IsActive = chkIsActive.Checked
                };

                DatabaseService.AddSite(newSite);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving site: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnTestSelectors_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtUrl.Text))
        {
            MessageBox.Show("Please enter a URL first.", "Test Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnTestSelectors.Enabled = false;
        txtTestResults.Text = "Testing selectors...\r\n";

        try
        {
            using var scraper = new NewsScraperService();

            var (links, title, body) = await scraper.TestSelectorsAsync(
                txtUrl.Text.Trim(),
                txtArticleSelector.Text.Trim(),
                txtTitleSelector.Text.Trim(),
                txtBodySelector.Text.Trim()
            );

            var results = new System.Text.StringBuilder();

            // Article links result
            results.AppendLine($"=== Article Links Found: {links.Count} ===");
            if (links.Count > 0)
            {
                for (int i = 0; i < Math.Min(5, links.Count); i++)
                {
                    results.AppendLine($"  {i + 1}. {links[i]}");
                }
                if (links.Count > 5)
                {
                    results.AppendLine($"  ... and {links.Count - 5} more");
                }
            }
            else
            {
                results.AppendLine("  No links found. Check your Article Link Selector.");
            }

            results.AppendLine();

            // Title result
            results.AppendLine("=== Title (from first article) ===");
            if (!string.IsNullOrEmpty(title))
            {
                results.AppendLine($"  {title}");
            }
            else if (string.IsNullOrWhiteSpace(txtTitleSelector.Text))
            {
                results.AppendLine("  No selector provided.");
            }
            else
            {
                results.AppendLine("  No title found. Check your Title Selector.");
            }

            results.AppendLine();

            // Body result
            results.AppendLine("=== Body Preview (from first article) ===");
            if (!string.IsNullOrEmpty(body))
            {
                var bodyPreview = body.Length > 300 ? body[..300] + "..." : body;
                results.AppendLine($"  {bodyPreview}");
            }
            else if (string.IsNullOrWhiteSpace(txtBodySelector.Text))
            {
                results.AppendLine("  No selector provided.");
            }
            else
            {
                results.AppendLine("  No body content found. Check your Body Selector.");
            }

            txtTestResults.Text = results.ToString();
        }
        catch (Exception ex)
        {
            txtTestResults.Text = $"Test failed with error:\r\n{ex.Message}";
        }
        finally
        {
            btnTestSelectors.Enabled = true;
        }
    }
}
