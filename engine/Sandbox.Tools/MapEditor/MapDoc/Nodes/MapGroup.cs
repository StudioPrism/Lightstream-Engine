using NativeMapDoc;
using System.ComponentModel.DataAnnotations;

namespace Editor.MapDoc;

/// <summary>
/// A map node which has the sole purpose of grouping other map nodes together.
/// </summary>
[Display( Name = "Group" ), Icon( "group" )]
public sealed class MapGroup : MapNode
{
	internal CMapGroup groupNative;

	internal MapGroup( HandleCreationData _ ) { }

	public MapGroup( MapDocument mapDocument = null )
	{
		ThreadSafe.AssertIsMainThread();

		// Default to the active map document if none specificed
		mapDocument ??= MapEditor.Hammer.ActiveMap;

		Assert.IsValid( mapDocument );

		using var h = IHandle.MakeNextHandle( this );
		mapDocument.native.CreateMapGroup();
	}

	internal override void OnNativeInit( CMapNode ptr )
	{
		base.OnNativeInit( ptr );

		groupNative = (CMapGroup)ptr;
	}

	internal override void OnNativeDestroy()
	{
		base.OnNativeDestroy();

		groupNative = default;
	}
}
