using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;
using TvShows.Scraper.DataAccess.Entities;

namespace TvShows.Scraper.DataAccess
{
	public class ShowsDbContext : DbContext
	{
		public virtual DbSet<Show> Shows { get; set; }
		public virtual DbSet<Actor> Actors { get; set; }
		public virtual DbSet<ShowActor> ShowsActors { get; set; }

		public ShowsDbContext(DbContextOptions<ShowsDbContext> options) : base(options)
		{
		}

		public virtual async Task<IDbContextTransaction> BeginTransactionAsync()
		{
			return await Database.BeginTransactionAsync();
		}

		public virtual async Task<int> SaveChangesAsync()
		{
			try
			{
				var result = await base.SaveChangesAsync();

				return result;
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ShowActor>()
				.HasKey(sa => new { sa.ActorId, sa.ShowId });

			modelBuilder.Entity<ShowActor>()
				.HasOne(show => show.Show)
				.WithMany(sa => sa.ShowActors)
				.HasForeignKey(show => show.ShowId);

			modelBuilder.Entity<ShowActor>()
				.HasOne(actor => actor.Actor)
				.WithMany(sa => sa.ShowActors)
				.HasForeignKey(actor => actor.ActorId);

			modelBuilder.Entity<Actor>().HasIndex(x => x.PersonId);
		}
	}
}
