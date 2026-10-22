using Editor.MapDoc;
using Sandbox;

namespace Editor.MapEditor;

partial class ToolFactory : IToolFactory
{
	[Event( "hammer.initialized" )]
	public static void Initialize()
	{
		IToolFactory.Instance = new ToolFactory();
	}

	public IEntityTool CreateEntityTool() => new EntityTool();
}

/// <summary>
/// Entity tool in Hammer, implements an interface called from native.
/// </summary>
partial class EntityTool : IEntityTool
{
	public EntityTool() => EditorEvent.Register( this );
	~EntityTool() => EditorEvent.Unregister( this );

	public string GetCurrentEntityClassName() => EntitySelector?.SelectedEntity ?? "info_player_start";

	[Event( "hammer.mapview.contextmenu" )]
	public static void OnMapViewContextMenu( Menu menu, MapView view )
	{
		var pointEntites = GameData.EntityClasses.Where( x => x.IsPointClass );

		menu.AddSeparator();
		var submenu = menu.AddMenu( "Create Point Entity" );

		void CreateEntity( string classname )
		{
			var entity = new MapEntity();
			entity.ClassName = classname;

			view.BuildRay( out Vector3 rayStart, out Vector3 rayEnd );
			var tr = Trace.Ray( rayStart, rayEnd ).Run( view.MapDoc.World );

			entity.Position = tr.HitPosition;

			Selection.Set( entity );
		}

		// Recent first ( most useful )
		{
			var recent = submenu.AddMenu( "Recent", "history" );
			foreach ( var r in EditorCookie.Get( "hammer.entitytool.recent", Enumerable.Empty<string>() ).Reverse() )
			{
				var item = pointEntites.Where( i => i.Name == r ).FirstOrDefault();
				if ( item == null ) continue;
				recent.AddOption( item.DisplayName, item.Icon, () => CreateEntity( item.Name ) );
			}
		}

		void CreateGameObject( GameObject go )
		{
			view.BuildRay( out Vector3 rayStart, out Vector3 rayEnd );
			var tr = Trace.Ray( rayStart, rayEnd ).Run( view.MapDoc.World );
			var rot = Rotation.LookAt( tr.Normal, Vector3.Up ) * Rotation.From( 90, 0, 0 );

			var mapGameObject = new MapGameObject( view.MapDoc, go );
			mapGameObject.Position = tr.HitPosition;
			mapGameObject.Angles = rot.Angles();

			History.MarkUndoPosition( $"New {go.Name}" );
			History.KeepNew( mapGameObject );

			Selection.Set( mapGameObject );
		}

		var create = menu.AddMenu( "Create GameObject", "add" );
		create.AddOption( "Empty", "dataset", () =>
		{
			var scene = view.MapDoc.World.Scene;
			using ( scene.Push() )
			{
				var go = new GameObject( true, "Object" );
				CreateGameObject( go );
			}
		} );

		foreach ( var entry in EditorUtility.Prefabs.GetTemplates() )
		{
			var menuPath = string.IsNullOrEmpty( entry.MenuPath ) ? entry.ResourceName : entry.MenuPath;
			create.AddOption( menuPath.Split( '/' ), entry.MenuIcon, () =>
			{
				var scene = view.MapDoc.World.Scene;
				using ( scene.Push() )
				{
					var go = SceneUtility.GetPrefabScene( entry )?.Clone();
					if ( !entry.DontBreakAsTemplate ) go.BreakFromPrefab();
					go.Name = menuPath.Split( '/' ).Last();

					CreateGameObject( go );
				}
			} );
		}

		submenu.AddSeparator();

		var items = pointEntites.GroupBy( x => x.Category?.ToLower() ).OrderBy( p => p.Key == null ).ThenBy( p => p.Key );

		foreach ( var group in items )
		{
			var sub = submenu.AddMenu( !string.IsNullOrEmpty( group.Key ) ? group.Key.Replace( "&", "&&" ).ToTitleCase() : "Uncategorized" );
			foreach ( var item in group )
			{
				sub.AddOption( item.DisplayName, item.Icon, () => CreateEntity( item.Name ) );
			}
		}
	}
}
