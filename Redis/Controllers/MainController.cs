using Microsoft.AspNetCore.Mvc;
using Redis.Database.Models;
using Redis.Services;
using Redis.Services.Caching;
using System.Diagnostics;

namespace Redis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        private readonly ISubFundUnitPriceService _subFundUnitPriceService;
        private readonly IRedisCacheService _cache;
        private readonly IRedisCache _redisCacheNew;
        private readonly IRedisService _redisService;
        public MainController(ISubFundUnitPriceService subFundUnitPriceService, IRedisCacheService cache, IRedisCache redisCacheNew, IRedisService redisService)
        {
            _subFundUnitPriceService = subFundUnitPriceService;
            _cache = cache;
            _redisCacheNew = redisCacheNew;
            _redisService = redisService;
        }

        [HttpPost("InsertAll")]
        public async Task Get()
        {

            var subfundsIds = await _subFundUnitPriceService.GetSubFundIds();
            foreach (var id in subfundsIds)
            {
                var funds = await _subFundUnitPriceService.GetBySubFundId(id);
                //Thread.Sleep(60);
                await _cache.SetDataAsync($"SubFundUnitPrices:SubfundId:{id}", funds);

            }
        }

        [HttpPost("InsertAllPaginated")]
        public async Task InsertPaginate()
        {
            var subfundsIds = await _subFundUnitPriceService.GetSubFundIds();
            foreach (var id in subfundsIds)
            {
                var funds = await _subFundUnitPriceService.GetBySubFundId(id);
                //Thread.Sleep(60);
                await _redisService.CacheSubFundPricesPaginatedAsync(id, funds.ToList());
            }
        }


        [HttpPost("InsertAllHashes")]
        public async Task PostHashes()
        {

            var subfundsIds = await _subFundUnitPriceService.GetSubFundIds();
            foreach (var id in subfundsIds)
            {
                var funds = await _subFundUnitPriceService.GetBySubFundId(id);
                await _redisService.CacheSubFundPricesWithoutTTLAsync(id, funds.ToList());
            }
        }

        [HttpDelete("DeleteHashes")]
        public async Task Delete()
        {

            var subfundsIds = await _subFundUnitPriceService.GetSubFundIds();
            foreach (var id in subfundsIds)
            {
                await _redisService.Delete(id);
            }
        }


        [HttpGet("GetHashes")]
        public async Task<string> GetHashes(int id)
        {
            long stopwatchCache;
            long stopwatchCacheHash;
            long stopwatchCachePaginated;
            long stopwatchCacheCity;
            long stopwatchDB;
            long stopwatchCacheString;

            stopwatchCachePaginated = Stopwatch.GetTimestamp();
            var funds3 = await _redisService.GetAllSubFundPricesAsync(id);
            await _redisService.GetAllSubFundPricesAsync(id + 1);
            await _redisService.GetAllSubFundPricesAsync(id + 2);
            await _redisService.GetAllSubFundPricesAsync(id + 3);
            await _redisService.GetAllSubFundPricesAsync(id + 4);
            await _redisService.GetAllSubFundPricesAsync(id + 5);
            await _redisService.GetAllSubFundPricesAsync(id + 6);
            await _redisService.GetAllSubFundPricesAsync(id + 7);
            await _redisService.GetAllSubFundPricesAsync(id + 8);
            await _redisService.GetAllSubFundPricesAsync(id + 9);

            TimeSpan elapsedCachePaginated = Stopwatch.GetElapsedTime(stopwatchCachePaginated);

            stopwatchCacheHash = Stopwatch.GetTimestamp();
            //var funds = await _redisService.GetSubFundPricesFromCacheAsync(id);
            //await _redisService.GetSubFundPricesFromCacheAsync(id + 1);
            //await _redisService.GetSubFundPricesFromCacheAsync(id + 2);
            //await _redisService.GetSubFundPricesFromCacheAsync(id + 3);
            //await _redisService.GetSubFundPricesFromCacheAsync(id + 4);
            //await _redisService.GetSubFundPricesFromCacheAsync(id + 5);
            //await _redisService.GetSubFundPricesFromCacheAsync(id + 6);
            //await _redisService.GetSubFundPricesFromCacheAsync(id + 7);
            //await _redisService.GetSubFundPricesFromCacheAsync(id + 8);
            //await _redisService.GetSubFundPricesFromCacheAsync(id + 9);

            TimeSpan elapsedCacheHash = Stopwatch.GetElapsedTime(stopwatchCacheHash);


            stopwatchCache = Stopwatch.GetTimestamp();
            var funds2 = await _redisCacheNew.GetCacheValueAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id}");
            await _redisCacheNew.GetCacheValueAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 1}");
            await _redisCacheNew.GetCacheValueAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 2}");
            await _redisCacheNew.GetCacheValueAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 3}");
            await _redisCacheNew.GetCacheValueAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 4}");
            await _redisCacheNew.GetCacheValueAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 5}");
            await _redisCacheNew.GetCacheValueAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 6}");
            await _redisCacheNew.GetCacheValueAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 7}");
            await _redisCacheNew.GetCacheValueAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 8}");
            await _redisCacheNew.GetCacheValueAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 9}");

            TimeSpan elapsedCache = Stopwatch.GetElapsedTime(stopwatchCache);

            stopwatchDB = Stopwatch.GetTimestamp();
            var funds4 = await _subFundUnitPriceService.GetBySubFundId(id);
            await _subFundUnitPriceService.GetBySubFundId(id + 1);
            await _subFundUnitPriceService.GetBySubFundId(id + 2);
            await _subFundUnitPriceService.GetBySubFundId(id + 3);
            await _subFundUnitPriceService.GetBySubFundId(id + 4);
            await _subFundUnitPriceService.GetBySubFundId(id + 5);
            await _subFundUnitPriceService.GetBySubFundId(id + 6);
            await _subFundUnitPriceService.GetBySubFundId(id + 7);
            await _subFundUnitPriceService.GetBySubFundId(id + 8);
            await _subFundUnitPriceService.GetBySubFundId(id + 9);

            TimeSpan elapsedDB = Stopwatch.GetElapsedTime(stopwatchDB);

            return $"Czas cache: {elapsedCache},  czas cacheHash: {elapsedCacheHash}, czas cachePaginated: {elapsedCachePaginated}, czas db: {elapsedDB}";
        }


        [HttpPost("InsertString")]
        public async Task PutString(string key, string value)
        {
            await _cache.SetDataAsync($"{key}", value);
        }

        [HttpGet("GetStringByKey")]
        public async Task<string> GetString(string key)
        {
            return await _cache.GetDataObjectAsync<string>($"{key}");
        }

        [HttpGet("GetSubFundsById")]
        public async Task<string> GetBySubfund(int id)
        {
            long stopwatchCache;
            long stopwatchCacheCity;
            long stopwatchDB;
            long stopwatchCacheString;

            stopwatchCache = Stopwatch.GetTimestamp();
            var funds = await _cache.GetDataClassAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id}");
            funds = await _cache.GetDataClassAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 1}");
            funds = await _cache.GetDataClassAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 2}");
            funds = await _cache.GetDataClassAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 3}");
            funds = await _cache.GetDataClassAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 4}");
            funds = await _cache.GetDataClassAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 5}");
            funds = await _cache.GetDataClassAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 6}");
            funds = await _cache.GetDataClassAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 7}");
            funds = await _cache.GetDataClassAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 8}");
            funds = await _cache.GetDataClassAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 9}");

            TimeSpan elapsedCache = Stopwatch.GetElapsedTime(stopwatchCache);


            stopwatchCacheString = Stopwatch.GetTimestamp();
            var funds3 = await _cache.GetDataObjectAsync<string>($"SubFundUnitPrices:SubfundId:{id}");
            funds3 = await _cache.GetDataObjectAsync<string>($"SubFundUnitPrices:SubfundId:{id + 1}");
            funds3 = await _cache.GetDataObjectAsync<string>($"SubFundUnitPrices:SubfundId:{id + 2}");
            funds3 = await _cache.GetDataObjectAsync<string>($"SubFundUnitPrices:SubfundId:{id + 3}");
            funds3 = await _cache.GetDataObjectAsync<string>($"SubFundUnitPrices:SubfundId:{id + 4}");
            funds3 = await _cache.GetDataObjectAsync<string>($"SubFundUnitPrices:SubfundId:{id + 5}");
            funds3 = await _cache.GetDataObjectAsync<string>($"SubFundUnitPrices:SubfundId:{id + 6}");
            funds3 = await _cache.GetDataObjectAsync<string>($"SubFundUnitPrices:SubfundId:{id + 7}");
            funds3 = await _cache.GetDataObjectAsync<string>($"SubFundUnitPrices:SubfundId:{id + 8}");
            funds3 = await _cache.GetDataObjectAsync<string>($"SubFundUnitPrices:SubfundId:{id + 9}");

            TimeSpan elapsedCacheString = Stopwatch.GetElapsedTime(stopwatchCacheString);

            stopwatchCacheCity = Stopwatch.GetTimestamp();
            var tete = await _cache.GetDataObjectAsync<string>($"city");
            tete = await _cache.GetDataObjectAsync<string>($"city");
            tete = await _cache.GetDataObjectAsync<string>($"city");
            tete = await _cache.GetDataObjectAsync<string>($"city");
            tete = await _cache.GetDataObjectAsync<string>($"city");
            tete = await _cache.GetDataObjectAsync<string>($"city");
            tete = await _cache.GetDataObjectAsync<string>($"city");
            tete = await _cache.GetDataObjectAsync<string>($"city");
            tete = await _cache.GetDataObjectAsync<string>($"city");
            tete = await _cache.GetDataObjectAsync<string>($"city");

            TimeSpan elapsedCacheCity = Stopwatch.GetElapsedTime(stopwatchCacheCity);


            stopwatchDB = Stopwatch.GetTimestamp();
            var funds2 = await _subFundUnitPriceService.GetBySubFundId(id);
            funds2 = await _subFundUnitPriceService.GetBySubFundId(id + 1);
            funds2 = await _subFundUnitPriceService.GetBySubFundId(id + 2);
            funds2 = await _subFundUnitPriceService.GetBySubFundId(id + 3);
            funds2 = await _subFundUnitPriceService.GetBySubFundId(id + 4);
            funds2 = await _subFundUnitPriceService.GetBySubFundId(id + 5);
            funds2 = await _subFundUnitPriceService.GetBySubFundId(id + 6);
            funds2 = await _subFundUnitPriceService.GetBySubFundId(id + 7);
            funds2 = await _subFundUnitPriceService.GetBySubFundId(id + 8);
            funds2 = await _subFundUnitPriceService.GetBySubFundId(id + 9);

            TimeSpan elapsedDB = Stopwatch.GetElapsedTime(stopwatchDB);

            return $"Czas cache: {elapsedCache}, czas cacheString: {elapsedCacheString}, czas db: {elapsedDB}, czas cacheCity: {elapsedCacheCity}";
        }
        //DObrymi przykładami dla klucza mogłoby być np. SubFund:{id}:SubfundUnitPrices pokazało
        [HttpGet("GetSubFundsByIdNew")]
        public async Task<string> GetBySubfundNew(int id)
        {
            long stopwatchCache;
            long stopwatchCacheCity;
            long stopwatchDB;
            long stopwatchCacheString;

            stopwatchCache = Stopwatch.GetTimestamp();
            var funds = await _redisCacheNew.GetCacheValueAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id}");
            funds = await _redisCacheNew.GetCacheValueAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 1}");
            funds = await _redisCacheNew.GetCacheValueAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 2}");
            funds = await _redisCacheNew.GetCacheValueAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 3}");
            funds = await _redisCacheNew.GetCacheValueAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 4}");
            funds = await _redisCacheNew.GetCacheValueAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 5}");
            funds = await _redisCacheNew.GetCacheValueAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 6}");
            funds = await _redisCacheNew.GetCacheValueAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 7}");
            funds = await _redisCacheNew.GetCacheValueAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 8}");
            funds = await _redisCacheNew.GetCacheValueAsync<IEnumerable<SubFundUnitPriceDTO>>($"SubFundUnitPrices:SubfundId:{id + 9}");

            TimeSpan elapsedCache = Stopwatch.GetElapsedTime(stopwatchCache);

            stopwatchDB = Stopwatch.GetTimestamp();
            var funds2 = await _subFundUnitPriceService.GetBySubFundId(id);
            funds2 = await _subFundUnitPriceService.GetBySubFundId(id + 1);
            funds2 = await _subFundUnitPriceService.GetBySubFundId(id + 2);
            funds2 = await _subFundUnitPriceService.GetBySubFundId(id + 3);
            funds2 = await _subFundUnitPriceService.GetBySubFundId(id + 4);
            funds2 = await _subFundUnitPriceService.GetBySubFundId(id + 5);
            funds2 = await _subFundUnitPriceService.GetBySubFundId(id + 6);
            funds2 = await _subFundUnitPriceService.GetBySubFundId(id + 7);
            funds2 = await _subFundUnitPriceService.GetBySubFundId(id + 8);
            funds2 = await _subFundUnitPriceService.GetBySubFundId(id + 9);

            TimeSpan elapsedDB = Stopwatch.GetElapsedTime(stopwatchDB);

            return $"Czas cache: {elapsedCache},  czas db: {elapsedDB}";
        }
    }
}
