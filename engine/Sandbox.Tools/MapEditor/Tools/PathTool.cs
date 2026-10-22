using Native;

namespace Editor.MapEditor;

/// <summary>
/// Interface for the addon layer to implement, this is called from native Hammer.
/// </summary>
public interface IPathTool
{
	internal void CreateUI( QWidget container ) => CreateUI( new Widget( container ) );
	public void CreateUI( Widget container );
	public string GetCurrentEntityClassName();
	public float GetRadiusOffset();
	public bool IsRadiusOffsetEnabled();
}

/// <summary>
/// Methods called from native to glue the remaining native tool code to here.
/// This will become redundant as the API matures.
/// </summary>
internal static class PathToolGlue
{
	internal static IPathTool Create()
	{
		var entityTool = IToolFactory.Instance.CreatePathTool();
		Sandbox.InteropSystem.Alloc( entityTool ); // dirty fucker
		return entityTool;
	}
}
