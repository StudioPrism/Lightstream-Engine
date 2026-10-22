using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Sandbox;

/// <summary>
/// Describes a gradient between multiple colors
/// </summary>
public struct Gradient
{
	/// <summary>
	/// The blend mode
	/// </summary>
	[JsonPropertyName( "blend" )]
	public BlendMode Blending { readonly get; set; }

	/// <summary>
	/// A list of color stops, which should be ordered by time
	/// </summary>
	[JsonPropertyName( "color" )]
	public ImmutableList<ColorFrame> Colors { readonly get; set; }

	/// <summary>
	/// A list of color stops, which should be ordered by time
	/// </summary>
	[JsonPropertyName( "alpha" )]
	public ImmutableList<AlphaFrame> Alphas { readonly get; set; }

	public Gradient( params ColorFrame[] frames )
	{
		Colors = ImmutableList<ColorFrame>.Empty.AddRange( frames );
	}

	public Gradient()
	{
		Colors = ImmutableList<ColorFrame>.Empty;
		Alphas = ImmutableList<AlphaFrame>.Empty;
	}

	/// <summary>
	/// A single float creates a flat color
	/// </summary>
	static public implicit operator Gradient( Color value )
	{
		var c = new Gradient();
		c.AddColor( 0.5f, value );
		return c;
	}

	/// <summary>
	/// Make a copy of this with changed keyframes
	/// </summary>
	public readonly Gradient WithFrames( ImmutableList<ColorFrame> frames )
	{
		var c = this;
		c.Colors = frames;
		return c;
	}

	/// <summary>
	/// Keyframes times and values should range between 0 and 1
	/// </summary>
	public struct ColorFrame
	{
		[JsonPropertyName( "t" )]
		public float Time { readonly get; set; } = 0.0f;

		[JsonPropertyName( "c" )]
		public Color Value { readonly get; set; } = Color.White;

		public ColorFrame( float timedelta, Color color )
		{
			Time = timedelta;
			Value = color;
		}
	}

	/// <summary>
	/// Keyframes times and values should range between 0 and 1
	/// </summary>
	public struct AlphaFrame
	{
		[JsonPropertyName( "t" )]
		public float Time { readonly get; set; } = 0.0f;

		[JsonPropertyName( "a" )]
		public float Value { readonly get; set; } = 1.0f;

		public AlphaFrame( float timedelta, float alpha )
		{
			Time = timedelta;
			Value = alpha;
		}
	}

	public ColorFrame this[int index]
	{
		get => Colors[index];
		set
		{
			Colors = Colors.SetItem( index, value );
		}
	}

	/// <summary>
	/// Add a color position
	/// </summary>
	public int AddColor( float x, in Color color ) => AddColor( new ColorFrame( Math.Clamp( x, 0, 1.0f ), color ) );

	/// <summary>
	/// Add an alpha position
	/// </summary>
	public int AddAlpha( float x, float alpha ) => AddAlpha( new AlphaFrame( Math.Clamp( x, 0, 1.0f ), alpha ) );

	/// <summary>
	/// If the lists aren't in time order for some reason, this will fix them. This should really 
	/// just be called when serializing, and in every other situation we should assume they're
	/// okay.
	/// </summary>
	public void FixOrder()
	{
		if ( !IsOrderedIncorrectly() )
			return;

		if ( Colors is not null )
			Colors = Colors.Sort( ( x, y ) => x.Time.CompareTo( y.Time ) );

		if ( Alphas is not null )
			Alphas = Alphas.Sort( ( x, y ) => x.Time.CompareTo( y.Time ) );

	}

	/// <summary>
	/// Returns true if the lists are not in time order
	/// </summary>
	private readonly bool IsOrderedIncorrectly()
	{
		if ( Colors is not null )
		{
			float time = float.MinValue;
			foreach ( var f in Colors )
			{
				if ( f.Time < time ) return true;
				time = f.Time;
			}
		}

		if ( Alphas is not null )
		{
			float time = float.MinValue;
			foreach ( var f in Alphas )
			{
				if ( f.Time < time ) return true;
				time = f.Time;
			}
		}

		return false;
	}

	/// <summary>
	/// Add given keyframe to this curve.
	/// </summary>
	/// <param name="keyframe">The keyframe to add.</param>
	/// <returns>The position of newly added keyframe in the <see cref="Colors"/> list.</returns>
	public int AddColor( in ColorFrame keyframe )
	{
		Colors ??= ImmutableList<ColorFrame>.Empty;

		for ( int i = 0; i < Colors.Count; i++ )
		{
			if ( Colors[i].Time > keyframe.Time )
			{
				Colors = Colors.Insert( i, keyframe );
				return i;
			}
		}

		Colors = Colors.Add( keyframe );

		return Colors.Count - 1;
	}

