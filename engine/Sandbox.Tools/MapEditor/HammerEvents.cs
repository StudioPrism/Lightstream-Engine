using Editor.MapDoc;
using NativeMapDoc;

namespace Editor.MapEditor;

/// <summary>
/// Events from hammerevents.h
/// </summary>
internal static class HammerEvents
{
	internal static void OnMapNodeDescriptionChanged( MapNode node )
	{
		// class_name probably changed, update the type description
		if ( node is MapEntity entity )
		{
			entity.UpdateTypeDescription();
		}
	}

	internal static void OnObjectAddedToDocument( MapNode node, MapWorld world )
	{

	}

	internal static void OnObjectRemovedFromDocument( MapNode node, MapWorld world )
	{

	}

	internal static void OnMeshesTiedToGameObject( MapGameObject mapGameObject )
	{
		if ( mapGameObject is null )
			return;

		mapGameObject.OnMeshesTied();
	}
}
