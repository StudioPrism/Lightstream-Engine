using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO.Compression;

namespace Facepunch.ShaderProc
{

	class ShaderCode
	{
		List<ShaderToken> tokens;
		public string ShaderName;
		//ShaderDoc Documentation;

		string[] reservedKeywords = { "#", "[", "]", "{", "}" }; // ex: #define, [branch],

		public ShaderCode( string shaderName, string source )
		{
			ShaderName = shaderName;

			//Documentation = ShaderDoc.GenerateDocumentation( source );

			tokens = new List<ShaderToken>();

			foreach ( var split in source.Split( '\n' ) )
			{
				tokens.Add( new ShaderToken( split ) );
			}
		}

		public ShaderCode Minify()
		{
			Obsfucate();
			RemoveComments();
			RemoveWhitespaces();
			//RemoveLines();
			return this;
		}

		public void Obsfucate()
		{
			// Todo
		}

		public void RemoveComments()
		{
			//Simple comments
			foreach ( var token in tokens )
			{
				token.RemoveEverythingAfter( "//" );
			}

			//Multline comments
			for ( int i = 0; i < tokens.Count; i++ )
			{
				var t = tokens[i];

				int indexStartComment = t.Token.IndexOf( "/*" );

				while ( indexStartComment >= 0 )
				{
					//Check if any is in the same line
					if ( t.Token.Substring( indexStartComment ).Contains( "*/" ) )
					{
						tokens[i].RemoveEverythingBetween( "/*", "*/" );
						indexStartComment = t.Token.IndexOf( "/*" );
						continue;
					}
					else
					{
						t.RemoveEverythingAfter( "/*" ); //Doesn't close on the same line, remove everything

						// And iterate until we find the closing statement
						int j = i;
						while ( j < tokens.Count - 1 )
						{
							j++;
							var tj = tokens[j];
							if ( tj.Token.Contains( "*/" ) )
							{
								// delete everything before '*/'
								tj.RemoveEverythingBefore( "*/" );

								//Remove any extra lines between /* and */
								tokens.RemoveRange( i + 1, j - i - 1 );
								break;
							}
						}
					}
					indexStartComment = t.Token.IndexOf( "/*" );
				}
			}
		}

		public void RemoveLines()
		{
			for ( int i = 1; i < tokens.Count - 1; i++ )
			{
				var t = tokens[i];
				var n = tokens[i - 1];
				//Todo: doesn't work well
				bool remove = false;
				//Don't remove lines from these tokens
				foreach ( var keyword in reservedKeywords )
				{
					if ( t.Token.StartsWith( keyword ) )
						continue;
					if ( n.Token.StartsWith( keyword ) )
						continue;

					remove = true;
				}

				if ( remove )
				{
					t.Token = t.Token + n.Token;
					tokens.RemoveAt( i + 1 );
				}
			}
		}

		public void RemoveWhitespaces()
		{
			foreach ( var t in tokens )
			{
				//Remove all tab and identation first
				t.Token = t.Token.TrimStart().TrimEnd();
			}
		}

		public override string ToString()
		{
			string output = "";
			foreach ( var token in tokens )
			{
				output += token.ToString() + "\n";
			}
			//Console.WriteLine( output );
			return output;
		}
	}
}
