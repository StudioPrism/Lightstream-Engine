using System.Collections.Generic;
using Sandbox;
using Sandbox.ActionGraphs;

namespace Editor.ActionGraphs;

#nullable enable

/// <summary>
/// Node creation menu entry for custom nodes implemented as action graph resources.
/// </summary>
public class GraphNodeType : LibraryNodeType
{
	[Event( GetGlobalNodeTypesEvent.EventName )]
	public new static void OnGetGlobalNodeTypes( GetGlobalNodeTypesEvent ev )
	{
		foreach ( var resource in ResourceLibrary.GetAll<ActionGraphResource>() )
		{
			if ( resource.Graph == null )
			{
				continue;
			}

			ev.Output.Add( new GraphNodeType( resource ) );
		}
	}

	public ActionGraphResource Resource { get; }

	public GraphNodeType( ActionGraphResource resource )
		: base( EditorNodeLibrary.Graph, null, new Dictionary<string, object?> { { "graph", resource.ResourcePath } } )
	{
		Resource = resource;
	}
}
