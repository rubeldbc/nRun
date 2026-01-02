# nRun

A Windows desktop application for automated news scraping and aggregation from multiple websites.

## Features

- **Multi-Site Scraping**: Configure and manage multiple news sources with custom CSS selectors
- **Automated Background Scraping**: Set intervals for automatic content fetching
- **PostgreSQL Storage**: Persistent storage for sites and scraped articles
- **Redis/Memurai Integration**: Optional real-time data synchronization
- **Headless Browser Support**: Selenium WebDriver with Chrome for JavaScript-rendered content
- **Site Management**: Add, edit, delete, and test news sources with selector validation
- **Debug Logging**: Real-time logs for monitoring scraping operations

## Requirements

- Windows 10/11
- .NET 8.0 Runtime
- PostgreSQL 12+
- Google Chrome (for web scraping)
- Memurai/Redis (optional, for data sync)

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/rubeldbc/nRun.git
   ```

2. Open `nRun.sln` in Visual Studio 2022+

3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

4. Build the solution:
   ```bash
   dotnet build
   ```

## Configuration

### Database Setup

1. Install PostgreSQL and create a database
2. Launch the application and go to **Settings**
3. Configure the database connection:
   - Host (default: `localhost`)
   - Port (default: `5432`)
   - Database name (default: `nrun_db`)
   - Username and Password

The application will automatically create the required tables on first connection.

### Adding News Sites

1. Click **Add Site** in the main window
2. Fill in the site details:
   - **Site Name**: Display name for the source
   - **Site URL**: Homepage URL of the news site
   - **Article Link Selector**: CSS selector for article links (e.g., `a.article-title`)
   - **Title Selector**: CSS selector for article title on detail page
   - **Body Selector**: CSS selector for article content (optional)
3. Use **Test Selectors** to validate your configuration

### Scraping Settings

- **Check Interval**: How often to scan for new articles (minutes)
- **Headless Mode**: Run browser in background (recommended)
- **Browser Timeout**: Maximum wait time for page loads
- **Auto-Start**: Begin scraping when application launches

## Usage

1. **Start/Stop**: Toggle automatic background scraping
2. **Scrape Now**: Manually scrape the selected site
3. **Manage Sites**: Bulk operations on news sources
4. **View Articles**: Double-click to open article in browser
5. **Memurai Sync**: Enable real-time data sync to Redis/Memurai

## Project Structure

```
nRun/
├── Data/                  # Database services
│   ├── DatabaseService.cs
│   └── SettingsManager.cs
├── Helpers/               # Utility classes
│   ├── LogHelper.cs
│   ├── SiteValidator.cs
│   └── UIHelper.cs
├── Models/                # Data models
│   ├── AppSettings.cs
│   ├── NewsInfo.cs
│   ├── ScrapeResult.cs
│   └── SiteInfo.cs
├── Services/              # Core services
│   ├── BackgroundScraperService.cs
│   ├── NewsScraperService.cs
│   ├── WebDriverService.cs
│   ├── LogoDownloadService.cs
│   ├── MemuraiService.cs
│   └── ServiceContainer.cs
├── UI/Forms/              # Windows Forms UI
│   ├── MainForm.cs
│   ├── SettingsForm.cs
│   ├── SiteEditForm.cs
│   └── SiteManagementForm.cs
├── Program.cs             # Application entry point
└── nRun.csproj            # Project configuration
```

## Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| Selenium.WebDriver | 4.25.0 | Browser automation |
| Selenium.WebDriver.ChromeDriver | 131.0.6778.6900 | Chrome driver |
| Npgsql | 8.0.6 | PostgreSQL client |
| StackExchange.Redis | 2.10.1 | Redis/Memurai client |
| HtmlAgilityPack | 1.11.67 | HTML parsing |
| ObjectListView.Official | 2.9.1 | Enhanced ListView control |
| SkiaSharp | 2.88.8 | Image processing |

## License

This project is for personal/educational use.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Submit a pull request
