using System.Diagnostics;
using System.IO.Compression;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace nRun.Services;

/// <summary>
/// Service for checking and syncing Chrome and ChromeDriver versions
/// </summary>
public class ChromeVersionService
{
    private const string ChromeDriverApiUrl = "https://googlechromelabs.github.io/chrome-for-testing/known-good-versions-with-downloads.json";
    private static readonly HttpClient _httpClient = new();

    /// <summary>
    /// Gets the installed Chrome browser version
    /// </summary>
    public static string? GetInstalledChromeVersion()
    {
        try
        {
   // Find Chrome version from Windows Registry
   string[] registryPaths = {
       @"SOFTWARE\Google\Chrome\BLBeacon",
    @"SOFTWARE\WOW6432Node\Google\Chrome\BLBeacon",
@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe"
            };

         foreach (var path in registryPaths)
 {
       using var key = Registry.LocalMachine.OpenSubKey(path) ?? Registry.CurrentUser.OpenSubKey(path);
      if (key != null)
      {
           var version = key.GetValue("version")?.ToString();
  if (!string.IsNullOrEmpty(version))
   return version;
          }
            }

 // Get version from Chrome executable
        string[] chromePaths = {
    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Google\Chrome\Application\chrome.exe"),
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Google\Chrome\Application\chrome.exe"),
       Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Google\Chrome\Application\chrome.exe")
   };

          foreach (var chromePath in chromePaths)
         {
         if (File.Exists(chromePath))
       {
        var versionInfo = FileVersionInfo.GetVersionInfo(chromePath);
         if (!string.IsNullOrEmpty(versionInfo.FileVersion))
                return versionInfo.FileVersion;
     }
     }

            return null;
        }
   catch
        {
 return null;
        }
    }

    /// <summary>
    /// Gets the installed ChromeDriver version
    /// </summary>
    public static string? GetInstalledChromeDriverVersion()
    {
    try
        {
            // First check in project directory
         var localDriverPath = GetLocalChromeDriverPath();
  if (File.Exists(localDriverPath))
    {
        return GetDriverVersionFromFile(localDriverPath);
            }

          // Find ChromeDriver in PATH
var pathDriverPath = FindChromeDriverInPath();
            if (!string.IsNullOrEmpty(pathDriverPath) && File.Exists(pathDriverPath))
            {
     return GetDriverVersionFromFile(pathDriverPath);
            }

            return null;
        }
        catch
        {
  return null;
        }
    }

    private static string? GetDriverVersionFromFile(string driverPath)
    {
        try
        {
     var process = new Process
       {
    StartInfo = new ProcessStartInfo
          {
  FileName = driverPath,
         Arguments = "--version",
              UseShellExecute = false,
                 RedirectStandardOutput = true,
         CreateNoWindow = true
  }
  };

   process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit(5000);

            // Extract version from "ChromeDriver 120.0.6099.109 ..."
            var match = Regex.Match(output, @"ChromeDriver\s+(\d+\.\d+\.\d+\.?\d*)");
       if (match.Success)
        return match.Groups[1].Value;

    return null;
        }
        catch
        {
            return null;
        }
    }

    private static string? FindChromeDriverInPath()
    {
        var pathEnv = Environment.GetEnvironmentVariable("PATH");
      if (string.IsNullOrEmpty(pathEnv))
        return null;

        foreach (var path in pathEnv.Split(Path.PathSeparator))
        {
     var driverPath = Path.Combine(path, "chromedriver.exe");
  if (File.Exists(driverPath))
       return driverPath;
        }

        return null;
    }

    /// <summary>
 /// Checks if Chrome and ChromeDriver major versions match
    /// </summary>
    public static VersionCheckResult CheckVersionCompatibility()
    {
        var chromeVersion = GetInstalledChromeVersion();
        var driverVersion = GetInstalledChromeDriverVersion();

        var result = new VersionCheckResult
 {
     ChromeVersion = chromeVersion,
    ChromeDriverVersion = driverVersion
        };

        if (string.IsNullOrEmpty(chromeVersion))
    {
      result.Status = VersionStatus.ChromeNotFound;
      result.Message = "Chrome browser not found. Please install Chrome.";
    return result;
        }

        if (string.IsNullOrEmpty(driverVersion))
        {
     result.Status = VersionStatus.DriverNotFound;
            result.Message = "ChromeDriver not found. Click to download.";
            return result;
   }

        var chromeMajor = GetMajorVersion(chromeVersion);
        var driverMajor = GetMajorVersion(driverVersion);

    if (chromeMajor == driverMajor)
        {
      result.Status = VersionStatus.Compatible;
            result.Message = "Chrome and ChromeDriver are compatible.";
        }
        else
        {
         result.Status = VersionStatus.Mismatch;
     result.Message = $"Version mismatch! Chrome: {chromeMajor}, Driver: {driverMajor}. Please download new ChromeDriver.";
        }

        return result;
    }

 private static int GetMajorVersion(string version)
    {
        var parts = version.Split('.');
     if (parts.Length > 0 && int.TryParse(parts[0], out int major))
            return major;
        return 0;
    }

