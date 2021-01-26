namespace TvShows.Scraper.Services.Caching.Settings
{
	public static class CacheKeys
	{
		public const string Shows = "Shows";

		public static string GetKey(string cacheKey, string hashKey)
		{
			return !string.IsNullOrEmpty(hashKey) ? $"{cacheKey}-{hashKey}" : cacheKey;
		}
	}
}
