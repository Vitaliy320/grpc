using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace dbms_core
{
    public class StorageManager
    {
        public Table GetSelectedTable(string dbname, string tablename, string name)
        {
            Table currentTable = JsonSerializer.Deserialize<Table>(File.ReadAllText(name));
            return currentTable;
        }

        public Database GetDatabaseFromPath(string dbname, string path)
        {
            Database database = JsonSerializer.Deserialize<Database>(File.ReadAllText(path));

            SerialiseDatabase(database);

            return database;
        }


        public void SerialiseDatabase(Database database)
        {
            var json = JsonSerializer.Serialize(database);
            File.WriteAllText(database.DatabasePath, json);
        }

        public void SerialiseTable(Table table, string dbName)
        {
            var json = JsonSerializer.Serialize(table);
            File.WriteAllText(table.TablePath, json);
        }
    }
}
