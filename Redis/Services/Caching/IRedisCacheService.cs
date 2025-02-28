namespace Redis.Services.Caching
{
    public interface IRedisCacheService
    {
        Task<T?> GetDataClassAsync<T>(string key);
        Task<T?> GetDataObjectAsync<T>(string key);
        Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime);
        Task<bool> SetDataAsync<T>(string key, T value);
    }
}
