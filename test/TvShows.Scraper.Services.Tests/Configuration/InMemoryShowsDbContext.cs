using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TvShows.Scraper.DataAccess;

namespace TvShows.Scraper.Services.Tests
{
	public abstract class InMemoryShowsDbContext
	{
		protected ShowsDbContext DbContext { get; set; }

		protected InMemoryShowsDbContext()
		{
			var options = new DbContextOptionsBuilder<ShowsDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.EnableSensitiveDataLogging()
				.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
				.ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
				.Options;

			DbContext = new ShowsDbContext(options);
		}
	}
}
