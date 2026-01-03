using nRun.Models;
using nRun.Services;

namespace nRun.UI.Forms;

public partial class SiteEditForm : Form
{
    private SiteInfo? _existingSite;
    private bool _isEditMode;

    // Navigation support
    private List<SiteInfo>? _allSites;
    private int _currentIndex = -1;

    // Constructor for adding new site
    public SiteEditForm() : this(null, null, -1) { }

    // Constructor for editing a single site
    public SiteEditForm(SiteInfo? site) : this(site, null, -1) { }

    // Constructor for navigation mode (with site list)
    public SiteEditForm(SiteInfo? site, List<SiteInfo>? allSites, int currentIndex)
    {
        InitializeComponent();

        _existingSite = site;
        _isEditMode = site != null;
        _allSites = allSites;
        _currentIndex = currentIndex;

        SetupEventHandlers();
        SetupNavigationPanel();
        LoadSiteData();
    }

    private void SetupEventHandlers()
    {
        btnSave.Click += BtnSave_Click;
        btnTestSelectors.Click += BtnTestSelectors_Click;
        btnPrevious.Click += BtnPrevious_Click;
        btnNext.Click += BtnNext_Click;
    }

    private void SetupNavigationPanel()
    {
        // Show navigation panel only when we have a list of sites
        if (_allSites != null && _allSites.Count > 1)
        {
            panelNavigation.Visible = true;
            UpdateNavigationState();
        }
        else
        {
            panelNavigation.Visible = false;
        }
    }

    private void UpdateNavigationState()
    {
        if (_allSites == null) return;

        btnPrevious.Enabled = _currentIndex > 0;
        btnNext.Enabled = _currentIndex < _allSites.Count - 1;
        lblNavInfo.Text = $"Site {_currentIndex + 1} of {_allSites.Count}";
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
        else
        {
            this.Text = "Add News Source";
        }
    }

