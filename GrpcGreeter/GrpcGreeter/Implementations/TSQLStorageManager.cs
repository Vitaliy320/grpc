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

        public Table GetSelectedTable(string dbname, string tablename, string name)
        {
            TSQLprovider TSQLprovider = new TSQLprovider(Constants.ServerAddress, dbname, tablename);
            Table currentTable = JsonSerializer.Deserialize<Table>(TSQLprovider.GetData()[0]);
            return currentTable;
        }

        public Database GetDatabaseFromPath(string dbname, string path)
        {
            TSQLprovider TSQLprovider = new TSQLprovider(Constants.ServerAddress, dbname);
            

            Database database = JsonSerializer.Deserialize<Database>(TSQLprovider.GetDb());

            //SerialiseDatabase(database);

            return database;
        }

        public void SerialiseDatabase(Database database)
        {
            TSQLprovider TSQLprovider = new TSQLprovider(Constants.ServerAddress, database.DatabaseName);
            var json = JsonSerializer.Serialize(database);
            TSQLprovider.UpdateOrCreateDb(json);
        }

        public void SerialiseTable(Table table, string dbName)
        {
            TSQLprovider TSQLprovider = new TSQLprovider(Constants.ServerAddress, dbName, table.TableName);
            var json = JsonSerializer.Serialize(table);
            TSQLprovider.ClearTable();
            TSQLprovider.CreateTable();
            TSQLprovider.InsertData(new List<string> { json });
        }

        public void DeleteTableFromName(string tableName, string dbName)
        {
            TSQLprovider TSQLprovider = new TSQLprovider(Constants.ServerAddress, dbName, tableName);
            TSQLprovider.DeleteTable();
        }
        
    }
}
