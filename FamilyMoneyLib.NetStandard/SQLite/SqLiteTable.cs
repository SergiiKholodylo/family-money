using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using FamilyMoneyLib.NetStandard.Storages;
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
            try
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
            catch (SqliteException e)
            {
                throw new StorageException(e.Message);
            }
        }

        public void DeleteDatabase()
        {
            try
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
            catch (SqliteException e)
            {
                throw new StorageException(e.Message);
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
            try
            {
                long id;
                using (var db =
                    new SqliteConnection($"Filename={_database}"))
                {
                    db.Open();

                    var fields = string.Empty;
                    var values = string.Empty;

                    var keyValuePairs = parameters as KeyValuePair<string, object>[] ?? parameters.ToArray();
                    foreach (var parameter in keyValuePairs)
                    {
                        if (parameter.Value == null) continue;
                        fields += parameter.Key + ",";
                        values += "@" + parameter.Key + ",";
                    }

                    fields = fields.Substring(0, fields.Length > 0 ? fields.Length - 1 : 0);
                    values = values.Substring(0, values.Length > 0 ? values.Length - 1 : 0);

                    var insertCommand = new SqliteCommand
                    {
                        Connection = db, CommandText = $"INSERT OR REPLACE INTO {_tableName} ({fields}) VALUES ({values});SELECT last_insert_rowid();",CommandType = CommandType.Text
                    };

                    foreach (var parameter in keyValuePairs)
                    {
                        if (parameter.Value == null) continue;
                        insertCommand.Parameters.Add(new SqliteParameter("@"+parameter.Key, parameter.Value));
                    }

                    object reader = insertCommand.ExecuteScalar();
                    id = (long)reader;


                    db.Close();
                }

                return id;
            }
            catch (SqliteException e)
            {
                throw new StorageException(e.Message);
            }
        }

        public void UpdateData(IEnumerable<KeyValuePair<string, object>> parameters, long id)
        {
            try
            {
                using (var db =
                    new SqliteConnection($"Filename={_database}"))
                {
                    var values = string.Empty;
                    var keyValuePairs = parameters as KeyValuePair<string, object>[] ?? parameters.ToArray();
                    foreach (var parameter in keyValuePairs)
                    {
                        if (parameter.Value == null) continue;
                        values += parameter.Key + " = @" + parameter.Key + ",";
                    }
                    values = values.Substring(0, values.Length > 0 ? values.Length - 1 : 0);

                    db.Open();

                    var updateCommand = new SqliteCommand
                    {
                        Connection = db,CommandType = CommandType.Text,
                        CommandText = $"UPDATE {_tableName} SET {values} WHERE Id={id}"
                    };
                    foreach (var parameter in keyValuePairs)
                    {
                        if(parameter.Value == null) continue;
                        updateCommand.Parameters.Add(new SqliteParameter("@" + parameter.Key, parameter.Value));
                    }

                    updateCommand.ExecuteReader();
                    db.Close();
                }
            }
            catch (SqliteException e)
            {
                throw new StorageException(e.Message);
            }
        }

        public IEnumerable<IDictionary<string, object>> SelectAll()
        {
            
            Debug.WriteLine($"{_tableName}");
            try
            {
                using (var db =
                    new SqliteConnection($"Filename={_database}"))
                {
                    db.Open();

                    var selectCommand = new SqliteCommand
                        ($"SELECT * from {_tableName}", db);
                    var query = selectCommand.ExecuteReader();
                    var objects = new List<IDictionary<string, object>>();
                    while (query.Read())
                    {
                        var fieldCount = query.FieldCount;
                        var line = new Dictionary<string, object>();
                        for (var i = 0; i < fieldCount; i++)
                        {
                            var name = query.GetName(i);
                            line.Add(name, query.GetValue(i));
                        }

                        objects.Add(line);
                    }

                    db.Close();
                    return objects;
                }
            }
            catch (SqliteException e)
            {
                throw new StorageException(e.Message);
            }
        }

        public void DeleteRecordById(long id)
        {
            try
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
            catch (SqliteException e)
            {
                throw new StorageException(e.Message);
            }
        }
    }
}
