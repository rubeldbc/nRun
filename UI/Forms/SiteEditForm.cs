using nRun.Data;
using nRun.Models;
using nRun.Services;
using SkiaSharp;
using System.Diagnostics;
using System.Drawing.Imaging;

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
        LoadLogoPreview();
    }

    private void SetupEventHandlers()
    {
        btnSave.Click += BtnSave_Click;
        btnTestSelectors.Click += BtnTestSelectors_Click;
        btnDownloadLogo.Click += BtnDownloadLogo_Click;
        btnPrevious.Click += BtnPrevious_Click;
        btnNext.Click += BtnNext_Click;
        txtUrl.TextChanged += TxtUrl_TextChanged;
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

    private void LoadLogoPreview()
    {
        // Dispose previous image if any
        if (picLogo.Image != null)
        {
            picLogo.Image.Dispose();
            picLogo.Image = null;
        }

        // Get logo name from existing site or extract from URL
        string? logoName = _existingSite?.SiteLogo;
        if (string.IsNullOrEmpty(logoName))
        {
            var siteUrl = txtUrl.Text.Trim();
            if (!string.IsNullOrEmpty(siteUrl))
            {
                logoName = LogoDownloadService.ExtractLogoNameFromUrl(siteUrl);
            }
        }

        if (string.IsNullOrEmpty(logoName))
        {
            lblLogoInfo.Text = "No logo";
            return;
        }

        // Try to find local logo
        var logoPath = LogoDownloadService.GetLogoPath(logoName);
        if (!string.IsNullOrEmpty(logoPath) && File.Exists(logoPath))
        {
            try
            {
                // Load image using SkiaSharp (supports WebP)
                var bitmap = ConvertFileToBitmap(logoPath);
                if (bitmap != null)
                {
                    picLogo.Image = bitmap;

                    // Get file info
                    var fileInfo = new FileInfo(logoPath);
                    var sizeKb = fileInfo.Length / 1024.0;
                    lblLogoInfo.Text = $"{bitmap.Width}x{bitmap.Height}\n{sizeKb:F1} KB (local)";
                }
                else
                {
                    lblLogoInfo.Text = "Error loading";
                }
            }
            catch
            {
                lblLogoInfo.Text = "Error loading";
            }
        }
        else
        {
            // Try to load from online
            LoadLogoFromOnlineAsync();
        }
    }

    private async void LoadLogoFromOnlineAsync()
    {
        var siteUrl = txtUrl.Text.Trim();
        if (string.IsNullOrEmpty(siteUrl))
        {
            lblLogoInfo.Text = "No logo";
            return;
        }

        lblLogoInfo.Text = "Loading...";
        progressLogo.Visible = true;

        try
        {
            var logoData = await LogoDownloadService.GetLogoFromOnlineAsync(siteUrl);
            if (logoData != null && logoData.Length > 0)
            {
                // Dispose previous image if any
                if (picLogo.Image != null)
                {
                    picLogo.Image.Dispose();
                    picLogo.Image = null;
                }

                // Convert using SkiaSharp (supports all formats including WebP)
                var bitmap = ConvertToBitmap(logoData);
                if (bitmap != null)
                {
                    picLogo.Image = bitmap;
                    lblLogoInfo.Text = $"{bitmap.Width}x{bitmap.Height}\n(online)";
                }
                else
                {
                    lblLogoInfo.Text = "No logo";
                }
            }
            else
            {
                lblLogoInfo.Text = "No logo";
            }
        }
        catch
        {
            lblLogoInfo.Text = "No logo";
        }
        finally
        {
            progressLogo.Visible = false;
        }
    }

    private void TxtName_TextChanged(object? sender, EventArgs e)
    {
        // No longer triggers logo preview - logo is based on URL, not name
    }

    private void TxtUrl_TextChanged(object? sender, EventArgs e)
    {
        // Update logo preview when site URL changes
        LoadLogoPreview();
    }

    /// <summary>
    /// Converts image data (including WebP) to System.Drawing.Bitmap using SkiaSharp
    /// </summary>
    private Bitmap? ConvertToBitmap(byte[] imageData)
    {
        try
        {
            using var skBitmap = SKBitmap.Decode(imageData);
            if (skBitmap == null) return null;

            var bitmap = new Bitmap(skBitmap.Width, skBitmap.Height, PixelFormat.Format32bppArgb);
            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);

            try
            {
                // Copy pixel data
                var srcPtr = skBitmap.GetPixels();
                var dstPtr = bitmapData.Scan0;
                var rowBytes = skBitmap.Width * 4;

                for (int y = 0; y < skBitmap.Height; y++)
                {
                    unsafe
                    {
                        Buffer.MemoryCopy(
                            (void*)(srcPtr + y * skBitmap.RowBytes),
                            (void*)(dstPtr + y * bitmapData.Stride),
                            rowBytes,
                            rowBytes);
                    }
                }
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }

            return bitmap;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Converts image data from file to System.Drawing.Bitmap using SkiaSharp
    /// </summary>
    private Bitmap? ConvertFileToBitmap(string filePath)
    {
        try
        {
            var imageData = File.ReadAllBytes(filePath);
            return ConvertToBitmap(imageData);
        }
        catch
        {
            return null;
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

    private void BtnPrevious_Click(object? sender, EventArgs e)
    {
        if (_allSites == null || _currentIndex <= 0) return;

        // Save current changes if needed
        if (!SaveCurrentSiteIfNeeded()) return;

        _currentIndex--;
        _existingSite = _allSites[_currentIndex];
        LoadSiteData();
        LoadLogoPreview();
        UpdateNavigationState();
        txtTestResults.Clear();
        Log($"Navigated to: {_existingSite.SiteName}");
    }

    private void BtnNext_Click(object? sender, EventArgs e)
    {
        if (_allSites == null || _currentIndex >= _allSites.Count - 1) return;

        // Save current changes if needed
        if (!SaveCurrentSiteIfNeeded()) return;

        _currentIndex++;
        _existingSite = _allSites[_currentIndex];
        LoadSiteData();
        LoadLogoPreview();
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
            var logoName = LogoDownloadService.ExtractLogoNameFromUrl(txtUrl.Text.Trim());

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

                DatabaseService.UpdateSite(_existingSite);
                Log($"Updated: {_existingSite.SiteName} (logo: {logoName})");
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

                DatabaseService.AddSite(newSite);
                _existingSite = newSite;
                _isEditMode = true;
                Log($"Added: {newSite.SiteName} (logo: {logoName})");
            }

            // Always download logo when site is saved
            _ = DownloadLogoOnSaveAsync();

            return true;
        }
        catch (Exception ex)
        {
            Log($"ERROR saving site: {ex.Message}");
            return false;
        }
    }

    private async Task DownloadLogoOnSaveAsync()
    {
        var siteUrl = txtUrl.Text.Trim();
        var siteName = txtName.Text.Trim();

        if (string.IsNullOrEmpty(siteUrl) || string.IsNullOrEmpty(siteName))
            return;

        try
        {
            // Show progress
            progressLogo.Visible = true;

            // Use logo name from database (already set during save)
            var existingLogoName = _existingSite?.SiteLogo;

            Log($"Downloading logo for: {siteName} (file: {existingLogoName}.webp)");
            var (logoPath, logoName) = await LogoDownloadService.DownloadLogoAsync(siteUrl, siteName, existingLogoName);

            if (!string.IsNullOrEmpty(logoPath))
            {
                Log($"Logo saved: {logoPath}");
                LoadLogoPreview();
            }
            else
            {
                Log("Could not download logo.");
            }
        }
        catch (Exception ex)
        {
            Log($"Logo download error: {ex.Message}");
        }
        finally
        {
            progressLogo.Visible = false;
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
            using var scraper = new NewsScraperService();

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

    #region Download Logo

    private async void BtnDownloadLogo_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtUrl.Text))
        {
            Log("ERROR: Please enter a URL first.");
            return;
        }

        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            Log("ERROR: Please enter a site name first.");
            return;
        }

        btnDownloadLogo.Enabled = false;
        btnDownloadLogo.Text = "Downloading...";
        progressLogo.Visible = true;

        // Use logo name from database if available, otherwise extract from URL
        var existingLogoName = _existingSite?.SiteLogo;
        if (string.IsNullOrEmpty(existingLogoName))
        {
            existingLogoName = LogoDownloadService.ExtractLogoNameFromUrl(txtUrl.Text.Trim());
        }

        Log($"Downloading logo for: {txtName.Text.Trim()} (file: {existingLogoName}.webp)");

        try
        {
            var (logoPath, logoName) = await LogoDownloadService.DownloadLogoAsync(txtUrl.Text.Trim(), txtName.Text.Trim(), existingLogoName);

            if (!string.IsNullOrEmpty(logoPath))
            {
                Log($"Logo downloaded: {logoPath}");

                // Update site logo name in database if site exists and not yet set
                if (_existingSite != null && string.IsNullOrEmpty(_existingSite.SiteLogo))
                {
                    _existingSite.SiteLogo = logoName;
                    DatabaseService.UpdateSite(_existingSite);
                }

                LoadLogoPreview();
            }
            else
            {
                Log("Could not download logo. The website may not have a favicon.");
            }
        }
        catch (Exception ex)
        {
            Log($"Error downloading logo: {ex.Message}");
        }
        finally
        {
            btnDownloadLogo.Enabled = true;
            btnDownloadLogo.Text = "Download Logo";
            progressLogo.Visible = false;
        }
    }

    #endregion
}
