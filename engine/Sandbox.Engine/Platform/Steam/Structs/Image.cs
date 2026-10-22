
namespace Steamworks.Data
{
	internal struct Image
	{
		internal uint Width;
		internal uint Height;
		internal byte[] Data;

		internal Color GetPixel( int x, int y )
		{
			if ( x < 0 || x >= Width ) throw new System.Exception( "x out of bounds" );
			if ( y < 0 || y >= Height ) throw new System.Exception( "y out of bounds" );

			Color c = new Color();

			var i = (y * Width + x) * 4;

			c.r = Data[i + 0];
			c.g = Data[i + 1];
			c.b = Data[i + 2];
			c.a = Data[i + 3];

			return c;
		}

		public override string ToString()
		{
			return $"{Width}x{Height} ({Data.Length}bytes)";
		}
	}

	internal struct Color
	{
		internal byte r, g, b, a;
	}
}
