#nullable enable

namespace Sandbox.ActionGraphs;

internal static class ResourceNodes
{
	/// <summary>
	/// A sound resource.
	/// </summary>
	[Obsolete( ConstantNodes.ObsoleteMessage ), ActionGraphNode( "const.sound" ), Pure, Category( "Resource" ), Title( "Sound File" ), Icon( "volume_up" )]
	public static SoundFile SoundFile( [Facepunch.ActionGraphs.Property] SoundFile value )
	{
		return value;
	}

	/// <summary>
	/// A sound event. It can play a set of random sounds with optionally random settings such as volume and pitch.
	/// </summary>
	[Obsolete( ConstantNodes.ObsoleteMessage ), ActionGraphNode( "const.soundevent" ), Pure, Category( "Resource" ), Title( "Sound Event" ), Icon( "volume_up" )]
	public static SoundEvent SoundEvent( [Facepunch.ActionGraphs.Property] SoundEvent value )
	{
		return value;
	}

	/// <summary>
	/// A model.
	/// </summary>
	[Obsolete( ConstantNodes.ObsoleteMessage ), ActionGraphNode( "const.model" ), Pure, Category( "Resource" ), Title( "Model" ), Icon( "view_in_ar" )]
	public static Model Model( [Facepunch.ActionGraphs.Property] Model value )
	{
		return value;
	}

	/// <summary>
	/// A material. Uses several Textures and a Shader with specific settings for more interesting visual effects.
	/// </summary>
	[Obsolete( ConstantNodes.ObsoleteMessage ), ActionGraphNode( "const.material" ), Pure, Category( "Resource" ), Title( "Material" ), Icon( "image" )]
	public static Material Material( [Facepunch.ActionGraphs.Property] Material value )
	{
		return value;
	}

	/// <summary>
	/// A prefab.
	/// </summary>
	[Obsolete( ConstantNodes.ObsoleteMessage ), ActionGraphNode( "const.prefab" ), Pure, Category( "Resource" ), Title( "Prefab" ), Icon( "ballot" )]
	public static PrefabFile Prefab( [Facepunch.ActionGraphs.Property] PrefabFile value )
	{
		return value;
	}

	/// <summary>
	/// An asset defined in C# and created through tools.
	/// </summary>
	[Obsolete( ConstantNodes.ObsoleteMessage ), ActionGraphNode( "const.resource" ), Pure, Category( "Resource" ), Title( "Game Resource" ), Icon( "hexagon" )]
	public static T GameResource<T>( [Facepunch.ActionGraphs.Property] T value )
		where T : GameResource
	{
		return value;
	}


	[ActionGraphNode( "resource.ref" ), Pure, Hide, Title( "{value|Resource Reference}" ), Category( "Resources" ), Description( "References a resource." ), Icon( "perm_media" )]
	public static T Reference<T>( [ActionGraphProperty] T value, [ActionGraphProperty] string? package = null )
		where T : Resource
	{
		return value;
	}
}
