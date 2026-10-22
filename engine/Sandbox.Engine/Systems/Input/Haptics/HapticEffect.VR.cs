using Sandbox.VR;

namespace Sandbox;

partial record class HapticEffect
{
	internal void Update( VRController vrController )
	{
		const float duration = 0.1f; // 100ms

		if ( ControllerPattern != null )
		{
			ControllerPattern.Update( out var amplitude, out var frequency );
			vrController.Rumble( duration, frequency, amplitude );
		}
	}
}
