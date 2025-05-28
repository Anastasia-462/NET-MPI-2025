using ComplexTask_LINQ.Test;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplexTask_LINQ.Database
{
    public class PgDatabaseConnection : IPgDatabaseConnection
    {
        private readonly string _connectionString;

        public PgDatabaseConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        public IEnumerable<T> ExecuteQuery<T>(string query)
        {
            using var connection = GetConnection();
            connection.Open();
            return connection.Query<T>(query);
        }
    }
}
