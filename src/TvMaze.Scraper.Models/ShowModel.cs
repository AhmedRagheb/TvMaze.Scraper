using System.Collections.Generic;

namespace TvShows.Scraper.Models
{
	public class ShowModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public IEnumerable<ActorModel> Cast { get; set; }
	}
}
