using System.Collections.Generic;
using System.Data;
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

        public long AddData(IEnumerable<KeyValuePair<string, object>> parameters)
        {
            /*
            this.command.CommandText = "INSERT INTO StringData (field1, field2) VALUES(@param1, @param2)";
            this.command.CommandType = CommandType.Text;
            this.command.Parameters.Add(new SQLiteParameter("@param1", data.Data));
            this.command.Parameters.Add(new SQLiteParameter("@param2", data.ByteIndex));
             */
            long id;
            using (var db =
                new SqliteConnection($"Filename={_database}"))
            {
                db.Open();

                var fields = string.Empty;
                var values = string.Empty;

                foreach (var parameter in parameters)
                {
                    fields += parameter.Key + ",";
                    values += "@" + parameter.Key + ",";
                }

                fields = fields.Substring(0, fields.Length > 0 ? fields.Length - 1 : 0);
                values = values.Substring(0, values.Length > 0 ? values.Length - 1 : 0);

                var insertCommand = new SqliteCommand
                {
                    Connection = db, CommandText = $"INSERT INTO {_tableName} ({fields}) VALUES ({values});SELECT last_insert_rowid();",CommandType = CommandType.Text
                };

                foreach (var parameter in parameters)
                {
                    insertCommand.Parameters.Add(new SqliteParameter("@"+parameter.Key, parameter.Value));
                }

                // Use parameterized query to prevent SQL injection attacks

                object reader = insertCommand.ExecuteScalar();
                id = (long)reader;


                db.Close();
            }

            return id;
        }


        public void UpdateData(IEnumerable<KeyValuePair<string, object>> parameters, long id)
        {
            using (var db =
                new SqliteConnection($"Filename={_database}"))
            {
                var values = string.Empty;
                foreach (var parameter in parameters)
                {
                    values += parameter.Key + " = @" + parameter.Key + ",";
                }
                values = values.Substring(0, values.Length > 0 ? values.Length - 1 : 0);

                db.Open();

                var updateCommand = new SqliteCommand
                {
                    Connection = db,CommandType = CommandType.Text,
                    CommandText = $"UPDATE {_tableName} SET {values} WHERE Id={id}"
                };
                foreach (var parameter in parameters)
                {
                    updateCommand.Parameters.Add(new SqliteParameter("@" + parameter.Key, parameter.Value));
                }
                // Use parameterized query to prevent SQL injection attacks

                updateCommand.ExecuteReader();
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
