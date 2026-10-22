namespace Sandbox;

/// <summary>
/// Component that creates a projected decal relative to its GameObject.
/// </summary>
[Hide]
[Obsolete( "DecalRenderer is obsolete, use Decal which uses textures instead of materials https://sbox.game/dev/doc/reference/components/decals/" )]
public class DecalRenderer : Renderer, Component.ExecuteInEditor
{
	[Obsolete]
	public Material Material { get; set; }

	[Property] public Vector3 Size { get; set; } = new( 32, 32, 256 );
	[Property] public Color TintColor { get; set; } = Color.White;
	[Property] public bool TriPlanar { get; set; } = false;
}
