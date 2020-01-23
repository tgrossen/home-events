using System;
using System.Data;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HomeEvents.Postgres
{
    class JsonObjectTypeHandler<T> : SqlMapper.TypeHandler<T>
    {
        public override void SetValue(IDbDataParameter parameter, T value)
        {
            parameter.Value = JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                ContractResolver = new CamelCaseExceptDictionaryKeysResolver()
            });
            parameter.DbType = DbType.Object;
        }

        public override T Parse(object value)
        {
            var json = (string) value;
            return JsonConvert.DeserializeObject<T>(json);
        }
    }

    class CamelCaseExceptDictionaryKeysResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonDictionaryContract CreateDictionaryContract(Type objectType)
        {
            JsonDictionaryContract contract = base.CreateDictionaryContract(objectType);

            contract.DictionaryKeyResolver = propertyName => propertyName;

            return contract;
        }
    }
}