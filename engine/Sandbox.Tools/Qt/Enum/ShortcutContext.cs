namespace Editor
{
	/// <summary>
	/// Scope of the shortcut. Requires focus at this level for the shortcut to be active.
	/// Defaults to <see cref="ShortcutContext.WindowShortcut"/>.
	/// </summary>
	public enum ShortcutContext
	{
		/// <summary>
		/// Shortcut is only active when the parent widget is focused.
		/// </summary>
		WidgetShortcut,
		/// <summary>
		/// Shortcut is only active when the window of the parent widget is focused.
		/// </summary>
		WindowShortcut,
		/// <summary>
		/// Shortcut is only active when one of the application windows is focused.
		/// </summary>
		ApplicationShortcut,
		/// <summary>
		/// Shortcut is only active when the parent widget, or a child of the parent widget, is focused.
		/// </summary>
		WidgetWithChildrenShortcut
	}
}

