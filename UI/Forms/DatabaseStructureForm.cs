using System.Text;
using Npgsql;

namespace nRun.UI.Forms;

public partial class DatabaseStructureForm : Form
{
    private readonly string _connectionString;
    private readonly Dictionary<string, DatabaseObject> _databaseObjects = new();

    public DatabaseStructureForm(string connectionString)
    {
        _connectionString = connectionString;
        InitializeComponent();
        SetupEventHandlers();
        LoadData();
    }

    private void SetupEventHandlers()
    {
        btnCopyStructure.Click += BtnCopyStructure_Click;
        btnCopyQuery.Click += BtnCopyQuery_Click;
        btnExportAll.Click += BtnExportAll_Click;
        btnClose.Click += BtnClose_Click;
        cboObjects.SelectedIndexChanged += CboObjects_SelectedIndexChanged;
    }

    private async void LoadData()
    {
        try
        {
            Cursor = Cursors.WaitCursor;

            await LoadDatabaseStructure();
            await LoadDatabaseObjects();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading database structure: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async Task LoadDatabaseStructure()
    {
        var sb = new StringBuilder();

        // First, collect all data using separate connections to avoid "command already in progress"
        string dbName;
        string serverVersion;
        var tables = new List<string>();
        var tableColumns = new Dictionary<string, List<ColumnInfo>>();
        var tableIndexes = new Dictionary<string, List<string>>();
        var tableConstraints = new Dictionary<string, List<string>>();
        var sequences = new List<string>();
        var views = new List<string>();
        var functions = new List<string>();

        // Get database info and tables
        await using (var conn = new NpgsqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            dbName = conn.Database;
            serverVersion = conn.PostgreSqlVersion.ToString();
            tables = await GetTables(conn);
            sequences = await GetSequences(conn);
            views = await GetViews(conn);
            functions = await GetFunctions(conn);
        }

        // Get columns for each table (separate connection)
        foreach (var table in tables)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var columns = new List<ColumnInfo>();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT
                    column_name,
                    data_type,
                    character_maximum_length,
                    is_nullable,
                    column_default
                FROM information_schema.columns
                WHERE table_schema = 'public' AND table_name = @tableName
                ORDER BY ordinal_position";
            cmd.Parameters.AddWithValue("tableName", table);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                columns.Add(new ColumnInfo
                {
                    Name = reader.GetString(0),
                    DataType = reader.GetString(1),
                    MaxLength = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                    IsNullable = reader.GetString(3) == "YES",
                    DefaultValue = reader.IsDBNull(4) ? null : reader.GetString(4)
                });
            }
            tableColumns[table] = columns;
        }

        // Get indexes and constraints for each table (separate connections)
        foreach (var table in tables)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            tableIndexes[table] = await GetTableIndexes(conn, table);
            tableConstraints[table] = await GetTableConstraints(conn, table);
        }

        // Now build the output string
        sb.AppendLine("=".PadRight(60, '='));
        sb.AppendLine($"DATABASE: {dbName}");
        sb.AppendLine($"SERVER VERSION: {serverVersion}");
        sb.AppendLine("=".PadRight(60, '='));
        sb.AppendLine();

        sb.AppendLine($"TABLES ({tables.Count}):");
        sb.AppendLine("-".PadRight(60, '-'));
        sb.AppendLine();

        foreach (var table in tables)
        {
            sb.AppendLine($"TABLE: {table}");
            sb.AppendLine("-".PadRight(40, '-'));

            if (tableColumns.TryGetValue(table, out var columns))
            {
                foreach (var col in columns)
                {
                    var maxLengthStr = col.MaxLength.HasValue ? $"({col.MaxLength})" : "";
                    var nullableStr = col.IsNullable ? "NULL" : "NOT NULL";
                    var defaultStr = string.IsNullOrEmpty(col.DefaultValue) ? "" : $" DEFAULT {col.DefaultValue}";
                    sb.AppendLine($"  {col.Name,-25} {col.DataType}{maxLengthStr,-15} {nullableStr}{defaultStr}");
                }
            }

            sb.AppendLine();

            if (tableIndexes.TryGetValue(table, out var indexes) && indexes.Count > 0)
            {
                sb.AppendLine($"  Indexes:");
                foreach (var idx in indexes)
                {
                    sb.AppendLine($"    - {idx}");
                }
                sb.AppendLine();
            }

            if (tableConstraints.TryGetValue(table, out var constraints) && constraints.Count > 0)
            {
                sb.AppendLine($"  Constraints:");
                foreach (var constraint in constraints)
                {
                    sb.AppendLine($"    - {constraint}");
                }
                sb.AppendLine();
            }

            sb.AppendLine();
        }

