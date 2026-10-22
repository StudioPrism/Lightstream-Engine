using System;
using System.Diagnostics.CodeAnalysis;

namespace Editor.ActionGraphs;

#nullable enable

/// <summary>
/// Node creation menu item for reroute / no-op nodes.
/// </summary>
public class RerouteNodeType : LibraryNodeType
{
	public RerouteNodeType()
		: base( EditorNodeLibrary.NoOperation )
	{

	}

	public override bool TryGetInput( Type valueType, [NotNullWhen( true )] out string? name )
	{
		name = "in";
		return true;
	}

	public override bool TryGetOutput( Type valueType, [NotNullWhen( true )] out string? name )
	{
		name = "out";
		return true;
	}
}
