
using GrpcGreeter.DbProviders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace GrpcGreeter.Implementations
{
    public class TSQLprovider 
    {
        private string _server;
        private string _db;
        private string _table;

        private string _tableNameForDbTable;

        public TSQLprovider(string server, string db, string table)
        {
            _server = server;
            _db = db;
            _table = table;

            _tableNameForDbTable = $"{_db}_root";
        }

        public TSQLprovider(string server, string db)
            : this(server, db, string.Empty) { }

        private string _connectionString =>
            $"Server={_server};Database={_db};Trusted_Connection=True;Integrated Security=SSPI;MultipleActiveResultSets=true";

        public void UpdateOrCreateDb(string stringDbData)
        {
            CreateDbIfNotExist();

            var deleteDbTable = SqlQueries.TruncateTable.WithParameters(_tableNameForDbTable);
            NonQuery(deleteDbTable);

            var stringData = GetInsertValues(new List<string> { stringDbData });
            var insertCommand = SqlQueries.Insert.WithParameters(_tableNameForDbTable, stringData);
            NonQuery(insertCommand);
        }

        private void CreateDbIfNotExist()
        {
            string connectToMasterDb = $"Server={_server};Integrated security=SSPI;database=master";

            using (SqlConnection connection = new SqlConnection(connectToMasterDb))
            {
                connection.Open();
                string expression = SqlQueries.CreateDbIfNotExist.WithParameters(_db);
                SqlCommand command = new SqlCommand(expression, connection);
                command.ExecuteNonQuery();
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string expression = SqlQueries.CreateTableIfNotExist.WithParameters(_tableNameForDbTable);
                SqlCommand command = new SqlCommand(expression, connection);
                command.ExecuteNonQuery();
            }
        }

        public void CreateTable()
        {
            var expression = SqlQueries.CreateTableIfNotExist.WithParameters(_table);

            NonQuery(expression);
        }

        public void DeleteTable()
        {
            var expression = SqlQueries.DeleteTableIfExist.WithParameters(_table);

            NonQuery(expression);
        }

        public List<string> GetData()
        {
            return Select();
        }

        public async Task<List<string>> GetDataAsync()
        {
            return await Task.Run(Select);
        }

        public void InsertData(List<string> data)
        {
            var insertValues = GetInsertValues(data);
            var expression = SqlQueries.Insert.WithParameters(_table, insertValues);

            NonQuery(expression);
        }

        public async Task InsertDataAsync(List<string> data)
        {
            await Task.Run(() =>
            {
                var insertValues = GetInsertValues(data);
                var expression = SqlQueries.Insert.WithParameters(_table, insertValues);

                NonQuery(expression);
            });
        }
        public void ClearTable()
        {
            var expression = SqlQueries.TruncateTable.WithParameters(_table);

            NonQuery(expression);
        }
        public string GetDb()
        {
            var expression = SqlQueries.SELECT.WithParameters(_tableNameForDbTable);
            var result = string.Empty;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(expression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result = reader.GetString(1);
                    }
                }
            }
            return result;
        }
        public void DeleteDatabase()
        {
            string connectToMasterDb = $"Server={_server};Integrated security=SSPI;database=master";

            using (SqlConnection connection = new SqlConnection(connectToMasterDb))
            {
                connection.Open();
                string expression = SqlQueries.DeleteDbIfExist.WithParameters(_db);
                SqlCommand command = new SqlCommand(expression, connection);
                command.ExecuteNonQuery();
            }
        }
        public List<string> GetDbsNames()
        {
            string connectToMasterDb = $"Server={_server};Integrated security=SSPI;database=master";
            var result = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectToMasterDb))
            {
                connection.Open();
                string expression = SqlQueries.SelectAllDbsNames;
                SqlCommand command = new SqlCommand(expression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetString(0));
                    }
                }
            }
            return result;
        }

        private string GetInsertValues(List<string> data)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var row in data)
            {
                sb.Append($"('{row}'),");
            }
            if (sb.Length > 0)
                sb.Length -= 1;
            return sb.ToString();
        }

        private List<string> Select()
        {
            var expression = SqlQueries.SELECT.WithParameters(_table);
            var result = new List<string>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(expression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetString(1));
                    }
                }
            }
            return result;
        }

        private void NonQuery(string expression)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(expression, connection);

                command.ExecuteNonQuery();
            }
        }
    }
}
