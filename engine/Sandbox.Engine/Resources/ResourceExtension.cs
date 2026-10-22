using Sandbox;
using Sandbox.Internal;

namespace Sandbox;

/// <summary>
/// Describes a resource extension at a base level
/// </summary>
internal interface IResourceExtension
{
	bool IsTargetting( Resource r );
	bool IsDefault();

	/// <summary>
	/// Find all resources that are targetting a specific resource
	/// </summary>
	public static Resource[] FindAllTargetting( Resource r )
	{
		return ResourceLibrary.GetAll<IResourceExtension>()
							.Where( x => x.IsTargetting( r ) )
							.OfType<Resource>()
							.ToArray();
	}

	/// <summary>
	/// Find all extensions that target a specific type.
	/// </summary>
	public static TypeDescription[] FindAllTypes( TypeLibrary library, Type targetType )
	{
		var t = library.GetGenericTypes( typeof( ResourceExtension<> ), new[] { targetType } );
		return t?.ToArray() ?? Array.Empty<TypeDescription>();
	}
}

/// <summary>
/// A GameResource type that adds extended properties to another resource type. You should prefer to use
/// the type with to generic arguments, and define your own type as the second argument. That way you get
/// access to the helper methods.
/// </summary>
public abstract class ResourceExtension<T> : GameResource, IResourceExtension where T : Resource
{
	/// <summary>
	/// If true then this is returned when calling FindForResourceOrDefault if
	/// no other extension is found targetting a specific resource.
	/// </summary>
	[Feature( "Extends", Icon = "playlist_add" )]
	[Title( "Default" )]
	public bool ExtensionDefault { get; set; }

	/// <summary>
	/// Extensions can target more than one resource.
	/// </summary>
	[Feature( "Extends" )]
	public List<T> ExtensionTargets { get; set; }

	bool IResourceExtension.IsTargetting( Resource r )
	{
		if ( ExtensionTargets is null ) return false;
		return ExtensionTargets.Contains( r );
	}

	bool IResourceExtension.IsDefault()
	{
		return ExtensionDefault;
	}
}


/// <summary>
/// An extension of ResourceExtension[t], this gives special helper methods for retrieving resources targetting
/// specific assets.
/// </summary>
public class ResourceExtension<T, TSelf> : ResourceExtension<T> where T : Resource where TSelf : ResourceExtension<T, TSelf>
{
	public static TSelf FindForResource( Resource r )
	{
		return FindAllForResource( r ).FirstOrDefault();
	}

	public static TSelf FindForResourceOrDefault( Resource r )
	{
		var t = FindForResource( r );
		if ( t is not null ) return t;

		return FindDefault();
	}

	public static IEnumerable<TSelf> FindAllForResource( Resource r )
	{
		return ResourceLibrary.GetAll<TSelf>()
							.Where( x => ((IResourceExtension)x).IsTargetting( r ) );
	}

	public static TSelf FindDefault()
	{
		return ResourceLibrary.GetAll<TSelf>()
							.FirstOrDefault( x => ((IResourceExtension)x).IsDefault() );
	}

}
