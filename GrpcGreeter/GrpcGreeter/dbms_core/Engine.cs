using GrpcGreeter;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;

namespace dbms_core
{
    class Engine
    {
        Database database;
        IStorageManager storageManager = new FileStorageManager();

        Table currentTable;
        Table intersectionTable = new Table();

        Constants constants = new Constants();

        public void CreateDatabase(string dbName, string directoryPath)
        {
            
            directoryPath = constants.path + dbName;
            DirectoryInfo dbDirectory = Directory.CreateDirectory(directoryPath);
            string databasePath = directoryPath + constants.backSlash + dbName + constants.databaseExtension;

            database = new Database(dbName, databasePath, directoryPath);

            storageManager.SerialiseDatabase(database);
            //var json = JsonSerializer.Serialize(database);
            //File.WriteAllText(database.DatabasePath, json);
            //return database;
        }

        public void SetServerDatabase(string dbName)
        {
            string dbPath = constants.path + dbName + constants.backSlash + dbName + constants.databaseExtension;
            database = storageManager.GetDatabaseFromPath(dbName, dbPath);
        }

        public void SetServerTable(string dbname, string tableName)
        {
            string tablePath = constants.path + database.DatabaseName + constants.backSlash + tableName + constants.tableExtension;
            currentTable = storageManager.GetSelectedTable(database.DatabaseName, tableName, tablePath);
        }

        public void SetIntersectionTable(string tableName)
        {
            intersectionTable.TableName = tableName;
        }

        public string GetDirectoryPath()
        {
            return database.DirectoryPath;
        }


        public void CreateTable(string tableName)
        {
            string tablePath = database.DirectoryPath + constants.backSlash + tableName + constants.tableExtension;
            Table table = new Table(tableName, tablePath);

            database.AddTable(table);


            storageManager.SerialiseTable(table, database.DatabaseName);
            storageManager.SerialiseDatabase(database);
        }

        public void DeleteTableFromName(string tableName)
        {
            string tablePath = constants.path + database.DatabaseName + constants.backSlash + tableName + constants.tableExtension;
             
            File.Delete(tablePath);
             
            database.Tables.RemoveAll(t => t.TableName == tableName);

            storageManager.SerialiseDatabase(database);
        }

        public void DeleteTableAtIndex(int index)
        {
            File.Delete(database.Tables[index].TablePath);

            database.Tables.RemoveAt(index);

            storageManager.SerialiseDatabase(database);

            //var jsonDatabase = JsonSerializer.Serialize(database);
            //File.WriteAllText(database.DatabasePath, jsonDatabase);
        }

        public void CreateColumn(string columnName, string columnType)
        {
            Column column = new Column();

            string type = column.AssignType(columnType);

            if (type == "wrong_type")
            {
                Console.WriteLine("Wrong type of column.");
            }
            else
            {
                column.ColumnName = columnName;
                column.ColumnType = columnType;
                currentTable.columns.Add(column);
            }

            storageManager.SerialiseTable(currentTable, database.DatabaseName);

            //var jsonTable = JsonSerializer.Serialize(currentTable);
            //File.WriteAllText(currentTable.TablePath, jsonTable);

            //return column;
        }

        //public Table GetSelectedTable(string name)
        //{
        //    currentTable = JsonSerializer.Deserialize<Table>(File.ReadAllText(name));
        //    return currentTable;
        //}

        public Database GetDatabaseFromPath(string path)
        {
            database = JsonSerializer.Deserialize<Database>(File.ReadAllText(path));

            storageManager.SerialiseDatabase(database);

            //var jsonDatabase = JsonSerializer.Serialize(database);
            //File.WriteAllText(database.DatabasePath, jsonDatabase);

            return database;
        }

        public Database GetCurrentDatabase()
        {
            return database;
        }

        public void IntersectTablesFromClient(string dbName, string tableName2, string tableName3)
        {
            string intersectionTablePath = constants.path + dbName + constants.backSlash + tableName3 + constants.tableExtension;

            string tablePath2 = constants.path + dbName + constants.backSlash + tableName2 + constants.tableExtension;

            IntersectTable(tablePath2, tableName3, intersectionTablePath);
        }

