using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TvShows.Scraper.Models;
using Xunit;

namespace TvShows.Scraper.Services.Tests
{
	public class TvMazeScraperServciceTests
	{
		private readonly Mock<FakeHttpMessageHandler> _fakeHttpMessageHandler;
		private readonly TvMazeScraperServcice _tvMazeScraperServcice;

		public TvMazeScraperServciceTests()
		{
			var mockHttpClientFactory = new Mock<IHttpClientFactory>();
			_fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
			var httpClient = new HttpClient(_fakeHttpMessageHandler.Object)
			{
				BaseAddress = new System.Uri("https://stup-api.tvmaze.com")
			};

			mockHttpClientFactory.Setup(mock => mock.CreateClient("TvMaze")).Returns(httpClient);

			_tvMazeScraperServcice = new TvMazeScraperServcice(mockHttpClientFactory.Object);
		}

		[Fact]
		public async Task GetShows_WithOK_ShouldReturn_ShowsModel()
		{
			_fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent("[{'id': 1, 'name': 'Kirby Buckets' }]")
			});

			var actual = await _tvMazeScraperServcice.GetShowsForPage(1);
			var expected = new List<ShowModel>
			{
				new ShowModel
				{
					Id = 1,
					Name = "Kirby Buckets"
				}
			};

			actual.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public async Task GetActorsForShow_WithOK_ShouldReturn_ActorsModel()
		{
			_fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent("{'id': 1, '_embedded': { 'cast': [{ 'person': { 'id': 1, 'name': 'Mike Vogel' } }] }}")
			});

			var actual = await _tvMazeScraperServcice.GetActorsForShow(1);
			var expected = new ShowActrosModel
			{
				Id = 1,
				Cast = new List<ActorModel>
				{
					new ActorModel
					{
						Id = 1,
						Name = "Mike Vogel"
					}
				}
			};

			actual.Should().BeEquivalentTo(expected);
		}

		[Fact]
		public async Task GetShows_WithNotFound_ShouldReturn_EmptyList()
		{
			_fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.NotFound,
				Content = new StringContent(string.Empty)
			});

			var actual = await _tvMazeScraperServcice.GetShowsForPage(1);
			actual.Should().BeEmpty();
		}
	}
}
