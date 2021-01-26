using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TvShows.Scraper.Services.Caching.Interfaces
{
	public interface ICacheService<T> where T : class
	{
		Task<T> AddOrUpdate(string cacheKey, string hashKey, T value, TimeSpan expiry);
		Task<T> Get(string cacheKey, string hashKey);
	}
}
