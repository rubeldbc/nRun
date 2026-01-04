# nRun

A comprehensive Windows desktop application for social media data collection and news scraping. Monitor TikTok and Facebook profiles, collect engagement metrics, and aggregate news articles from multiple websites.

## Features

### Social Media Data Collection

#### TikTok Monitoring
- **Profile Tracking**: Add and monitor TikTok profiles by username or URL
- **Bulk Import**: Import multiple profiles from CSV or TXT files
- **Metrics Collection**: Track followers, hearts, and video counts over time
- **Change Tracking**: Visualize growth with color-coded change indicators
- **Scheduled Collection**: Set multiple daily schedules for automated data collection
- **Rate Limiting**: Configurable delays between requests with jitter

#### Facebook Page Monitoring
- **Page Tracking**: Monitor Facebook pages for follower counts and engagement
- **Bulk Import**: Import pages from CSV with company type, page type, and region metadata
- **Avatar Management**: Automatic download and WebP conversion of page avatars
- **Chunk Processing**: Process large page lists in configurable chunks
- **Scheduled Collection**: Multiple daily schedules with countdown timers

### News Scraping
- **Multi-Site Scraping**: Configure and manage multiple news sources with custom CSS selectors
- **Automated Background Scraping**: Set intervals for automatic content fetching
- **No-Scrape Time Window**: Configure time periods when scraping is paused (e.g., overnight)
- **Site Management**: Add, edit, delete, and test news sources with selector validation
- **Logo Auto-Download**: Automatic favicon/logo extraction and WebP conversion

### Data Storage & Integration
- **PostgreSQL Storage**: Persistent storage for profiles, collected data, and articles
- **Redis/Memurai Integration**: Optional real-time data synchronization
- **Data Filtering**: Filter collected data by profile and date range
- **Export Capabilities**: View and analyze collected metrics

### Technical Features
- **Headless Browser Support**: Selenium WebDriver with Chrome for JavaScript-rendered content
- **Service Architecture**: Clean separation of concerns with service layer
- **Memory Management**: Proper resource disposal and event handler cleanup
- **Process Cleanup**: Automatic cleanup of orphaned Chrome/ChromeDriver processes
- **Debug Logging**: Real-time logs with error tracking

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

### TikTok Data Collection Setup

1. Navigate to the **TikTok** tab
2. Add profiles:
   - Click **Add ID** to add individual profiles
   - Use **Bulk Import** to import from CSV/TXT files
3. Configure schedules:
   - Add collection schedules (time of day)
   - Enable/disable schedules with checkboxes
4. Set delay between requests (seconds)
5. Click **Start** to begin scheduled collection

### Facebook Page Monitoring Setup

1. Navigate to the **Facebook** tab
2. Add pages:
   - Click **Add ID** to add individual pages
   - Use **Bulk Import** with CSV format: `page_link,company_type,page_type,region`
3. Configure schedules and chunk settings:
   - Chunk size: How many pages to process per batch
   - Chunk delay: Minutes to wait between batches
4. Click **Start** to begin scheduled collection

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
- **No-Scrape Window**: Set start/end times to pause scraping

## Usage

### News Scraping
1. **Start/Stop**: Toggle automatic background scraping
2. **Scrape Now**: Manually scrape the selected site
3. **Manage Sites**: Bulk operations on news sources
4. **View Articles**: Double-click to open article in browser
5. **Memurai Sync**: Enable real-time data sync to Redis/Memurai

### Social Media Monitoring
1. **Add Profiles/Pages**: Use Add ID or Bulk Import
2. **Set Schedules**: Configure collection times
3. **Start Collection**: Begin scheduled monitoring
4. **View Data**: Filter and analyze collected metrics
5. **Status Column**: Enable/disable individual profiles

## Project Structure

