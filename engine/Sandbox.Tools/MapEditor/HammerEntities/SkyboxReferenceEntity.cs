namespace Editor.MapEditor.EntityDefinitions;

//
// This is a weird entity, the c++ game entity system finds these by classname
// and treats them as a point prefab loading the target map as a spawngroup
//

[Library( "skybox_reference" )]
[HammerEntity]
[EditorModel( "models/editor/skybox_reference.vmdl" )]
[Title( "Skybox Reference" ), Category( "Fog & Sky" ), Icon( "photo_camera" )]
[Global( "3dskybox" )]
class SkyBoxReferenceEntity : HammerEntityDefinition
{
	[Property( "targetMapName", Title = "Map Name" ), FGDType( "instance_file" )]
	public string TargetMapName { get; set; }

	[Property( "fixupNames", Title = "Fixup Entity Names" )]
	public bool FixupNames { get; set; } = false;

	[Property( "worldGroupID" )]
	public string WorldGroupID { get; set; } = "skyboxWorldGroup0";
}
