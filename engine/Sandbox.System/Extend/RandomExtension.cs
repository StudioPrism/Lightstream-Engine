using System.Runtime.CompilerServices;

namespace Sandbox;

public static partial class SandboxSystemExtensions
{

	static float[] _numbers;

	/// <summary>
	/// Total number of available randoms
	/// </summary>
	const int _numbersCount = 1024 * 8;
	const int _numbersMask = _numbersCount - 1; // see comment in FloatDeterministic below

	static SandboxSystemExtensions()
	{
		_numbers = new float[_numbersCount];

		var random = new Random( 8322 );

		for ( int i = 0; i < _numbersCount; i++ )
		{
			_numbers[i] = random.Float();
		}
	}

	/// <summary>
	/// Returns a double between min and max
	/// </summary>
	public static double Double( this Random self, double min, double max )
	{
		return min + (max - min) * self.NextDouble();
	}

	/// <summary>
	/// Returns a random float between 0 and 1
	/// </summary>
	public static float Float( this Random self )
	{
		return (float)self.NextDouble();
	}

	/// <summary>
	/// Returns a random float between min and max
	/// </summary>
	public static float Float( this Random self, float min, float max )
	{
		return min + (max - min) * self.Float();
	}

	/// <summary>
	/// Returns a random float between 0 and max (or 1)
	/// </summary>
	public static float Float( this Random self, float max ) => self.Float( 0, max );

	/// <summary>
	/// Returns a random double between 0 and max (or 1)
	/// </summary>
	public static double Double( Random self, double max = 1.0 )
	{
		return Double( self, 0, max );
	}

	/// <summary>
	/// Returns a random int between min and max (inclusive)
	/// </summary>
	public static int Int( this Random self, int min, int max )
	{
		return self.Next( min, max + 1 );
	}

	/// <summary>
	/// Returns a random int between 0 and max (inclusive)
	/// </summary>
	public static int Int( this Random self, int max )
	{
		return Int( self, 0, max );
	}

	/// <summary>
	/// Returns a random Color
	/// </summary>
	public static Color Color( this Random self )
	{
		float h = Float( self, 0, 255 );
		float s = 0;
		float v = 255;
		float brightness = v * 1.4f / 255.0f;

		brightness *= 0.7f / (0.01f + (float)System.Math.Sqrt( brightness ));
		brightness = Math.Clamp( brightness, 0.0f, 1.0f );
		Vector3 hue = (h < 86) ? new Vector3( (85 - h) / 85.0f, (h - 0) / 85.0f, 0 ) : (h < 171) ? new Vector3( 0, (170 - h) / 85.0f, (h - 85) / 85.0f ) : new Vector3( (h - 170) / 85.0f, 0, (255 - h) / 84.0f );
		Vector3 color = (hue + s / 255.0f * (Vector3.One - hue)) * brightness;

		return new Color( color.x, color.y, color.z, 1.0f );
	}

	/// <summary>
	/// Returns a uniformly random rotation.
	/// </summary>
	public static Rotation Rotation( this Random self )
	{
		var v4 = self.Gaussian4D();
		return new Rotation( v4.x, v4.y, v4.z, v4.w ).Normal;
	}

	/// <summary>
	/// Returns the angles of a uniformly random rotation.
	/// </summary>
	public static Angles Angles( this Random self )
	{
		return self.Rotation().Angles();
	}

	/// <summary>
	/// Sample from a Gaussian distribution with a given mean and standard deviation.
	/// </summary>
	public static float Gaussian( this Random self, float mean = 0f, float stdDev = 1f )
	{
		// From https://stackoverflow.com/a/218600

		var u1 = 1f - self.NextSingle();
		var u2 = 1f - self.NextSingle();
		var randStdNormal = MathF.Sqrt( -2.0f * MathF.Log( u1 ) ) * MathF.Sin( 2.0f * MathF.PI * u2 );

		return mean + stdDev * randStdNormal;
	}

	/// <summary>
	/// Sample from a 2D Gaussian distribution with a given mean and standard deviation.
	/// </summary>
	public static Vector2 Gaussian2D( this Random self, Vector2? mean = null, Vector2? stdDev = null )
	{
		return new Vector2(
			self.Gaussian( mean?.x ?? 0f, stdDev?.x ?? 1f ),
			self.Gaussian( mean?.y ?? 0f, stdDev?.y ?? 1f ) );
	}

