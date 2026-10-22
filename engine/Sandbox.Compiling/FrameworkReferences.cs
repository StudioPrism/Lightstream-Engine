using Microsoft.CodeAnalysis;
using System.IO;
using System.Reflection;

namespace Sandbox;

/// <summary>
/// Loads the framework assemblies from the bin/ref folder and makes 
/// them available globally to every compiler.
/// </summary>
[SkipHotload]
static class FrameworkReferences
{
	public static CaseInsensitiveDictionary<PortableExecutableReference> All { get; } = new();

	static FrameworkReferences()
	{
		var assembly = Assembly.GetExecutingAssembly();
		var resourceNames = assembly.GetManifestResourceNames()
									.Where( name => name.EndsWith( ".dll", StringComparison.OrdinalIgnoreCase ) )
									.ToArray();

		var referenceFiles = new List<string>();

		foreach ( var resourceName in resourceNames )
		{
			using ( var stream = assembly.GetManifestResourceStream( resourceName ) )
			{
				var meta = MetadataReference.CreateFromStream( stream, default, default, resourceName );
				All[resourceName] = meta;
			}
		}
	}

	static Assembly FindLoadedAssembly( string name )
	{
		var loadedAssembly = AppDomain.CurrentDomain.GetAssemblies()
			.FirstOrDefault( assembly => string.Compare( assembly.GetName().Name, name, StringComparison.OrdinalIgnoreCase ) == 0 );
		if ( loadedAssembly != null )
			return loadedAssembly;

		try
		{
			// .NET lazy loads assemblies so we might need to load it now...
			return Assembly.Load( name );
		}
		catch
		{
			return null;
		}
	}

	/// <summary>
	/// Find a framework reference by its assembly name
	/// </summary>
	public static PortableExecutableReference FindByName( string name )
	{
		if ( string.IsNullOrWhiteSpace( name ) )
			throw new ArgumentException( $"cannot be null or empty", nameof( name ) );

		//
		// Find a ref assembly
		//
		if ( All.TryGetValue( $"{name}.dll", out var frameworkReference ) )
		{
			return frameworkReference;
		}

		//
		// Find the assembly in our list of loaded assemblies
		// We should really only do this for things like Sandbox.* ?
		//
		var assembly = FindLoadedAssembly( name );

		if ( assembly == null )
		{
			throw new System.Exception( $"Couldn't find {name}.dll" );
		}

		if ( string.IsNullOrEmpty( assembly.Location ) )
		{
			throw new System.Exception( $"Found assembly {name}.dll ({assembly}) - but can't find PortableExecutableReference" );
		}


		return MetadataReference.CreateFromFile( assembly.Location );
	}

	private static List<string> LoadEmbeddedReferenceAssemblies()
	{
		var assembly = Assembly.GetExecutingAssembly();
		var resourceNames = assembly.GetManifestResourceNames()
			.Where( name => name.EndsWith( ".dll", StringComparison.OrdinalIgnoreCase ) )
			.ToArray();

		var tempDirectory = Path.Combine( Path.GetTempPath(), "EmbeddedRefs" );
		Directory.CreateDirectory( tempDirectory );

		var referenceFiles = new List<string>();

		foreach ( var resourceName in resourceNames )
		{
			var outputPath = Path.Combine( tempDirectory, resourceName );
			using ( var resourceStream = assembly.GetManifestResourceStream( resourceName ) )
			using ( var fileStream = new FileStream( outputPath, FileMode.Create, FileAccess.Write ) )
			{
				resourceStream.CopyTo( fileStream );
			}
			referenceFiles.Add( outputPath );
		}

		return referenceFiles;
	}
}
