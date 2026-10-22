namespace Sandbox;

/// <summary>
/// When applied to a component, the editor will draw a selectable handle sprite for the gameobject in scene
/// </summary>
public class EditorHandleAttribute : System.Attribute
{
	public string Texture { get; set; }
	public string Icon { get; set; }
	public Color Color { get; set; } = Color.White;


	public EditorHandleAttribute( string texture )
	{
		Texture = texture;
	}

	public EditorHandleAttribute()
	{

	}
}
