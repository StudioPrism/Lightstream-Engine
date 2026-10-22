namespace Sandbox.VR;

[Expose]
public enum MotionRange
{
	/// <summary>
	/// The default motion range. Provides hand poses that either estimate or fully represent the user's hand.
	/// </summary>
	Hand,

	/// <summary>
	/// Provides hand poses that estimate how the user's hand wraps around a controller, if they're using one.
	/// </summary>
	Controller
}
