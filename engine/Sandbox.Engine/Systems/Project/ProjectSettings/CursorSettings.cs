namespace Sandbox;

[Expose]
public class CursorSettings : ConfigData
{
	[Expose]
	public struct Cursor
	{
		[KeyProperty, ResourceType( "jpg" )]
		public string Image { get; set; }

		[KeyProperty]
		public Vector2 Hotspot { get; set; }
	}

	[Hide]
	public override int Version => 2;

	public Dictionary<string, Cursor> Cursors { get; set; }
}
