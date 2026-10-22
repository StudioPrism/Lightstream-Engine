namespace Editor.MapEditor.EntityDefinitions;

/// <summary>
/// A generic brush/mesh that can toggle its visibility and collisions, and can be broken.
/// </summary>
[Library( "func_brush" )]
[Solid, HammerEntity, RenderFields, VisGroup( VisGroup.Dynamic )]
[Title( "Brush (Static)" ), Category( "Gameplay" ), Icon( "brush" )]
class FuncBrushEntity : HammerEntityDefinition
{
	[Property] public bool Enabled { get; set; } = true;
	[Property] public bool Collisions { get; set; } = true;
	[Property( "health" ), Title( "Health" )] public bool HealthOverride { get; set; } = true;
}
