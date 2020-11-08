using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using dbms_core;
using GrpcGreeterClient.dbms_core;

namespace GrpcGreeterClient
{
    class Parser
    {
        private static Dictionary<string, List<string>> commands = new Dictionary<string, List<string>>() 
        {
            { "crtdb",    new List<string>{ "dbname"                                      } },
            { "loaddb",   new List<string>{ "dbname"                                      } },
            { "crtabl",   new List<string>{ "dbname",  "tablname"                         } },
            { "deltabl",  new List<string>{ "dbname",  "tablname"                         } },
            { "addcol",   new List<string>{ "dbname",  "tablname", "colname", "coltype"   } },
            { "addrow",   new List<string>{ "dbname",  "tablname"                         } },
            { "showrows", new List<string>{ "dbname",  "tablname"                         } },
            { "editrow",  new List<string>{ "dbname",  "tablname", "id"                   } },
            { "intabl",   new List<string>{ "dbname",  "tablname", "tablname", "tablname" } }
        };

        RequestSender requestSender = new RequestSender();
        public static async Task Parse(string input)
        {    
            List<string> words = input.Split(' ').ToList();
            List<string> parameters; // = new List<string>();
            List<string> currentCommand = new List<string>();

            for (int i = 0; i < words.Count; i++)
            {
                if (commands.ContainsKey(words[i]))
                {
                    commands.TryGetValue(words[i], out parameters);

                    await ExecuteCommand(words[i], parameters, words);
                }
            }
        }

        public static async Task ExecuteCommand(string command, List<string> parameters, List<string> words)
        {
            switch (command)
            {
                case "crtdb":
                    {
                        if (parameters[0] == "dbname") 
                        {
                            await RequestSender.CreateDatabaseAsync(words[2]); 
                        }
                        break;
                    }
                case "loaddb":
                    {
                        if (parameters[0] == "dbname")
                        {
                            await RequestSender.LoadDatabase(words[2]);
                        }
                        break;
                    }
                case "crtabl":
                    {
                        if (parameters[0] == "dbname" && parameters[1] == "tablname")
                        {
                            await RequestSender.CreateTable(words[2], words[4]);
                        }
                        break;
                    }
                case "addcol":
                    {
                        if (parameters[0] == "dbname"  && parameters[1] == "tablname" && 
                            parameters[2] == "colname" && parameters[3] == "coltype")
                        {
                            await RequestSender.AddColumn(words[2], words[4], words[6], words[8]);
                        }
                        break;
                    }
                case "addrow":
                    {
                        if (parameters[0] == "dbname" && parameters[1] == "tablname")
                        {
                            string row = "";
                            for (int i = 5; i < words.Count; i++)
                            {
                                row += words[i];
                                row += '&';
                            }
                            await RequestSender.AddRow(words[2], words[4], row);
                        }
                        break;
                    }
                case "showrows":
                    {
                        if (parameters[0] == "dbname" && parameters[1] == "tablname")
                        {
                            await RequestSender.ShowRows(words[2], words[4]);
                        }
                        break;
                    }
                case "editrow":
                    {
                        if (parameters[0] == "dbname" && parameters[1] == "tablname" && parameters[2] == "id")
                        {
                            string row = "";
                            for (int i = 7; i < words.Count; i++)
                            {
                                row += words[i];
                                row += '&';
                            }
                            await RequestSender.EditRow(words[2], words[4], Convert.ToInt32(words[6]), row);
                        }
                        break;
                    }
                case "intabl":
                    {
                        if (parameters[0] == "dbname" && parameters[1] == "tablname" && parameters[2] == "tablname" && parameters[3] == "tablname")
                        {
                            await RequestSender.IntersectTables(words[2], words[4], words[6], words[8]);
                        }
                        break;
                    }
                case "deltabl":
                    {
                        if (parameters[0] == "dbname" && parameters[1] == "tablname")
                        {
                            await RequestSender.DeleteTable(words[2], words[4]);
                        }
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Error in command: " + command);
                        break;
                    }
            }
                
        }
    }
}