	/// <summary>
	/// Sample from a 3D Gaussian distribution with a given mean and standard deviation.
	/// </summary>
	public static Vector3 Gaussian3D( this Random self, Vector3? mean = null, Vector3? stdDev = null )
	{
		return new Vector3(
			self.Gaussian( mean?.x ?? 0f, stdDev?.x ?? 1f ),
			self.Gaussian( mean?.y ?? 0f, stdDev?.y ?? 1f ),
			self.Gaussian( mean?.z ?? 0f, stdDev?.z ?? 1f ) );
	}

	/// <summary>
	/// Sample from a 4D Gaussian distribution with a given mean and standard deviation.
	/// </summary>
	public static Vector4 Gaussian4D( this Random self, Vector4? mean = null, Vector4? stdDev = null )
	{
		return new Vector4(
			self.Gaussian( mean?.x ?? 0f, stdDev?.x ?? 1f ),
			self.Gaussian( mean?.y ?? 0f, stdDev?.y ?? 1f ),
			self.Gaussian( mean?.z ?? 0f, stdDev?.z ?? 1f ),
			self.Gaussian( mean?.w ?? 0f, stdDev?.w ?? 1f ) );
	}

	/// <summary>
	/// Uniformly samples a 2D position from a square with coordinates in the range -<paramref name="extents"/> to +<paramref name="extents"/>.
	/// </summary>
	public static Vector2 VectorInSquare( this Random self, float extents = 1f )
	{
		return new Vector2( self.Float( -extents, extents ), self.Float( -extents, extents ) );
	}

	/// <summary>
	/// Uniformly samples a 3D position from a cube with coordinates in the range -<paramref name="extents"/> to +<paramref name="extents"/>.
	/// </summary>
	public static Vector3 VectorInCube( this Random self, float extents = 1f )
	{
		return new Vector3( self.Float( -extents, extents ), self.Float( -extents, extents ), self.Float( -extents, extents ) );
	}

	/// <summary>
	/// Uniformly samples a 3D position from a cube
	/// </summary>
	public static Vector3 VectorInCube( this Random self, in BBox box )
	{
		return new Vector3( self.Float( box.Mins.x, box.Maxs.x ), self.Float( box.Mins.y, box.Maxs.y ), self.Float( box.Mins.z, box.Maxs.z ) );
	}

	internal static Vector2 VectorOnCircle( this Random self, float radius = 1f )
	{
		var angle = self.Float( -MathF.PI, MathF.PI );
		return new Vector2( MathF.Cos( angle ), MathF.Sin( angle ) ) * radius;
	}

	internal static Vector3 VectorOnSphere( this Random self, float radius = 1f )
	{
		var z = self.Float( -1.0f, 1.0f );
		var equator = self.VectorOnCircle();
		var xyScale = MathF.Sqrt( 1f - z * z );

		return new Vector3( equator * xyScale, z ) * radius;
	}

	/// <summary>
	/// Uniformly samples a 2D position from all points with distance at most <paramref name="radius"/> from the origin.
	/// </summary>
	public static Vector2 VectorInCircle( this Random self, float radius = 1f )
	{
		return self.VectorOnCircle( radius ) * MathF.Sqrt( self.Float( 0f, 1f ) );
	}

	/// <summary>
	/// Uniformly samples a 3D position from all points with distance at most <paramref name="radius"/> from the origin.
	/// </summary>
	public static Vector3 VectorInSphere( this Random self, float radius = 1f )
	{
		return self.VectorOnSphere( radius ) * MathF.Pow( self.Float( 0f, 1f ), 1f / 3f );
	}

	/// <summary>
	/// Returns a random value in an array
	/// </summary>
	public static T FromArray<T>( this Random self, T[] array, T defVal = default )
	{
		if ( array is null || array.Length == 0 )
			return defVal;

		return array[self.Next( 0, array.Length )];
	}

	/// <summary>
	/// Returns a random value in a list
	/// </summary>
	public static T FromList<T>( this Random self, List<T> array, T defVal = default )
	{
		if ( array is null || array.Count == 0 )
			return defVal;

		return array[self.Next( 0, array.Count )];
	}

	/// <summary>
	/// Get a random float (0-1) from a pre-calculated list. This is faster, and if you put the same seed in, it will always return the same number.
	/// The downside is that it only has 8192 variations of floats, but that seem like enough for most things.
	/// </summary>
	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	public static float FloatDeterministic( this Random self, int i )
	{
		// We use bitmasking instead of modulo because _numbersCount is a power of two.
		// It's way faster and handles negative values correctly due to two's complement math.
		// Future devs: don't change _numbersCount unless you *really* know what you're doing.

		return _numbers[i & _numbersMask];
	}
}
