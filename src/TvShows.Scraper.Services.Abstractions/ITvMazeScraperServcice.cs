using System.Collections.Generic;
using System.Threading.Tasks;
using TvShows.Scraper.Models;

namespace TvShows.Scraper.Services.Abstractions
{
	public interface ITvMazeScraperServcice
	{
		Task<List<ShowModel>> GetShowsForPage(int page);
		Task<ShowActrosModel> GetActorsForShow(int showId);
	}
}
