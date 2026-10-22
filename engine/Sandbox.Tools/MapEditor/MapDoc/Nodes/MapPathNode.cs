using NativeMapDoc;
using System.ComponentModel.DataAnnotations;

namespace Editor.MapDoc;

/// <summary>
/// Nodes along a <see cref="MapPath"/>
/// </summary>
[Display( Name = "Path Node" ), Icon( "adjust" )]
public sealed class MapPathNode : MapNode
{
	internal CMapPathNode pathNodeNative;

	internal MapPathNode( HandleCreationData _ ) { }

	// Deliberate no constructor, should only be created through the MapPath

	internal override void OnNativeInit( CMapNode ptr )
	{
		base.OnNativeInit( ptr );

		pathNodeNative = (CMapPathNode)ptr;
	}

	internal override void OnNativeDestroy()
	{
		base.OnNativeDestroy();

		pathNodeNative = default;
	}
}
