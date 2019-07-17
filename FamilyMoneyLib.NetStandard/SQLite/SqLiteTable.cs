using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace FamilyMoneyLib.NetStandard.SQLite
{
    //(Primary_Key INTEGER PRIMARY KEY, Text_Entry NVARCHAR(2048) NULL)
    //https://docs.microsoft.com/en-us/windows/uwp/data-access/sqlite-databases

    public class SqLiteTable
    {
        private readonly string _database;
        private readonly string _tableName;
        private readonly string _tableDefinition;

        public SqLiteTable(string database, string tableName, string tableDefinition)
        {
            _database = database;
            _tableName = tableName;
            _tableDefinition = tableDefinition;
        }

        public void InitializeDatabase()
        {
            using (var db =
                new SqliteConnection($"Filename={_database}"))
            {
                db.Open();

                var tableCommand =  "CREATE TABLE IF NOT " +
                                    $"EXISTS {_tableName} "+
                                    _tableDefinition;

                var createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }

        public void DeleteDatabase()
        {
            using (var db =
                new SqliteConnection($"Filename={_database}"))
            {
                db.Open();

                var tableCommand = "DROP TABLE " +
                                   $"'{_tableName}' "; 

                var deleteTable = new SqliteCommand(tableCommand, db);

                deleteTable.ExecuteReader();
            }

        }

        public long AddData(string values)
        {
            long id;
            using (var db =
                new SqliteConnection($"Filename={_database}"))
            {
                db.Open();

                var insertCommand = new SqliteCommand
                {
                    Connection = db, CommandText = $"INSERT INTO {_tableName} VALUES ({values});SELECT last_insert_rowid();"
                };

                // Use parameterized query to prevent SQL injection attacks

                object reader = insertCommand.ExecuteScalar();
                id = (long)reader;


                db.Close();
            }

            return id;
        }


        public void UpdateData(string values, long id)
        {
            using (var db =
                new SqliteConnection($"Filename={_database}"))
            {
                db.Open();

                var updateCommandCommand = new SqliteCommand
                {
                    Connection = db,
                    CommandText = $"UPDATE {_tableName} SET {values} WHERE Id={id}"
                };

                // Use parameterized query to prevent SQL injection attacks

                updateCommandCommand.ExecuteReader();
                db.Close();
            }
        }

        public IEnumerable<object> GetData()
        {
            using (var db =
                new SqliteConnection($"Filename={_database}"))
            {
                db.Open();

                var selectCommand = new SqliteCommand
                    ($"SELECT * from {_tableName}", db);
                var query = selectCommand.ExecuteReader();
                while (query.Read())
                {
                    var fieldCount = query.FieldCount;
                    var objects = new List<object>();
                    for (var i = 0; i < fieldCount; i++)
                    {
                        objects.Add(query.GetValue(i));
                    }

                    db.Close();
                    return objects.ToArray();
                }
            }

            return default(object[]);
        }

        public IEnumerable<object[]> SelectAll()
        {
            using (var db =
                new SqliteConnection($"Filename={_database}"))
            {
                db.Open();

                var selectCommand = new SqliteCommand
                    ($"SELECT * from {_tableName}", db);
                var query = selectCommand.ExecuteReader();
                var objects = new List<object[]>();
                while (query.Read())
                {
                    var fieldCount = query.FieldCount;
                    var line = new List<object>();
                    for (var i = 0; i < fieldCount; i++)
                    {
                        line.Add(query.GetValue(i));
                    }

                    objects.Add(line.ToArray());
                }

                db.Close();
                return objects;
            }
        }

        public void DeleteRecordById(long id)
        {
            using (var db =
                new SqliteConnection($"Filename={_database}"))
            {
                db.Open();

                var tableCommand = "DELETE FROM " +
                                   $"'{_tableName}' WHERE id={id}";

                var createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }
    }
}
