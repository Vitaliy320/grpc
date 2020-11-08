using dbms_core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GrpcGreeter.Implementations
{
    public class TSQLStorageManager : IStorageManager
    {

        private string _server;
        private string _db;
        private string _table;

        private string _tableNameForDbTable;

        private string _connectionString =>
            $"Server={_server};Database={_db};Trusted_Connection=True;Integrated Security=SSPI;MultipleActiveResultSets=true";

        public Table GetSelectedTable(string name)
        {
            Table currentTable = JsonSerializer.Deserialize<Table>();
            return currentTable;
        }

        public Database GetDatabaseFromPath(string path)
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

        public void SerialiseTable(Table table)
        {
            var json = JsonSerializer.Serialize(table);
            File.WriteAllText(table.TablePath, json);
        }
    }
}