    /// <summary>
    /// Downloads the matching ChromeDriver for installed Chrome version
    /// </summary>
    public static async Task<DownloadResult> DownloadMatchingChromeDriverAsync(IProgress<int>? progress = null)
    {
        var result = new DownloadResult();

     try
  {
       var chromeVersion = GetInstalledChromeVersion();
   if (string.IsNullOrEmpty(chromeVersion))
        {
   result.Success = false;
       result.Message = "Chrome browser not found.";
            return result;
       }

            var majorVersion = GetMajorVersion(chromeVersion);
      progress?.Report(10);

     // Find available versions from API
         var downloadUrl = await GetChromeDriverDownloadUrlAsync(majorVersion);
  if (string.IsNullOrEmpty(downloadUrl))
 {
      result.Success = false;
        result.Message = $"ChromeDriver not found for Chrome version {majorVersion}.";
  return result;
        }

            progress?.Report(30);

  // Download
     var tempZipPath = Path.Combine(Path.GetTempPath(), "chromedriver.zip");
            using (var response = await _httpClient.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead))
        {
  response.EnsureSuccessStatusCode();

     await using var fs = new FileStream(tempZipPath, FileMode.Create);
      await response.Content.CopyToAsync(fs);
       }

            progress?.Report(70);

     // Extract
   var extractPath = GetLocalChromeDriverDirectory();
            if (Directory.Exists(extractPath))
            {
// Delete old driver
      try
    {
        var oldDriver = GetLocalChromeDriverPath();
         if (File.Exists(oldDriver))
               File.Delete(oldDriver);
                }
       catch { }
            }
         else
            {
    Directory.CreateDirectory(extractPath);
        }

        ZipFile.ExtractToDirectory(tempZipPath, extractPath, true);

        // Extract chromedriver.exe from nested folder
    var extractedDriver = Directory.GetFiles(extractPath, "chromedriver.exe", SearchOption.AllDirectories).FirstOrDefault();
     if (extractedDriver != null)
            {
    var targetPath = GetLocalChromeDriverPath();
       if (extractedDriver != targetPath)
     {
             File.Move(extractedDriver, targetPath, true);
     }
          }

          progress?.Report(90);

     // Delete temp file
     try { File.Delete(tempZipPath); } catch { }

            // Nested folders cleanup
      foreach (var dir in Directory.GetDirectories(extractPath))
     {
      try { Directory.Delete(dir, true); } catch { }
      }

            progress?.Report(100);

 result.Success = true;
            result.Message = "ChromeDriver downloaded and installed successfully.";
            result.InstalledPath = GetLocalChromeDriverPath();
}
        catch (Exception ex)
        {
      result.Success = false;
            result.Message = $"Download error: {ex.Message}";
        }

        return result;
    }

    private static async Task<string?> GetChromeDriverDownloadUrlAsync(int majorVersion)
{
    try
     {
            var response = await _httpClient.GetStringAsync(ChromeDriverApiUrl);
    using var doc = JsonDocument.Parse(response);

  var versions = doc.RootElement.GetProperty("versions");

         // Find the latest matching version
     string? bestMatchUrl = null;
            string? bestMatchVersion = null;

   foreach (var versionEntry in versions.EnumerateArray())
            {
        var version = versionEntry.GetProperty("version").GetString();
         if (string.IsNullOrEmpty(version))
     continue;

    if (GetMajorVersion(version) == majorVersion)
      {
    var downloads = versionEntry.GetProperty("downloads");
          if (downloads.TryGetProperty("chromedriver", out var driverDownloads))
                    {
            foreach (var download in driverDownloads.EnumerateArray())
 {
       var platform = download.GetProperty("platform").GetString();
          if (platform == "win64" || platform == "win32")
          {
  bestMatchUrl = download.GetProperty("url").GetString();
         bestMatchVersion = version;
            }
      }
   }
}
            }

            return bestMatchUrl;
        }
        catch
        {
        // Fallback URL pattern
       return $"https://storage.googleapis.com/chrome-for-testing-public/{majorVersion}.0.0.0/win64/chromedriver-win64.zip";
        }
    }

    /// <summary>
    /// Local ChromeDriver directory path
    /// </summary>
 public static string GetLocalChromeDriverDirectory()
    {
        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ChromeDriver");
    }

    /// <summary>
    /// Local ChromeDriver executable path
    /// </summary>
    public static string GetLocalChromeDriverPath()
    {
 return Path.Combine(GetLocalChromeDriverDirectory(), "chromedriver.exe");
    }
}

/// <summary>
/// Version check result
/// </summary>
public class VersionCheckResult
{
    public string? ChromeVersion { get; set; }
    public string? ChromeDriverVersion { get; set; }
    public VersionStatus Status { get; set; }
    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// Version status enum
/// </summary>
public enum VersionStatus
{
    Compatible,
    Mismatch,
    ChromeNotFound,
    DriverNotFound
}

/// <summary>
/// Download result
/// </summary>
public class DownloadResult
{
    public bool Success { get; set; }
 public string Message { get; set; } = string.Empty;
    public string? InstalledPath { get; set; }
}
