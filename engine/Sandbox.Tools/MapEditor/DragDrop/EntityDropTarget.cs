using Editor.MapDoc;

namespace Editor.MapEditor;

internal class EntityDropTarget : IMapViewDropTarget
{
	private MapEntity _entity;

	public EntityDropTarget( string className, MapView view )
	{
		Vector3 normal = Vector3.Zero;
		Vector3 position = Vector3.Zero;
		view.native.GetDropTarget( ref normal, ref position, view.MousePosition );

		_entity = new MapEntity( view.native.GetMapDoc() )
		{
			ClassName = className,
			Position = position
		};

		view.native.EnterFreeDragMode( view.MousePosition, _entity, Vector3.Up, true );
	}

	public void DragMove( MapView view )
	{
		if ( !_entity.IsValid() )
			return;

		view.native.UpdateFreeDragMode( view.MousePosition, false );
	}

	public void DragDropped( MapView view )
	{
		view.native.ExitFreeDragMode( false );

		History.MarkUndoPosition( $"New Entity: {_entity.ClassName}" );
		History.KeepNew( _entity );
	}

	public void DragLeave( MapView view )
	{
		view.native.ExitFreeDragMode( true );

		if ( _entity.IsValid() )
			view.MapDoc.DeleteNode( _entity );
	}
}
