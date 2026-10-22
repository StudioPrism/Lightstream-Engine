using System;

namespace Editor.MapEditor;

[AttributeUsage( AttributeTargets.Class, AllowMultiple = true )]
public class CanDropAttribute : Attribute, ITypeAttribute
{
	public Type TargetType { get; set; }
	public string PackageTypeOrExtension { get; init; }

	/// <summary>
	/// Can drop a package or asset extension of this type.
	/// </summary>
	public CanDropAttribute( string packageType )
	{
		PackageTypeOrExtension = packageType;
	}
}

/// <summary>
/// Provides an interface for dragging and dropping <see cref="Asset"/> or <see cref="Package"/> on a map view.
/// Use with <see cref="CanDropAttribute"/> to register your drop target for a <see cref="Package.Type"/> or <see cref="GameResource"/> type.
/// </summary>
public interface IMapViewDropTarget
{
	// TODO: I don't like that there's 2 methods here...

	/// <summary>
	/// An asset started being dragged over a Hammer view..
	/// </summary>
	public void DragEnter( Asset asset, MapView view ) { }

	/// <summary>
	/// An sbox.game package started being dragged over a Hammer view..
	/// </summary>
	public void DragEnter( Package package, MapView view ) { }

	/// <summary>
	/// Called when the mouse cursor moves over a Hammer view while dragging an asset or a package.
	/// </summary>
	public void DragMove( MapView view ) { }

	/// <summary>
	/// Called when a dragged an asset or a package gets finally dropped on a Hammer view.
	/// </summary>
	public void DragDropped( MapView view ) { }

	/// <summary>
	/// Called when a dragged an asset or a package gets dragged outside of a Hammer view.
	/// This is a good spot to clean up any created nodes.
	/// </summary>
	public void DragLeave( MapView view ) { }

	public void DrawGizmos( MapView view ) { }
}
