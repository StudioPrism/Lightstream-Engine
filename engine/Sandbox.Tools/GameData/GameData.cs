namespace Editor;

/// <summary>
/// Lets all native and managed tools know about any engine / game entities.
/// </summary>
public static partial class GameData
{
	internal static Native.CGameData NativeGameData;

	internal static List<MapClass> InternalEntityClasses { get; set; } = new();

	/// <summary>
	/// A list of all entity classes exposed to tools.
	/// </summary>
	public static IReadOnlyList<MapClass> EntityClasses => InternalEntityClasses;

	/// <summary>
	/// This is called one time only when tools are first initialized.
	/// The passed CGameData is kept around for the entirety of the application session and is accessible
	/// across all tools.
	///
	/// This is called just after the native engine loads whatever is left in .fgd files.
	/// We copy them to C# so we can access them from managed.
	/// </summary>
	internal static void Initialize( Native.CGameData gameData )
	{
		NativeGameData = gameData;

		CopyNativeEntities();
	}

	/// <summary>
	/// Copies the native gamedata for access from C#.
	/// </summary>
	internal static void CopyNativeEntities()
	{
		var count = NativeGameData.GetClassCount();
		for ( int i = 0; i < count; i++ )
		{
			var gdclass = NativeGameData.GetClass( i );

			if ( gdclass.GetClassType() != GameDataClassType.GenericPointClass &&
				gdclass.GetClassType() != GameDataClassType.GenericSolidClass &&
				gdclass.GetClassType() != GameDataClassType.PathClass &&
				gdclass.GetClassType() != GameDataClassType.CableClass )
				continue;

			var mapClass = MapClass.FromNative( gdclass );
			InternalEntityClasses.Add( mapClass );
		}
	}

	/// <summary>
	/// All loaded sbox.game packages for this session to load entities for tools from.
	/// </summary>
	public static Package[] LoadedPackages { get; set; } = PackageManager.ActivePackages.Where( x => x.Tags.Contains( "hammer" ) ).Select( x => x.Package ).ToArray();

	/// <summary>
	/// Loads the entity classes from a remote sbox.game game or addon into Hammer.
	/// </summary>
	public static Task LoadEntitiesFromPackage( Package package )
	{
		// TODO - onhammerclose PackageManager.RemoevTag( "hammer" )
		return PackageManager.InstallAsync( new PackageLoadOptions( package.FullIdent, "hammer" ) );
	}
}
