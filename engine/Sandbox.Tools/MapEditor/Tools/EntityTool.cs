using Native;

namespace Editor.MapEditor;

// Let addon layer provide us with a factory, each Hammer session needs to have it's own Entity Tool
public interface IToolFactory
{
	public static IToolFactory Instance { get; set; }

	public IEntityTool CreateEntityTool();
	public IPathTool CreatePathTool();
	// TODO: Get Block tool here
}

/// <summary>
/// Interface for the addon layer to implement, this is called from native Hammer.
/// </summary>
public interface IEntityTool
{
	internal void CreateUI( QWidget container ) => CreateUI( new Widget( container ) );
	public void CreateUI( Widget container );
	public string GetCurrentEntityClassName();

	public static void StartBlockEntityCreation( string className )
	{
		EntityToolGlue.ToolEntity.StartBlockEntityCreation( className );
	}
}

/// <summary>
/// Methods called from native to glue the remaining native tool code to here.
/// This will become redundant as the API matures.
/// </summary>
internal static class EntityToolGlue
{
	internal static CToolEntity ToolEntity;

	internal static IEntityTool Create( CToolEntity toolEntity )
	{
		ToolEntity = toolEntity;

		var entityTool = IToolFactory.Instance.CreateEntityTool();
		Sandbox.InteropSystem.Alloc( entityTool ); // dirty fucker
		return entityTool;
	}
}
