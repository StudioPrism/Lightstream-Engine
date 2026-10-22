namespace Sandbox.VR;

/// <summary>
/// Accessors for <see cref="VRController.GetFingerValue(Sandbox.VR.FingerValue)"/>
/// </summary>
public enum FingerValue
{
	/// <summary>
	/// Represents the curling motion of the thumb.
	/// </summary>
	ThumbCurl = 0,

	/// <summary>
	/// Represents the curling motion of the index finger.
	/// </summary>
	IndexCurl = 1,

	/// <summary>
	/// Represents the curling motion of the middle finger.
	/// </summary>
	MiddleCurl = 2,

	/// <summary>
	/// Represents the curling motion of the ring finger.
	/// </summary>
	RingCurl = 3,

	/// <summary>
	/// Represents the curling motion of the pinky finger.
	/// </summary>
	PinkyCurl = 4,

	/// <summary>
	/// Represents the splaying motion between the thumb and index finger.
	/// </summary>
	ThumbIndexSplay = 10,

	/// <summary>
	/// Represents the splaying motion between the index and middle fingers.
	/// </summary>
	IndexMiddleSplay = 11,

	/// <summary>
	/// Represents the splaying motion between the middle and ring fingers.
	/// </summary>
	MiddleRingSplay = 12,

	/// <summary>
	/// Represents the splaying motion between the ring and pinky fingers.
	/// </summary>
	RingPinkySplay = 13,
}
