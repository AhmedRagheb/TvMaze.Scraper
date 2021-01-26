using System.Collections.Generic;
using System.Threading.Tasks;
using TvShows.Scraper.Models;

namespace TvShows.Scraper.Services.Abstractions
{
	public interface IShowsService
	{
		Task<IReadOnlyCollection<ShowModel>> GetShows(int page);
	}
}
