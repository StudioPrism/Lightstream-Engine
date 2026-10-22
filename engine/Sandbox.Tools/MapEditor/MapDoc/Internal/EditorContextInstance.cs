using Editor.MapDoc;

namespace Editor.MapEditor;

internal class EditorContextInstance : EditorContext
{
	private MapDocument mapDoc;

	public EditorContextInstance( MapDocument mapDoc )
	{
		this.mapDoc = mapDoc;

		Selection = Editor.MapEditor.Selection
							.All
							.OfType<MapEntity>()
							.Select( x => x.SerializedObject )
							.Cast<EditorContext.EntityObject>()
							.ToHashSet();
	}

	//
	// TODO: we're iterating over a list provided by native
	//	we we might as well have World.FindTargets() and iterate over that
	//	from native somehow instead of trying to recreate it here
	//

	public override EditorContext.EntityObject FindTarget( string name )
	{
		return mapDoc.World.Children.OfType<MapEntity>()
			.Where( x => IsTarget( x, name ) )
			.Select( x => x.SerializedObject as EditorContext.EntityObject )
			.FirstOrDefault();
	}

	private bool IsTarget( MapEntity x, string name )
	{
		return x.entityNative.TargetNameMatches( name );
	}

	public override EditorContext.EntityObject[] FindTargets( string name )
	{
		return mapDoc.World.Children.OfType<MapEntity>()
			.Where( x => IsTarget( x, name ) )
			.Select( x => x.SerializedObject as EditorContext.EntityObject )
			.ToArray();
	}

}
