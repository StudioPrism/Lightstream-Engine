
namespace Sandbox;

/// <summary>
/// If this exists and is enabled in a scene, then the client will hear from this point rather than
/// from the cameras point of view.
/// </summary>
[Category( "Audio" )]
[Title( "Listener" )]
[Icon( "hearing" )]
[EditorHandle( "materials/gizmo/audiolistener.png" )]
[Alias( "SoundListener" )]
[Tint( EditorTint.Green )]
public sealed class AudioListener : Component
{
	/// <summary>
	/// If true, while the audio listener position will be used, the rotation element will come from the camera.
	/// </summary>
	[Property]
	public bool UseCameraDirection { get; set; } = true;

	private Audio.Listener _listener;

	protected override void OnEnabled()
	{
		Transform.OnTransformChanged += UpdatePosition;

		Scene.RemoveListener( _listener );
		_listener = Scene.AddListener();

		UpdatePosition();
	}

	protected override void OnDisabled()
	{
		Transform.OnTransformChanged -= UpdatePosition;

		Scene.RemoveListener( _listener );
		_listener = default;
	}

	void UpdatePosition()
	{
		var tx = WorldTransform.WithScale( 1 );

		if ( UseCameraDirection && Scene.IsValid() && Scene.Camera.IsValid() )
		{
			tx.Rotation = Scene.Camera.WorldRotation;
		}

		if ( _listener is not null )
		{
			_listener.Transform = tx;
		}
	}
}
