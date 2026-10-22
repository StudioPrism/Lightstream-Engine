using Editor.MapDoc;

namespace Editor.MapEditor;

/// <summary>
/// A list of selected map node's assets that only gets updated when your selection changes.
/// </summary>
internal class SelectedAssets
{
	static List<Asset> Assets = new();
	static bool Dirty = true;

	public static IEnumerable<Asset> All
	{
		get
		{
			UpdateList();
			return Assets;
		}
	}

	static SelectedAssets()
	{
		Selection.OnChanged += () =>
		{
			Dirty = true;
		};
	}

	static void EvaluateMapNode( MapNode node, List<Asset> assets )
	{
		if ( node is MapMesh mesh )
		{
			assets.AddRange( mesh.GetFaceMaterialAssets() );
		}

		//
		// This is how Valve were doing it, we might be able to do it better with our managed types
		//
		if ( node is MapEntity ent )
		{
			var assetName = ent.GetKeyValue( "model" ) ?? ent.GetKeyValue( "particle" );
			if ( !string.IsNullOrEmpty( assetName ) )
			{
				var asset = AssetSystem.FindByPath( assetName );
				if ( asset != null )
					Assets.Add( asset );
			}
		}

		// Iterate on the children
		foreach ( var child in node.Children )
		{
			EvaluateMapNode( child, assets );
		}
	}

	static void UpdateList()
	{
		if ( !Dirty )
			return;

		Assets.Clear();

		foreach ( var select in Selection.All )
		{
			EvaluateMapNode( select, Assets );
		}

		Assets = Assets.Distinct().ToList();

		Dirty = false;
	}
}
