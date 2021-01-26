using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TvShows.Scraper.Models;
using TvShows.Scraper.Services.Abstractions;

namespace TvShows.Scraper.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ShowsController : ControllerBase
	{
		private readonly IShowsStorageService _showsGetterService;

		public ShowsController(IShowsStorageService showsGetterService)
		{
			_showsGetterService = showsGetterService;
		}

		[HttpGet]
		[ProducesResponseType(typeof(IReadOnlyCollection<ShowModel>), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> Get([FromQuery]int page)
		{
			var shows = await _showsGetterService.GetShows(page);

			return Ok(shows);
		}
	}
}
