using System.Text;
using BrightIdeasSoftware;
using nRun.Models;
using nRun.Services;

namespace nRun.UI.Forms;

public partial class SiteManagementForm : Form
{
    private List<SiteInfo> _sites = new();
    private readonly HashSet<string> _modifiedSiteIds = new();
    private readonly List<SiteInfo> _newSites = new();
    private readonly List<string> _deletedSiteIds = new();
    private bool _isDownloadingLogos = false;

    private bool HasUnsavedChanges => _newSites.Count > 0 || _modifiedSiteIds.Count > 0 || _deletedSiteIds.Count > 0;

    public SiteManagementForm()
    {
        InitializeComponent();
        SetupEventHandlers();
        SetupObjectListView();
        LoadSites();
    }

    private void SetupEventHandlers()
    {
        btnImportCsv.Click += BtnImportCsv_Click;
        btnExportCsv.Click += BtnExportCsv_Click;
        btnAddRow.Click += BtnAddRow_Click;
        btnDeleteSelected.Click += BtnDeleteSelected_Click;
        btnRefresh.Click += BtnRefresh_Click;
        btnSaveAll.Click += BtnSaveAll_Click;
        btnCancel.Click += BtnCancel_Click;

        olvSites.CellEditFinished += OlvSites_CellEditFinished;
        this.FormClosing += SiteManagementForm_FormClosing;
    }

    private void SiteManagementForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (_isDownloadingLogos)
        {
            MessageBox.Show("Please wait until logo downloads are complete.", "Download in Progress",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            e.Cancel = true;
        }
    }

    private void Log(string message, string level = "INFO")
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
        var logLine = $"[{timestamp}] [{level}] {message}{Environment.NewLine}";

        txtLog.AppendText(logLine);
        txtLog.SelectionStart = txtLog.Text.Length;
        txtLog.ScrollToCaret();

