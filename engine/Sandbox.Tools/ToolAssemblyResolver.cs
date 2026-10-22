using Mono.Cecil;
using System;
using System.IO;
using System.Reflection;
using Sandbox;

namespace Tools
{
	internal class ToolAssemblyResolver : IAssemblyResolver
	{
		private readonly DefaultAssemblyResolver _fallbackResolver = new();

		public ToolAssemblyResolver()
		{
			var rootAsm = typeof( EngineLoop ).Assembly;
			var asmDir = Path.GetDirectoryName( rootAsm.Location );

			_fallbackResolver.AddSearchDirectory( asmDir );
		}

		private Dictionary<string, AssemblyDefinition> Loaded { get; } = new( StringComparer.OrdinalIgnoreCase );

		public void Dispose()
		{
			foreach ( var (_, asmDef) in Loaded )
			{
				asmDef.Dispose();
			}

			Loaded.Clear();
		}

		private static string GetKey( Assembly asm )
		{
			return asm.GetName().Name;
		}

		public void AddAssembly( Assembly asm, TrustedBinaryStream stream )
		{
			var key = GetKey( asm );

			if ( Loaded.Remove( key, out var asmDef ) )
			{
				asmDef.Dispose();
			}

			var oldPos = stream.Position;

			try
			{
				var copy = new MemoryStream( new byte[stream.Length] );

				stream.Seek( 0, SeekOrigin.Begin );
				stream.CopyTo( copy );

				copy.Seek( 0, SeekOrigin.Begin );

				asmDef = AssemblyDefinition.ReadAssembly( copy, new ReaderParameters { AssemblyResolver = this } );
			}
			finally
			{
				stream.Seek( oldPos, SeekOrigin.Begin );
			}

			Loaded[key] = asmDef;
		}

		public void RemoveAssembly( Assembly asm )
		{
			var key = GetKey( asm );

			if ( Loaded.Remove( key, out var asmDef ) )
			{
				asmDef.Dispose();
			}
		}

		public AssemblyDefinition Resolve( AssemblyNameReference name )
		{
			return Loaded.TryGetValue( name.Name, out var asmDef ) ? asmDef : _fallbackResolver.Resolve( name );
		}

		public AssemblyDefinition Resolve( AssemblyNameReference name, ReaderParameters parameters )
		{
			return Loaded.TryGetValue( name.Name, out var asmDef ) ? asmDef : _fallbackResolver.Resolve( name, parameters );
		}
	}
}
