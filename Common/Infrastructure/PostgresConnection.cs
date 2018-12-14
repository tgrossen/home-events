using System;
using Microsoft.Extensions.Configuration;
using System.Data;
using Npgsql;
using Dapper;

namespace common.Infrastructure {
    public interface IPostgresConnection {
        IDbConnection Connection { get; }
    }
    public class PostgresConnection : IPostgresConnection {
        private readonly IConfiguration _configuration;

        public PostgresConnection(IConfiguration configuration) {
            _configuration = configuration;
        }

        public IDbConnection Connection {
            get {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                string connectionString = _configuration.GetSection("DBInfo").GetSection("ConnectionString").Value;
                return new NpgsqlConnection(connectionString);
            }
        }
    }
}