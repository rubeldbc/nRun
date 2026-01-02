using Npgsql;
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
        var settings = ServiceContainer.Settings.LoadSettings();
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
            var tkProfileExists = await TableExists(conn, "tk_profile");
            var tkDataExists = await TableExists(conn, "tk_data");

            if (tkProfileExists && tkDataExists)
            {
                LogMessage("All tables already exist. Showing structure...");
                LogMessage("");
                await ShowTableStructure(conn, "tk_profile");
                await ShowTableStructure(conn, "tk_data");
            }
            else
            {
                // Create tables that don't exist (tk_profile must be created first due to FK)
                if (!tkProfileExists)
                {
                    await CreateTkProfileTable(conn);
                    LogMessage("Created table: tk_profile");
                }

                if (!tkDataExists)
                {
                    await CreateTkDataTable(conn);
                    LogMessage("Created table: tk_data");
                }

                LogMessage("");
                LogMessage("Tables created successfully!");
                LogMessage("");

                // Show structure of all tables
                await ShowTableStructure(conn, "tk_profile");
                await ShowTableStructure(conn, "tk_data");
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
            "WARNING: This will permanently delete ALL TikTok data!\n\n" +
            "- All TikTok profile data will be deleted\n" +
            "- All TikTok statistics data will be deleted\n\n" +
            "Are you sure you want to continue?",
            "Delete Tables",
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

            // Drop tk_data first (has FK to tk_profile)
            if (await TableExists(conn, "tk_data"))
            {
                await using var cmd1 = conn.CreateCommand();
                cmd1.CommandText = "DROP TABLE tk_data CASCADE";
                await cmd1.ExecuteNonQueryAsync();
                LogMessage("  Dropped table: tk_data");
            }

            // Drop tk_profile
            if (await TableExists(conn, "tk_profile"))
            {
                await using var cmd2 = conn.CreateCommand();
                cmd2.CommandText = "DROP TABLE tk_profile CASCADE";
                await cmd2.ExecuteNonQueryAsync();
                LogMessage("  Dropped table: tk_profile");
            }

            LogMessage("All tables dropped successfully.");

            LogMessage("");
            LogMessage("=== DATABASE DELETION COMPLETE ===");
            LogMessage("You can now click 'Create Tables' to recreate the database structure.");
            LogMessage("");

            UpdateStatus("Tables deleted", Color.Orange);

            // Reinitialize DatabaseService (it will show as not connected)
            ServiceContainer.Database.Initialize();
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

    private async Task CreateTkProfileTable(NpgsqlConnection conn)
    {
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE tk_profile (
                status BOOLEAN DEFAULT TRUE,
                user_id BIGINT PRIMARY KEY,
                username VARCHAR(100),
                nickname VARCHAR(255),
                region VARCHAR(10),
                created_at_ts TIMESTAMP,
                created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            )";
        await cmd.ExecuteNonQueryAsync();

        // Create index on username for searching
        await using var indexCmd = conn.CreateCommand();
        indexCmd.CommandText = "CREATE INDEX idx_tk_profile_username ON tk_profile(username)";
        await indexCmd.ExecuteNonQueryAsync();
    }

    private async Task CreateTkDataTable(NpgsqlConnection conn)
    {
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE tk_data (
                data_id SERIAL PRIMARY KEY,
                user_id BIGINT NOT NULL REFERENCES tk_profile(user_id) ON DELETE CASCADE,
                follower_count BIGINT,
                heart_count BIGINT,
                video_count INT,
                recorded_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            )";
        await cmd.ExecuteNonQueryAsync();

        // Create indexes
        await using var indexCmd1 = conn.CreateCommand();
        indexCmd1.CommandText = "CREATE INDEX idx_tk_data_user ON tk_data(user_id)";
        await indexCmd1.ExecuteNonQueryAsync();

        await using var indexCmd2 = conn.CreateCommand();
        indexCmd2.CommandText = "CREATE INDEX idx_tk_data_recorded ON tk_data(recorded_at DESC)";
        await indexCmd2.ExecuteNonQueryAsync();
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
            var appSettings = ServiceContainer.Settings.LoadSettings();
            appSettings.DbHost = txtHost.Text.Trim();
            appSettings.DbPort = (int)numPort.Value;
            appSettings.DbName = txtDatabase.Text.Trim();
            appSettings.DbUser = txtUsername.Text.Trim();
            appSettings.DbPassword = txtPassword.Text;

            ServiceContainer.Settings.SaveSettings(appSettings);

            // Update DatabaseService connection string
            ServiceContainer.Database.UpdateConnectionString(appSettings.GetConnectionString());

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
