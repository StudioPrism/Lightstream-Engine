using Native;
using NativeModelDoc;

namespace Editor.ModelEditor;

public static partial class ModelDoc
{
	static CModelDocEditorApp? App;

	public static bool Open => App != null;

	public static Asset ModelAsset => Open ? AssetSystem.FindByPath( App?.GetSessionModel() ) : null;

	internal static void Init( CModelDocEditorApp app )
	{
		App = app;
	}

	[Event( "tools.gamedata.refresh" )]
	internal static void RefreshGameData()
	{
		if ( App?.IsValid == true )
		{
			App?.RefreshGameData();
		}
	}

	internal static void OnToolsMenu( QMenu qmenu )
	{
		EditorEvent.Run( "modeldoc.menu.tools", new Menu( qmenu ) );
	}
}
