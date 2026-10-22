namespace Editor.MapEditor;

partial class ToolFactory : IToolFactory
{
	public IPathTool CreatePathTool() => new PathTool();
}

/// <summary>
/// Path Entity tool in Hammer, implements an interface called from native.
/// </summary>
partial class PathTool : IPathTool
{
	public PathTool() => EditorEvent.Register( this );
	~PathTool() => EditorEvent.Unregister( this );

	public string GetCurrentEntityClassName() => EntitySelector?.SelectedEntity ?? "path_generic";
	public float GetRadiusOffset() => Settings.Radius;
	public bool IsRadiusOffsetEnabled() => Settings.OffsetByRadius;
}
