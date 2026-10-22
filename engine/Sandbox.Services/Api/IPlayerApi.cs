using Refit;

namespace Sandbox.Services;

public partial class ServiceApi
{
	public interface IPlayerApi
	{
		[Get( "/player/{steamid}" )]
		Task<Player> Get( long steamid );

		[Get( "/player/{steamid}/overview" )]
		Task<PlayerOverview> GetOverview( long steamid );

		[Get( "/player/{steamid}/feed" )]
		Task<PlayerFeedEntry[]> GetFeed( long steamid, int take );

		[Get( "/player/{steamid}/achievementprogress" )]
		Task<PlayerAchievementProgress[]> GetAchievementProgress( long steamid, int take = 10 );
	}
}
