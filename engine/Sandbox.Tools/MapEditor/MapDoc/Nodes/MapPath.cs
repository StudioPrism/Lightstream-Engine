using NativeMapDoc;
using System.ComponentModel.DataAnnotations;

namespace Editor.MapDoc;

/// <summary>
/// Path containing a bunch of <see cref="MapPathNode"/>
/// </summary>
[Display( Name = "Path" ), Icon( "conversion_path" )]
public sealed class MapPath : MapNode
{
	internal CMapPath pathNative;

	internal MapPath( HandleCreationData _ ) { }

	public MapPath( MapDocument mapDocument = null )
	{
		ThreadSafe.AssertIsMainThread();

		// Default to the active map document if none specificed
		mapDocument ??= MapEditor.Hammer.ActiveMap;

		Assert.IsValid( mapDocument );

		using var h = IHandle.MakeNextHandle( this );
		mapDocument.native.CreateMapPath();
	}

	internal override void OnNativeInit( CMapNode ptr )
	{
		base.OnNativeInit( ptr );

		pathNative = (CMapPath)ptr;
	}

	internal override void OnNativeDestroy()
	{
		base.OnNativeDestroy();

		pathNative = default;
	}
}
