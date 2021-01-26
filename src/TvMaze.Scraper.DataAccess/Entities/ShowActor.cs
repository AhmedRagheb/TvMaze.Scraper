namespace TvShows.Scraper.DataAccess.Entities
{
	public class ShowActor
	{
		public int ShowId { get; set; }
		public int ActorId { get; set; }

		public virtual Show Show { get; set; }
		public virtual Actor Actor { get; set; }
	}
}
