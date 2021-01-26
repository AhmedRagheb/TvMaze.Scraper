using System.Threading.Tasks;

namespace TvShows.Scraper.Services.Abstractions
{
	public interface IShowsScraperService
	{
		Task ScrapeShows(int page);
	}
}
