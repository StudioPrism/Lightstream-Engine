namespace Sandbox.Services;

/// <summary>
/// Allows access to stats for the current game. Stats are defined by the game's author
/// and can be used to track anything from player actions to performance metrics. They are
/// how you submit data to leaderboards.
/// </summary>
public static partial class Achievements
{
	/// <summary>
	/// Stats for the current map
	/// </summary>
	public static class Map
	{
		[Title( "Unlock Map Achievement" )]
		[Category( "Services/Achievements" )]
		[Icon( "emoji_events" )]
		[ActionGraphNode( "services.achievements.map.unlock" )]
		public static void Unlock( string name )
		{
			var package = Application.MapPackage;
			if ( package is null ) return;

			var collection = package.GetCachedAchievements();
			if ( collection is null ) return;

			collection.ManualUnlock( name );
		}

		public static IEnumerable<Achievement> All => Application.MapPackage?.GetCachedAchievements()?.All ?? Enumerable.Empty<Achievement>();

		[Title( "Get Achievement" )]
		[Category( "Services/Achievements" )]
		[Icon( "emoji_events" )]
		[ActionGraphNode( "services.achievements.map.get" ), Pure]
		public static Achievement Get( string name ) => All.Where( x => x.Name == name ).FirstOrDefault();
	}
}

