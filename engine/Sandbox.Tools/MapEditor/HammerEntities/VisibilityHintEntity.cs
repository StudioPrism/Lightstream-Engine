namespace Editor.MapEditor.EntityDefinitions;

[Library( "visibility_hint" )]
[EditorSprite( "editor/visibility_hint.vmat" )]
[HammerEntity]
[Title( "Visibility Hint" ), Icon( "sticky_note_2" )]
[BoundsHelper( "box_mins", "box_maxs", true, false )]
sealed class VisibilityHintEntity : HammerEntityDefinition
{
	[Property( "box_mins", Title = "Box Mins" ), DefaultValue( "-64 -64 -64" )]
	public Vector3 BoxMins { get; set; }

	[Property( "box_maxs", Title = "Box Maxs" ), DefaultValue( "64 64 64" )]
	public Vector3 BoxMaxs { get; set; }

	public enum HintTypeChoices
	{
		[Title( "Use High Resolution 8 unit grid" )]
		UseHighResolution = 0,
		[Title( "Use Normal Resolution 16 unit grid" )]
		UseNormalResolution = 1,
		[Title( "Use Medium Resolution 32 unit grid" )]
		UseMediumResolution = 2,
		[Title( "Use Low Resolution 64 unit grid" )]
		UseLowResolution = 3,
		[Title( "Use Lower Resolution 64 unit grid, fewer intial clusters" )]
		UseLowerResolution = 8,
		[Title( "Use Lowest Resolution 256 unit grid, fewer intial clusters" )]
		UseLowestResolution = 9,
		[Title( "X Axis split" )]
		XAxisSplit = 4,
		[Title( "Y Axis split" )]
		YAxisSplit = 5,
		[Title( "Z Axis split" )]
		ZAxisSplit = 6,
	}

	[Property( "hintType", Title = "Hint Type" )]
	[DefaultValue( "3" )]
	public HintTypeChoices HintType { get; set; }
}
