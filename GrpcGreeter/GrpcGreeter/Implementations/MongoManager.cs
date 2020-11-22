using dbms_core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GrpcGreeter.Implementations.Providers;

namespace GrpcGreeter.Implementations
{
    public class MongoManager : IStorageManager
    {
        public Table GetSelectedTable(string dbname, string tablename, string name)
        {
            MONGOprovider MONGOprovider = new MONGOprovider(Constants.ServerAddress, dbname, tablename);
            Table currentTable = JsonSerializer.Deserialize<Table>(MONGOprovider.GetData()[0]);
            return currentTable;
        }

        public Database GetDatabaseFromPath(string dbname, string path)
        {
            MONGOprovider MONGOprovider = new MONGOprovider(Constants.ServerAddress, dbname);


            Database database = JsonSerializer.Deserialize<Database>(MONGOprovider.GetDb());

            //SerialiseDatabase(database);

            return database;
        }

        public void SerialiseDatabase(Database database)
        {
            MONGOprovider MONGOprovider = new MONGOprovider(Constants.ServerAddress, database.DatabaseName);
            var json = JsonSerializer.Serialize(database);
            MONGOprovider.UpdateOrCreateDb(json);
        }

        public void SerialiseTable(Table table, string dbName)
        {
            MONGOprovider MONGOprovider = new MONGOprovider(Constants.ServerAddress, dbName, table.TableName);
            var json = JsonSerializer.Serialize(table);
            MONGOprovider.ClearTable();
            MONGOprovider.CreateTable();
            MONGOprovider.InsertData(new List<string> { json });
        }

        public void DeleteTableFromName(string tableName, string dbName)
        {
            MONGOprovider MONGOprovider = new MONGOprovider(Constants.ServerAddress, dbName, tableName);
            MONGOprovider.DeleteTable();
        }
    }
}
