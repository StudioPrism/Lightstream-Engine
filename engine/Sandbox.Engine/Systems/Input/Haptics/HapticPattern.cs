namespace Sandbox;

/// <summary>
/// Contains a haptic pattern, which consists of frequency and amplitude values that can change over time.
/// </summary>
public partial record class HapticPattern( float Length, Curve FrequencyCurve, Curve AmplitudeCurve )
{
	public float LengthScale = 1f;
	public float FrequencyScale = 1f;
	public float AmplitudeScale = 1f;

	public int Position { get; private set; } = 0;

	internal void Reset()
	{
		Position = 0;

		LengthScale = 1f;
		FrequencyScale = 1f;
		AmplitudeScale = 1f;
	}

	internal void Update( out float amplitude, out float frequency )
	{
		frequency = 0f;
		amplitude = 0f;

		try
		{
			var length = Length * LengthScale * 1000f;
			var time = Position / length;

			if ( Position > length )
				return;

			GetValue( time, out frequency, out amplitude );
		}
		finally
		{
			Position += (int)(Time.Delta * 1000f);
		}
	}

	public void GetValue( float t, out float frequency, out float amplitude )
	{
		frequency = FrequencyCurve.Evaluate( t ) * FrequencyScale;
		amplitude = AmplitudeCurve.Evaluate( t ) * AmplitudeScale;
	}
}
