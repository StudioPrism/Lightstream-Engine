namespace Sandbox;

partial record class HapticPattern
{
	/// <summary>
	/// A haptic pattern that represents a light, soft impact.
	/// </summary>
	public static HapticPattern SoftImpact => new(
		/* Length */
		0.1f,

		/* Frequency */
		new Curve(
			new Curve.Frame( 0.0f, 0.0f ),
			new Curve.Frame( 0.5f, 0.5f ),
			new Curve.Frame( 1.0f, 0.0f )
		),

		/* Amplitude */
		new Curve(
			new Curve.Frame( 0.0f, 0.0f ),
			new Curve.Frame( 0.5f, 1.0f ),
			new Curve.Frame( 1.0f, 0.0f )
		)
	);

	/// <summary>
	/// A haptic pattern that represents a hard, sudden impact.
	/// </summary>
	public static HapticPattern HardImpact => new(
		/* Length */
		0.25f,

		/* Frequency */
		new Curve(
			new Curve.Frame( 0.5f, 0.0f ),
			new Curve.Frame( 1.0f, 0.0f )
		),

		/* Amplitude */
		new Curve(
			new Curve.Frame( 0.5f, 1.0f ),
			new Curve.Frame( 1.0f, 0.0f )
		)
	);

	/// <summary>
	/// A haptic pattern that represents a constant low-frequency rumble.
	/// </summary>
	public static HapticPattern Rumble => new(
		/* Length */
		0.1f,

		/* Frequency */
		new Curve(
			new Curve.Frame( 0.0f, 0.0f ),
			new Curve.Frame( 1.0f, 0.0f )
		),

		/* Amplitude */
		new Curve(
			new Curve.Frame( 0.0f, 1.0f ),
			new Curve.Frame( 1.0f, 1.0f )
		)
	);

	/// <summary>
	/// A haptic pattern that feels like a heartbeat.
	/// </summary>
	public static HapticPattern Heartbeat => new(
	  /* Length */
	  1.0f,

	  /* Frequency */
	  new Curve(
			new Curve.Frame( 0.0f, 0.0f ),
			new Curve.Frame( 0.25f, 0.2f ),
			new Curve.Frame( 0.5f, 0.0f ),
			new Curve.Frame( 0.75f, 0.1f ),
			new Curve.Frame( 1.0f, 0.0f )
	  ),

	  /* Amplitude */
	  new Curve(
			new Curve.Frame( 0.0f, 0.0f ),
			new Curve.Frame( 0.25f, 1.0f ),
			new Curve.Frame( 0.5f, 0.0f ),
			new Curve.Frame( 0.75f, 0.5f ),
			new Curve.Frame( 1.0f, 0.0f )
	  )
	);
}
