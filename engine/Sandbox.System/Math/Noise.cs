
namespace Sandbox.Utility;

/// <summary>
/// Provides access to coherent noise utilities.
///
/// All of these functions should return between 0 and 1.
/// </summary>
public static partial class Noise
{
	static FastNoise perlin;
	static FastNoise simplex;
	static FastNoise fbm;

	static Noise()
	{
		perlin = new FastNoise( 5633 );
		perlin.SetNoiseType( FastNoise.NoiseType.Perlin );
		perlin.SetFrequency( 0.03f );

		simplex = new FastNoise( 5633 );
		simplex.SetNoiseType( FastNoise.NoiseType.Simplex );
		simplex.SetFrequency( 0.03f );

		fbm = new FastNoise( 5633 );
		fbm.SetNoiseType( FastNoise.NoiseType.PerlinFractal );
		fbm.SetFrequency( 0.03f );
		fbm.SetFractalGain( 0.5f );
		fbm.SetFractalLacunarity( 2.0f );
	}

	internal static float ConvertRange( float f ) => (1f + f) * 0.5f;

	/// <summary>
	/// 2D <a href="https://en.wikipedia.org/wiki/Perlin_noise">Perlin noise</a> function.
	/// For a thread-safe alternative with more options, use <see cref="PerlinField"/>.
	/// </summary>
	/// <param name="x">Input on the X axis.</param>
	/// <param name="y">Input on the Y axis.</param>
	/// <returns>Resulting noise at given coordinates, in range of 0 to 1.</returns>
	public static float Perlin( float x, float y = 0f ) => ConvertRange( perlin.GetNoise( x, y ) );

	/// <summary>
	/// 3D <a href="https://en.wikipedia.org/wiki/Perlin_noise">Perlin noise</a> function.
	/// For a thread-safe alternative with more options, use <see cref="PerlinField"/>.
	/// </summary>
	/// <param name="x">Input on the X axis.</param>
	/// <param name="y">Input on the Y axis.</param>
	/// <param name="z">Input on the Z axis.</param>
	/// <returns>Resulting noise at given coordinates, in range of 0 to 1.</returns>
	public static float Perlin( float x, float y, float z ) => ConvertRange( perlin.GetNoise( x, y, z ) );

	/// <summary>
	/// 2D <a href="https://en.wikipedia.org/wiki/Simplex_noise">Simplex noise</a> function.
	/// For a thread-safe alternative with more options, use <see cref="SimplexField"/>.
	/// </summary>
	/// <param name="x">Input on the X axis.</param>
	/// <param name="y">Input on the Y axis.</param>
	/// <returns>Resulting noise at given coordinates, in range of 0 to 1.</returns>
	public static float Simplex( float x, float y = 0f ) => ConvertRange( simplex.GetNoise( x, y ) );

	/// <summary>
	/// 3D <a href="https://en.wikipedia.org/wiki/Simplex_noise">Simplex noise</a> function.
	/// For a thread-safe alternative with more options, use <see cref="SimplexField"/>.
	/// </summary>
	/// <param name="x">Input on the X axis.</param>
	/// <param name="y">Input on the Y axis.</param>
	/// <param name="z">Input on the Z axis.</param>
	/// <returns>Resulting noise at given coordinates, in range of 0 to 1.</returns>
	public static float Simplex( float x, float y, float z ) => ConvertRange( simplex.GetNoise( x, y, z ) );

	/// <summary>
	/// <a href="https://en.wikipedia.org/wiki/Fractional_Brownian_motion">Fractional Brownian Motion</a> noise, a.k.a. Fractal Perlin noise.
	/// For a thread-safe alternative with more options, use <see cref="PerlinField"/> with <see cref="FractalParameters"/>.
	/// </summary>
	/// <param name="octaves">Number of octaves for the noise. Higher values are slower but produce more detailed results. 3 is a good starting point.</param>
	/// <param name="x">Input on the X axis.</param>
	/// <param name="y">Input on the Y axis.</param>
	/// <param name="z">Input on the Z axis.</param>
	/// <returns>Resulting noise at given coordinates, in range of 0 to 1.</returns>
	public static float Fbm( int octaves, float x, float y = 0.0f, float z = 0.0f )
	{
		fbm.SetFractalOctaves( octaves );

		return ConvertRange( fbm.GetNoise( x, y, z ) );
	}

	/// <summary>
	/// <a href="https://en.wikipedia.org/wiki/Fractional_Brownian_motion">Fractional Brownian Motion</a> noise, a.k.a. Fractal Perlin noise.
	/// </summary>
	/// <param name="octaves">Number of octaves for the noise. Higher values are slower but produce more detailed results. 3 is a good starting point.</param>
	/// <param name="x">Input on the X axis.</param>
	/// <param name="y">Input on the Y axis.</param>
	public static Vector3 FbmVector( int octaves, float x, float y = 0.0f )
	{
		fbm.SetFractalOctaves( octaves );

		return new Vector3( fbm.GetNoise( x, y, 0.0f ), fbm.GetNoise( x, y, 256.0f ), fbm.GetNoise( x, y, 512.0f ) );
	}
}