```
nRun/
├── Data/                      # Data access layer
│   ├── DatabaseService.cs     # PostgreSQL operations
│   └── SettingsManager.cs     # JSON settings persistence
├── Helpers/                   # Utility classes
│   ├── LogHelper.cs
│   ├── SiteValidator.cs
│   └── UIHelper.cs
├── Models/                    # Data models
│   ├── AppSettings.cs         # Application settings
│   ├── NewsInfo.cs            # News article model
│   ├── TkProfile.cs           # TikTok profile model
│   ├── TkData.cs              # TikTok metrics model
│   ├── TkSchedule.cs          # TikTok schedule model
│   ├── FbProfile.cs           # Facebook page model
│   ├── FbData.cs              # Facebook metrics model
│   ├── FbSchedule.cs          # Facebook schedule model
│   ├── ScrapeResult.cs        # Scraping result model
│   └── SiteInfo.cs            # News site model
├── Services/                  # Business logic layer
│   ├── Interfaces/            # Service interfaces
│   │   ├── IDatabaseService.cs
│   │   ├── ISettingsManager.cs
│   │   ├── ILogoDownloadService.cs
│   │   ├── IMemuraiService.cs
│   │   ├── INoScrapWindowService.cs
│   │   ├── IScheduleCalculationService.cs
│   │   └── IBulkImportService.cs
│   ├── BackgroundScraperService.cs    # Background news scraping
│   ├── NewsScraperService.cs          # News article extraction
│   ├── TikTokScraperService.cs        # TikTok profile scraping
│   ├── TikTokDataCollectionService.cs # TikTok scheduled collection
│   ├── FacebookScraperService.cs      # Facebook page scraping
│   ├── FacebookDataCollectionService.cs # Facebook scheduled collection
│   ├── NoScrapWindowService.cs        # No-scrape time window logic
│   ├── ScheduleCalculationService.cs  # Schedule timing calculations
│   ├── BulkImportService.cs           # File parsing for bulk import
│   ├── WebDriverService.cs            # Selenium wrapper
│   ├── WebDriverFactory.cs            # WebDriver lifecycle management
│   ├── LogoDownloadService.cs         # Favicon/logo handling
│   ├── MemuraiService.cs              # Redis/Memurai sync
│   ├── ProcessCleanupService.cs       # Chrome process cleanup
│   ├── RateLimiter.cs                 # Request rate limiting
│   └── ServiceContainer.cs            # Dependency injection
├── UI/Forms/                  # Windows Forms UI
│   ├── MainForm.cs            # Main application window
│   ├── AboutForm.cs           # About dialog
│   ├── SettingsForm.cs        # Application settings
│   ├── SiteEditForm.cs        # News site editor
│   ├── SiteManagementForm.cs  # Bulk site operations
│   ├── TikTokIdManagerForm.cs # TikTok profile manager
│   ├── TikTokScheduleForm.cs  # TikTok schedule editor
│   ├── FacebookIdManagerForm.cs # Facebook page manager
│   ├── FacebookScheduleForm.cs  # Facebook schedule editor
│   ├── DatabaseConnectionForm.cs # Database setup
│   └── ArticleViewForm.cs     # Article content viewer
├── Program.cs                 # Application entry point
└── nRun.csproj               # Project configuration
```

## Technical Architecture

### Service Layer
The application uses a service-oriented architecture with:
- **ServiceContainer**: Centralized service registration and access
- **Interface-based design**: All services implement interfaces for testability
- **Event-driven communication**: Services emit events for status updates and data changes

### Data Collection Flow
1. **Scheduler** monitors configured schedules
2. **Collector Service** triggers data collection at scheduled times
3. **Scraper Service** uses Selenium WebDriver to fetch profile data
4. **Database Service** persists collected metrics
5. **UI** updates via event handlers

### Resource Management
- **WebDriver Factory**: Manages WebDriver lifecycle and cleanup
- **Process Cleanup Service**: Ensures orphaned Chrome processes are terminated
- **IDisposable implementation**: Proper resource disposal throughout
- **Event handler cleanup**: Prevents memory leaks from event subscriptions

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

## Developer

**Md. Kamrul Islam Rubel**
- Contact: +8801760002332
- Facebook: [fb.com/rubel.social](https://fb.com/rubel.social)

## License

This project is for personal/educational use.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Submit a pull request
