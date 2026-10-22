namespace Editor.MapEditor;

[Dock( "Hammer", "Cloud Browser", "cloud_download" )]
internal class HammerCloudBrowser : CloudAssetBrowser
{
	public static HammerCloudBrowser Instance { get; private set; }

	public HammerCloudBrowser( Widget parent ) : base( parent, null )
	{
		Instance = this;

		OnPackageHighlight = async ( p ) =>
		{
			if ( p.TypeName == "material" )
			{
				var asset = await AssetSystem.InstallAsync( p.FullIdent );
				Hammer.SetCurrentMaterial( asset );
			}
		};
	}
}
