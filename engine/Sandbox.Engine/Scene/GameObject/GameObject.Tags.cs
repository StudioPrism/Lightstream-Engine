namespace Sandbox;

public partial class GameObject
{
	[ActionGraphInclude]
	public GameTags Tags { get; init; }

	/// <summary>
	/// Called by GameTags when the tags change
	/// </summary>
	internal void OnTagsUpdatedInternal()
	{
		Components.ForEach( "TagsUpdated", false, c => c.OnTagsUpdatedInternal() );
	}
}
