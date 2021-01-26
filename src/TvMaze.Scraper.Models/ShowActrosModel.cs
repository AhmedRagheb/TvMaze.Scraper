using System.Collections.Generic;

namespace TvShows.Scraper.Models
{
	public class ShowActrosModel
	{
		public int Id { get; set; }
		public List<ActorModel> Cast { get; set; }
	}
}
