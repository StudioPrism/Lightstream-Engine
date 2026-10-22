using Native;
namespace Editor;

public partial class EditorUtility
{
	public static partial class Clipboard
	{
		public static void Copy( string text )
		{
			QApp.SetClipboardText( text );
		}

		public static string Paste()
		{
			return QApp.GetClipboardText();
		}
	}
}
