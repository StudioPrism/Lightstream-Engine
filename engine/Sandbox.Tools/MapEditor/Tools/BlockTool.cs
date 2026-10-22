using Editor.MapDoc;
using Editor.MeshEditor;
using Native;

namespace Editor.MapEditor;

/// <summary>
/// Interface for the addon layer to implement, this is called from native Hammer.
/// </summary>
public interface IBlockTool
{
	public static IBlockTool Instance { get; set; }

	public PrimitiveBuilder Current { get; set; }
	public bool InProgress { get; set; }
	public string EntityOverride { get; set; }

	public Widget BuildUI();



	/// <summary>
	/// Tells the tool a parameter has changed and that we should redraw.
	/// </summary>
	public static void UpdateTool()
	{
		if ( !BlockToolGlue.ToolBlock.IsValid )
			return;

		BlockToolGlue.ToolBlock.SetPrimitiveType2D( Instance.Current.Is2D );
		BlockToolGlue.ToolBlock.OnObjectTypeChanged();
	}

	public static bool OrientPrimitives
	{
		get => BlockToolGlue.ToolBlock.GetOrientPrimitives();
		set => BlockToolGlue.ToolBlock.SetOrientPrimitives( value );
	}
}

/// <summary>
/// Methods called from native to glue the remaining native tool code to here.
/// This will become redundant as the API matures.
/// </summary>
internal static class BlockToolGlue
{
	internal static CToolBlock ToolBlock;

	internal static void BuildGeometry( MapMesh mapMesh, CToolBlockState state )
	{
		Assert.IsValid( mapMesh );

		// make the map node and construct the geometry here with our managed apis, dog food
		PrimitiveBuilder.PolygonMesh mesh = new();
		IBlockTool.Instance.Current.SetFromBox( state.GetAABBBounds() );
		IBlockTool.Instance.Current.Build( mesh );

		// Got an override material? e.g from the entity tool, apply it to all the faces
		if ( !string.IsNullOrEmpty( ToolBlock.m_OverrideMaterial ) )
		{
			mesh.Faces.ForEach( f => f.Material = ToolBlock.m_OverrideMaterial );
		}

		mapMesh.ConstructFromPolygons( mesh );

		// what the fuck is this shit
		var flags = TransformFlags.LockMaterial | TransformFlags.Rotate;
		using ( mapMesh.TransformOperation( TransformOperationMode.Object, flags ) )
		{
			mapMesh.Transform( state.GetDragWorkPlane().GetWorkPlaneToWorldTransform(), flags );
		}
	}

	internal static void SetInProgress( bool inProgress ) => IBlockTool.Instance.InProgress = inProgress;
	internal static QWidget BuildUI( CToolBlock toolBlock )
	{
		ToolBlock = toolBlock;
		return IBlockTool.Instance.BuildUI()._widget;
	}

	internal static void SetOverrideEntity( string entityName ) => IBlockTool.Instance.EntityOverride = entityName;
}
