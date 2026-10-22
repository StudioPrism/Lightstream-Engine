

using Mono.Cecil;
using System;
using System.Threading.Tasks;

namespace Sandbox.Internal;

//
// Not currently used anywhere right now, but we may need it in the future.
//
public class AssemblyModifier
{
	public string Rename { get; set; }
	public Dictionary<string, string> ChangeReference { get; set; } = new();


	CustomResolver resolver = new CustomResolver();

	public void AddResolvable( byte[] assembly )
	{
		//AssemblyDefinition.ReadAssembly( assembly );
	}

	public byte[] Modify( byte[] dllBytes )
	{
		using var stream = new MemoryStream( dllBytes );

		AssemblyDefinition ad = AssemblyDefinition.ReadAssembly( stream, new ReaderParameters { AssemblyResolver = resolver, InMemory = true, ReadingMode = ReadingMode.Immediate } );

		if ( Rename != null )
		{
			ad.Name = new AssemblyNameDefinition( Rename, ad.Name.Version );
		}

		foreach ( var cr in ChangeReference )
		{
			var found = ad.MainModule.AssemblyReferences.FirstOrDefault( x => x.Name == cr.Key );
			if ( found == null ) continue;

			found.Name = cr.Value;
		}

		using var outStream = new MemoryStream();
		ad.Write( outStream, new WriterParameters { } );

		return outStream.ToArray();
	}


	class CustomResolver : DefaultAssemblyResolver
	{
		readonly Dictionary<string, AssemblyDefinition> cache = new();

		public void Cache( AssemblyDefinition assembly )
		{
			cache[assembly.FullName] = assembly;
		}

		public override AssemblyDefinition Resolve( AssemblyNameReference name )
		{
			if ( cache.TryGetValue( name.FullName, out var cached ) )
				return cached;

			AssemblyDefinition assembly;

			try
			{
				//
				// Only resolve shit that we already have loaded
				//
				var loaded = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault( x => x.GetName().Name == name.Name && !string.IsNullOrEmpty( x.Location ) );
				if ( loaded != null )
				{
					assembly = AssemblyDefinition.ReadAssembly( loaded.Location );
					if ( assembly != null )
					{
						Cache( assembly );
						return assembly;
					}
				}
			}
			catch ( AssemblyResolutionException )
			{
			}

			return null;
		}
	}
}
