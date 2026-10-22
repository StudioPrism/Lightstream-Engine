namespace Sandbox;

/// <summary>
/// Represents the current state of a device's motion sensor(s).
/// </summary>
public struct InputMotionData
{
	/// <summary>
	/// The raw value from the input device's gyroscope.
	/// </summary>
	public Angles Gyroscope;

	/// <summary>
	/// The raw value from the input device's accelerometer.
	/// </summary>
	public Vector3 Accelerometer;
}
