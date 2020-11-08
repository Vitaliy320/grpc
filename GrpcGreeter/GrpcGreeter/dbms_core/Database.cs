using System;
using System.Collections.Generic;
using System.Text;

namespace dbms_core
{
    public class Database
    {
        public string DatabaseName { get; set; }
        public string DatabasePath { get; set; }
        public string DirectoryPath { get; set; }
        
        public List<Table> Tables { get; set; } = new List<Table>();

        public Database()
        {

        }
        public Database(string name, string databasePath, string directoryPath)
        {
            DatabaseName = name;
            DatabasePath = databasePath;
            DirectoryPath = directoryPath;

        }
        public void AddTable(Table table)
        {
            Tables.Add(table);
        }
    }
}
