namespace Sandbox.Interpolation;

/// <summary>
/// State information about a <see cref="Vector3"/>. Used for interpolation buffer.
/// </summary>
struct Vector3State
{
	public readonly Vector3 Value;

	public Vector3State( Vector3 value )
	{
		Value = value;
	}

	public static IInterpolator<Vector3State> CreateInterpolator() => Interpolator.Instance;

	private class Interpolator : IInterpolator<Vector3State>
	{
		public static readonly Interpolator Instance = new();

		public Vector3State Interpolate( Vector3State a, Vector3State b, float delta )
		{
			return new( Vector3.Lerp( a.Value, b.Value, delta ) );
		}
	}
}
