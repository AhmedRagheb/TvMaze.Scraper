using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TvShows.Scraper.Services.Caching.Interfaces;
using TvShows.Scraper.Services.Caching.Settings;

namespace TvShows.Scraper.Services.Caching
{
	public class MemoryCacheService<T> : ICacheService<T> where T : class
	{
		private readonly IMemoryCache _cacheClient;

		public MemoryCacheService(IMemoryCache cacheClient)
		{
			_cacheClient = cacheClient;
		}

		public async Task<T> AddOrUpdate(string cacheKey, string hashKey, T value, TimeSpan expiry)
		{
			var key = CacheKeys.GetKey(cacheKey, hashKey);

			var cachedValue = await Task.FromResult(_cacheClient.Set(key, value, expiry));

			return cachedValue;
		}

		public async Task<T> Get(string cacheKey, string hashKey)
		{
			var key = CacheKeys.GetKey(cacheKey, hashKey);

			var cachedValue = await Task.FromResult(_cacheClient.Get<T>(key));

			return cachedValue;
		}
	}
}
