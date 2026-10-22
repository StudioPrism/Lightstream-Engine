namespace Sandbox.Interpolation;

/// <summary>
/// State information about a <see cref="Rotation"/>. Used for interpolation buffer.
/// </summary>
struct RotationState
{
	public readonly Rotation Rotation;

	public RotationState( Rotation rotation )
	{
		Rotation = rotation;
	}

	public static IInterpolator<RotationState> CreateInterpolator() => Interpolator.Instance;

	private class Interpolator : IInterpolator<RotationState>
	{
		public static readonly Interpolator Instance = new();

		public RotationState Interpolate( RotationState a, RotationState b, float delta )
		{
			return new( Rotation.Lerp( a.Rotation, b.Rotation, delta ) );
		}
	}
}
