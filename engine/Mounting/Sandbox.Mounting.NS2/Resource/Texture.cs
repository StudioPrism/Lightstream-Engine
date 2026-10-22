
class DdsTextureLoader( string fullPath ) : ResourceLoader<GameMount>
{
	protected override object Load()
	{
		return TextureLoader.FromDds( File.ReadAllBytes( fullPath ) );
	}
}
