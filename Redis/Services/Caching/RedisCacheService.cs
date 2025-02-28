using Newtonsoft.Json;
using StackExchange.Redis;

namespace Redis.Services.Caching
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase? _db;

        public RedisCacheService(IConnectionMultiplexer? cache)
        {
            _db = cache.GetDatabase();
        }

        public async Task<T?> GetDataClassAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);

            if (!value.HasValue)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(value);
        }


        public async Task<T?> GetDataObjectAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);

            if (!value.HasValue)
            {
                return default;
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)value.ToString();
            }
            return default;
        }

        public async Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = await _db.StringSetAsync(key, JsonConvert.SerializeObject(value), expiryTime);
            return isSet;
        }
        public async Task<bool> SetDataAsync<T>(string key, T value)
        {
            var isSet = await _db.StringSetAsync(key, JsonConvert.SerializeObject(value));
            return isSet;
        }






    }
}
