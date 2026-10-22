using Editor.MapDoc;

namespace Editor.MapEditor;

[CanDrop( "sndscape" )]
class SoundscapeDropTarget : IMapViewDropTarget
{
	MapEntity SoundScapeEntity { get; set; }

	public void DragEnter( Package package, MapView view )
	{
		SoundScapeEntity = new MapEntity();
		SoundScapeEntity.ClassName = "snd_soundscape";
		SetSoundFromPackage( package, SoundScapeEntity );
	}

	public void DragEnter( Asset asset, MapView view )
	{
		SoundScapeEntity = new MapEntity();
		SoundScapeEntity.ClassName = "snd_soundscape";
		SoundScapeEntity.SetKeyValue( "soundscape", asset.Path );
	}

	public void DragMove( MapView view )
	{
		view.BuildRay( out Vector3 rayStart, out Vector3 rayEnd );
		var tr = Trace.Ray( rayStart, rayEnd ).Run( view.MapDoc.World );
		SoundScapeEntity.Position = tr.HitPosition + tr.Normal * 16.0f;
	}

	public void DragLeave( MapView view )
	{
		if ( SoundScapeEntity.IsValid() ) view.MapDoc.DeleteNode( SoundScapeEntity );
	}

	public void DragDropped( MapView view )
	{
		History.MarkUndoPosition( "New Soundscape" );
		History.KeepNew( SoundScapeEntity );
		SoundScapeEntity = null;
	}

	static async void SetSoundFromPackage( Package package, MapEntity ent )
	{
		var asset = await AssetSystem.InstallAsync( package.FullIdent );
		if ( ent.IsValid() ) ent.SetKeyValue( "soundscape", asset.Path );
	}
}
