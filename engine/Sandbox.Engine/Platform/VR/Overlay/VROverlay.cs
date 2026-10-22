namespace Sandbox.VR;

/// <summary>
/// <para>VR overlays draw over the top of the 3D scene, they will not be affected by lighting,
/// post processing effects or anything else in the world.<br />
/// This makes them ideal for HUDs or menus, or anything else that should be local to the
/// HMD or tracked devices.</para>
/// 
/// <para>If you need something in the world, consider using WorldPanel
/// and WorldInput instead.</para>
/// </summary>
[Obsolete( "Unsupported by OpenXR. Please use WorldPanel." )]
public partial class VROverlay : IDisposable
{
	// Not really explained in OpenVR, but used in HLVR for HUD transforms
	internal static VROverlay HeadsetViewOverlay => Find( "system.HeadsetView" );

	internal static List<WeakReference<VROverlay>> All = new();
	internal static VROverlay Focused;

	internal ulong handle;
	internal string key;
	static internal int KeyCount;

	internal static VROverlay Find( string key )
	{
		return default;
	}

	private VROverlay( ulong handle )
	{
		this.handle = handle;
	}

	public VROverlay()
	{

	}

	~VROverlay() => Dispose( false );

	/// <summary>
	/// Destroys this overlay.
	/// </summary>
	public void Dispose()
	{
		Dispose( true );
		GC.SuppressFinalize( this );
	}

	/// <summary>
	/// Destroys this overlay.
	/// </summary>
	protected virtual void Dispose( bool disposing )
	{

	}

	static internal void DisposeAll()
	{
		for ( int i = All.Count - 1; i >= 0; i-- )
		{
			// Remove any dead references
			if ( !All[i].TryGetTarget( out var overlay ) )
			{
				continue;
			}

			overlay.Dispose();
			All.Clear();
		}
	}

	internal bool _visible;

	/// <summary>
	/// Shows or hides the VR overlay.
	/// </summary>
	public bool Visible
	{
		get => false;
		set
		{
			if ( _visible == value ) return;

			_visible = value;
		}
	}

	internal Transform _transform = Transform.Zero;

	/// <summary>
	/// Sets the transform to absolute tracking origin
	/// </summary>
	public Transform Transform
	{
		get => _transform;
		set
		{
			if ( _transform == value ) return;
			_transform = value;
			SetTransformAbsolute( value );
		}
	}

	internal static Matrix SteamVrMatrixFromTransform( Transform transform )
	{
		var mat = (Matrix.CreateTranslation( transform.Position )
			* Matrix.CreateRotation( transform.Rotation )
			* Matrix.CreateScale( transform.Scale ));

		return mat.ToSteamVrCoordinateSystem.Transpose();
	}

	/// <summary>
	/// Sets the transform to absolute tracking origin
	/// </summary>
	public void SetTransformAbsolute( Transform transform )
	{

	}

	/// <summary>
	/// Sets the rendering sort order for the overlay.
	/// </summary>
	public uint SortOrder
	{
		set { }
	}

	internal float _widthInMeters = 1.0f;

	/// <summary>
	/// The width of the overlay quad.
	/// By default overlays are rendered on a quad that is 1 meter across.
	/// </summary>
	public float Width
	{
		get => _widthInMeters / 0.0254f;
		set
		{
			if ( _widthInMeters == value * 0.0254f ) return;

			_widthInMeters = value * 0.0254f;
		}
	}

	internal float _curvature;

	/// <summary>
	/// Use to draw overlay as a curved surface. Curvature is a percentage from (0..1] where 1 is a fully closed cylinder.
	/// For a specific radius, curvature can be computed as: overlay.width / (2 PI r).
	/// </summary>
	public float Curvature
	{
		get => _curvature;
		set
		{
			if ( _curvature == value ) return;

			_curvature = value;
		}
	}

	internal Color _color;

	/// <summary>
	/// Sets the color tint of the overlay quad. Use 0.0 to 1.0 per channel.
	/// Sets the alpha of the overlay quad. Use 1.0 for 100 percent opacity to 0.0 for 0 percent opacity.
	/// </summary>
	public Color Color
	{
		get => _color;
		set
		{
			if ( _color == value ) return;

			_color = value;
		}
	}

	internal Texture _texture;

	/// <summary>
	/// Texture that is rendered on the overlay quad.
	/// <see cref="TextureBuilder"/>
	/// </summary>
	public Texture Texture
	{
		get => _texture;
		set
		{
			_texture = value;
		}
	}

	private Vector2 _mouseScale;

	/// <summary>
	/// Sets the mouse scaling factor that is used for mouse events. 
	/// </summary>
	public Vector2 MouseScale
	{
		get => _mouseScale;
		set
		{
			if ( _mouseScale == value ) return;

			_mouseScale = value;
		}
	}

	/// <summary>
	/// Triggers a haptic event on the laser mouse controller for this overlay
	/// </summary>
	internal void TriggerLaserMouseHapticVibration( float durationSeconds, float frequency, float amplitude )
	{
		if ( handle == 0 ) return;
	}
}
