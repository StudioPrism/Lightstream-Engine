using Editor.MapDoc;
using NativeHammer;
using System;

namespace Editor.MapEditor;

/// <summary>
/// Undo/redo history for the current active mapdoc
/// </summary>
public static partial class History
{
	internal static CHistory Native => CHistory.GetHistory();

	/// <summary>
	/// Mark new undo position
	/// </summary>
	/// <param name="name"></param>
	public static void MarkUndoPosition( string name )
	{
		if ( !Native.IsValid ) return;
		Native.MarkUndoPosition( new NativeMapDoc.CSelection( 0 ), name ); // lol
	}

	/// <summary>
	/// Keeps a map node and all its children, so changes to it can be undone.
	/// </summary>
	public static void Keep( MapNode node )
	{
		if ( !Native.IsValid ) return;
		if ( node == null ) throw new ArgumentNullException( nameof( node ) );
		Native.Keep( node );
	}

	/// <summary>
	/// Keeps a new object node and all of its children, so they can be deleted on an undo.
	/// </summary>
	public static void KeepNew( MapNode node )
	{
		if ( !Native.IsValid ) return;
		if ( node == null ) throw new ArgumentNullException( nameof( node ) );
		Native.KeepNew( node );
	}
}
