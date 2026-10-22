namespace Editor.MapEditor.EntityDefinitions;

/// <summary>
/// This entity defines the spawn point of the player in first person shooter gamemodes.
/// </summary>
[Library( "info_player_start" )]
[HammerEntity, EditorModel( "models/editor/spawnpoint.vmdl", "#e39c0d", FixedBounds = true )]
[Title( "Player Spawnpoint" ), Category( "Player" ), Icon( "place" )]
class SpawnPointEntity : HammerEntityDefinition { }
