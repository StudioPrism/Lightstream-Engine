namespace Editor.MovieMaker;

#nullable enable

public sealed class MovieMakerConfig : ConfigData
{
	public List<TrackPreset> TrackPresets { get; init; } = new();
}
