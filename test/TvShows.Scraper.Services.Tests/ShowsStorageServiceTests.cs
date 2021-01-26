using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Microsoft.EntityFrameworkCore;
using TvShows.Scraper.Models;
using TvShows.Scraper.DataAccess.Entities;
using Moq;
using TvShows.Scraper.Services.Abstractions;

namespace TvShows.Scraper.Services.Tests
{
	public class ShowsStorageServiceTests : InMemoryShowsDbContext
	{
		private readonly ShowsStorageService _showsStorageService;
		private readonly Mock<IActorsStorageService> _actorsStorageServiceMoq;

		public ShowsStorageServiceTests()
		{
			_actorsStorageServiceMoq = new Mock<IActorsStorageService>();
			_showsStorageService = new ShowsStorageService(DbContext, _actorsStorageServiceMoq.Object);
		}

		[Fact]
		public async Task GetShows_Should_ListOfShows()
		{
			// prepare
			var actors = new List<Actor>
			{
				new Actor
				{
					Id = 1,
					PersonId = 1,
					Name = "Mackenzie Lintz",
					BirthDay = new System.DateTime(1996, 11, 22)
				}
			};
			await DbContext.Actors.AddRangeAsync(actors);

			var shows = new List<Show>
			{
				new Show
				{
					Id = 1,
					TvShowId = 1,
					Name = "Kirby Buckets",
					ShowActors = new List<ShowActor>
                    {
						new ShowActor
                        {
							ActorId = 1
                        }
                    }
				}
			};
			await DbContext.Shows.AddRangeAsync(shows);

			await DbContext.SaveChangesAsync();

			// act
			var actual = await _showsStorageService.GetShows(1);

			// assert
			var expected = new List<ShowModel>()
			{
				new ShowModel
				{
					Id = 1,
					Name = "Kirby Buckets",
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

			actual.Should().BeEquivalentTo(expected);
		}
	}
}
