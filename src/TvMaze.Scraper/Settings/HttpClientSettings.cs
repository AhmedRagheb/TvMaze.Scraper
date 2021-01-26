using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using System;
using System.Net.Http;
using TvShows.Scraper.Models;

namespace TvShows.Scraper.Api.Settings
{
	public static class HttpClientSettings
	{
		public static IServiceCollection AddTvMazeHttpClient(this IServiceCollection services)
		{
			using var provider = services.BuildServiceProvider();

			var tvMazeSettingOptions = provider.GetService<IOptions<TvMazeSettingModel>>();

			var retryPolicy = HttpPolicyExtensions
				.HandleTransientHttpError()
				.Or<TimeoutRejectedException>()
				.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
				.WaitAndRetryAsync(new[]
					{
						TimeSpan.FromSeconds(1),
						TimeSpan.FromSeconds(5),
						TimeSpan.FromSeconds(10)
					});

			var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(30);

			services.AddHttpClient("TvMaze", client =>
			{
				client.BaseAddress = tvMazeSettingOptions.Value.BaseUrl;
				client.Timeout = TimeSpan.FromSeconds(30);
			})
			.AddPolicyHandler(retryPolicy)
			.AddPolicyHandler(timeoutPolicy);

			return services;
		}
	}
}
