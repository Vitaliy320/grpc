using System;
using System.Net.Http;
using System.Threading.Channels;
using System.Threading.Tasks;
using dbms_core;
//using GrpcGreeter;
using Grpc.Net.Client;
using GrpcGreeterClient.dbms_core;

namespace GrpcGreeterClient.dbms_core
{
    public class RequestSender
    {
        static GrpcChannel channel;
        static Greeter.GreeterClient client;
        static Constants constants = new Constants();

        public RequestSender()
        {
            channel = GrpcChannel.ForAddress("https://localhost:5001");
            client = new Greeter.GreeterClient(channel);
        }
        public static async Task CreateDatabaseAsync(string name)
        {
            //Console.WriteLine("Type a database name:");
            //var databaseName = Console.ReadLine();

            var userPath = constants.path + name + constants.databaseExtension;

            var reply = await client.CreateDatabaseAsync(new DatabaseRequest
            {
                Name = name,
                Path = userPath
            });

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Database created:");
            Console.WriteLine(reply.DatabaseName + ", " + reply.DatabasePath);
        }

        public static async Task LoadDatabase(string name)
        {
            //Console.WriteLine("Type a database name:");
            //var databaseName = Console.ReadLine();

            var userPath = constants.path + name + constants.backSlash + name + constants.databaseExtension;

            var reply = await client.LoadDatabaseFromPathAsync(new LoadDatabaseRequest
            {
                DatabaseName = name,
                DatabasePath = userPath
            });

            var a = reply.DatabasePath;

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Database loadaed: " + name);
        }

        public static async Task CreateTable(string dbName, string tableName)
        {
            var reply = await client.CreateTableAsync(new CreateTableRequest
            {
                DatabaseName = dbName,
                TableName = tableName
            });

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Table created:");
            Console.WriteLine(reply.DatabaseName + ", " + reply.TableName);
        }

        public static async Task AddColumn(string dbName, string tableName, string columnName, string columnType)
        {
            var reply = await client.AddColumnAsync(new AddColumnRequest
            {
                DatabaseName = dbName,
                TableName = tableName,
                ColumnName = columnName,
                ColumnType = columnType
            });

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Column added:");
            Console.WriteLine(reply.DatabaseName + ", " + reply.TableName + ", " + reply.ColumnName + ", " + reply.ColumnType);
        }

        public static async Task AddRow(string dbName, string tableName, string value)
        {
            var reply = await client.AddRowAsync(new AddRowRequest
            {
                DatabaseName = dbName,
                TableName = tableName,
                Value = value
            });

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Row added:");
            Console.WriteLine(reply.DatabaseName + ", " + reply.TableName + ", " + reply.Value);
        }

        public static async Task ShowRows(string dbName, string tableName)
        {
            var reply = await client.ShowRowsAsync(new ShowRowsRequest 
            {
                DatabaseName = dbName,
                TableName = tableName
            });

            Console.WriteLine(reply.Rows);
        }

        public static async Task EditRow(string dbName, string tableName, int id, string value)
        {
            var reply = await client.EditRowAsync(new EditRowRequest
            {
                DatabaseName = dbName,
                TableName = tableName,
                Id = id,
                Value = value
            });
        }

        public static async Task IntersectTables(string dbName, string tablename1, string tablename2, string tablename3)
        {
            var reply = await client.IntersectTablesAsync(new IntersectTablesRequest
            {
                DatabaseName = dbName,
                TableName1 = tablename1,
                TableName2 = tablename2,
                Tablename3 = tablename3
            });
        }

        public static async Task DeleteTable(string dbName, string tablename)
        {
            var reply = await client.DeleteTableAsync(new DeleteTableRequest
            {
                DatabaseName = dbName,
                TableName = tablename
            });
        }
    }
}