        if (sequences.Count > 0)
        {
            sb.AppendLine($"SEQUENCES ({sequences.Count}):");
            sb.AppendLine("-".PadRight(60, '-'));
            foreach (var seq in sequences)
            {
                sb.AppendLine($"  {seq}");
            }
            sb.AppendLine();
        }

        if (views.Count > 0)
        {
            sb.AppendLine($"VIEWS ({views.Count}):");
            sb.AppendLine("-".PadRight(60, '-'));
            foreach (var view in views)
            {
                sb.AppendLine($"  {view}");
            }
            sb.AppendLine();
        }

        if (functions.Count > 0)
        {
            sb.AppendLine($"FUNCTIONS ({functions.Count}):");
            sb.AppendLine("-".PadRight(60, '-'));
            foreach (var func in functions)
            {
                sb.AppendLine($"  {func}");
            }
            sb.AppendLine();
        }

        txtDatabaseStructure.Text = sb.ToString();
    }

    private class ColumnInfo
    {
        public string Name { get; set; } = "";
        public string DataType { get; set; } = "";
        public int? MaxLength { get; set; }
        public bool IsNullable { get; set; }
        public string? DefaultValue { get; set; }
    }

    private async Task LoadDatabaseObjects()
    {
        _databaseObjects.Clear();
        cboObjects.Items.Clear();

        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        // Load tables
        var tables = await GetTables(conn);
        foreach (var table in tables)
        {
            var obj = new DatabaseObject
            {
                Name = table,
                Type = "TABLE",
                DisplayName = $"[TABLE] {table}"
            };
            _databaseObjects[obj.DisplayName] = obj;
            cboObjects.Items.Add(obj.DisplayName);
        }

        // Load indexes (only non-primary key indexes)
        var indexes = await GetAllUserIndexes(conn);
        foreach (var idx in indexes)
        {
            var obj = new DatabaseObject
            {
                Name = idx,
                Type = "INDEX",
                DisplayName = $"[INDEX] {idx}"
            };
            _databaseObjects[obj.DisplayName] = obj;
            cboObjects.Items.Add(obj.DisplayName);
        }

        // Load sequences (only standalone sequences, not auto-generated ones)
        var sequences = await GetStandaloneSequences(conn);
        foreach (var seq in sequences)
        {
            var obj = new DatabaseObject
            {
                Name = seq,
                Type = "SEQUENCE",
                DisplayName = $"[SEQUENCE] {seq}"
            };
            _databaseObjects[obj.DisplayName] = obj;
            cboObjects.Items.Add(obj.DisplayName);
        }

        // Load views
        var views = await GetViews(conn);
        foreach (var view in views)
        {
            var obj = new DatabaseObject
            {
                Name = view,
                Type = "VIEW",
                DisplayName = $"[VIEW] {view}"
            };
            _databaseObjects[obj.DisplayName] = obj;
            cboObjects.Items.Add(obj.DisplayName);
        }

        // Load functions
        var functions = await GetFunctions(conn);
        foreach (var func in functions)
        {
            var obj = new DatabaseObject
            {
                Name = func,
                Type = "FUNCTION",
                DisplayName = $"[FUNCTION] {func}"
            };
            _databaseObjects[obj.DisplayName] = obj;
            cboObjects.Items.Add(obj.DisplayName);
        }

        if (cboObjects.Items.Count > 0)
        {
            cboObjects.SelectedIndex = 0;
        }
    }

    private async Task<List<string>> GetTables(NpgsqlConnection conn)
    {
        var tables = new List<string>();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT table_name
            FROM information_schema.tables
            WHERE table_schema = 'public' AND table_type = 'BASE TABLE'
            ORDER BY table_name";

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            tables.Add(reader.GetString(0));
        }
        return tables;
    }

    private async Task<List<string>> GetTableIndexes(NpgsqlConnection conn, string tableName)
    {
        var indexes = new List<string>();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT indexname
            FROM pg_indexes
            WHERE schemaname = 'public' AND tablename = @tableName
            ORDER BY indexname";
        cmd.Parameters.AddWithValue("tableName", tableName);

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            indexes.Add(reader.GetString(0));
        }
        return indexes;
    }

    private async Task<List<string>> GetAllUserIndexes(NpgsqlConnection conn)
    {
        var indexes = new List<string>();
        await using var cmd = conn.CreateCommand();
        // Exclude primary key indexes and unique constraint indexes
        cmd.CommandText = @"
            SELECT i.indexname
            FROM pg_indexes i
            LEFT JOIN pg_constraint c ON c.conname = i.indexname
            WHERE i.schemaname = 'public'
                AND c.conname IS NULL
                AND i.indexname NOT LIKE '%_pkey'
            ORDER BY i.indexname";

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            indexes.Add(reader.GetString(0));
        }
        return indexes;
    }

    private async Task<List<string>> GetTableConstraints(NpgsqlConnection conn, string tableName)
    {
        var constraints = new List<string>();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT
                tc.constraint_name,
                tc.constraint_type
            FROM information_schema.table_constraints tc
            WHERE tc.table_schema = 'public' AND tc.table_name = @tableName
            ORDER BY tc.constraint_name";
        cmd.Parameters.AddWithValue("tableName", tableName);

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var name = reader.GetString(0);
            var type = reader.GetString(1);
            constraints.Add($"{name} ({type})");
        }
        return constraints;
    }

    private async Task<List<string>> GetSequences(NpgsqlConnection conn)
    {
        var sequences = new List<string>();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT sequence_name
            FROM information_schema.sequences
            WHERE sequence_schema = 'public'
            ORDER BY sequence_name";

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            sequences.Add(reader.GetString(0));
        }
        return sequences;
    }

    private async Task<List<string>> GetStandaloneSequences(NpgsqlConnection conn)
    {
        var sequences = new List<string>();
        await using var cmd = conn.CreateCommand();
        // Get sequences that are not owned by any table column (standalone sequences)
        cmd.CommandText = @"
            SELECT s.relname
            FROM pg_class s
            JOIN pg_namespace n ON s.relnamespace = n.oid
            WHERE s.relkind = 'S'
                AND n.nspname = 'public'
                AND NOT EXISTS (
                    SELECT 1 FROM pg_depend d
                    WHERE d.objid = s.oid AND d.deptype = 'a'
                )
            ORDER BY s.relname";

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            sequences.Add(reader.GetString(0));
        }
        return sequences;
    }

    private async Task<List<string>> GetViews(NpgsqlConnection conn)
    {
        var views = new List<string>();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT table_name
            FROM information_schema.views
            WHERE table_schema = 'public'
            ORDER BY table_name";

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            views.Add(reader.GetString(0));
        }
        return views;
    }

    private async Task<List<string>> GetFunctions(NpgsqlConnection conn)
    {
        var functions = new List<string>();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT DISTINCT routine_name
            FROM information_schema.routines
            WHERE routine_schema = 'public' AND routine_type = 'FUNCTION'
            ORDER BY routine_name";

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            functions.Add(reader.GetString(0));
        }
        return functions;
    }

    private async void CboObjects_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cboObjects.SelectedItem == null) return;

        var displayName = cboObjects.SelectedItem.ToString();
        if (string.IsNullOrEmpty(displayName) || !_databaseObjects.TryGetValue(displayName, out var obj)) return;

        try
        {
            Cursor = Cursors.WaitCursor;
            var query = await GenerateCreateQuery(obj);
            txtQuery.Text = query;
        }
        catch (Exception ex)
        {
            txtQuery.Text = $"-- Error generating query: {ex.Message}";
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async Task<string> GenerateCreateQuery(DatabaseObject obj)
    {
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        return obj.Type switch
        {
            "TABLE" => await GenerateTableCreateQuery(conn, obj.Name),
            "INDEX" => await GenerateIndexCreateQuery(conn, obj.Name),
            "SEQUENCE" => await GenerateSequenceCreateQuery(conn, obj.Name),
            "VIEW" => await GenerateViewCreateQuery(conn, obj.Name),
            "FUNCTION" => await GenerateFunctionCreateQuery(conn, obj.Name),
            _ => $"-- Unknown object type: {obj.Type}"
        };
    }

    private async Task<string> GenerateTableCreateQuery(NpgsqlConnection conn, string tableName)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"-- Table: {tableName}");
        sb.AppendLine($"CREATE TABLE {tableName} (");

        // Get columns with SERIAL detection
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT
                c.column_name,
                c.data_type,
                c.character_maximum_length,
                c.numeric_precision,
                c.numeric_scale,
                c.is_nullable,
                c.column_default,
                CASE
                    WHEN c.column_default LIKE 'nextval%' AND c.data_type = 'integer' THEN 'SERIAL'
                    WHEN c.column_default LIKE 'nextval%' AND c.data_type = 'bigint' THEN 'BIGSERIAL'
                    WHEN c.column_default LIKE 'nextval%' AND c.data_type = 'smallint' THEN 'SMALLSERIAL'
                    ELSE NULL
                END as serial_type
            FROM information_schema.columns c
            WHERE c.table_schema = 'public' AND c.table_name = @tableName
            ORDER BY c.ordinal_position";
        cmd.Parameters.AddWithValue("tableName", tableName);

        var columns = new List<string>();
        await using (var reader = await cmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                var colName = reader.GetString(0);
                var dataType = reader.GetString(1);
                var maxLength = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2);
                var precision = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3);
                var scale = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4);
                var nullable = reader.GetString(5) == "YES";
                var defaultVal = reader.IsDBNull(6) ? null : reader.GetString(6);
                var serialType = reader.IsDBNull(7) ? null : reader.GetString(7);

                string typeStr;
                string defaultStr;

                if (!string.IsNullOrEmpty(serialType))
                {
                    // Use SERIAL/BIGSERIAL instead of INTEGER + DEFAULT nextval
                    typeStr = serialType;
                    defaultStr = "";
                }
                else
                {
                    typeStr = FormatDataType(dataType, maxLength, precision, scale);
                    defaultStr = string.IsNullOrEmpty(defaultVal) ? "" : $" DEFAULT {defaultVal}";
                }

                var nullStr = nullable ? "" : " NOT NULL";
                columns.Add($"    {colName} {typeStr}{nullStr}{defaultStr}");
            }
        }

        // Get primary key
        await using var pkCmd = conn.CreateCommand();
        pkCmd.CommandText = @"
            SELECT kcu.column_name
            FROM information_schema.table_constraints tc
            JOIN information_schema.key_column_usage kcu
                ON tc.constraint_name = kcu.constraint_name
                AND tc.table_schema = kcu.table_schema
            WHERE tc.table_schema = 'public'
                AND tc.table_name = @tableName
                AND tc.constraint_type = 'PRIMARY KEY'
            ORDER BY kcu.ordinal_position";
        pkCmd.Parameters.AddWithValue("tableName", tableName);

        var pkColumns = new List<string>();
        await using (var reader = await pkCmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                pkColumns.Add(reader.GetString(0));
            }
        }

        if (pkColumns.Count > 0)
        {
            columns.Add($"    PRIMARY KEY ({string.Join(", ", pkColumns)})");
        }

        sb.AppendLine(string.Join(",\n", columns));
        sb.AppendLine(");");

        return sb.ToString();
    }

    private async Task<string> GenerateIndexCreateQuery(NpgsqlConnection conn, string indexName)
    {
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT indexdef
            FROM pg_indexes
            WHERE schemaname = 'public' AND indexname = @indexName";
        cmd.Parameters.AddWithValue("indexName", indexName);

        var result = await cmd.ExecuteScalarAsync();
        if (result != null)
        {
            return $"{result};";
        }
        return $"-- Index not found: {indexName}";
    }

    private async Task<string> GenerateSequenceCreateQuery(NpgsqlConnection conn, string sequenceName)
    {
        var sb = new StringBuilder();

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT
                data_type,
                start_value,
                minimum_value,
                maximum_value,
                increment,
                cycle_option
            FROM information_schema.sequences
            WHERE sequence_schema = 'public' AND sequence_name = @seqName";
        cmd.Parameters.AddWithValue("seqName", sequenceName);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            var dataType = reader.GetString(0);
            var startValue = reader.GetString(1);
            var minValue = reader.GetString(2);
            var maxValue = reader.GetString(3);
            var increment = reader.GetString(4);
            var cycle = reader.GetString(5) == "YES" ? "CYCLE" : "NO CYCLE";

            sb.AppendLine($"CREATE SEQUENCE {sequenceName}");
            sb.AppendLine($"    AS {dataType}");
            sb.AppendLine($"    START WITH {startValue}");
            sb.AppendLine($"    INCREMENT BY {increment}");
            sb.AppendLine($"    MINVALUE {minValue}");
            sb.AppendLine($"    MAXVALUE {maxValue}");
            sb.AppendLine($"    {cycle};");
        }
        else
        {
            sb.AppendLine($"-- Sequence not found: {sequenceName}");
        }

        return sb.ToString();
    }

    private async Task<string> GenerateViewCreateQuery(NpgsqlConnection conn, string viewName)
    {
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT view_definition
            FROM information_schema.views
            WHERE table_schema = 'public' AND table_name = @viewName";
        cmd.Parameters.AddWithValue("viewName", viewName);

        var result = await cmd.ExecuteScalarAsync();
        if (result != null)
        {
            return $"CREATE OR REPLACE VIEW {viewName} AS\n{result}";
        }
        return $"-- View not found: {viewName}";
    }

    private async Task<string> GenerateFunctionCreateQuery(NpgsqlConnection conn, string functionName)
    {
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT pg_get_functiondef(p.oid)
            FROM pg_proc p
            JOIN pg_namespace n ON p.pronamespace = n.oid
            WHERE n.nspname = 'public' AND p.proname = @funcName
            LIMIT 1";
        cmd.Parameters.AddWithValue("funcName", functionName);

        var result = await cmd.ExecuteScalarAsync();
        if (result != null)
        {
            return $"{result};";
        }
        return $"-- Function not found: {functionName}";
    }

    private static string FormatDataType(string dataType, int? maxLength, int? precision, int? scale)
    {
        return dataType.ToLower() switch
        {
            "character varying" => maxLength.HasValue ? $"VARCHAR({maxLength})" : "VARCHAR",
            "character" => maxLength.HasValue ? $"CHAR({maxLength})" : "CHAR",
            "numeric" => precision.HasValue && scale.HasValue ? $"NUMERIC({precision},{scale})" : "NUMERIC",
            "integer" => "INTEGER",
            "bigint" => "BIGINT",
            "smallint" => "SMALLINT",
            "real" => "REAL",
            "double precision" => "DOUBLE PRECISION",
            "boolean" => "BOOLEAN",
            "timestamp without time zone" => "TIMESTAMP",
            "timestamp with time zone" => "TIMESTAMPTZ",
            "date" => "DATE",
            "time without time zone" => "TIME",
            "time with time zone" => "TIMETZ",
            "text" => "TEXT",
            "bytea" => "BYTEA",
            "uuid" => "UUID",
            "json" => "JSON",
            "jsonb" => "JSONB",
            "array" => "ARRAY",
            _ => dataType.ToUpper()
        };
    }

    private void BtnCopyStructure_Click(object? sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtDatabaseStructure.Text))
        {
            Clipboard.SetText(txtDatabaseStructure.Text);
            MessageBox.Show("Database structure copied to clipboard.", "Copied",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void BtnCopyQuery_Click(object? sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtQuery.Text))
        {
            Clipboard.SetText(txtQuery.Text);
            MessageBox.Show("Query copied to clipboard.", "Copied",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private async void BtnExportAll_Click(object? sender, EventArgs e)
    {
        using var saveDialog = new SaveFileDialog
        {
            Filter = "SQL Files (*.sql)|*.sql|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
            DefaultExt = "sql",
            FileName = "database_schema.sql"
        };

        if (saveDialog.ShowDialog() != DialogResult.OK) return;

        try
        {
            Cursor = Cursors.WaitCursor;

            var script = await GenerateFullDatabaseScript();
            await File.WriteAllTextAsync(saveDialog.FileName, script);

            MessageBox.Show(
                $"Database schema exported successfully!\n\n" +
                $"File: {saveDialog.FileName}\n\n" +
                "You can execute this script in any PostgreSQL database to recreate the same structure.",
                "Export Complete",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error exporting schema: {ex.Message}", "Export Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async Task<string> GenerateFullDatabaseScript()
    {
        var sb = new StringBuilder();

        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        // Header
        sb.AppendLine("-- ============================================================");
        sb.AppendLine($"-- Database Schema Export");
        sb.AppendLine($"-- Source Database: {conn.Database}");
        sb.AppendLine($"-- Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        sb.AppendLine($"-- PostgreSQL Version: {conn.PostgreSqlVersion}");
        sb.AppendLine("-- ============================================================");
        sb.AppendLine("-- This script will recreate the entire database structure.");
        sb.AppendLine("-- Execute this script in an empty PostgreSQL database.");
        sb.AppendLine("-- ============================================================");
        sb.AppendLine();

        // Get tables sorted by dependencies (tables without foreign keys first)
        var sortedTables = await GetTablesSortedByDependency(conn);

        // 1. Create standalone sequences first
        var standaloneSequences = await GetStandaloneSequences(conn);
        if (standaloneSequences.Count > 0)
        {
            sb.AppendLine("-- ============================================================");
            sb.AppendLine("-- SEQUENCES");
            sb.AppendLine("-- ============================================================");
            sb.AppendLine();

            foreach (var seq in standaloneSequences)
            {
                var query = await GenerateSequenceCreateQuery(conn, seq);
                sb.AppendLine(query);
                sb.AppendLine();
            }
        }

        // 2. Create tables (in dependency order - no foreign keys yet)
        sb.AppendLine("-- ============================================================");
        sb.AppendLine("-- TABLES");
        sb.AppendLine("-- ============================================================");
        sb.AppendLine();

        foreach (var table in sortedTables)
        {
            var query = await GenerateTableCreateQuery(conn, table);
            sb.AppendLine(query);
            sb.AppendLine();
        }

        // 3. Add foreign key constraints after all tables are created
        var foreignKeys = await GetAllForeignKeys(conn);
        if (foreignKeys.Count > 0)
        {
            sb.AppendLine("-- ============================================================");
            sb.AppendLine("-- FOREIGN KEY CONSTRAINTS");
            sb.AppendLine("-- ============================================================");
            sb.AppendLine();

            foreach (var fk in foreignKeys)
            {
                sb.AppendLine(fk);
                sb.AppendLine();
            }
        }

        // 4. Create indexes
        var userIndexes = await GetAllUserIndexes(conn);
        if (userIndexes.Count > 0)
        {
            sb.AppendLine("-- ============================================================");
            sb.AppendLine("-- INDEXES");
            sb.AppendLine("-- ============================================================");
            sb.AppendLine();

            foreach (var idx in userIndexes)
            {
                var query = await GenerateIndexCreateQuery(conn, idx);
                sb.AppendLine(query);
                sb.AppendLine();
            }
        }

        // 5. Create views
        var views = await GetViews(conn);
        if (views.Count > 0)
        {
            sb.AppendLine("-- ============================================================");
            sb.AppendLine("-- VIEWS");
            sb.AppendLine("-- ============================================================");
            sb.AppendLine();

            foreach (var view in views)
            {
                var query = await GenerateViewCreateQuery(conn, view);
                sb.AppendLine(query);
                sb.AppendLine();
            }
        }

        // 6. Create functions
        var functions = await GetFunctions(conn);
        if (functions.Count > 0)
        {
            sb.AppendLine("-- ============================================================");
            sb.AppendLine("-- FUNCTIONS");
            sb.AppendLine("-- ============================================================");
            sb.AppendLine();

            foreach (var func in functions)
            {
                var query = await GenerateFunctionCreateQuery(conn, func);
                sb.AppendLine(query);
                sb.AppendLine();
            }
        }

        sb.AppendLine("-- ============================================================");
        sb.AppendLine("-- END OF SCHEMA EXPORT");
        sb.AppendLine("-- ============================================================");

        return sb.ToString();
    }

    private async Task<List<string>> GetTablesSortedByDependency(NpgsqlConnection conn)
    {
        // Get all tables
        var tables = await GetTables(conn);

        // Get foreign key dependencies
        var dependencies = new Dictionary<string, List<string>>();
        foreach (var table in tables)
        {
            dependencies[table] = new List<string>();
        }

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT DISTINCT
                tc.table_name,
                ccu.table_name AS referenced_table
            FROM information_schema.table_constraints tc
            JOIN information_schema.constraint_column_usage ccu
                ON tc.constraint_name = ccu.constraint_name
            WHERE tc.table_schema = 'public'
                AND tc.constraint_type = 'FOREIGN KEY'
                AND tc.table_name != ccu.table_name";

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var table = reader.GetString(0);
            var referencedTable = reader.GetString(1);
            if (dependencies.ContainsKey(table))
            {
                dependencies[table].Add(referencedTable);
            }
        }

        // Topological sort
        var sorted = new List<string>();
        var visited = new HashSet<string>();
        var visiting = new HashSet<string>();

        void Visit(string table)
        {
            if (visited.Contains(table)) return;
            if (visiting.Contains(table))
            {
                // Circular dependency - just add it
                sorted.Add(table);
                visited.Add(table);
                return;
            }

            visiting.Add(table);

            if (dependencies.TryGetValue(table, out var deps))
            {
                foreach (var dep in deps)
                {
                    if (dependencies.ContainsKey(dep))
                    {
                        Visit(dep);
                    }
                }
            }

            visiting.Remove(table);
            visited.Add(table);
            sorted.Add(table);
        }

        foreach (var table in tables)
        {
            Visit(table);
        }

        return sorted;
    }

    private async Task<List<string>> GetAllForeignKeys(NpgsqlConnection conn)
    {
        var foreignKeys = new List<string>();

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT
                tc.table_name,
                tc.constraint_name,
                kcu.column_name,
                ccu.table_name AS foreign_table_name,
                ccu.column_name AS foreign_column_name,
                rc.delete_rule,
                rc.update_rule
            FROM information_schema.table_constraints tc
            JOIN information_schema.key_column_usage kcu
                ON tc.constraint_name = kcu.constraint_name
                AND tc.table_schema = kcu.table_schema
            JOIN information_schema.constraint_column_usage ccu
                ON tc.constraint_name = ccu.constraint_name
            JOIN information_schema.referential_constraints rc
                ON tc.constraint_name = rc.constraint_name
            WHERE tc.table_schema = 'public'
                AND tc.constraint_type = 'FOREIGN KEY'
            ORDER BY tc.table_name, tc.constraint_name";

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var tableName = reader.GetString(0);
            var constraintName = reader.GetString(1);
            var colName = reader.GetString(2);
            var foreignTable = reader.GetString(3);
            var foreignCol = reader.GetString(4);
            var deleteRule = reader.GetString(5);
            var updateRule = reader.GetString(6);

            var onDelete = deleteRule != "NO ACTION" ? $" ON DELETE {deleteRule}" : "";
            var onUpdate = updateRule != "NO ACTION" ? $" ON UPDATE {updateRule}" : "";

            foreignKeys.Add($"ALTER TABLE {tableName} ADD CONSTRAINT {constraintName} FOREIGN KEY ({colName}) REFERENCES {foreignTable}({foreignCol}){onDelete}{onUpdate};");
        }

        return foreignKeys;
    }

    private void BtnClose_Click(object? sender, EventArgs e)
    {
        this.Close();
    }

    private class DatabaseObject
    {
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public string DisplayName { get; set; } = "";
    }
}
