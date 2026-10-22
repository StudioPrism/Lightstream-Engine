namespace Sandbox;

[Flags, Expose]
public enum StereoTargetEye
{
	/// <summary>
	/// Don't render in stereo
	/// </summary>
	None = 0,

	/// <summary>
	/// Only render the left eye
	/// </summary>
	LeftEye,

	/// <summary>
	/// Only render the right eye
	/// </summary>
	RightEye,

	/// <summary>
	/// Render both eyes in stereo
	/// </summary>
	Both = LeftEye | RightEye
}
