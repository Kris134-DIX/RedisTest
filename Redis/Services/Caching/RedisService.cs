using MessagePack;
using Redis.Database.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace Redis.Services.Caching
{
    public class RedisService : IRedisService
    {
        private readonly IDatabase _db;

        public RedisService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        private static string GetRedisKey(int subFundId) => $"SubFundUnitPrices:{subFundId}";

        public async Task CacheSubFundPricesAsync(int subFundId, List<SubFundUnitPriceDTO> prices)
        {
            string redisKey = GetRedisKey(subFundId);
            var hashEntries = prices.Select((p, i) => new HashEntry(i.ToString(), JsonSerializer.Serialize(p))).ToArray();

            await _db.HashSetAsync(redisKey, hashEntries);

        }

        public async Task<List<SubFundUnitPriceDTO>> GetSubFundPricesFromCacheAsync(int subFundId)
        {
            string redisKey = GetRedisKey(subFundId);
            HashEntry[] entries = await _db.HashGetAllAsync(redisKey);

            return entries.Select(e => JsonSerializer.Deserialize<SubFundUnitPriceDTO>(e.Value!)).ToList()!;
        }

        public async Task CacheSubFundPricesWithoutTTLAsync(int subFundId, List<SubFundUnitPriceDTO> prices)
        {
            string redisKey = GetRedisKey(subFundId);
            var hashEntries = prices.Select((p, i) => new HashEntry(i.ToString(), JsonSerializer.Serialize(p))).ToArray();

            await _db.HashSetAsync(redisKey, hashEntries);  // Brak TTL
        }

        public async Task CacheSubFundPricesPaginatedAsync(int subFundId, List<SubFundUnitPriceDTO> prices)
        {
            string redisKey = GetRedisKey(subFundId);
            var batch = _db.CreateBatch();

            foreach (var price in prices)
            {
                double score = price.UnitPriceDate?.ToOADate() ?? 0;
                byte[] data = MessagePackSerializer.Serialize(price);  // ✅ Używamy MessagePack
                batch.SortedSetAddAsync(redisKey, data, score);
            }
            batch.Execute();
        }

        public async Task<List<SubFundUnitPriceDTO>> GetAllSubFundPricesAsync(int subFundId, int pageSize = 500)
        {
            string redisKey = GetRedisKey(subFundId);
            long totalCount = await _db.SortedSetLengthAsync(redisKey);

            var allPrices = new List<SubFundUnitPriceDTO>();
            for (long start = 0; start < totalCount; start += pageSize)
            {
                long end = start + pageSize - 1;
                var entries = await _db.SortedSetRangeByRankAsync(redisKey, start, end);

                allPrices.AddRange(entries.Select(e => MessagePackSerializer.Deserialize<SubFundUnitPriceDTO>(e!)));
            }

            return allPrices;
        }

        public async Task Delete(int suyFundId)
        {
            string redisKey = GetRedisKey(suyFundId);
            await _db.KeyDeleteAsync(redisKey);
        }
    }
    public interface IRedisService
    {
        Task CacheSubFundPricesAsync(int subFundId, List<SubFundUnitPriceDTO> prices);
        Task<List<SubFundUnitPriceDTO>> GetSubFundPricesFromCacheAsync(int subFundId);
        Task CacheSubFundPricesWithoutTTLAsync(int subFundId, List<SubFundUnitPriceDTO> prices);
        Task CacheSubFundPricesPaginatedAsync(int subFundId, List<SubFundUnitPriceDTO> prices);
        Task<List<SubFundUnitPriceDTO>> GetAllSubFundPricesAsync(int subFundId, int pageSize = 500);
        Task Delete(int suyFundId);
    }
}
