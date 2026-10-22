using System;

namespace Editor.MapEditor;

/// <summary>
/// Provides drop targets for Hammer's map views.
/// </summary>
internal static class MapViewDropTarget
{
	internal static IMapViewDropTarget CurrentDropTarget;

	/// <summary>
	/// Called from native when something is dragged into the map view.
	/// Returning true lets native know we're handling it, false lets native handle it.
	/// </summary>
	internal static bool OnDragEnter( QDragEnterEvent e, MapView view )
	{
		var mimedata = e.mimeData();
		var dd = new DragData( mimedata );

		// Asset file (probably)
		if ( dd.Url != null && dd.Url.IsFile )
		{
			var asset = AssetSystem.FindByPath( Uri.UnescapeDataString( dd.Url.AbsolutePath ) );
			if ( asset == null )
				return false;

			var attr = EditorTypeLibrary.GetAttributes<CanDropAttribute>().Where( a => a.PackageTypeOrExtension == asset.AssetType.FileExtension ).FirstOrDefault();
			if ( attr == null )
				return false;

			CurrentDropTarget = (IMapViewDropTarget)Activator.CreateInstance( attr.TargetType );
			CurrentDropTarget.DragEnter( asset, view );

			return true;

			// If it's a GameResource resolve it automatically for drop targets... Or does this complicate it more?
			/*if ( asset.TryLoadResource<GameResource>( out var assetObject ) )
			{
				var resourceType = assetObject.GetType();

				var attrtype = Global.Assembly.GetTypes().Where( x => x.GetCustomAttribute<CanDropAttribute>()?.ResourceType == resourceType ).FirstOrDefault();
				if ( attrtype == null )
					return false;

				CurrentDropTarget = (IMapViewDropTarget)Activator.CreateInstance( attrtype );
				CurrentDropTarget.DragEnter( assetObject, view );
				
				return true;
			}*/
		}
		else if ( dd.Text.StartsWith( "entity:" ) )
		{
			var className = dd.Text["entity:".Length..];
			if ( string.IsNullOrWhiteSpace( className ) )
				return false;

			CurrentDropTarget = new EntityDropTarget( className, view );

			return true;
		}

		if ( dd.Url is null )
			return false;

		// We only handle packages, let native deal with anything that isn't this
		var package = Package.Fetch( dd.Url.ToString(), false ).GetAwaiter().GetResult();
		if ( package == null )
			return false;

		var attr2 = EditorTypeLibrary.GetAttributes<CanDropAttribute>().Where( a => a.PackageTypeOrExtension == package.TypeName ).FirstOrDefault();
		if ( attr2 == null )
			return false;

		CurrentDropTarget = (IMapViewDropTarget)Activator.CreateInstance( attr2.TargetType );
		CurrentDropTarget.DragEnter( package, view );

		return true;
	}

	/// <summary>
	/// Called from native each time the user moves their mouse whilst dragging something over a map view.
	/// </summary>
	internal static bool OnDragMove( QDragMoveEvent e, MapView view )
	{
		if ( CurrentDropTarget == null )
			return false;

		CurrentDropTarget.DragMove( view );

		return true;
	}

	/// <summary>
	/// Called from native when the user drops something onto a map view.
	/// </summary>
	internal static bool OnDrop( QDropEvent e, MapView view )
	{
		if ( CurrentDropTarget == null )
			return false;

		CurrentDropTarget.DragDropped( view );
		CurrentDropTarget = null;

		return true;
	}

	/// <summary>
	/// Called from native when the user cancels a drag operation.
	/// </summary>
	internal static void OnDragLeave( MapView view )
	{
		CurrentDropTarget?.DragLeave( view );
		CurrentDropTarget = null;
	}

	/// <summary>
	/// Queried by native Hammer to know if we're currently dragging something. Kinda shit.
	/// But this is used to stop switching modes whilst drag dropping.
	/// </summary>
	internal static bool GetDragAndDropActive()
	{
		return CurrentDropTarget != null;
	}
}