        public void IntersectTable(string tablePath, string intersectionTableName, string intersectionTablePath)
        {
            Table table = JsonSerializer.Deserialize<Table>(File.ReadAllText(tablePath));

            Table intersectionTable = new Table();

            List<List<Cell>> result = new List<List<Cell>>();
            Table newTable = new Table();

            if (CheckColumnTypes(currentTable, table))
            {
                int count1 = currentTable.RowsList.Count;

                int count2 = table.RowsList.Count;

                for (int i = 0; i < count1; i++)
                {
                    for (int j = 0; j < count2; j++)
                    {
                        if (CheckRowMatch(currentTable.RowsList[i], table.RowsList[j]))
                        {
                            result.Add(currentTable.RowsList[i]);
                        }
                    }
                }
            }
            
            intersectionTable.RowsList = result;
            intersectionTable.columns = table.columns;
            intersectionTable.TableName = intersectionTableName;
            intersectionTable.TablePath = intersectionTablePath;

            //var jsonTable = JsonSerializer.Serialize(intersectionTable);
            //File.WriteAllText(intersectionTable.TablePath, jsonTable);

            storageManager.SerialiseTable(intersectionTable, database.DatabaseName);

            database.AddTable(intersectionTable);

            storageManager.SerialiseDatabase(database);
            //var jsonDatabase = JsonSerializer.Serialize(database);
            //File.WriteAllText(database.DatabasePath, jsonDatabase);

        }

        private bool CheckRowMatch(List<Cell> list1, List<Cell> list2)
        {
            if (list1.Count != list2.Count)
                return false;

            for (int i = 1; i < list1.Count; i++)
            {
                if (list1[i].Value != list2[i].Value)
                    return false;
            }
            return true;
        }

        private bool CheckColumnTypes(Table table1, Table table2)
        {
            if (table1.columns.Count != table2.columns.Count)
            {
                return false;
            }

            int counter = 0;
            foreach (Column column1 in table1.columns)
            {
                foreach (Column column2 in table2.columns)
                {
                    if (column1.ColumnType == column2.ColumnType &&
                        column1.ColumnName == column2.ColumnName)
                    {
                        counter++;
                    }
                }
            }
            if (counter == table1.columns.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int DeleteCellsAtIds(IEnumerable<string> ids, int numberOfRows)
        {
            currentTable.RowsList.RemoveAll(x => ids.Contains(x[0].Value));

            //var jsonTable = JsonSerializer.Serialize(currentTable);
            //File.WriteAllText(currentTable.TablePath, jsonTable);

            storageManager.SerialiseTable(currentTable, database.DatabaseName);

            return currentTable.columns.Count();
        }

        public void DeleteRow(int id)
        {
            for (int i = 0; i < currentTable.RowsList.Count; i++)
            {
                if (currentTable.RowsList[i][1].Value == id.ToString())
                {
                    currentTable.RowsList.RemoveAt(i);
                }
            }
        }

        public void AddRowFromClient(string value)
        {
            List<string> cellValues = value.Split('&').ToList<string>();
            List<Cell> cells = new List<Cell>();

            cells.Add(new Cell { Value = Guid.NewGuid().ToString(), Type = typeof(Guid).AssemblyQualifiedName });

            foreach (var cellValue in cellValues)
            {
                if (cellValue.Length > 0)
                {
                    cells.Add(new Cell(cellValue));
                }
            }

            AddRow(cells);
        }
        public void AddRow(List<Cell> row)
        {
            currentTable.RowsList.Add(row);

            storageManager.SerialiseTable(currentTable, database.DatabaseName);
            //var jsonTable = JsonSerializer.Serialize(currentTable);
            //File.WriteAllText(currentTable.TablePath, jsonTable);
        }

        public string GetRows()
        {
            string rows = "";
            for (int i = 0; i < currentTable.RowsList.Count; i++)
            {
                for (int j = 1; j < currentTable.RowsList[i].Count; j++)
                {
                    rows += currentTable.RowsList[i][j].Value + " ";
                }
                rows += "\n";
            }
            return rows;
        }

        public void AddEditedRows(List<Cell> cellList)
        {
            currentTable.RowsList.Add(cellList);

            storageManager.SerialiseTable(currentTable, database.DatabaseName);
            //var jsonTable = JsonSerializer.Serialize(currentTable);
            //File.WriteAllText(currentTable.TablePath, jsonTable);
        }

        public string AssignType(string type)
        {
            switch (type)
            {
                case "integer":
                    return "integer";
                case "real":
                    return "real";
                case "char":
                    return "char";
                case "string":
                    return "string";
                case "cinteger":
                    return "cinteger";
                case "creal":
                    return "creal";
                default: return "wrong_type";
            }
        }

        //public void SerialiseDatabase(Database database)
        //{
        //    var json = JsonSerializer.Serialize(database);
        //    File.WriteAllText(database.DatabasePath, json);
        //}

        //public void SerialiseTable(Table table)
        //{
        //    var json = JsonSerializer.Serialize(table);
        //    File.WriteAllText(table.TablePath, json);
        //}



        //public void SerialiseToFile(object objectToBeSerialised, int a)
        //{
        //    var json = JsonSerializer.Serialize(objectToBeSerialised);
        //    if (objectToBeSerialised.GetType() == typeof(Database))
        //    {
        //        File.WriteAllText((Database)objectToBeSerialised.data, jsonTable);
        //    }
        //}
    }
}
