using System;
using System.Net.Http;
using System.Threading.Tasks;
using dbms_core;
//using GrpcGreeter;
using Grpc.Net.Client;
using GrpcGreeterClient.dbms_core;
using System.IO;

namespace GrpcGreeterClient
{
    class Program
    {
        public static RequestSender requestSender = new RequestSender();
        public static Constants constants = new Constants();
        public static Greeter.GreeterClient client;

        static async Task Main(string[] args)
        {
            Parser parser = new Parser();
            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            client = new Greeter.GreeterClient(channel);

            //requestSender requestSender = new requestSender();
            //Constants constants = new Constants();

            bool continueRunning = true;

            while (continueRunning)
            {
                Console.WriteLine("Available commands:\ncrtdb    dbname                           - create a database\ncrtabl   dbname tablname                  - create a table\ndeltabl  dbname tablname                  - delete a table");
                Console.WriteLine("addcol   dbname tablname colname coltype  - add a column\naddrow   dbname tablname                  - add a row\nshowrows dbname tablname                  - show rows of a table\neditrow  dbname tablname index            - edit a row");
                Console.WriteLine("intabl   dbname tablname tablname         - intersect tables");
                Console.WriteLine();
                Console.WriteLine("Type your command:");

                string input = Console.ReadLine();
                await Parser.Parse(input);

                Console.WriteLine();
                Console.WriteLine();
            }



        }
        //public static async Task Test()
        //{
        //    Console.WriteLine("Type a database name:");
        //    var databaseName = Console.ReadLine();

        //    var userPath = constants.path + databaseName + constants.databaseExtension;

        //    var reply = await client.CreateDatabaseAsync(new DatabaseRequest
        //    {
        //        Name = databaseName,
        //        Path = userPath
        //    });

        //    Console.WriteLine("Database created:");
        //    Console.WriteLine(reply.DatabaseName + ", " + reply.DatabasePath);
        //}

        //public static async Task StateStart(RequestSender requestSender, Constants constants)
        //{
        //    Console.WriteLine("Select an action:");
        //    Console.WriteLine("0 - create a database");
        //    Console.WriteLine("1 - load a database");
        //    int choice = Convert.ToInt32(Console.ReadLine());
        //    if (choice == 0)
        //    {
        //        await RequestSender.CreateDatabaseAsync();
        //        StateManageTables();
        //    }
        //    if (choice == 1)
        //    {
        //        await RequestSender.LoadDatabase();
        //        StateManageTables();
        //    }
        //}

        //public static async Task StateManageTables()
        //{
        //    Console.WriteLine("Select an action:");
        //    Console.WriteLine("0 - create a database");
        //    Console.WriteLine("1 - load a database");
        //    Console.WriteLine("2 - create a table");
        //    Console.WriteLine("3 - select a database");
        //    Console.WriteLine("4 - delete a table");

        //    int choice = Convert.ToInt32(Console.ReadLine());

        //    switch (choice)
        //    {
        //        case 0:
        //            {
        //                await RequestSender.CreateDatabaseAsync();
        //                break;
        //            }
        //    }

        //    string databaseName = Console.ReadLine();
        //    string databaseDirectory = constants.path + databaseName;

        //    DirectoryInfo directoryInfo = new DirectoryInfo(constants.path);

        //    //string[] files = Directory.GetFiles(constants.path + databaseName + constants.databaseExtension, "*ProfileHandler.cs", SearchOption.AllDirectories);

        //    //string[] databaseFolders = Directory.GetDirectories(constants.path);

        //    //foreach (var databaseFolder in databaseFolders)
        //    //{
        //    //    if (databaseFolder == databaseDirectory)
        //    //    {
        //    //        requestSender.LoadDatabase(databaseName, databaseDirectory + databaseName + constants.databaseExtension);
        //    //    }
        //    //}
        //}
        

    }
}
