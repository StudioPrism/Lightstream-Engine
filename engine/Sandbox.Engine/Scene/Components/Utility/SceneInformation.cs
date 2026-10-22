namespace Sandbox;

[Expose]
[Title( "Scene Information" )]
[Category( "Utility" )]
[Icon( "info" )]
[EditorHandle( Icon = "info" )]
public class SceneInformation : Component, ISceneMetadata
{
	[Property] public string Title { get; set; }
	[Property] public TagSet SceneTags { get; set; } = new TagSet();
	[Property] public string Group { get; set; }
	[Property] public string Version { get; set; }
	[Property] public string Author { get; set; }
	[Property, TextArea] public string Description { get; set; }
	[Property, TextArea] public string Changes { get; set; }

	public Dictionary<string, string> GetMetadata()
	{
		var d = new Dictionary<string, string>();

		if ( !string.IsNullOrEmpty( Title ) ) d["Title"] = Title;
		if ( !string.IsNullOrEmpty( Description ) ) d["Description"] = Description;
		if ( !string.IsNullOrEmpty( Group ) ) d["Group"] = Group;
		if ( !string.IsNullOrEmpty( Version ) ) d["Version"] = Version;
		if ( !string.IsNullOrEmpty( Author ) ) d["Author"] = Author;
		if ( !string.IsNullOrEmpty( Changes ) ) d["Changes"] = Changes;

		if ( SceneTags is not null && SceneTags.Any() )
		{
			d["Tags"] = string.Join( ",", SceneTags.TryGetAll() );
		}

		return d;
	}
}
