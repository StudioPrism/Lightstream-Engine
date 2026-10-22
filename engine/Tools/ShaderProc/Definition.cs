using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Facepunch.ShaderProc
{
	public class Definition
	{

		public System.IO.DirectoryInfo Root { get; private set; }
		public string Filename { get; private set; }
		public string FullText { get; private set; }

		//Actual data
		public string ShaderDir { get; private set; }
		public string InteropOutput { get; private set; }
		public List<string> Whitelist = new List<string>();
		public List<string> DocumentList = new List<string>();

		public static Definition FromFile( string filename )
		{
			var folder = System.IO.Path.GetDirectoryName( filename );
			if ( string.IsNullOrEmpty( folder ) ) folder = ".";

			var text = System.IO.File.ReadAllText( filename );

			var d = new Definition();
			d.Filename = System.IO.Path.GetFileName( filename );
			d.Root = new System.IO.DirectoryInfo( folder );

			d.ParseFrom( text );
			return d;
		}

		private void ParseFrom( string text )
		{
			foreach ( var split in text.Split( '\n' ) )
			{
				var arg = split.Split( ' ' );

				//Whiteline
				if ( arg.Length < 2 )
					continue;

				if ( arg.Length > 2 )
					throw new Exception( $"Undefined number of arguments for definition: {split}" );

				switch ( arg[0] )
				{
					case "shaderdir":
						ShaderDir = ParseExtractString( arg[1] );
						break;
					case "output":
						InteropOutput = ParseExtractString( arg[1] );
						break;
					case "whitelist":
						Whitelist.Add( ParseExtractString( arg[1] ) );
						break;
					case "document":
						DocumentList.Add( ParseExtractString( arg[1] ) );
						break;
					default:
						throw new Exception( $"Unknown argument ${arg[0]}" );
				}

			}
		}

		private string ParseExtractString( string text )
		{
			if ( text[0] == '\"' )
			{
				int i = 1;
				while ( 1 < text.Length && text[i] != '\"' )
					i++;

				if ( text[i] != '\"' )
					throw new Exception( $"Malformed argument {text}" );

				return text.Substring( 1, i - 1 );
			}
			return "";
		}

	}
}