    private void Log(string message)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss");
        txtTestResults.AppendText($"[{timestamp}] {message}{Environment.NewLine}");
        txtTestResults.SelectionStart = txtTestResults.Text.Length;
        txtTestResults.ScrollToCaret();
    }

    #region Navigation

    private void BtnPrevious_Click(object? sender, EventArgs e) => NavigateToSite(-1);
    private void BtnNext_Click(object? sender, EventArgs e) => NavigateToSite(1);

    private void NavigateToSite(int direction)
    {
        if (_allSites == null) return;

        var newIndex = _currentIndex + direction;
        if (newIndex < 0 || newIndex >= _allSites.Count) return;

        // Save current changes if needed
        if (!SaveCurrentSiteIfNeeded()) return;

        _currentIndex = newIndex;
        _existingSite = _allSites[_currentIndex];
        LoadSiteData();
        UpdateNavigationState();
        txtTestResults.Clear();
        Log($"Navigated to: {_existingSite.SiteName}");
    }

    private bool SaveCurrentSiteIfNeeded()
    {
        // Check if there are unsaved changes
        if (_existingSite != null)
        {
            bool hasChanges =
                txtName.Text.Trim() != _existingSite.SiteName ||
                txtUrl.Text.Trim() != _existingSite.SiteLink ||
                (txtCategory.Text.Trim() != (_existingSite.SiteCategory ?? "")) ||
                (txtCountry.Text.Trim() != (_existingSite.SiteCountry ?? "")) ||
                txtArticleSelector.Text.Trim() != _existingSite.ArticleLinkSelector ||
                txtTitleSelector.Text.Trim() != _existingSite.TitleSelector ||
                txtBodySelector.Text.Trim() != _existingSite.BodySelector ||
                chkIsActive.Checked != _existingSite.IsActive;

            if (hasChanges)
            {
                var result = MessageBox.Show(
                    "Save changes to current site before navigating?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Cancel)
                    return false;

                if (result == DialogResult.Yes)
                {
                    if (!SaveSite())
                        return false;
                }
            }
        }
        return true;
    }

    #endregion

    #region Save

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (SaveSite())
        {
            this.DialogResult = DialogResult.OK;
            // Don't close if in navigation mode
            if (_allSites == null || _allSites.Count <= 1)
            {
                this.Close();
            }
            else
            {
                Log("Site saved successfully!");
            }
        }
    }

    private bool SaveSite()
    {
        // Validate
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            Log("ERROR: Please enter a site name.");
            txtName.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtUrl.Text))
        {
            Log("ERROR: Please enter a site URL.");
            txtUrl.Focus();
            return false;
        }

        if (!Uri.TryCreate(txtUrl.Text, UriKind.Absolute, out _))
        {
            Log("ERROR: Please enter a valid URL.");
            txtUrl.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtArticleSelector.Text))
        {
            Log("ERROR: Please enter an article link selector.");
            txtArticleSelector.Focus();
            return false;
        }

        try
        {
            // Extract logo name from URL automatically
            var logoName = ServiceContainer.LogoDownload.ExtractLogoNameFromUrl(txtUrl.Text.Trim());

            if (_isEditMode && _existingSite != null)
            {
                // Update existing
                _existingSite.SiteName = txtName.Text.Trim();
                _existingSite.SiteLink = txtUrl.Text.Trim();
                _existingSite.SiteLogo = logoName; // Auto-set logo name from URL
                _existingSite.SiteCategory = string.IsNullOrWhiteSpace(txtCategory.Text) ? null : txtCategory.Text.Trim();
                _existingSite.SiteCountry = string.IsNullOrWhiteSpace(txtCountry.Text) ? null : txtCountry.Text.Trim();
                _existingSite.ArticleLinkSelector = txtArticleSelector.Text.Trim();
                _existingSite.TitleSelector = txtTitleSelector.Text.Trim();
                _existingSite.BodySelector = txtBodySelector.Text.Trim();
                _existingSite.IsActive = chkIsActive.Checked;

                ServiceContainer.Database.UpdateSite(_existingSite);
                Log($"Updated: {_existingSite.SiteName}");
            }
            else
            {
                // Create new
                var newSite = new SiteInfo
                {
                    SiteName = txtName.Text.Trim(),
                    SiteLink = txtUrl.Text.Trim(),
                    SiteLogo = logoName, // Auto-set logo name from URL
                    SiteCategory = string.IsNullOrWhiteSpace(txtCategory.Text) ? null : txtCategory.Text.Trim(),
                    SiteCountry = string.IsNullOrWhiteSpace(txtCountry.Text) ? null : txtCountry.Text.Trim(),
                    ArticleLinkSelector = txtArticleSelector.Text.Trim(),
                    TitleSelector = txtTitleSelector.Text.Trim(),
                    BodySelector = txtBodySelector.Text.Trim(),
                    IsActive = chkIsActive.Checked
                };

                ServiceContainer.Database.AddSite(newSite);
                _existingSite = newSite;
                _isEditMode = true;
                Log($"Added: {newSite.SiteName}");
            }

            return true;
        }
        catch (Exception ex)
        {
            Log($"ERROR saving site: {ex.Message}");
            return false;
        }
    }

    #endregion

    #region Test Selectors

    private async void BtnTestSelectors_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtUrl.Text))
        {
            Log("ERROR: Please enter a URL first.");
            return;
        }

        btnTestSelectors.Enabled = false;
        Log("Testing selectors...");

        try
        {
            var scraper = new NewsScraperService();

            var (links, title, body) = await scraper.TestSelectorsAsync(
                txtUrl.Text.Trim(),
                txtArticleSelector.Text.Trim(),
                txtTitleSelector.Text.Trim(),
                txtBodySelector.Text.Trim()
            );

            // Article links result
            Log($"=== Article Links Found: {links.Count} ===");
            if (links.Count > 0)
            {
                for (int i = 0; i < Math.Min(5, links.Count); i++)
                {
                    Log($"  {i + 1}. {links[i]}");
                }
                if (links.Count > 5)
                {
                    Log($"  ... and {links.Count - 5} more");
                }
            }
            else
            {
                Log("  No links found. Check your Article Link Selector.");
            }

            // Title result
            Log("=== Title (from first article) ===");
            if (!string.IsNullOrEmpty(title))
            {
                Log($"  {title}");
            }
            else if (string.IsNullOrWhiteSpace(txtTitleSelector.Text))
            {
                Log("  No selector provided.");
            }
            else
            {
                Log("  No title found. Check your Title Selector.");
            }

            // Body result
            Log("=== Body Preview (from first article) ===");
            if (!string.IsNullOrEmpty(body))
            {
                var bodyPreview = body.Length > 300 ? body[..300] + "..." : body;
                Log($"  {bodyPreview}");
            }
            else if (string.IsNullOrWhiteSpace(txtBodySelector.Text))
            {
                Log("  No selector provided.");
            }
            else
            {
                Log("  No body content found. Check your Body Selector.");
            }
        }
        catch (Exception ex)
        {
            Log($"Test failed with error: {ex.Message}");
        }
        finally
        {
            btnTestSelectors.Enabled = true;
        }
    }

    #endregion
}
