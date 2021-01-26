using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using TvShows.Scraper.DataAccess;

namespace TvShows.Scraper.Api.Settings
{
	public static class DatabaseSettings
	{
		public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
		{
			services.AddDbContext<ShowsDbContext>(options =>
			{
				options.UseSqlServer(connectionString);
#if DEBUG
				options.UseLoggerFactory(new LoggerFactory(new[]
				{
					new DebugLoggerProvider()
				}));
#endif
			});

			services.AddTransient<ShowsDbContext>();

			return services;
		}

		public static IApplicationBuilder UseDatabase(this IApplicationBuilder app)
		{
			using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
			var context = serviceScope.ServiceProvider.GetRequiredService<ShowsDbContext>();

			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();

			return app;
		}
	}
}
