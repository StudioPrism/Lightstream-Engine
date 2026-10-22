using System.IO;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;

namespace Sandbox;

internal static class ReflectionExtensions
{
	/// <summary>
	/// The assembly might have a pdb embedded inside. So load it with PEReader and have a look inside to see if it
	/// is in there. Then if it is, load the assembly with the pdb.
	/// I don't know why this isn't done by default.. but apparently it's not. So we have to do it manually.
	/// </summary>
	public static Assembly LoadFromStreamWithEmbeds( this AssemblyLoadContext loadContext, Stream stream )
	{
		byte[] pdbData = null;

		//
		// Open PEReader
		//
		using ( var peReader = new PEReader( stream, PEStreamOptions.LeaveOpen ) )
		{
			var pdbEntry = peReader.ReadDebugDirectory().FirstOrDefault( x => x.Type == DebugDirectoryEntryType.EmbeddedPortablePdb );

			//
			// At some point we should probably throw a fit if it doesn't have the pdb.. but we have a few months of addons
			// that do not have the pdb embedded, and it doesn't really matter that much.
			//

			if ( pdbEntry.Type == DebugDirectoryEntryType.EmbeddedPortablePdb )
			{
				unsafe
				{
					var pdbReader = peReader.ReadEmbeddedPortablePdbDebugDirectoryData( pdbEntry ).GetMetadataReader();
					pdbData = new ReadOnlySpan<byte>( pdbReader.MetadataPointer, pdbReader.MetadataLength ).ToArray();
				}
			}
		}

		stream.Position = 0;

		//
		// We have a pdb - load using that!
		//
		if ( pdbData != null )
		{
			using var pdbStream = new System.IO.MemoryStream( pdbData );
			return loadContext.LoadFromStream( stream, pdbStream );
		}

		return loadContext.LoadFromStream( stream );
	}

	/// <summary>
	/// Try to get the event for which this member is a backing field.
	/// </summary>
	public static EventInfo GetEventInfo( this FieldInfo fieldInfo )
	{
		if ( fieldInfo.GetCustomAttribute<CompilerGeneratedAttribute>() is null )
		{
			return null;
		}

		var bFlags = BindingFlags.Public | BindingFlags.NonPublic
			| (fieldInfo.IsStatic ? BindingFlags.Static : BindingFlags.Instance);

		if ( fieldInfo.DeclaringType?.GetEvent( fieldInfo.Name, bFlags ) is not { } eventInfo )
		{
			return null;
		}

		if ( eventInfo.EventHandlerType != fieldInfo.FieldType )
		{
			return null;
		}

		return eventInfo;
	}

#nullable enable

	/// <summary>
	/// Looks through the inheritance hierarchy of <paramref name="type"/>, including its
	/// implemented interfaces, for constructed instances of <paramref name="genericTypeDef"/>.
	/// </summary>
	public static Type? GetInheritedConstructedGenericType( this Type? type, Type genericTypeDef )
	{
		if ( type is null )
		{
			return null;
		}

		Assert.True( genericTypeDef.IsGenericTypeDefinition );

		if ( genericTypeDef.IsInterface )
		{
			foreach ( var iface in type.GetInterfaces() )
			{
				if ( iface.IsConstructedGenericType && iface.GetGenericTypeDefinition() == genericTypeDef )
				{
					return iface;
				}
			}

			return null;
		}

		while ( type is not null )
		{
			if ( type.IsConstructedGenericType && type.GetGenericTypeDefinition() == genericTypeDef )
			{
				return type;
			}

			type = type.BaseType;
		}

		return null;
	}

#nullable disable
}
