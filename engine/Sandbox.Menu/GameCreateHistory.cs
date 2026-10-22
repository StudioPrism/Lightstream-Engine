using Sandbox.Engine;

namespace Sandbox.Internal;

public partial class GameCreateHistory
{
	public Package Package { get; set; }
	public Dictionary<string, string> Config { get; set; }

	internal static void OnCreateGame( Package game, Dictionary<string, string> details )
	{
		GlobalContext.AssertMenu();

		var list = GetHistory();
		list.RemoveAll( x => x.Package == null || x.Package.FullIdent == game.FullIdent );

		while ( list.Count > 10 )
			list.RemoveAt( list.Count - 1 );

		list.Insert( 0, new GameCreateHistory() { Package = game, Config = details } );

		Game.Cookies.Set( "created_games", list );
		Game.Cookies.Save();
	}

	public static List<GameCreateHistory> GetHistory()
	{
		GlobalContext.AssertMenu();

		return Game.Cookies.Get<List<GameCreateHistory>>( "created_games", null ) ?? new List<GameCreateHistory>();
	}

	public static void Remove( string ident )
	{
		GlobalContext.AssertMenu();

		var list = GetHistory();
		list.RemoveAll( x => x.Package.FullIdent == ident );

		Game.Cookies.Set( "created_games", list );
		Game.Cookies.Save();
	}
}
