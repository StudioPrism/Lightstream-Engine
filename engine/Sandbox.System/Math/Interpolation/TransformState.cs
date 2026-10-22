namespace Sandbox.Interpolation;

/// <summary>
/// State information about a transform. Used for interpolation buffer.
/// </summary>
struct TransformState
{
	public readonly Transform Transform;

	public TransformState( Transform transform )
	{
		Transform = transform;
	}

	public static IInterpolator<TransformState> CreateInterpolator() => Interpolator.Instance;

	private class Interpolator : IInterpolator<TransformState>
	{
		public static readonly Interpolator Instance = new();

		public TransformState Interpolate( TransformState a, TransformState b, float delta )
		{
			return new( Transform.Lerp( a.Transform, b.Transform, delta, true ) );
		}
	}
}
