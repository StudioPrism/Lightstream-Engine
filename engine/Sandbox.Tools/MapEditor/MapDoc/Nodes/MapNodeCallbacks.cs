using Editor.MapDoc;
using NativeMapDoc;

namespace Editor.MapEditor;

internal class MapNodeCallbacks
{
	internal static void OnAddToWorld( MapNode mapNode, MapWorld mapWorld ) => mapNode.OnAddedToWorld( mapWorld );
	internal static void OnRemoveFromWorld( MapNode mapNode, MapWorld mapWorld ) => mapNode.OnRemovedFromWorld( mapWorld );

	internal static void PreSaveToFile( MapNode mapNode ) => mapNode.PreSaveToFile();
	internal static void PostLoadFromFile( MapNode mapNode ) => mapNode.PostLoadFromFile();

	internal static void PostLoadDocument( MapDocument mapDoc ) => mapDoc.PostLoadDocument();

	internal static void OnCopyFrom( MapNode mapNode, MapNode copyFrom, int flags ) => mapNode.OnCopyFrom( copyFrom, flags );
	internal static void OnParentChanged( MapNode mapNode, MapNode parent ) => mapNode.OnParentChanged( parent );
	internal static void OnTransformChanged( MapNode mapNode, Vector3 position, Angles angle, Vector3 scale ) => mapNode.OnNativeTransformChanged( position, angle, scale );
	internal static void OnSetEnabled( MapNode mapNode, bool enabled ) => mapNode.OnSetEnabled( enabled );

	internal static void GetMimeData( MapNode mapNode, nint data ) => mapNode.GetMimeData( (DragData)QObject.FindOrCreate( new QMimeData( data ) ) );
	internal static string GetGameObjectName( MapGameObject mapNode ) => mapNode.GetGameObjectName();

	internal static void GetWorldResourceReferencesAndDependencies( MapWorld world, CUtlSymbolTable references ) => world.GetWorldResourceReferencesAndDependencies( references );
}
