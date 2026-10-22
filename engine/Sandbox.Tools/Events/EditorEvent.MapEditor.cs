using Sandbox;

namespace Editor;

public static partial class EditorEvent
{
	/// <summary>
	/// Events that happen within the map editor.
	/// </summary>
	public static class MapEditor
	{
		/// <summary>
		/// Called when the user selects / deselects any object in the map and <see cref="Editor.MapEditor.Selection.All"></see> is changed.
		/// </summary>
		public class SelectionChanged : EventAttribute { public SelectionChanged() : base( "hammer.selection.changed" ) { } }

		/// <summary>
		/// Called when the map view is right clicked, <see cref="Editor.Menu"/> is passed.
		/// </summary>
		public class MapViewContextMenu : EventAttribute { public MapViewContextMenu() : base( "hammer.mapview.contextmenu" ) { } }

	}
}
