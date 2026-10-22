using System.Runtime.InteropServices;

public partial struct Color : IEquatable<Color>
{
	[StructLayout( LayoutKind.Sequential )]
	public struct Rgba16 : IEquatable<Rgba16>
	{
		/// <summary>
		/// The red color component
		/// </summary>
		public System.Half r;

		/// <summary>
		/// The green color component
		/// </summary>
		public System.Half g;

		/// <summary>
		/// The blue color component
		/// </summary>
		public System.Half b;

		/// <summary>
		/// The alpha/transparency color component, in range of 0 (fully transparent) to 255 (fully opaque).
		/// </summary>
		public System.Half a;

		public Rgba16( Color color )
		{
			this.r = (Half)(color.r);
			this.g = (Half)(color.g);
			this.b = (Half)(color.b);
			this.a = (Half)(color.a);
		}

		/// <summary>
		/// Convert this object to <see cref="Color"/>.
		/// </summary>
		/// <returns>The converted color struct.</returns>
		public readonly Color ToColor()
		{
			return new Color( (float)r, (float)g, (float)b, (float)a );
		}

		public static implicit operator Rgba16( Color value ) => new( value );

		public override string ToString()
		{
			return $"R:{r:0.00},G:{g:0.00},B:{b:0.00},A:{a:0.00}";
		}

		#region equality
		public static bool operator ==( Rgba16 left, Rgba16 right ) => left.Equals( right );
		public static bool operator !=( Rgba16 left, Rgba16 right ) => !(left == right);
		public override bool Equals( object obj ) => obj is Color32 color && Equals( color );
		public readonly bool Equals( Rgba16 o ) => (r, g, b, a) == (o.r, o.g, o.b, o.a);
		public readonly override int GetHashCode() => HashCode.Combine( r, g, b, a );
		#endregion
	}

}
