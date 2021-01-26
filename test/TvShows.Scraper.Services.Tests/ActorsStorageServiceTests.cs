using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Microsoft.EntityFrameworkCore;
using TvShows.Scraper.Models;
using TvShows.Scraper.DataAccess.Entities;

namespace TvShows.Scraper.Services.Tests
{
	public class ActorsStorageServiceTests : InMemoryShowsDbContext
	{
		private readonly ActorsStorageService _actorsStorageService;

		public ActorsStorageServiceTests()
		{
			_actorsStorageService = new ActorsStorageService(DbContext);
		}

		[Fact]
		public async Task Save_Should_SaveActorsInTheDb()
		{
			// prepare
			var actors = new List<ActorModel>
			{
				new ActorModel
				{
					Id = 1,
					Name = "Mackenzie Lintz",
					BirthDay = new System.DateTime(1996, 11, 22)
				},
				new ActorModel
				{
					Id = 2,
					Name = "Dean Norris",
					BirthDay = new System.DateTime(1963, 04, 08)
				}
			};

			// act
			await _actorsStorageService.SaveActors(actors);

			// assert
			var expected = new List<Actor>
			{
				new Actor
				{
					PersonId = 1,
					Name = "Mackenzie Lintz",
					BirthDay = new System.DateTime(1996, 11, 22)
				},
				new Actor
				{
					PersonId = 2,
					Name = "Dean Norris",
					BirthDay = new System.DateTime(1963, 04, 08)
				}
			};
			var actual = await DbContext.Actors.ToListAsync();

			actual.Should().BeEquivalentTo(expected, options => options.Excluding(p => p.Id));
		}

		[Fact]
		public async Task GetActorByPersonId_Should_ReturnActorId()
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
				},
				new Actor
				{
					Id = 2,
					PersonId = 2,
					Name = "Dean Norris",
					BirthDay = new System.DateTime(1963, 04, 08)
				}
			};

			await DbContext.Actors.AddRangeAsync(actors);
			await DbContext.SaveChangesAsync();

			// act
			var actual = await _actorsStorageService.GetActorByPersonId(1);

			// assert
			actual.Should().Be(1);
		}
	}
}
