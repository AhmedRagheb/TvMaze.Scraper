using System.Collections.Generic;
using System.Threading.Tasks;
using TvShows.Scraper.Models;

namespace TvShows.Scraper.Services.Abstractions
{
	public interface IShowsStorageService
	{
		Task<List<ShowModel>> GetShows(int page);
		Task SaveShows(List<ShowModel> shows);
	}
}
