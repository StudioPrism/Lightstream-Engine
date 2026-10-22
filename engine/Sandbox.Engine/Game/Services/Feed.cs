using Sandbox.Services.Players;

namespace Sandbox.Services;

/// <summary>
/// Activity Feed
/// </summary>
public sealed class Feed
{
	public DateTimeOffset Timestamp { get; init; }
	public string Text { get; init; }
	public string Url { get; init; }
	public string EntryType { get; init; }
	public string Image { get; init; }
	public Profile Player { get; init; }
	public Package Package { get; init; }

	// Internal - because exposed through MenuUtility because games don't need to access this
	internal static async Task<Feed[]> GetFeed( int take = 20 )
	{
		take = take.Clamp( 1, 50 );

		try
		{
			var posts = await Sandbox.Backend.Players.GetFeed( AccountInformation.SteamId, take );
			if ( posts is null ) return Array.Empty<Feed>();

			return posts.Select( x => From( x ) ).ToArray();
		}
		catch ( Exception )
		{
			return default;
		}
	}
	internal static Feed From( PlayerFeedEntry p )
	{
		if ( p is null ) return default;

		return new Feed
		{
			Timestamp = p.Timestamp,
			Text = p.Text,
			Url = p.Url,
			EntryType = p.EntryType,
			Image = p.Image,
			Player = Sandbox.Services.Players.Profile.From( p.Player ),
			Package = RemotePackage.FromDto( p.Package ),
		};
	}
}
