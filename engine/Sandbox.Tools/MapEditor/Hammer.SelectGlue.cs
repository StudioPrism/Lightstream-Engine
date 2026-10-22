using System;

namespace Editor.MapEditor;

//
// Glue for selection by assets, there's a fair bit of complexity to this that doesn't make sense
// to expose on public API, but we want this stuff for our asset browser.
//

public static partial class Hammer
{
	/// <summary>
	/// Selects all map nodes using the asset, appending them to the current selection.
	/// </summary>
	public static void SelectObjectsUsingAsset( Asset asset )
	{
		AssertAppValid();
		ArgumentNullException.ThrowIfNull( asset );
		App.SelectObjectsUsingAsset( asset.GetCompiledFile() );
	}

	/// <summary>
	/// Selects all faces using the asset, forces <see cref="Selection.SelectMode"/> to <see cref="SelectMode.Faces"/>
	/// </summary>
	public static void SelectFacesUsingMaterial( Asset asset )
	{
		AssertAppValid();
		ArgumentNullException.ThrowIfNull( asset );
		App.SelectFacesUsingMaterial( asset.GetCompiledFile() );
	}

	/// <summary>
	/// Assigns the asset to the current selection.
	/// </summary>
	public static void AssignAssetToSelection( Asset asset )
	{
		AssertAppValid();
		ArgumentNullException.ThrowIfNull( asset );
		App.AssignAssetToSelection( asset.GetCompiledFile() );
	}

	/// <summary>
	/// Opens a Entity Report dialog showing all entities using this asset.
	/// </summary>
	public static void ShowEntityReportForAsset( Asset asset )
	{
		AssertAppValid();
		ArgumentNullException.ThrowIfNull( asset );
		App.ShowEntityReportForAsset( asset.GetCompiledFile() );
	}
}
