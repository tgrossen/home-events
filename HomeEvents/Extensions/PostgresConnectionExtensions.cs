// using System.Collections.Generic;
// using System.Data;
// using System.Linq;
// using Dapper;
// using HomeEvents.Infrastructure;
// using HomeEvents.Postgres;
//
// namespace HomeEvents.Extensions
// {
//     public static class PostgresConnectionExtensions
//     {
//
//         public static void Insert<T>(this IPostgresConnection connection, string tableName, T data, IDbTransaction transaction = null) where T : class
//         {
//             var propertyNames = GetPropertyInfo<T>();
//             var columnNames = GetColumnNames(propertyNames.Select(x => x.Name));
//             var bindingNames = GetBindingNames(propertyNames);
//             var statement = $"INSERT INTO {tableName} ({columnNames}) VALUES ({bindingNames});";
//
//             connection.WriteData(statement, data);
//         }
//
//         public static void Upsert<T>(this IPostgresConnection connection, string tableName, string primaryKey, T data, IDbTransaction transaction = null) where T : class
//         {
//             UpsertMultiple(connection, tableName, primaryKey, new List<T> { data }, transaction);
//         }
//
//         public static void Upsert<T>(this IPostgresConnection connection, string tableName, string[] primaryKeys, T data, IDbTransaction transaction = null) where T : class
//         {
//             UpsertMultiple(connection, tableName, primaryKeys, new List<T> { data }, transaction);
//         }
//
//         public static void InsertMultiple<T>(this IPostgresConnection connection, string tableName, IEnumerable<T> data, IDbTransaction transaction = null) where T : class
//         {
//             var propertyNames = GetPropertyInfo<T>();
//             var columnNames = GetColumnNames(propertyNames.Select(x => x.Name));
//             var bindingNames = GetBindingNames(propertyNames);
//             var statement = $"INSERT INTO {tableName} ({columnNames}) VALUES ({bindingNames});";
//
//             connection.WriteData(statement, data, transaction);
//         }
//
//         public static void UpsertMultiple<T>(this IPostgresConnection connection, string tableName, string primaryKey, IEnumerable<T> data, IDbTransaction transaction = null) where T : class
//         {
//             var propertyNames = GetPropertyInfo<T>();
//             var columnNames = GetColumnNames(propertyNames.Select(x => x.Name));
//             var bindingNames = GetBindingNames(propertyNames);
//             var statement = $"INSERT INTO {tableName} ({columnNames}) VALUES ({bindingNames}) ON CONFLICT ({primaryKey}) DO UPDATE SET ({columnNames}) = ({GetColumnNames(propertyNames.Select(x => x.Name), true)});";
//
//             connection.WriteData(statement, data, transaction);
//         }
//
//         public static void UpsertMultiple<T>(this IPostgresConnection connection, string tableName, string[] primaryKeys, IEnumerable<T> data, IDbTransaction transaction = null) where T : class
//         {
//             var propertyNames = GetPropertyInfo<T>();
//             var columnNames = GetColumnNames(propertyNames.Select(x => x.Name));
//             var bindingNames = GetBindingNames(propertyNames);
//             var primaryKeyString = string.Join(", ", primaryKeys);
//             var statement = $"INSERT INTO {tableName} ({columnNames}) VALUES ({bindingNames}) ON CONFLICT ({primaryKeyString}) DO UPDATE SET ({columnNames}) = ({GetColumnNames(propertyNames.Select(x => x.Name), true)});";
//
//             connection.WriteData(statement, data, transaction);
//         }
//
//         public static T QuerySingle<T>(this IPostgresConnection connection, string tableName, IDictionary<string, object> queryParameterMap) where T : class
//         {
//             var queryMap = BuildQueryParameters(queryParameterMap);
//             var statement = $"SELECT * FROM {tableName} WHERE {queryMap} LIMIT 1";
//             return connection.ReadData<T>(statement, new DynamicParameters(queryParameterMap)).FirstOrDefault();
//         }
//
//         public static IList<T> QueryAll<T>(this IPostgresConnection connection, string tableName) where T : class
//         {
//             var statement = $"SELECT * FROM {tableName}";
//             return connection.ReadData<T>(statement).ToList();
//         }
//
//         public static IList<T> QueryAll<T>(this IPostgresConnection connection, string tableName, IDictionary<string, object> queryParameterMap) where T : class
//         {
//             var queryMap = BuildQueryParameters(queryParameterMap);
//             var statement = $"SELECT * FROM {tableName} WHERE {queryMap}";
//             return connection.ReadData<T>(statement, new DynamicParameters(queryParameterMap)).ToList();
//         }
//
//         public static void Delete(this IPostgresConnection connection, string tableName, IDictionary<string, object> queryParameterMap, IDbTransaction transaction = null)
//         {
//             var queryMap = BuildQueryParameters(queryParameterMap);
//             var statement = $"DELETE FROM {tableName} WHERE {queryMap}";
//             connection.WriteData(statement, new DynamicParameters(queryParameterMap), transaction);
//         }
//
//         public static void TruncateTable(this IPostgresConnection connection, string tableName, IDbTransaction transaction = null)
//         {
//             var statement = $"TRUNCATE {tableName} CASCADE;";
//             connection.WriteData(statement, transaction: transaction);
//         }
//
//         static IEnumerable<PostgresPropertyInfo> GetPropertyInfo<T>() where T : class
//         {
//             return typeof(T).GetProperties().Where(x => !x.GetCustomAttributes<PostgresIgnoreAttribute>().Any()).Select(x =>
//             {
//                 var customAttributes = x.CustomAttributes;
//                 var isJson = customAttributes.Any(a => a.AttributeType == typeof(PostgresJsonAttribute));
//                 return new PostgresPropertyInfo
//                 {
//                     Name = x.Name,
//                     Type = isJson ? "json" : ""
//                 };
//             });
//         }
//
//         static string GetBindingNames(IEnumerable<PostgresPropertyInfo> propertyNames)
//         {
//             return string.Join(", ", propertyNames.Select(x =>
//             {
//                 if (x.Type == "json")
//                 {
//                     return $"CAST(@{x.Name} AS JSON)";
//                 }
//
//                 return "@" + x.Name;
//             }));
//         }
//
//         static string GetColumnNames(IEnumerable<string> propertyNames, bool excludes = false)
//         {
//             return string.Join(", ", propertyNames.Select(x => excludes ? "EXCLUDED." + x.ToSnakeCase() : x.ToSnakeCase()));
//         }
//
//         static string BuildQueryParameters(IDictionary<string, object> queryParameterMap)
//         {
//             return string.Join(" AND ", queryParameterMap.Select(x => x.Key + " = @" + x.Key));
//         }
//     }
//
//     class PostgresPropertyInfo
//     {
//         public string Name { get; set; }
//         public string Type { get; set; }
//     }
// }