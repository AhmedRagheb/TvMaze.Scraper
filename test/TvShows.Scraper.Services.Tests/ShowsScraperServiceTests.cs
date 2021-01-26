using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TvShows.Scraper.Models;
using TvShows.Scraper.Services.Abstractions;
using Xunit;

namespace TvShows.Scraper.Services.Tests
{
	public class ShowsScraperServiceTests
	{
		private readonly ShowsScraperService _showsScraperService;
		private readonly Mock<ITvMazeScraperServcice> _tvMazeScraperGetterServciceMock;
		private readonly Mock<IShowsStorageService> _showsStorageServiceMock;

		public ShowsScraperServiceTests()
		{
			_tvMazeScraperGetterServciceMock = new Mock<ITvMazeScraperServcice>();
			_showsStorageServiceMock = new Mock<IShowsStorageService>();
			_showsScraperService = new ShowsScraperService(_tvMazeScraperGetterServciceMock.Object, _showsStorageServiceMock.Object);
		}

		[Fact]
		public async Task ScrapeShows_Should_Verify_Call_GetShowsForPage()
		{
			// prepare
			_tvMazeScraperGetterServciceMock.Setup(x => x.GetShowsForPage(1)).ReturnsAsync(new List<ShowModel>());
			_tvMazeScraperGetterServciceMock.Setup(x => x.GetActorsForShow(1)).ReturnsAsync(new ShowActrosModel());

			// act
			await _showsScraperService.ScrapeShows(1);

			// assert
			_tvMazeScraperGetterServciceMock.Verify(x => x.GetShowsForPage(1), Times.Once);
		}

		[Fact]
		public async Task ScrapeShows_Should_Verify_Call_SaveShows()
		{
			// prepare
			var shows = new List<ShowModel>()
			{
				new ShowModel
				{
					Id = 1,
					Name = "Game of thrones",
					Cast = new List<ActorModel>
					{
						new ActorModel
						{
							Id = 1,
							Name = "Mackenzie Lintz",
							BirthDay = new System.DateTime(1996, 11, 22)
						}
					}
				}
			};

			var cast = new ShowActrosModel
			{
				Id = 1,
				Cast = new List<ActorModel>
				{
					new ActorModel
					{
						Id = 1,
						Name = "Mackenzie Lintz",
						BirthDay = new System.DateTime(1996, 11, 22)
					}
				}
			};

			_tvMazeScraperGetterServciceMock.Setup(x => x.GetShowsForPage(1)).ReturnsAsync(shows);
			_tvMazeScraperGetterServciceMock.Setup(x => x.GetActorsForShow(1)).ReturnsAsync(cast);

			// act
			await _showsScraperService.ScrapeShows(1);

			// assert
			_showsStorageServiceMock.Verify(x => x.SaveShows(shows), Times.Once);
		}
	}
}
