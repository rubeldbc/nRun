using Npgsql;
using nRun.Data;
using nRun.Models;
using nRun.Services;

namespace nRun.UI.Forms;

public partial class DatabaseConnectionForm : Form
{
    public DatabaseConnectionForm()
    {
        InitializeComponent();
        SetupEventHandlers();
        LoadSettings();
    }

    private void SetupEventHandlers()
    {
        btnTest.Click += BtnTest_Click;
        btnConnect.Click += BtnConnect_Click;
        btnCreateTables.Click += BtnCreateTables_Click;
        btnDeleteDatabase.Click += BtnDeleteDatabase_Click;
        btnSave.Click += BtnSave_Click;
    }

    private void LoadSettings()
    {
        var settings = SettingsManager.LoadSettings();
        txtHost.Text = settings.DbHost;
        numPort.Value = settings.DbPort;
        txtDatabase.Text = settings.DbName;
        txtUsername.Text = settings.DbUser;
        txtPassword.Text = settings.DbPassword;
    }

    private DbConnectionSettings GetConnectionSettings()
    {
        return new DbConnectionSettings
        {
            Host = txtHost.Text.Trim(),
            Port = (int)numPort.Value,
            Database = txtDatabase.Text.Trim(),
            Username = txtUsername.Text.Trim(),
            Password = txtPassword.Text
        };
    }

    private async void BtnTest_Click(object? sender, EventArgs e)
    {
        btnTest.Enabled = false;
        LogMessage("Testing connection...");

        try
        {
            var settings = GetConnectionSettings();
            var connString = settings.GetConnectionString();

            // Debug: show connection string (mask password)
            var maskedConn = connString.Replace(settings.Password, "****");
            LogMessage($"Connection string: {maskedConn}");
            LogMessage($"Password length: {settings.Password.Length} chars");

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            LogMessage($"SUCCESS: Connected to PostgreSQL server");
            LogMessage($"  Server Version: {conn.PostgreSqlVersion}");
            LogMessage($"  Database: {conn.Database}");

            UpdateStatus("Connection OK", Color.Green);
        }
        catch (Exception ex)
        {
            LogMessage($"ERROR: {ex.Message}");
            UpdateStatus("Connection Failed", Color.Red);
        }
        finally
        {
            btnTest.Enabled = true;
        }
    }

    private async void BtnConnect_Click(object? sender, EventArgs e)
    {
        btnConnect.Enabled = false;
        LogMessage("Connecting to database...");

        try
        {
            var settings = GetConnectionSettings();

            // First try to connect to the server (without specific database)
            var serverConnString = settings.GetServerConnectionString();

            await using (var conn = new NpgsqlConnection(serverConnString))
            {
                await conn.OpenAsync();
                LogMessage("Connected to PostgreSQL server");

                // Check if database exists
                await using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT 1 FROM pg_database WHERE datname = @dbname";
                cmd.Parameters.AddWithValue("dbname", settings.Database);

                var exists = await cmd.ExecuteScalarAsync() != null;

                if (!exists)
                {
                    LogMessage($"Database '{settings.Database}' does not exist. Creating...");

                    await using var createCmd = conn.CreateCommand();
                    createCmd.CommandText = $"CREATE DATABASE \"{settings.Database}\"";
                    await createCmd.ExecuteNonQueryAsync();

                    LogMessage($"Database '{settings.Database}' created successfully");
                }
                else
                {
                    LogMessage($"Database '{settings.Database}' exists");
                }
            }

            // Now connect to the specific database
            var dbConnString = settings.GetConnectionString();
            await using (var conn = new NpgsqlConnection(dbConnString))
            {
                await conn.OpenAsync();
                LogMessage($"Connected to database '{settings.Database}'");
            }

            UpdateStatus("Connected", Color.Green);
            btnCreateTables.Enabled = true;
        }
        catch (Exception ex)
        {
            LogMessage($"ERROR: {ex.Message}");
            UpdateStatus("Connection Failed", Color.Red);
        }
        finally
        {
            btnConnect.Enabled = true;
        }
    }

    private async void BtnCreateTables_Click(object? sender, EventArgs e)
    {
        btnCreateTables.Enabled = false;
        LogMessage("Checking tables...");

        try
        {
            var settings = GetConnectionSettings();
            var connString = settings.GetConnectionString();

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            // Check if tables exist
            var siteInfoExists = await TableExists(conn, "site_info");
            var newsInfoExists = await TableExists(conn, "news_info");
            var appSettingsExists = await TableExists(conn, "app_settings");

            if (siteInfoExists && newsInfoExists && appSettingsExists)
            {
                LogMessage("All tables already exist. Showing structure...");
                LogMessage("");
                await ShowTableStructure(conn, "site_info");
                await ShowTableStructure(conn, "news_info");
                await ShowTableStructure(conn, "app_settings");
            }
            else
            {
                // Create tables that don't exist
                if (!siteInfoExists)
                {
                    await CreateSiteInfoTable(conn);
                    LogMessage("Created table: site_info");
                }

                if (!newsInfoExists)
                {
                    await CreateNewsInfoTable(conn);
                    LogMessage("Created table: news_info");
                }

                if (!appSettingsExists)
                {
                    await CreateAppSettingsTable(conn);
                    LogMessage("Created table: app_settings");
                }

                LogMessage("");
                LogMessage("Tables created successfully!");
                LogMessage("");

                // Show structure of all tables
                await ShowTableStructure(conn, "site_info");
                await ShowTableStructure(conn, "news_info");
                await ShowTableStructure(conn, "app_settings");
            }
        }
        catch (Exception ex)
        {
            LogMessage($"ERROR: {ex.Message}");
        }
        finally
        {
            btnCreateTables.Enabled = true;
        }
    }

