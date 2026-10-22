namespace Sandbox.Utility;

#nullable enable

/// <summary>
/// A noise function that can be sampled at a 1-, 2-, or 3D position.
/// Samples will be between <c>0</c> and <c>1</c>. Thread-safe.
/// </summary>
public interface INoiseField
{
	/// <summary>
	/// Sample at a 1D position.
	/// </summary>
	/// <returns>A noise value between <c>0</c> and <c>1</c>.</returns>
	[Pure] public float Sample( float x ) => Sample( x, 0f );

	/// <summary>
	/// Sample at a 2D position.
	/// </summary>
	/// <returns>A noise value between <c>0</c> and <c>1</c>.</returns>
	[Pure] public float Sample( float x, float y ) => Sample( x, y, 0f );

	/// <summary>
	/// Sample at a 3D position.
	/// </summary>
	/// <returns>A noise value between <c>0</c> and <c>1</c>.</returns>
	[Pure] float Sample( float x, float y, float z );

	/// <summary>
	/// Sample at a 2D position.
	/// </summary>
	/// <returns>A noise value between <c>0</c> and <c>1</c>.</returns>
	[Pure] public float Sample( Vector2 vec ) => Sample( vec.x, vec.y );

	/// <summary>
	/// Sample at a 3D position.
	/// </summary>
	/// <returns>A noise value between <c>0</c> and <c>1</c>.</returns>
	[Pure] public float Sample( Vector3 vec ) => Sample( vec.x, vec.y, vec.z );
}

partial class Noise
{
	/// <summary>
	/// Parameters for constructing a noise field. Use <see cref="FractalParameters"/> if you
	/// want a noise field made from multiple octaves.
	/// </summary>
	/// <param name="Seed">Seed state to initialize the field with.</param>
	/// <param name="Frequency">How quickly should samples change across space.</param>
	public record Parameters(
		int Seed = 5633,
		float Frequency = 0.01f );

	/// <summary>
	/// Parameters for constructing a <a href="https://en.wikipedia.org/wiki/Pink_noise">fractal</a>
	/// noise field, which layers multiple octaves of a noise function with increasing frequency
	/// and reducing amplitudes.
	/// </summary>
	/// <param name="Seed">Seed state to initialize the field with.</param>
	/// <param name="Frequency">How quickly should samples change across space.</param>
	/// <param name="Octaves">How many layers of noise to use.</param>
	/// <param name="Gain">How much to multiply the amplitude of each successive octave by.</param>
	/// <param name="Lacunarity">How much to multiply the frequency of each successive octave by.</param>
	public record FractalParameters(
		int Seed = 5633,
		float Frequency = 0.01f,
		int Octaves = 4,
		float Gain = 0.5f,
		float Lacunarity = 2f )
		: Parameters( Seed, Frequency );

	/// <summary>
	/// Creates a <a href="https://en.wikipedia.org/wiki/Value_noise">Value noise</a> field,
	/// effectively smoothly sampled white noise. Use a <see cref="FractalParameters"/> for the
	/// field to have multiple octaves.
	/// </summary>
	[Pure]
	public static INoiseField ValueField( Parameters parameters )
	{
		return new FastNoiseField( parameters is FractalParameters
			? FastNoise.NoiseType.ValueFractal
			: FastNoise.NoiseType.Value, parameters );
	}

	/// <summary>
	/// Creates a <a href="https://en.wikipedia.org/wiki/Perlin_noise">Perlin noise</a> field,
	/// which smoothly samples a grid of random gradients. Use a <see cref="FractalParameters"/>
	/// for the field to have multiple octaves.
	/// </summary>
	[Pure]
	public static INoiseField PerlinField( Parameters parameters )
	{
		return new FastNoiseField( parameters is FractalParameters
			? FastNoise.NoiseType.PerlinFractal
			: FastNoise.NoiseType.Perlin, parameters );
	}

	/// <summary>
	/// Creates a <a href="https://en.wikipedia.org/wiki/Simplex_noise">Simplex noise</a> field,
	/// a cheaper gradient noise function similar to <see cref="PerlinField"/>. Use a
	/// <see cref="FractalParameters"/> for the field to have multiple octaves.
	/// </summary>
	[Pure]
	public static INoiseField SimplexField( Parameters parameters )
	{
		return new FastNoiseField( parameters is FractalParameters
			? FastNoise.NoiseType.SimplexFractal
			: FastNoise.NoiseType.Simplex, parameters );
	}
}

file sealed class FastNoiseField : INoiseField
{
	private readonly FastNoise.NoiseType _type;
	private readonly Noise.Parameters _parameters;
	private readonly FastNoise _impl;

	internal FastNoiseField( FastNoise.NoiseType type, Noise.Parameters parameters )
	{
		_type = type;
		_parameters = parameters;

		_impl = new FastNoise( parameters.Seed );
		_impl.SetNoiseType( type );
		_impl.SetFrequency( parameters.Frequency );

		if ( parameters is Noise.FractalParameters fractalParameters )
		{
			_impl.SetFractalOctaves( fractalParameters.Octaves );
			_impl.SetFractalGain( fractalParameters.Gain );
			_impl.SetFractalLacunarity( fractalParameters.Lacunarity );
		}
	}

	float INoiseField.Sample( float x, float y ) => Noise.ConvertRange( _impl.GetNoise( x, y ) );
	float INoiseField.Sample( float x, float y, float z ) => Noise.ConvertRange( _impl.GetNoise( x, y, z ) );

	public override string ToString()
	{
		return $"{_type} {_parameters}";
	}
}
