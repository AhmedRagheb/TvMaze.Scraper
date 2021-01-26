using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TvShows.Scraper.Services.Tests
{
	public class FakeHttpMessageHandler : HttpMessageHandler
	{
		public virtual HttpResponseMessage Send(HttpRequestMessage request)
		{
			return Send(request);
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
		{
			return Task.FromResult(Send(request));
		}
	}
}