    private async void BtnDeleteDatabase_Click(object? sender, EventArgs e)
    {
        // Confirm deletion
        var result = MessageBox.Show(
            "WARNING: This will permanently delete ALL data!\n\n" +
            "- All site configurations will be deleted\n" +
            "- All scraped news articles will be deleted\n" +
            "- All logo files will be deleted\n\n" +
            "Are you sure you want to continue?",
            "Delete Database",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button2);

        if (result != DialogResult.Yes)
        {
            LogMessage("Delete operation cancelled.");
            return;
        }

        // Second confirmation
        result = MessageBox.Show(
            "This action CANNOT be undone!\n\nType 'DELETE' in your mind and click Yes to confirm.",
            "Final Confirmation",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Stop,
            MessageBoxDefaultButton.Button2);

        if (result != DialogResult.Yes)
        {
            LogMessage("Delete operation cancelled.");
            return;
        }

        btnDeleteDatabase.Enabled = false;
        LogMessage("Starting database deletion...");

        try
        {
            var settings = GetConnectionSettings();
            var connString = settings.GetConnectionString();

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            // Drop tables in correct order (due to foreign key constraints)
            LogMessage("Dropping tables...");

            // Drop news_info first (has FK to site_info)
            if (await TableExists(conn, "news_info"))
            {
                await using var cmd1 = conn.CreateCommand();
                cmd1.CommandText = "DROP TABLE news_info CASCADE";
                await cmd1.ExecuteNonQueryAsync();
                LogMessage("  Dropped table: news_info");
            }

            // Drop site_info
            if (await TableExists(conn, "site_info"))
            {
                await using var cmd2 = conn.CreateCommand();
                cmd2.CommandText = "DROP TABLE site_info CASCADE";
                await cmd2.ExecuteNonQueryAsync();
                LogMessage("  Dropped table: site_info");
            }

            // Drop app_settings
            if (await TableExists(conn, "app_settings"))
            {
                await using var cmd3 = conn.CreateCommand();
                cmd3.CommandText = "DROP TABLE app_settings CASCADE";
                await cmd3.ExecuteNonQueryAsync();
                LogMessage("  Dropped table: app_settings");
            }

            LogMessage("All tables dropped successfully.");

            // Delete logo files
            LogMessage("Deleting logo files...");
            var logosFolder = LogoDownloadService.GetLogosFolder();
            if (Directory.Exists(logosFolder))
            {
                var files = Directory.GetFiles(logosFolder);
                int deletedCount = 0;
                foreach (var file in files)
                {
                    try
                    {
                        File.Delete(file);
                        deletedCount++;
                    }
                    catch (Exception ex)
                    {
                        LogMessage($"  Failed to delete: {Path.GetFileName(file)} - {ex.Message}");
                    }
                }
                LogMessage($"  Deleted {deletedCount} logo files.");
            }
            else
            {
                LogMessage("  Logos folder does not exist.");
            }

            LogMessage("");
            LogMessage("=== DATABASE DELETION COMPLETE ===");
            LogMessage("You can now click 'Create Tables' to recreate the database structure.");
            LogMessage("");

            UpdateStatus("Tables deleted", Color.Orange);

            // Reinitialize DatabaseService (it will show as not connected)
            DatabaseService.Initialize();
        }
        catch (Exception ex)
        {
            LogMessage($"ERROR: {ex.Message}");
            UpdateStatus("Delete Failed", Color.Red);
        }
        finally
        {
            btnDeleteDatabase.Enabled = true;
        }
    }

    private async Task<bool> TableExists(NpgsqlConnection conn, string tableName)
    {
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT EXISTS (
                SELECT FROM information_schema.tables
                WHERE table_schema = 'public'
                AND table_name = @tableName
            )";
        cmd.Parameters.AddWithValue("tableName", tableName);
        return (bool)(await cmd.ExecuteScalarAsync() ?? false);
    }

