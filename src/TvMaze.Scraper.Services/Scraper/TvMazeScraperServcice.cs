using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TvShows.Scraper.Models;
using TvShows.Scraper.Services.Abstractions;

namespace TvShows.Scraper.Services
{
	public class TvMazeScraperServcice : ITvMazeScraperServcice
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public TvMazeScraperServcice(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<List<ShowModel>> GetShowsForPage(int page)
		{
			var shows = new List<ShowModel>();

			try
			{
				using var client = _httpClientFactory.CreateClient("TvMaze");

				var response = await client.GetAsync($"/shows?page={page}");

				if (response.StatusCode == HttpStatusCode.OK)
				{
					var result = await response.Content.ReadAsStringAsync();
					var showsModel = JsonConvert.DeserializeObject<IEnumerable<ShowModel>>(result).GroupBy(cast => cast.Id).Select(cast => cast.First()).ToList();
					shows.AddRange(showsModel);
				}
			}
			catch (Exception e)
			{
				throw e;
			}

			return shows;
		}

		public async Task<ShowActrosModel> GetActorsForShow(int showId)
		{
			var showActrosModel = new ShowActrosModel();

			try
			{
				using var client = _httpClientFactory.CreateClient("TvMaze");

				var response = await client.GetAsync($"/shows/{showId}?embed=cast");

				if (response.StatusCode == HttpStatusCode.OK)
				{
					var result = await response.Content.ReadAsStringAsync();
					var tvMazeShowDetails = JsonConvert.DeserializeObject<TvMazeShowDetails>(result);
					showActrosModel.Id = tvMazeShowDetails.Id;
					showActrosModel.Cast = tvMazeShowDetails._embedded.Cast.Select(c => new ActorModel
					{
						Id = c.Person.Id,
						Name = c.Person.Name,
						BirthDay = c.Person.BirthDay
					}).ToList();
				}
			}
			catch (Exception e)
			{
				throw e;
			}

			return showActrosModel;
		}

		public class TvMazeShowDetails
		{
			public int Id { get; set; }
			public TvMazeShowDetailsEmbedded _embedded { get; set; }
		}

		public class TvMazeShowDetailsEmbedded
		{
			public List<TvMazeCastModel> Cast { get; set; }
		}

		public class TvMazeCastModel
		{
			public ActorModel Person { get; set; }
		}
	}
}