	public int AddAlpha( in AlphaFrame keyframe )
	{
		Alphas ??= ImmutableList<AlphaFrame>.Empty;

		for ( int i = 0; i < Alphas.Count; i++ )
		{
			if ( Alphas[i].Time > keyframe.Time )
			{
				Alphas = Alphas.Insert( i, keyframe );
				return i;
			}
		}

		Alphas = Alphas.Add( keyframe );

		return Alphas.Count - 1;
	}

	/// <summary>
	/// Given a time, get the keyframes on either side of it, along with the delta of where we are between
	/// </summary>
	readonly (ColorFrame previous, ColorFrame next, float delta) GetSurroundingColors( float time )
	{
		var length = Colors?.Count ?? 0;
		if ( length == 0 ) return (default, default, 0);

		var firstFrame = Colors[0];
		if ( length == 1 || time <= firstFrame.Time ) return (firstFrame, firstFrame, 0);

		var lastFrame = Colors[length - 1];

		// handle looping here? time = time % lastFrame.Time;

		if ( time > lastFrame.Time ) // clamp to end
		{
			return (lastFrame, lastFrame, 1);
		}

		int prev = 0;
		for ( int i = 0; i < length; i++ )
		{
			if ( Colors[i].Time < time )
			{
				prev = i;
				continue;
			}

			var delta = time.LerpInverse( Colors[prev].Time, Colors[i].Time );
			if ( float.IsNaN( delta ) ) delta = 1f;

			return (Colors[prev], Colors[i], delta);
		}

		// should never happen but okay
		return (lastFrame, lastFrame, 1);
	}

	/// <summary>
	/// Given a time, get the keyframes on either side of it, along with the delta of where we are between
	/// </summary>
	readonly (AlphaFrame previous, AlphaFrame next, float delta) GetSurroundingAlphas( float time )
	{
		var length = Alphas?.Count ?? 0;

		if ( length == 0 ) return (new AlphaFrame( 0, 1.0f ), new AlphaFrame( 0, 1.0f ), 0);

		var firstFrame = Alphas[0];
		if ( length == 1 || time <= firstFrame.Time ) return (firstFrame, firstFrame, 0);

		var lastFrame = Alphas[length - 1];

		// handle looping here? time = time % lastFrame.Time;

		if ( time > lastFrame.Time ) // clamp to end
		{
			return (lastFrame, lastFrame, 1);
		}

		int prev = 0;
		for ( int i = 0; i < length; i++ )
		{
			if ( Alphas[i].Time < time )
			{
				prev = i;
				continue;
			}

			var delta = time.LerpInverse( Alphas[prev].Time, Alphas[i].Time );
			if ( float.IsNaN( delta ) ) delta = 1f;

			return (Alphas[prev], Alphas[i], delta);
		}

		// should never happen but okay
		return (lastFrame, lastFrame, 1);
	}

	/// <summary>
	/// Describes how the line should behave when entering/leaving a frame
	/// </summary>
	public enum BlendMode
	{
		/// <summary>
		/// Linear interoplation between
		/// </summary>
		[Icon( "show_chart" )]
		Linear,

		/// <summary>
		/// No interpolation use last raw value
		/// </summary>
		[Icon( "turn_sharp_right" )]
		Stepped,

	}

	/// <summary>
	/// Evaluate the blend using the time, which is generally between 0 and 1
	/// </summary>
	[MethodImpl( MethodImplOptions.AggressiveInlining )]
	public readonly Color Evaluate( float time )
	{
		if ( Colors?.Count == 0 && Alphas?.Count == 0 ) return Color.White;
		if ( Colors?.Count == 1 && Alphas?.Count == 0 ) return Colors[0].Value;
		if ( Colors?.Count == 1 && Alphas?.Count == 1 ) return Colors[0].Value.WithAlphaMultiplied( Alphas[0].Value );
		if ( Colors?.Count == 0 && Alphas?.Count == 1 ) return Color.White.WithAlphaMultiplied( Alphas[0].Value );

		var col = GetSurroundingColors( time );
		var alp = GetSurroundingAlphas( time );

		if ( Blending == BlendMode.Linear )
		{
			var alpha = MathX.Lerp( alp.previous.Value, alp.next.Value, alp.delta, true );
			return Color.Lerp( col.previous.Value, col.next.Value, col.delta, true ).WithAlphaMultiplied( alpha );
		}

		// BlendMode.Stepped
		var stepAlpha = alp.next.Time <= time ? alp.next.Value : alp.previous.Value;
		if ( col.next.Time <= time ) return col.next.Value.WithAlphaMultiplied( stepAlpha );
		return col.previous.Value.WithAlphaMultiplied( stepAlpha );
	}

	/// <summary>
	/// Create a gradient from colors spaced out evenly
	/// </summary>
	public static Gradient FromColors( params Color[] colors )
	{
		var g = new Gradient();
		if ( colors.Length == 0 ) return g;
		if ( colors.Length == 1 ) return colors[0];

		var step = 1.0f / (colors.Length - 1);
		for ( int i = 0; i < colors.Length; i++ )
		{
			g.AddColor( i * step, colors[i] );
		}

		return g;
	}
}
