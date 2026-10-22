using Sandbox;

class QuakeTexture( string pakDir, string fileName ) : ResourceLoader<QuakeMount>
{
	public string PakDir { get; set; } = pakDir;
	public string FileName { get; set; } = fileName;

	public BinaryReader Read()
	{
		return new BinaryReader( Host.GetFileStream( PakDir, FileName ) );
	}

	protected override object Load()
	{
		var palette = Host.GetPalette( PakDir );
		if ( palette is null ) return null;

		using var br = Read();

		var width = br.ReadInt32();
		var height = br.ReadInt32();

		var length = width * height;
		var data = br.ReadBytes( length );

		var imageData = new byte[length * 4];
		int offset = 0;

		for ( var i = 0; i < length; i++ )
		{
			var index = data[i];
			var paletteOffset = index * 3;

			imageData[offset++] = palette[paletteOffset];
			imageData[offset++] = palette[paletteOffset + 1];
			imageData[offset++] = palette[paletteOffset + 2];
			imageData[offset++] = (index == 255) ? (byte)0 : (byte)255;
		}

		return Texture.Create( width, height )
			.WithData( imageData )
			.WithMips()
			.Finish();
	}
}
