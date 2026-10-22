using Editor.MapDoc;
using Native;
using NativeHammer;
using System;

namespace Editor.MapEditor;

public static partial class Hammer
{
	internal static CHammerApp App;

	public static Asset MapAsset
	{
		get
		{
			AssertAppValid();

			var idx = App.GetActiveMapAsset();
			if ( idx < 0 )
				return null;

			return AssetSystem.Get( (uint)idx );
		}
	}

	/// <summary>
	/// The active editor session's map document.
	/// </summary>
	public static MapDocument ActiveMap
	{
		get
		{
			AssertAppValid();
			return App.GetActiveMapDoc();
		}
	}

	/// <summary>
	/// If the Hammer app has been opened.
	/// </summary>
	public static bool Open => App.IsValid;

	/// <summary>
	/// The Hammer app's window.
	/// </summary>
	public static Window Window { get; internal set; }

	/// <summary>
	/// Current Material - you can set this programmatically with <see cref="SetCurrentMaterial(Asset)"/>
	/// </summary>
	public static Material CurrentMaterial { get; private set; }

	/// <summary>
	/// Called once when Hammer is opened.
	/// Does not get called again if Hammer is closed and reopened.
	/// </summary>
	internal static void Init( CHammerApp app )
	{
		App = app;

		//
		// Let user space know.
		//
		EditorEvent.Run( "hammer.initialized" );
	}

	internal static void RunFrame() { }
	internal static void Shutdown() { }

	/// <summary>
	/// Reloads the active editor session from file with user prompt
	/// </summary>
	public static void ReloadFromFile()
	{
		AssertAppValid();
		App.OnFileReload();
	}

	/// <summary>
	/// Screenspace rendering context called for each map view.
	/// </summary>
	internal static void RenderMapViewHUD( MapView mapView, CToolRenderContext nativeRenderContext )
	{
		ToolRender.native = nativeRenderContext;
		EditorEvent.Run( "hammer.rendermapviewhud" ); // TODO: We have a CMapView, maybe this should be a managed MapView.Draw() call
		ToolRender.native = default;
	}

	/// <summary>
	/// Called from native Hammer whenever the active material is changed.
	/// </summary>
	internal static void UpdateActiveMaterial( NativeEngine.IMaterial material )
	{
		CurrentMaterial = Material.FromNative( material );
	}

	internal static void AssertAppValid()
	{
		if ( App.IsValid == false ) throw new Exception( "Hammer is not open" );
	}

	/// <summary>
	/// Sets the currently used material to the specified asset.
	/// </summary>
	/// <remarks>
	/// I'd happily merge together this into a get setter, but it's a mix of a Material and an Asset
	/// </remarks>
	public static void SetCurrentMaterial( Asset asset )
	{
		AssertAppValid();
		ArgumentNullException.ThrowIfNull( asset );
		if ( asset.AssetType != AssetType.Material ) throw new ArgumentException( "Asset must be a material" );

		App.SetCurrentTexture( asset.GetCompiledFile() );
	}

	/// <summary>
	/// Called from native when the map view is right clicked
	/// </summary>
	internal static void OnMapViewOpenContextMenu( MapView view, QMenu qmenu )
	{
		Assert.IsValid( view );
		EditorEvent.Run( "hammer.mapview.contextmenu", new Menu( qmenu ), view );
	}

	internal static void OnCreateGameObjectMenu()
	{
		var menu = new ContextMenu( null ); // pass parent from native?
		menu.OpenAtCursor( false );

		menu.AddOption( "Empty", "dataset", () =>
		{
			using var scope = ActiveMap.World.EditorSession.Scene.Push();
			var mgo = new MapGameObject( ActiveMap, new GameObject( true, "Object" ) );
			Selection.Set( mgo );
		} );

		foreach ( var entry in EditorUtility.Prefabs.GetTemplates() )
		{
			var menuPath = entry.MenuPath ?? string.Empty;
			menu.AddOption( menuPath.Split( '/' ), entry.MenuIcon, () =>
			{
				using var scope = ActiveMap.World.EditorSession.Scene.Push();
				var go = SceneUtility.GetPrefabScene( entry )?.Clone();
				if ( !entry.DontBreakAsTemplate ) go.BreakFromPrefab();
				go.Name = menuPath.Split( '/' ).Last();

				var mapGameObject = new MapGameObject( ActiveMap, go );

				History.MarkUndoPosition( $"New {go.Name}" );
				History.KeepNew( mapGameObject );

				Selection.Set( mapGameObject );
			} );
		}
	}

	internal static void DirtyViewHuds()
	{
		App.MarkAllViewHudsDirty();
	}

	/// <summary>
	/// Refreshes GameData on map nodes
	/// </summary>
	[Event( "tools.gamedata.refresh" )]
	internal static void RefreshGameData()
	{
		if ( App.IsValid )
		{
			App.RefreshEntitiesGameData();
		}
	}
}
