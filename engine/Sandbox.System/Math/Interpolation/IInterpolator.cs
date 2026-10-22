namespace Sandbox.Interpolation;

/// <summary>
/// Implement this on a type to handle interpolation between two values.
/// </summary>
/// <typeparam name="T"></typeparam>
interface IInterpolator<T>
{
	T Interpolate( T a, T b, float delta );
}

class DelegateInterpolator<T> : IInterpolator<T>
{
	public delegate T InterpolateDelegate( T a, T b, float delta );
	private readonly InterpolateDelegate Func;

	public DelegateInterpolator( InterpolateDelegate func )
	{
		Func = func;
	}

	public T Interpolate( T a, T b, float delta )
	{
		return Func( a, b, delta );
	}
}
