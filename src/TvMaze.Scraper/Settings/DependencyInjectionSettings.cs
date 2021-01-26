using Microsoft.Extensions.DependencyInjection;
using TvShows.Scraper.Services;
using TvShows.Scraper.Services.Abstractions;

namespace TvShows.Scraper.Api.Settings
{
	public static class DependencyInjectionSettings
	{
		public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
		{
			services.AddTransient<IShowsStorageService, ShowsStorageService>();
			services.AddTransient<IActorsStorageService, ActorsStorageService>();

			services.AddTransient<ITvMazeScraperServcice, TvMazeScraperServcice>();
			services.AddTransient<IShowsScraperService, ShowsScraperService>();

			return services;
		}
	}
}
