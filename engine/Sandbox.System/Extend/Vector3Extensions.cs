
namespace Sandbox;

public static partial class SandboxSystemExtensions
{
	/// <summary>
	/// Generates a Catmull-Rom spline from a collection of Vector3 points. Needs at least 3 points to create a spline.
	/// </summary>
	/// <param name="points">The collection of Vector3 points.</param>
	/// <param name="interpolation">The number of interpolated points to generate between each pair of input points.</param>
	/// <returns>An IEnumerable of Vector3 points representing the Catmull-Rom spline.</returns>
	public static IEnumerable<Vector3> CatmullRomSpline( this IEnumerable<Vector3> points, int interpolation )
	{
		Vector3 previousPoint = Vector3.Zero;
		Vector3 currentPoint = Vector3.Zero;
		Vector3 nextPoint = Vector3.Zero;
		Vector3 nextNextPoint = Vector3.Zero;

		int pointIndex = 0;
		foreach ( var point in points )
		{
			if ( pointIndex == 0 )
			{
				currentPoint = point;
				nextPoint = point;
			}
			else if ( pointIndex == 1 )
			{
				nextPoint = point;
				previousPoint = currentPoint; // Assuming a simple linear extrapolation for the first control point
			}
			else if ( pointIndex == 2 )
			{
				nextNextPoint = point;
				// Generate the first set of interpolated points now that we have enough points
				for ( int i = 0; i < interpolation; i++ )
				{
					float t = (float)i / interpolation;
					yield return Vector3.CatmullRomSpline( previousPoint, currentPoint, nextPoint, nextNextPoint, t );
				}
			}
			else
			{
				previousPoint = currentPoint;
				currentPoint = nextPoint;
				nextPoint = nextNextPoint;
				nextNextPoint = point;
				for ( int i = 0; i < interpolation; i++ )
				{
					float t = (float)i / interpolation;
					yield return Vector3.CatmullRomSpline( previousPoint, currentPoint, nextPoint, nextNextPoint, t );
				}
			}
			pointIndex++;
		}

		// Handle the last segment
		if ( pointIndex > 2 )
		{
			previousPoint = currentPoint;
			currentPoint = nextPoint;
			nextPoint = nextNextPoint;
			nextNextPoint = nextNextPoint + (nextNextPoint - nextPoint); // Assuming a simple linear extrapolation for the last control point

			for ( int i = 0; i <= interpolation; i++ )
			{
				float t = (float)i / interpolation;
				yield return Vector3.CatmullRomSpline( previousPoint, currentPoint, nextPoint, nextNextPoint, t );
			}
		}
	}

	public static IEnumerable<Vector3> TcbSpline( this IEnumerable<Vector3> points, int interpolation, float tension, float continuity, float bias )
	{
		Vector3 previousPoint = Vector3.Zero;
		Vector3 currentPoint = Vector3.Zero;
		Vector3 nextPoint = Vector3.Zero;
		Vector3 nextNextPoint = Vector3.Zero;

		int pointIndex = 0;
		foreach ( var point in points )
		{
			if ( pointIndex == 0 )
			{
				currentPoint = point;
				nextPoint = point;
			}
			else if ( pointIndex == 1 )
			{
				nextPoint = point;
				previousPoint = currentPoint; // Assuming a simple linear extrapolation for the first control point
			}
			else if ( pointIndex == 2 )
			{
				nextNextPoint = point;
				// Generate the first set of interpolated points now that we have enough points
				for ( int i = 0; i < interpolation; i++ )
				{
					float t = (float)i / interpolation;
					yield return Vector3.TcbSpline( previousPoint, currentPoint, nextPoint, nextNextPoint, tension, continuity, bias, t );
				}
			}
			else
			{
				previousPoint = currentPoint;
				currentPoint = nextPoint;
				nextPoint = nextNextPoint;
				nextNextPoint = point;
				for ( int i = 0; i < interpolation; i++ )
				{
					float t = (float)i / interpolation;
					yield return Vector3.TcbSpline( previousPoint, currentPoint, nextPoint, nextNextPoint, tension, continuity, bias, t );
				}
			}
			pointIndex++;
		}

		// Handle the last segment
		if ( pointIndex > 2 )
		{
			previousPoint = currentPoint;
			currentPoint = nextPoint;
			nextPoint = nextNextPoint;
			nextNextPoint = nextNextPoint + (nextNextPoint - nextPoint); // Assuming a simple linear extrapolation for the last control point

			for ( int i = 0; i <= interpolation; i++ )
			{
				float t = (float)i / interpolation;
				yield return Vector3.TcbSpline( previousPoint, currentPoint, nextPoint, nextNextPoint, tension, continuity, bias, t );
			}
		}
	}
}
