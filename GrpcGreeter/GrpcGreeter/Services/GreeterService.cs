using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dbms_core;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcGreeter
{
    public class GreeterService : Greeter.GreeterBase
    {
        Engine engine = new Engine();

        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override Task<DatabaseReply> CreateDatabase(DatabaseRequest request, ServerCallContext context)
        {
            engine.CreateDatabase(request.Name, request.Path);

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("===============================================================================================================");
            Console.WriteLine("Database created:");
            Console.WriteLine("name = " + request.Name + ", path = " + request.Path);
            Console.WriteLine("===============================================================================================================");

            return Task.FromResult(new DatabaseReply
            {
                DatabaseName = request.Name,
                DatabasePath = request.Path
            });
        }

        public override Task<LoadDatabaseReply> LoadDatabaseFromPath(LoadDatabaseRequest request, ServerCallContext context)
        {
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("===============================================================================================================");
            Console.WriteLine("Database loaded:");
            Console.WriteLine("name = " + request.DatabaseName + ", path = " + request.DatabasePath);
            Console.WriteLine("===============================================================================================================");


            engine.GetDatabaseFromPath(request.DatabasePath);
            return Task.FromResult(new LoadDatabaseReply
            {
                DatabaseName = request.DatabaseName,
                DatabasePath = request.DatabasePath
            });
        }

        public override Task<CreateTableReply> CreateTable(CreateTableRequest request, ServerCallContext context)
        {
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("===============================================================================================================");
            Console.WriteLine("Table created:");
            Console.WriteLine("database name = " + request.DatabaseName + ", table = " + request.TableName);
            Console.WriteLine("===============================================================================================================");


            engine.SetServerDatabase(request.DatabaseName);

            engine.CreateTable(request.TableName);

            return Task.FromResult(new CreateTableReply
            {
                TableName = request.TableName,
                DatabaseName = request.DatabaseName
            });
        }

        public override Task<AddColumnReply> AddColumn(AddColumnRequest request, ServerCallContext context)
        {
            engine.SetServerDatabase(request.DatabaseName);
            
            engine.SetServerTable(request.TableName);

            engine.CreateColumn(request.ColumnName, request.ColumnType);

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("===============================================================================================================");
            Console.WriteLine("Column added:");
            Console.WriteLine("database name = " + request.DatabaseName + ", table = "       + request.TableName + 
                              ", column name = " + request.ColumnName   + ", column type = " + request.ColumnType);
            Console.WriteLine("===============================================================================================================");

            return Task.FromResult(new AddColumnReply
            {
                DatabaseName = request.DatabaseName,
                TableName = request.TableName,
                ColumnName = request.ColumnName,
                ColumnType = request.ColumnType
            });
        }

        public override Task<AddRowReply> AddRow(AddRowRequest request, ServerCallContext context)
        {
            engine.SetServerDatabase(request.DatabaseName);

            engine.SetServerTable(request.TableName);

            engine.AddRowFromClient(request.Value);

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("===============================================================================================================");
            Console.WriteLine("Row added:");
            Console.WriteLine("database name = " + request.DatabaseName + ", table = " + request.TableName + ", row = " + "[" + request.Value + "]");
            Console.WriteLine("===============================================================================================================");

            return Task.FromResult(new AddRowReply
            {
                DatabaseName = request.DatabaseName,
                TableName = request.TableName,
                Value = request.Value
            });
        }

        public override Task<ShowRowsReply> ShowRows(ShowRowsRequest request, ServerCallContext context)
        {
            engine.SetServerDatabase(request.DatabaseName);

            engine.SetServerTable(request.TableName);

            string rows = engine.GetRows();

            Console.WriteLine("===============================================================================================================");
            Console.WriteLine("Rows displayed on a client:");
            Console.WriteLine("database name = " + request.DatabaseName + ", table = " + request.TableName);
            Console.WriteLine("===============================================================================================================");

            return Task.FromResult(new ShowRowsReply
            {
                DatabaseName = request.DatabaseName,
                TableName = request.TableName,
                Rows = rows
            });
        }

        public override Task<EditRowReply> EditRow(EditRowRequest request, ServerCallContext context)
        {
            engine.SetServerDatabase(request.DatabaseName);

            engine.SetServerTable(request.TableName);

            engine.DeleteRow(request.Id);

            engine.AddRowFromClient(request.Value);

            Console.WriteLine("===============================================================================================================");
            Console.WriteLine("Row edited:");
            Console.WriteLine("database name = " + request.DatabaseName + ", table = " + request.TableName + ", edited row = " + "[" + request.Value + "]");
            Console.WriteLine("===============================================================================================================");

            return Task.FromResult(new EditRowReply
            {
                DatabaseName = request.DatabaseName,
                TableName = request.TableName,
                Id = request.Id,
                Value = request.Value
            });
        }

        public override Task<IntersectTablesReply> IntersectTables(IntersectTablesRequest request, ServerCallContext context)
        {
            engine.SetServerDatabase(request.DatabaseName);

            engine.SetServerTable(request.TableName1);

            //engine.SetIntersectionTable(request.TableName2);

            engine.IntersectTablesFromClient(request.DatabaseName, request.TableName2, request.Tablename3);

            Console.WriteLine("===============================================================================================================");
            Console.WriteLine("Tables intersected:");
            Console.WriteLine("first table name = " + request.TableName1 + ", second table  name = " + request.TableName2 + ", intersection table name = " + request.Tablename3);
            Console.WriteLine("===============================================================================================================");

            return Task.FromResult(new IntersectTablesReply
            {
                DatabaseName = request.DatabaseName,
                TableName1 = request.TableName1,
                TableName2 = request.TableName2,
                Tablename3 = request.Tablename3
            });
        }

        public override Task<DeleteTableReply> DeleteTable(DeleteTableRequest request, ServerCallContext context)
        {
            engine.SetServerDatabase(request.DatabaseName);

            engine.SetServerTable(request.TableName);

            engine.DeleteTableFromName(request.TableName);

            return Task.FromResult(new DeleteTableReply
            {
                DatabaseName = request.DatabaseName,
                TableName = request.TableName
            });
        }
    }
}
