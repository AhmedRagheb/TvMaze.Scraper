using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TvShows.Scraper.Models;
using TvShows.Scraper.Services.Abstractions;

namespace TvShows.Scraper.Services
{
	public class ShowsScraperService : IShowsScraperService
	{
		private readonly ITvMazeScraperServcice _tvMazeScraperGetterServcice;
		private readonly IShowsStorageService _showsStorageService;

		public ShowsScraperService(ITvMazeScraperServcice tvMazeScraperGetterServcice, IShowsStorageService showsStorageService)
		{
			_tvMazeScraperGetterServcice = tvMazeScraperGetterServcice;
			_showsStorageService = showsStorageService;
		}

		public async Task ScrapeShows(int page)
		{
			var shows = await GetShows(page);

			await _showsStorageService.SaveShows(shows);
		}

		private async Task<List<ShowModel>> GetShows(int page)
		{
			var shows = await _tvMazeScraperGetterServcice.GetShowsForPage(page);
			var showsIds = shows.Select(show => show.Id).ToList();

			var castTasks = showsIds.Select(_tvMazeScraperGetterServcice.GetActorsForShow).ToArray().AsParallel();
			var showsDetails = await Task.WhenAll(castTasks);

			MapShowsCast(shows, showsDetails);

			return shows;
		}

		private void MapShowsCast(List<ShowModel> shows, ShowActrosModel[] showsDetails)
		{
			foreach (var show in shows)
			{
				var showDetails = showsDetails.Where(details => details.Id == show.Id).FirstOrDefault();
				if (showDetails != null)
				{
					show.Cast = showDetails.Cast
						.OrderByDescending(cast => cast.BirthDay)
						.GroupBy(cast => cast.Id)
						.Select(cast => cast.First())
						.ToList();
				}
			}
		}
	}
}
