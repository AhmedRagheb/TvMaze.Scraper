using System;

namespace TvShows.Scraper.Services.Common
{
	public static class Utilities
	{
		public static int DefaultPaginationSkip(int page, int paginationSize = Constants.PageDefaultSize)
		{
			var skipAmount = Math.Max((page - 1) * paginationSize, 0);

			return skipAmount;
		}
	}
}
