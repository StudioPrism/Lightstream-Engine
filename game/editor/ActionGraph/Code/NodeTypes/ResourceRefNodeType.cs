using System.Threading.Tasks;
using Facepunch.ActionGraphs;

namespace Editor.ActionGraphs;

#nullable enable

/// <summary>
/// Node creation menu entry representing a <see cref="Sandbox.Resource"/> reference.
/// </summary>
public class ResourceRefNodeType : LibraryNodeType
{
	public DragAssetData AssetData { get; }

	public ResourceRefNodeType( DragAssetData assetData )
		: base( EditorNodeLibrary.Get( "resource.ref" )! )
	{
		AssetData = assetData;
	}

	protected override EditorNode OnCreateEditorNode( EditorActionGraph editorGraph, Node node )
	{
		var editorNode = base.OnCreateEditorNode( editorGraph, node );

		_ = InitNodeAsync( node, editorNode );

		return editorNode;
	}

	private async Task InitNodeAsync( Node node, EditorNode editorNode )
	{
		AssetData.ProgressChanged += progress =>
		{
			editorNode.OverrideTitle = $"Downloading - {progress * 100:N0}%";
			editorNode.MarkDirty();
		};

		var asset = await AssetData.GetAssetAsync();

		editorNode.OverrideTitle = null;

		var resource = asset?.LoadResource();

		node.Properties["T"].Value = resource?.GetType();
		node.Properties["value"].Value = resource;

		if ( AssetData.PackageIdent is { } packageIdent )
		{
			node.Properties["package"].Value = packageIdent;
		}

		node.UpdateParameters();

		editorNode.MarkDirty();
	}
}
