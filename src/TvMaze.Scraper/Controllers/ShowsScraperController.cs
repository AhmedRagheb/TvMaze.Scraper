using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TvShows.Scraper.Services.Abstractions;

namespace TvShows.Scraper.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ShowsScraperController : ControllerBase
	{
		private readonly IShowsScraperService _showsScraperService;

		public ShowsScraperController(IShowsScraperService showsScraperService)
		{
			_showsScraperService = showsScraperService;
		}

		[HttpPut]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		public async Task<IActionResult> Put()
		{
			await _showsScraperService.ScrapeShows(1);

			return NoContent();
		}
	}
}
