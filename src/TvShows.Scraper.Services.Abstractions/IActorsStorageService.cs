using System.Collections.Generic;
using System.Threading.Tasks;
using TvShows.Scraper.Models;

namespace TvShows.Scraper.Services.Abstractions
{
	public interface IActorsStorageService
	{
		Task SaveActors(List<ActorModel> actors);
		Task<int> GetActorByPersonId(int personId);
	}
}
