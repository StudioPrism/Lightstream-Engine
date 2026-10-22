using Editor.MapDoc;
using NativeMapDoc;
using System;

namespace Editor.MapEditor;

/// <summary>
/// Current selection set for the active map
/// </summary>
/// <remarks>
/// Currently this only supports <see cref="MapNode"/> selections.
/// There are selections of vertices, edges, faces too that would likely change this API
/// </remarks>
public static class Selection
{
	/// <summary>
	/// Called when the selection in Hammer is changed
	/// </summary>
	public static event Action OnChanged;

	/// <summary>
	/// The current selection mode e.g Meshes or Objects
	/// </summary>
	public static SelectMode SelectMode
	{
		get
		{
			if ( !NativeSelection.IsValid ) return default;
			return NativeSelection.GetMode();
		}
		set
		{
			if ( !NativeSelection.IsValid ) return;
			NativeSelection.SetMode( value, SelectionConversionMethod_t.SELECTION_CONVERT_STANDARD );
		}
	}

	/// <summary>
	/// The position of the selection's pivot
	/// </summary>
	public static Vector3 PivotPosition
	{
		get
		{
			if ( !NativeSelection.IsValid ) return default;
			return NativeSelection.ActiveSelectionSet().GetPivotPosition();
		}
		set
		{
			if ( !NativeSelection.IsValid ) return;
			NativeSelection.ActiveSelectionSet().SetPivot( value );
		}
	}

	/// <summary>
	/// All the map nodes in the current selection set
	/// </summary>
	public static IEnumerable<MapNode> All
	{
		get
		{
			if ( !NativeSelection.IsValid )
				yield break;

			var count = NativeSelection.ActiveSelectionSet().Count();
			for ( int i = 0; i < count; i++ )
			{
				var node = NativeSelection.ActiveSelectionSet().GetSelectedObject( i );
				yield return node;
			}
		}
	}

	/// <summary>
	/// Add the map node to the current set
	/// </summary>
	public static void Add( MapNode node )
	{
		NativeSelection.ActiveSelectionSet().SelectObject( node, SelectionOperation_t.SELECT_OP_ADD );
	}

	/// <summary>
	/// Clear the current set, making the map node the only selected node
	/// </summary>
	public static void Set( MapNode node )
	{
		NativeSelection.ActiveSelectionSet().SelectObject( node, SelectionOperation_t.SELECT_OP_SET );
	}

	/// <summary>
	/// Remove this map node from the current set if it exists
	/// </summary>
	public static void Remove( MapNode node )
	{
		NativeSelection.ActiveSelectionSet().SelectObject( node, SelectionOperation_t.SELECT_OP_REMOVE );
	}

	/// <summary>
	/// Clear everything from the current selection set
	/// </summary>
	public static void Clear()
	{
		NativeSelection.ActiveSelectionSet().RemoveAll();
	}

	/// <summary>
	/// Add all to the current selection
	/// </summary>
	public static void SelectAll()
	{
		NativeSelection.ActiveSelectionSet().SelectAll();
	}

	/// <summary>
	/// Invert the current selection
	/// </summary>
	public static void InvertSelection()
	{
		NativeSelection.ActiveSelectionSet().InvertSelection();
	}

	internal static CSelection NativeSelection
	{
		get
		{
			if ( !Hammer.ActiveMap.IsValid() ) return default;
			return Hammer.ActiveMap.native.GetSelection();
		}
	}

	internal static void OnSelectionChanged()
	{
		OnChanged?.Invoke();
		EditorEvent.Run( "hammer.selection.changed" );
	}
}

public enum SelectMode
{
	/// <summary>
	/// Select groups, ungrouped entities, and ungrouped solids
	/// </summary>
	Groups = 0,
	/// <summary>
	/// Select entities and solids not in entities
	/// </summary>
	Objects,
	/// <summary>
	/// Select point entities, solids in entities, solids
	/// </summary>
	Meshes,
	/// <summary>
	/// Select vertices
	/// </summary>
	Verticies,
	/// <summary>
	/// Select edges
	/// </summary>
	Edges,
	/// <summary>
	/// Select faces
	/// </summary>
	Faces,
	/// <summary>
	/// Select nav mesh components
	/// </summary>
	Nav,
	/// <summary>
	/// Select the grid tiles
	/// </summary>
	Tiles
}
