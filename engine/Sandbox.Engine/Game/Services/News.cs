namespace Sandbox.Services;

/// <summary>
/// News Posts
/// </summary>
public sealed class News
{
	public Guid Id { get; private set; }
	public DateTimeOffset Created { get; private set; }
	public string Title { get; private set; }
	public string Summary { get; private set; }
	public string Url { get; private set; }
	public Sandbox.Services.Players.Profile Author { get; private set; }
	public Package Package { get; private set; }
	public string Media { get; private set; }

	public static async Task<News[]> GetPlatformNews( int take = 10, int skip = 0 )
	{
		take = take.Clamp( 1, 20 );
		skip = skip.Clamp( 0, 1000 );

		try
		{
			var posts = await Sandbox.Backend.News.GetPlatformNews( take, skip );
			if ( posts is null ) return Array.Empty<News>();

			return await Task.WhenAll<News>( posts.Select( async x => await FromAsync( x ) ) );
		}
		catch ( Exception )
		{
			return default;
		}
	}

	public static async Task<News[]> GetPackageNews( string package, int take = 10, int skip = 0 )
	{
		take = take.Clamp( 1, 20 );
		skip = skip.Clamp( 0, 1000 );

		try
		{
			var posts = await Sandbox.Backend.News.GetPackageNews( package, take, skip );
			if ( posts is null ) return Array.Empty<News>();

			return await Task.WhenAll<News>( posts.Select( async x => await FromAsync( x ) ) );
		}
		catch ( Exception )
		{
			return default;
		}
	}

	public static async Task<News[]> GetOrganizationNews( string org, int take = 10, int skip = 0 )
	{
		take = take.Clamp( 1, 20 );
		skip = skip.Clamp( 0, 1000 );

		try
		{
			var posts = await Sandbox.Backend.News.GetOrganizationNews( org, take, skip );
			if ( posts is null ) return Array.Empty<News>();

			return await Task.WhenAll<News>( posts.Select( async x => await FromAsync( x ) ) );
		}
		catch ( Exception )
		{
			return default;
		}
	}

	public static async Task<News[]> GetNews( int take = 10, int skip = 0 )
	{
		take = take.Clamp( 1, 20 );
		skip = skip.Clamp( 0, 1000 );

		try
		{
			var posts = await Sandbox.Backend.News.GetNews( take, skip );
			if ( posts is null ) return Array.Empty<News>();

			return await Task.WhenAll<News>( posts.Select( async x => await FromAsync( x ) ) );
		}
		catch ( Exception )
		{
			return default;
		}
	}

	internal static async Task<News> FromAsync( Sandbox.Services.NewsPostDto p )
	{
		if ( p is null ) return default;

		var package = string.IsNullOrWhiteSpace( p.Package ) ? null : await Package.Fetch( p.Package, true );

		// TODO - NewsPostDto actually has all the sections and the content
		// so we could show these guys 100% in engine at some point.

		return new News
		{
			Id = p.Id,
			Created = p.Created,
			Title = p.Title,
			Summary = p.Summary,
			Url = p.Url,
			Author = Sandbox.Services.Players.Profile.From( p.Author ),
			Package = package,
			Media = p.Media
		};
	}

	internal static News From( Sandbox.Services.NewsPostDto p )
	{
		if ( p is null ) return default;

		Package.TryGetCached( p.Package, out var package, true );

		// TODO - NewsPostDto actually has all the sections and the content
		// so we could show these guys 100% in engine at some point.

		return new News
		{
			Id = p.Id,
			Created = p.Created,
			Title = p.Title,
			Summary = p.Summary,
			Url = p.Url,
			Author = Sandbox.Services.Players.Profile.From( p.Author ),
			Package = package,
			Media = p.Media
		};
	}
}
