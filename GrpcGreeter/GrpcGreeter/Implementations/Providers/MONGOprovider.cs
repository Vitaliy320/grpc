using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter.Implementations.Providers
{
    public class MONGOprovider
    {
        private string _connectionString;
        private string _dbName;
        private string _tableName;
        private string _dbTableName;

        private const string DATA = "Data";

        private MongoClient _mongoClient;
        private IMongoDatabase _dataBase;
        private IMongoDatabase DataBase
        {
            get
            {
                if (_dataBase == null)
                    _dataBase = _mongoClient.GetDatabase(_dbName);
                return _dataBase;
            }
            set
            {
                _dataBase = value;
            }
        }

        public MONGOprovider(string connectionString, string dbName, string tableName = "")
        {
            _connectionString = connectionString;
            _dbName = dbName;
            _tableName = tableName;
            _dbTableName = $"db_{_dbName}_name";

            _mongoClient = new MongoClient(_connectionString);
        }

        public void ClearTable()
        {
            DataBase.DropCollection(_tableName);
            CreateTable();
        }

        public void CreateTable()
        {
            try
            {
                DataBase.CreateCollection(_tableName);
                var collection = DataBase.GetCollection<BsonDocument>(_tableName);
                collection.InsertOne(new BsonDocument
            {
                {DATA, new BsonArray() }
            });
            }
            catch
            {

            }
        }

        public void DeleteTable()
        {
            DataBase.DropCollection(_tableName);
        }

        public List<string> GetData()
        {
            var collection = DataBase.GetCollection<DbData>(_tableName);

            var data = collection.Find(new BsonDocument()).First();

            return data.Data;
        }

        public async Task<List<string>> GetDataAsync()
        {
            var collection = DataBase.GetCollection<string>(_tableName);

            var data = (await collection.FindAsync(new BsonDocument())).ToList();
            return data;
        }

        public string GetDb()
        {
            var collection = DataBase.GetCollection<DbDataBaseInfo>(_dbTableName);
            var data = collection.Find(new BsonDocument()).First();
            return data.Data;
        }

        public void InsertData(List<string> data)
        {
            var collection = DataBase.GetCollection<BsonDocument>(_tableName);
            var update = Builders<BsonDocument>.Update.PushEach(DATA, data);
            collection.UpdateMany(new BsonDocument(), update);
        }

        public async Task InsertDataAsync(List<string> data)
        {
            var collection = DataBase.GetCollection<BsonDocument>(_tableName);
            var update = Builders<BsonDocument>.Update.Push(DATA, data);
            await collection.UpdateManyAsync(default, update);
        }

        public void UpdateOrCreateDb(string stringDbData)
        {
            try
            {
                DataBase.DropCollection(_dbTableName);
            }
            finally
            {
                DataBase.CreateCollection(_dbTableName);
                var collection = DataBase.GetCollection<BsonDocument>(_dbTableName);

                collection.InsertOne(new BsonDocument
                {
                    {DATA, stringDbData }
                });
            }
        }

        public void DeleteDatabase()
        {
            try
            {
                _mongoClient.DropDatabase(_dbName);
            }
            catch { }
        }

        public List<string> GetDbsNames()
        {
            return _mongoClient.ListDatabaseNames().ToList();
        }

        private class DbData
        {
            public object _id { get; set; }
            public List<string> Data { get; set; }
        }

        private class DbDataBaseInfo
        {
            public object _id { get; set; }
            public string Data { get; set; }
        }
    }
}