    private async Task CreateSiteInfoTable(NpgsqlConnection conn)
    {
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE site_info (
                site_id VARCHAR(20) PRIMARY KEY,
                site_name VARCHAR(255) NOT NULL,
                site_link VARCHAR(500) NOT NULL,
                site_logo VARCHAR(500),
                site_category VARCHAR(100),
                site_country VARCHAR(100),
                is_active BOOLEAN DEFAULT TRUE,
                article_link_selector TEXT,
                title_selector TEXT,
                body_selector TEXT,
                success_count INTEGER DEFAULT 0,
                failure_count INTEGER DEFAULT 0,
                last_checked TIMESTAMP,
                created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            )";
        await cmd.ExecuteNonQueryAsync();

        // Create index on site_link
        await using var indexCmd = conn.CreateCommand();
        indexCmd.CommandText = "CREATE INDEX idx_site_info_link ON site_info(site_link)";
        await indexCmd.ExecuteNonQueryAsync();
    }

    private async Task CreateNewsInfoTable(NpgsqlConnection conn)
    {
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE news_info (
                serial BIGSERIAL PRIMARY KEY,
                site_id VARCHAR(20) NOT NULL REFERENCES site_info(site_id) ON DELETE CASCADE,
                news_title TEXT NOT NULL,
                news_text TEXT,
                news_url VARCHAR(1000) NOT NULL UNIQUE,
                is_read BOOLEAN DEFAULT FALSE,
                created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            )";
        await cmd.ExecuteNonQueryAsync();

        // Create indexes
        await using var indexCmd1 = conn.CreateCommand();
        indexCmd1.CommandText = "CREATE INDEX idx_news_info_site ON news_info(site_id)";
        await indexCmd1.ExecuteNonQueryAsync();

        await using var indexCmd2 = conn.CreateCommand();
        indexCmd2.CommandText = "CREATE INDEX idx_news_info_created ON news_info(created_at DESC)";
        await indexCmd2.ExecuteNonQueryAsync();

        await using var indexCmd3 = conn.CreateCommand();
        indexCmd3.CommandText = "CREATE INDEX idx_news_info_url ON news_info(news_url)";
        await indexCmd3.ExecuteNonQueryAsync();
    }

    private async Task CreateAppSettingsTable(NpgsqlConnection conn)
    {
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE app_settings (
                id INTEGER PRIMARY KEY DEFAULT 1 CHECK (id = 1),
                check_interval_minutes INTEGER DEFAULT 5,
                delay_between_links_seconds INTEGER DEFAULT 2,
                max_articles_per_site INTEGER DEFAULT 10,
                use_headless_browser BOOLEAN DEFAULT TRUE,
                browser_timeout_seconds INTEGER DEFAULT 30,
                auto_start_scraping BOOLEAN DEFAULT FALSE,
                last_modified TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            )";
        await cmd.ExecuteNonQueryAsync();

        // Insert default settings
        await using var insertCmd = conn.CreateCommand();
        insertCmd.CommandText = "INSERT INTO app_settings (id) VALUES (1) ON CONFLICT DO NOTHING";
        await insertCmd.ExecuteNonQueryAsync();
    }

    private async Task ShowTableStructure(NpgsqlConnection conn, string tableName)
    {
        LogMessage($"=== Table: {tableName} ===");

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT column_name, data_type, character_maximum_length, is_nullable, column_default
            FROM information_schema.columns
            WHERE table_name = @tableName
            ORDER BY ordinal_position";
        cmd.Parameters.AddWithValue("tableName", tableName);

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var colName = reader.GetString(0);
            var dataType = reader.GetString(1);
            var maxLength = reader.IsDBNull(2) ? "" : $"({reader.GetInt32(2)})";
            var nullable = reader.GetString(3) == "YES" ? "NULL" : "NOT NULL";
            var defaultVal = reader.IsDBNull(4) ? "" : $" DEFAULT {reader.GetString(4)}";

            LogMessage($"  {colName}: {dataType}{maxLength} {nullable}{defaultVal}");
        }
        LogMessage("");
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        try
        {
            var appSettings = SettingsManager.LoadSettings();
            appSettings.DbHost = txtHost.Text.Trim();
            appSettings.DbPort = (int)numPort.Value;
            appSettings.DbName = txtDatabase.Text.Trim();
            appSettings.DbUser = txtUsername.Text.Trim();
            appSettings.DbPassword = txtPassword.Text;

            SettingsManager.SaveSettings(appSettings);

            // Update DatabaseService connection string
            DatabaseService.UpdateConnectionString(appSettings.GetConnectionString());

            LogMessage("Settings saved successfully");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LogMessage(string message)
    {
        if (InvokeRequired)
        {
            Invoke(() => LogMessage(message));
            return;
        }

        txtLog.AppendText(message + Environment.NewLine);
        txtLog.ScrollToCaret();
    }

    private void UpdateStatus(string status, Color color)
    {
        if (InvokeRequired)
        {
            Invoke(() => UpdateStatus(status, color));
            return;
        }

        lblStatus.Text = status;
        lblStatus.ForeColor = color;
    }
}
