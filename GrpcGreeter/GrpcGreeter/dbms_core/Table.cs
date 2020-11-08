using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace dbms_core
{
    public class Table
    {
        [JsonPropertyName("tableName")]
        public string TableName{ get; set; }
        [JsonPropertyName("tablePath")]
        public string TablePath { get; set; }
        [JsonPropertyName("tableColumns")]
        public List<Column> columns { get; set; } = new List<Column>();
        [JsonPropertyName("tableRowsList")]
        public List<List<Cell>> RowsList { get; set; } = new List<List<Cell>>();

        public Table()
        {

        }
        public Table(string name, string path)
        {
            TableName = name;
            TablePath = path;
        }
        public List<string> ReadTypes(string input)
        {
            char[] alphabet = new char[52];
            int[] typeIndexes = { 0, 1, 2, 3, 4, 5 };

            List<string> words = new List<string>();
            List<int> TypesInts = new List<int>();

            for (int i = 0; i < 26; i++)
            {
                alphabet[i] = (char)('a' + i);
                alphabet[i + 26] = (char)('A' + i);
            }
            //foreach (char symbol in input)
            //{
            //    if (typeIndexes.Contains(Convert.ToInt32(symbol) - 48))
            //    {
            //        words.Add(symbol.ToString());
            //        TypesInts.Add(Convert.ToInt32(symbol) - 48);
            //    }
            //}
            //foreach (int index in TypesInts)
            //{
            //    columns.Add(new Column("current name", index));
            //    Console.WriteLine("current name, " + index);
            //}
            return new List<string>();
        }
    }
}
