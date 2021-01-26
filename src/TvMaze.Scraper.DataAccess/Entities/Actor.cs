using System;
using System.Collections.Generic;

namespace TvShows.Scraper.DataAccess.Entities
{
	public class Actor
	{
		public int Id { get; set; }
		public int PersonId { get; set; }
		public string Name { get; set; }
		public DateTime? BirthDay { get; set; }

		public virtual ICollection<ShowActor> ShowActors { get; set; }
	}
}
