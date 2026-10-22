using Editor.MapDoc;

namespace Editor.MapEditor;

[CanDrop( "sound" )]
class SoundDropTarget : IMapViewDropTarget
{
	MapEntity SoundEntity { get; set; }

	public void DragEnter( Package package, MapView view )
	{
		SoundEntity = new MapEntity();
		SoundEntity.ClassName = "snd_event_point";
		SetSoundFromPackage( package, SoundEntity );
	}

	public void DragEnter( Asset asset, MapView view )
	{
		SoundEntity = new MapEntity();
		SoundEntity.ClassName = "snd_event_point";
		SoundEntity.SetKeyValue( "soundName", asset.Path );
	}

	public void DragMove( MapView view )
	{
		view.BuildRay( out Vector3 rayStart, out Vector3 rayEnd );
		var tr = Trace.Ray( rayStart, rayEnd ).Run( view.MapDoc.World );
		SoundEntity.Position = tr.HitPosition + tr.Normal * 16.0f;
	}

	public void DragLeave( MapView view )
	{
		if ( SoundEntity.IsValid() ) view.MapDoc.DeleteNode( SoundEntity );
	}

	public void DragDropped( MapView view )
	{
		History.MarkUndoPosition( "New Sound" );
		History.KeepNew( SoundEntity );
		SoundEntity = null;
	}

	static async void SetSoundFromPackage( Package package, MapEntity ent )
	{
		var asset = await AssetSystem.InstallAsync( package.FullIdent );
		if ( ent.IsValid() ) ent.SetKeyValue( "soundName", asset.Path );
	}
}
