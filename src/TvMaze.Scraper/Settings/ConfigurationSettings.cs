using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TvShows.Scraper.Models;

namespace TvShows.Scraper.Api.Settings
{
	public static class ConfigurationSettings
	{
		public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<TvMazeSettingModel>(configuration.GetSection("TvMazeApi"));

			return services;
		}
	}
}
