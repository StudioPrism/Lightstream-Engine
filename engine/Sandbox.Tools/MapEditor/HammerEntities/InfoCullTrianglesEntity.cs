namespace Editor.MapEditor.EntityDefinitions;

/// <summary>
/// A static (compile-time) volume that will delete geometry inside it as part of map compile.
/// </summary>
[Library( "info_cull_triangles" )]
[EditorSprite( "editor/info_cull_triangles.vmat" )]
[HammerEntity]
[Title( "Cull Triangles" ), Icon( "details" )]
[BoundsHelper( "box_size" )]
class InfoCullTrianglesEntity : HammerEntityDefinition
{
	[Property( "box_size", Title = "Box Size" ), DefaultValue( "128 128 128" )]
	public Vector3 BoxSize { get; set; }

	[Property( "limit_to_world", Title = "Limit to Prefab" )]
	public bool LimitToWorld { get; set; } = false;

	[Property( "targets", Title = "Target Objects" )]
	[FGDType( "node_id_List" )]
	public string TargetObjects { get; set; }

	public enum GeometryTypeChoices
	{
		[Title( "Everything" )]
		Everything = 0,
		[Title( "Only Static Props" )]
		OnlyStaticProps = 1,
		[Title( "Only World Geometry" )]
		OnlyWorldGeometry = 2,
	}

	[Property( "geometry_type", Title = "Apply To" )]
	[DefaultValue( "0" )]
	public GeometryTypeChoices GeometryType { get; set; }
}
