using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TvShows.Scraper.DataAccess;
using TvShows.Scraper.DataAccess.Entities;
using TvShows.Scraper.Models;
using TvShows.Scraper.Services.Abstractions;
using TvShows.Scraper.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace TvShows.Scraper.Services
{
	public class ActorsStorageService : IActorsStorageService
	{
		private readonly ShowsDbContext _showsDbContext;

		public ActorsStorageService(ShowsDbContext showsDbContext)
		{
			_showsDbContext = showsDbContext;
		}

		public async Task SaveActors(List<ActorModel> actors)
		{
			var rowCount = 0;
			foreach (var actor in actors)
			{
				var isExist = await _showsDbContext.Actors.AnyAsync(c => c.PersonId == actor.Id);
				if (!isExist)
				{
					var newActor = new Actor
					{
						PersonId = actor.Id,
						Name = actor.Name,
						BirthDay = actor.BirthDay
					};

					await _showsDbContext.Actors.AddAsync(newActor);

					rowCount++;
					if (rowCount % Constants.BatchSize == 0)
					{
						await _showsDbContext.SaveChangesAsync();
					}
				}
			}

			// Save any remaining rows
			await _showsDbContext.SaveChangesAsync();
		}

		public async Task<int> GetActorByPersonId(int personId)
		{
			return await _showsDbContext.Actors.Where(c => personId == c.PersonId).Select(c => c.Id).FirstAsync();
		}
	}
}