        // Also log errors and warnings to the error log
        if (level == "ERROR" || level == "WARN")
        {
            txtErrorLog.AppendText(logLine);
            txtErrorLog.SelectionStart = txtErrorLog.Text.Length;
            txtErrorLog.ScrollToCaret();
        }
    }

    private void SetupObjectListView()
    {
        // Configure checkbox column for IsActive
        olvColIsActive.AspectGetter = (obj) => ((SiteInfo)obj).IsActive;
        olvColIsActive.AspectPutter = (obj, value) =>
        {
            var site = (SiteInfo)obj;
            site.IsActive = value is bool b ? b : false;
            MarkAsModified(site);
        };

        // Handle null values for optional fields
        olvColCategory.AspectGetter = (obj) => ((SiteInfo)obj).SiteCategory ?? "";
        olvColCategory.AspectPutter = (obj, value) =>
        {
            var site = (SiteInfo)obj;
            site.SiteCategory = string.IsNullOrWhiteSpace(value?.ToString()) ? null : value.ToString()!.Trim();
            MarkAsModified(site);
        };

        olvColCountry.AspectGetter = (obj) => ((SiteInfo)obj).SiteCountry ?? "";
        olvColCountry.AspectPutter = (obj, value) =>
        {
            var site = (SiteInfo)obj;
            site.SiteCountry = string.IsNullOrWhiteSpace(value?.ToString()) ? null : value.ToString()!.Trim();
            MarkAsModified(site);
        };

        // Setup aspect putters for other editable columns
        olvColSiteName.AspectPutter = (obj, value) =>
        {
            var site = (SiteInfo)obj;
            site.SiteName = value?.ToString()?.Trim() ?? "";
            MarkAsModified(site);
        };

        olvColSiteUrl.AspectPutter = (obj, value) =>
        {
            var site = (SiteInfo)obj;
            site.SiteLink = value?.ToString()?.Trim() ?? "";
            // Auto-set logo name from URL
            if (!string.IsNullOrEmpty(site.SiteLink))
            {
                site.SiteLogo = ServiceContainer.LogoDownload.ExtractLogoNameFromUrl(site.SiteLink);
            }
            MarkAsModified(site);
        };

        olvColArticleSelector.AspectPutter = (obj, value) =>
        {
            var site = (SiteInfo)obj;
            site.ArticleLinkSelector = value?.ToString()?.Trim() ?? "";
            MarkAsModified(site);
        };

        olvColTitleSelector.AspectPutter = (obj, value) =>
        {
            var site = (SiteInfo)obj;
            site.TitleSelector = value?.ToString()?.Trim() ?? "";
            MarkAsModified(site);
        };

        olvColBodySelector.AspectPutter = (obj, value) =>
        {
            var site = (SiteInfo)obj;
            site.BodySelector = value?.ToString()?.Trim() ?? "";
            MarkAsModified(site);
        };
    }

    private void MarkAsModified(SiteInfo site)
    {
        if (!_newSites.Contains(site))
        {
            _modifiedSiteIds.Add(site.SiteId);
        }
        UpdateStatus();
    }

    private void LoadSites()
    {
        _sites = ServiceContainer.Database.GetAllSites();
        _modifiedSiteIds.Clear();
        _newSites.Clear();
        _deletedSiteIds.Clear();
        olvSites.SetObjects(_sites);
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        var parts = new List<string>();

        if (_sites.Count > 0)
            parts.Add($"{_sites.Count} sites");

        if (_newSites.Count > 0)
            parts.Add($"{_newSites.Count} new");

        if (_modifiedSiteIds.Count > 0)
            parts.Add($"{_modifiedSiteIds.Count} modified");

        if (_deletedSiteIds.Count > 0)
            parts.Add($"{_deletedSiteIds.Count} to delete");

        lblStatus.Text = string.Join(" | ", parts);
        lblStatus.ForeColor = HasUnsavedChanges ? Color.Blue : SystemColors.ControlText;
    }

    private void OlvSites_CellEditFinished(object? sender, CellEditEventArgs e)
    {
        if (e.RowObject is SiteInfo site)
        {
            MarkAsModified(site);
            olvSites.RefreshObject(site);
        }
    }

    #region CSV Import/Export

    private void BtnImportCsv_Click(object? sender, EventArgs e)
    {
        using var openDialog = new OpenFileDialog
        {
            Title = "Import Sites from CSV",
            Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
            FilterIndex = 1
        };

        if (openDialog.ShowDialog() != DialogResult.OK)
            return;

        try
        {
            var importedSites = ParseCsvFile(openDialog.FileName);

            if (importedSites.Count == 0)
            {
                MessageBox.Show("No valid sites found in the CSV file.", "Import Result",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Ask user how to handle imports
            var result = MessageBox.Show(
                $"Found {importedSites.Count} sites in CSV.\n\n" +
                "Click Yes to ADD to existing sites.\n" +
                "Click No to REPLACE all sites.\n" +
                "Click Cancel to abort.",
                "Import Mode",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (result == DialogResult.Cancel)
                return;

            if (result == DialogResult.No)
            {
                // Replace mode - mark all existing for deletion
                foreach (var site in _sites.Where(s => !_newSites.Contains(s)))
                {
                    _deletedSiteIds.Add(site.SiteId);
                }
                _sites.Clear();
                _newSites.Clear();
                _modifiedSiteIds.Clear();
            }

            // Add imported sites
            foreach (var site in importedSites)
            {
                _sites.Add(site);
                _newSites.Add(site);
            }

            olvSites.SetObjects(_sites);
            UpdateStatus();

            MessageBox.Show($"Imported {importedSites.Count} sites. Click 'Save All' to persist changes.",
                "Import Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error importing CSV: {ex.Message}", "Import Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private List<SiteInfo> ParseCsvFile(string filePath)
    {
        var sites = new List<SiteInfo>();
        var lines = File.ReadAllLines(filePath, Encoding.UTF8);

        if (lines.Length < 2)
            return sites;

        // Parse header to determine column indices
        var header = ParseCsvLine(lines[0]);
        var columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        for (int i = 0; i < header.Length; i++)
        {
            columnMap[header[i].Trim()] = i;
        }

        // Expected headers (case-insensitive)
        // siteName, siteUrl, latestLinkSelector, titleSelector, bodySelector, Category, Country

        for (int lineNum = 1; lineNum < lines.Length; lineNum++)
        {
            var line = lines[lineNum];
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var values = ParseCsvLine(line);

            var site = new SiteInfo();

            // Map CSV columns to SiteInfo properties
            if (columnMap.TryGetValue("siteName", out int nameIdx) && nameIdx < values.Length)
                site.SiteName = values[nameIdx].Trim();

            if (columnMap.TryGetValue("siteUrl", out int urlIdx) && urlIdx < values.Length)
                site.SiteLink = values[urlIdx].Trim();

            if (columnMap.TryGetValue("latestLinkSelector", out int articleIdx) && articleIdx < values.Length)
                site.ArticleLinkSelector = values[articleIdx].Trim();

            if (columnMap.TryGetValue("titleSelector", out int titleIdx) && titleIdx < values.Length)
                site.TitleSelector = values[titleIdx].Trim();

            if (columnMap.TryGetValue("bodySelector", out int bodyIdx) && bodyIdx < values.Length)
                site.BodySelector = values[bodyIdx].Trim();

            if (columnMap.TryGetValue("Category", out int catIdx) && catIdx < values.Length)
                site.SiteCategory = string.IsNullOrWhiteSpace(values[catIdx]) ? null : values[catIdx].Trim();

            if (columnMap.TryGetValue("Country", out int countryIdx) && countryIdx < values.Length)
                site.SiteCountry = string.IsNullOrWhiteSpace(values[countryIdx]) ? null : values[countryIdx].Trim();

            // Validate required fields
            if (!string.IsNullOrWhiteSpace(site.SiteName) && !string.IsNullOrWhiteSpace(site.SiteLink))
            {
                // Auto-set logo name from URL
                site.SiteLogo = ServiceContainer.LogoDownload.ExtractLogoNameFromUrl(site.SiteLink);
                sites.Add(site);
            }
        }

        return sites;
    }

    private string[] ParseCsvLine(string line)
    {
        var result = new List<string>();
        var current = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    // Escaped quote
                    current.Append('"');
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if ((c == ',' || c == '\t') && !inQuotes)
            {
                result.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        result.Add(current.ToString());
        return result.ToArray();
    }

    private void BtnExportCsv_Click(object? sender, EventArgs e)
    {
        using var saveDialog = new SaveFileDialog
        {
            Title = "Export Sites to CSV",
            Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
            FilterIndex = 1,
            DefaultExt = "csv",
            FileName = $"sites_export_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
        };

        if (saveDialog.ShowDialog() != DialogResult.OK)
            return;

        try
        {
            var sb = new StringBuilder();

            // Write header
            sb.AppendLine("siteName\tsiteUrl\tlatestLinkSelector\ttitleSelector\tbodySelector\tCategory\tCountry");

            // Write data
            foreach (var site in _sites)
            {
                sb.AppendLine($"{EscapeCsvField(site.SiteName)}\t{EscapeCsvField(site.SiteLink)}\t{EscapeCsvField(site.ArticleLinkSelector)}\t{EscapeCsvField(site.TitleSelector)}\t{EscapeCsvField(site.BodySelector)}\t{EscapeCsvField(site.SiteCategory ?? "")}\t{EscapeCsvField(site.SiteCountry ?? "")}");
            }

            File.WriteAllText(saveDialog.FileName, sb.ToString(), Encoding.UTF8);

            MessageBox.Show($"Exported {_sites.Count} sites to CSV.", "Export Successful",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error exporting CSV: {ex.Message}", "Export Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private string EscapeCsvField(string field)
    {
        if (string.IsNullOrEmpty(field))
            return "";

        if (field.Contains('\t') || field.Contains('"') || field.Contains('\n') || field.Contains('\r'))
        {
            return "\"" + field.Replace("\"", "\"\"") + "\"";
        }
        return field;
    }

    #endregion

    #region Row Management

    private void BtnAddRow_Click(object? sender, EventArgs e)
    {
        var newSite = new SiteInfo
        {
            SiteName = "New Site",
            SiteLink = "https://",
            IsActive = true
        };

        _sites.Add(newSite);
        _newSites.Add(newSite);
        olvSites.SetObjects(_sites);
        olvSites.EnsureModelVisible(newSite);
        olvSites.SelectObject(newSite);
        UpdateStatus();

        // Start editing the site name
        olvSites.EditSubItem(olvSites.SelectedItem, 0);
    }

    private void BtnDeleteSelected_Click(object? sender, EventArgs e)
    {
        var selectedSites = olvSites.SelectedObjects.Cast<SiteInfo>().ToList();

        if (selectedSites.Count == 0)
        {
            MessageBox.Show("Please select sites to delete.", "No Selection",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var result = MessageBox.Show(
            $"Delete {selectedSites.Count} selected site(s)?\n\nThis will be applied when you click 'Save All'.",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result != DialogResult.Yes)
            return;

        foreach (var site in selectedSites)
        {
            _sites.Remove(site);

            if (_newSites.Contains(site))
            {
                // New site that hasn't been saved - just remove from new list
                _newSites.Remove(site);
            }
            else
            {
                // Existing site - mark for deletion
                _deletedSiteIds.Add(site.SiteId);
                _modifiedSiteIds.Remove(site.SiteId);
            }
        }

        olvSites.SetObjects(_sites);
        UpdateStatus();
    }

    private void BtnRefresh_Click(object? sender, EventArgs e)
    {
        if (!ConfirmDiscardChanges("Refreshing will discard them."))
            return;

        LoadSites();
    }

    private bool ConfirmDiscardChanges(string action)
    {
        if (!HasUnsavedChanges) return true;

        var result = MessageBox.Show(
            $"You have unsaved changes. {action}\n\nContinue?",
            "Unsaved Changes",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        return result == DialogResult.Yes;
    }

    #endregion

    #region Save/Cancel

    private async void BtnSaveAll_Click(object? sender, EventArgs e)
    {
        // Validate all sites
        var errors = new List<string>();

        for (int i = 0; i < _sites.Count; i++)
        {
            var site = _sites[i];

            if (string.IsNullOrWhiteSpace(site.SiteName))
                errors.Add($"Row {i + 1}: Site name is required");

            if (string.IsNullOrWhiteSpace(site.SiteLink))
                errors.Add($"Row {i + 1}: Site URL is required");
            else if (!Uri.TryCreate(site.SiteLink, UriKind.Absolute, out _))
                errors.Add($"Row {i + 1}: Invalid URL format");

            if (string.IsNullOrWhiteSpace(site.ArticleLinkSelector))
                errors.Add($"Row {i + 1}: Article link selector is required");
        }

        if (errors.Count > 0)
        {
            MessageBox.Show(
                "Please fix the following errors:\n\n" + string.Join("\n", errors.Take(10)) +
                (errors.Count > 10 ? $"\n...and {errors.Count - 10} more" : ""),
                "Validation Errors",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        try
        {
            int addedCount = 0;
            int updatedCount = 0;
            int deletedCount = 0;
            var sitesNeedingLogos = new List<SiteInfo>();

            // Delete marked sites
            foreach (var siteId in _deletedSiteIds)
            {
                ServiceContainer.Database.DeleteSite(siteId);
                deletedCount++;
            }

            // Add new sites
            foreach (var site in _newSites)
            {
                // Ensure logo name is set from URL
                if (string.IsNullOrEmpty(site.SiteLogo) && !string.IsNullOrEmpty(site.SiteLink))
                {
                    site.SiteLogo = ServiceContainer.LogoDownload.ExtractLogoNameFromUrl(site.SiteLink);
                }
                ServiceContainer.Database.AddSite(site);
                sitesNeedingLogos.Add(site);
                addedCount++;
            }

            // Update modified sites
            foreach (var siteId in _modifiedSiteIds)
            {
                var site = _sites.FirstOrDefault(s => s.SiteId == siteId);
                if (site != null)
                {
                    // Ensure logo name is set from URL
                    if (string.IsNullOrEmpty(site.SiteLogo) && !string.IsNullOrEmpty(site.SiteLink))
                    {
                        site.SiteLogo = ServiceContainer.LogoDownload.ExtractLogoNameFromUrl(site.SiteLink);
                    }
                    ServiceContainer.Database.UpdateSite(site);
                    sitesNeedingLogos.Add(site);
                    updatedCount++;
                }
            }

            // Clear tracking
            _newSites.Clear();
            _modifiedSiteIds.Clear();
            _deletedSiteIds.Clear();
            UpdateStatus();

            Log($"Saved {addedCount} new, {updatedCount} updated, {deletedCount} deleted sites.");

            // Download logos for all saved sites (await to prevent form closing)
            if (sitesNeedingLogos.Count > 0)
            {
                Log($"Starting logo download for {sitesNeedingLogos.Count} sites...");
                btnSaveAll.Enabled = false;
                btnCancel.Enabled = false;

                await DownloadLogosAsync(sitesNeedingLogos);

                btnSaveAll.Enabled = true;
                btnCancel.Enabled = true;
            }

            MessageBox.Show(
                $"Changes saved successfully!\n\nAdded: {addedCount}\nUpdated: {updatedCount}\nDeleted: {deletedCount}",
                "Save Complete",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving changes: {ex.Message}", "Save Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        if (!ConfirmDiscardChanges("Discard them?"))
            return;

        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }

    #endregion

    #region Logo Download

    private async Task DownloadLogosAsync(List<SiteInfo> sites)
    {
        if (sites.Count == 0)
            return;

        _isDownloadingLogos = true;

        // Show progress controls
        progressLogos.Visible = true;
        lblLogoProgress.Visible = true;
        progressLogos.Maximum = sites.Count;
        progressLogos.Value = 0;

        int current = 0;
        int successCount = 0;
        int failCount = 0;

        foreach (var site in sites)
        {
            current++;

            if (string.IsNullOrEmpty(site.SiteLink) || string.IsNullOrEmpty(site.SiteLogo))
            {
                progressLogos.Value = current;
                Log($"Skipped logo for {site.SiteName} - missing URL or logo name", "WARN");
                continue;
            }

            try
            {
                lblLogoProgress.Text = $"Downloading logo {current}/{sites.Count}: {site.SiteName}";
                var (logoPath, logoName) = await ServiceContainer.LogoDownload.DownloadLogoAsync(site.SiteLink, site.SiteName, site.SiteLogo);

                if (!string.IsNullOrEmpty(logoPath))
                {
                    Log($"Downloaded logo: {site.SiteName} -> {logoName}.webp");
                    successCount++;
                }
                else
                {
                    Log($"Could not download logo for: {site.SiteName}", "WARN");
                    failCount++;
                }
            }
            catch (Exception ex)
            {
                Log($"Error downloading logo for {site.SiteName}: {ex.Message}", "ERROR");
                failCount++;
            }

            progressLogos.Value = current;
        }

        // Hide progress controls
        progressLogos.Visible = false;
        lblLogoProgress.Visible = false;
        _isDownloadingLogos = false;

        Log($"Logo download complete: {successCount} succeeded, {failCount} failed");
    }

    #endregion
}
