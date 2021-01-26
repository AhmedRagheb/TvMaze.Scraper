using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TvShows.Scraper.DataAccess;
using TvShows.Scraper.DataAccess.Entities;
using TvShows.Scraper.Models;
using TvShows.Scraper.Services.Abstractions;
using TvShows.Scraper.Services.Common;

namespace TvShows.Scraper.Services
{
	public class ShowsStorageService : IShowsStorageService
	{
		private readonly ShowsDbContext _showsDbContext;
		private readonly IActorsStorageService _actorsStorageService;

		public ShowsStorageService(ShowsDbContext showsDbContext, IActorsStorageService actorsStorageService)
		{
			_showsDbContext = showsDbContext;
			_actorsStorageService = actorsStorageService;
		}

		public async Task<List<ShowModel>> GetShows(int page)
		{
			var shows = await _showsDbContext.Shows
				.Include(showCast => showCast.ShowActors)
				.ThenInclude(actor => actor.Actor)
				.OrderBy(showCast => showCast.Id)
				.Select(show => new ShowModel
				{
					Id = show.TvShowId,
					Name = show.Name,
					Cast = show.ShowActors
						.OrderByDescending(showCast => showCast.Actor.BirthDay)
						.Select(showCast => new ActorModel
						{
							Id = showCast.Actor.PersonId,
							Name = showCast.Actor.Name,
							BirthDay = showCast.Actor.BirthDay
						})
				})
				.Skip(Utilities.DefaultPaginationSkip(page))
				.Take(Constants.PageDefaultSize)
				.ToListAsync();

			return shows;
		}

		public async Task SaveShows(List<ShowModel> shows)
		{
			await SaveShowActors(shows);

			var rowCount = 0;
			var transaction = await _showsDbContext.BeginTransactionAsync();

			try
			{
				foreach (var showModel in shows)
				{
					var show = new Show
					{
						TvShowId = showModel.Id,
						Name = showModel.Name
					};

					await CreateShowCast(showModel, show);

					await _showsDbContext.Shows.AddAsync(show);

					rowCount++;
					if (rowCount % Constants.BatchSize == 0)
					{
						await _showsDbContext.SaveChangesAsync();
					}
				}

				// Save any remaining rows
				await _showsDbContext.SaveChangesAsync();
				await transaction.CommitAsync();
			}
			catch (Exception e)
			{
				await transaction.RollbackAsync();
				throw e;
			}
		}

		private async Task CreateShowCast(ShowModel showModel, Show show)
		{
			if (showModel.Cast != null && showModel.Cast.Count() > 0)
			{
				var showActors = new List<ShowActor>();
				foreach (var c in showModel.Cast)
				{
					var actorId = await _actorsStorageService.GetActorByPersonId(c.Id);
					var showActor = new ShowActor
					{
						ActorId = actorId
					};

					showActors.Add(showActor);
				}

				show.ShowActors = showActors;
			}
		}

		private async Task SaveShowActors(IEnumerable<ShowModel> shows)
		{
			var showsWithCast = shows.Where(c => c.Cast != null && c.Cast.Count() > 0);
			var actorsModel = showsWithCast.SelectMany(c => c.Cast).ToList();

			await _actorsStorageService.SaveActors(actorsModel);
		}
	}
}
