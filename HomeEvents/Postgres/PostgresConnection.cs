using System.Data;
using System.Threading.Tasks;
using Dapper;
using HomeEvents.Settings;
using Npgsql;

namespace HomeEvents.Postgres {
    public interface IPostgresConnection {
        Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null,  int? commandTimeout = null, CommandType? commandType = null);
        Task<T> QuerySingleAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
    }
    
    public class PostgresConnection : IPostgresConnection {
        readonly IHomeEventsSettings homeEventsSettings;
        IDbConnection dbConnection;

        public PostgresConnection(IHomeEventsSettings homeEventsSettings) {
            this.homeEventsSettings = homeEventsSettings;
            dbConnection = null;
        }

        string ConnectionStringBuilder()
        {
            return $"Host={homeEventsSettings.PostgresHost};Port={homeEventsSettings.PostgresPort};User ID={homeEventsSettings.PostgresUsername};Password={homeEventsSettings.PostgresPassword};Database={homeEventsSettings.PostgresDatabase};Pooling=true;";
        }

        IDbConnection Connection {
            get {
                if (dbConnection == null)
                {
                    DefaultTypeMap.MatchNamesWithUnderscores = true;
                    var connectionString = ConnectionStringBuilder();
                    dbConnection = new NpgsqlConnection(connectionString);
                }
                
                return dbConnection;
            }
        }

        public async Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            Connection.Open();
            var results = await dbConnection.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
            Connection.Close();
            return results;
        }

        public async Task<T> QuerySingleAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            Connection.Open();
            var results = await dbConnection.QuerySingleAsync<T>(sql, param, transaction, commandTimeout, commandType);
            Connection.Close();
            return results;
        }
    }
}