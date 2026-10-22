namespace Sandbox;

public partial class DebugOverlaySystem
{
	List<Entry> entries = new();

	class Entry : IDisposable
	{
		public bool CreatedDuringFixed;
		public bool SingleFrame = true;
		public float life;
		public SceneObject sceneObject;

		public Entry( float duration, bool fixedUpdate, SceneObject so )
		{
			CreatedDuringFixed = fixedUpdate;
			sceneObject = so;

			if ( duration > 0 )
			{
				life = duration;
				SingleFrame = false;
			}
		}

		public void Dispose()
		{
			sceneObject?.Delete();
			sceneObject = default;
		}
	}

	/// <summary>
	/// Add an entry manually
	/// </summary>
	void Add( float duration, SceneObject so )
	{
		// common flags for debug overlays
		so.Flags.IncludeInCubemap = false;
		so.Tags.Add( "debugoverlay" );

		var entry = new Entry( duration, inFixedUpdate, so );
		entries.Add( entry );
	}
}
