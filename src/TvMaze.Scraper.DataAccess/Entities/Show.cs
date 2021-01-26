using System.Collections.Generic;

namespace TvShows.Scraper.DataAccess.Entities
{
	public class Show
	{
		public int Id { get; set; }
		public int TvShowId { get; set; }
		public string Name { get; set; }

		public virtual ICollection<ShowActor> ShowActors { get; set; }
	}
}
