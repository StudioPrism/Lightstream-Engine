using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Sandbox.Engine.Shaders;

class ShaderPreprocessor
{
	ShaderPreprocessorOptions Options { get; set; }
	List<string> IncludedFiles = new();

	public ShaderPreprocessor( ShaderPreprocessorOptions options )
	{
		Options = options;
	}

	bool HandleInclude( string includeFile, string parentFile, out string includeContents )
	{
		includeContents = string.Empty;

		var path = $"{Path.GetDirectoryName( parentFile ).Replace( '\\', '/' )}/{includeFile}";

		// Try searching relative
		if ( !ShaderCompile.FileSystem.FileExists( path ) )
		{
			path = $"shaders/{includeFile}";
		}

		// Maybe it's in base, otherwise give up
		if ( !ShaderCompile.FileSystem.FileExists( path ) )
		{
			return false;
		}

		bool coreContent = EngineFileSystem.CoreContent.FileExists( path );
		if ( coreContent && Options.IgnoreCoreIncludes )
		{
			return false;
		}

		// Already included, handle it with an empty string out
		if ( IncludedFiles.Contains( path ) )
		{
			return true;
		}

		// Open the file somehow
		string source = ShaderCompile.FileSystem.ReadAllText( path );
		includeContents = Preprocess( source, ShaderCompile.FileSystem.GetFullPath( path ), path );

		IncludedFiles.Add( path );

		return true;
	}

	Regex IncludePattern = new Regex( "#include \"([^>]+)\"", RegexOptions.Compiled );

	public string Preprocess( string source, string absolutePath, string relativePath )
	{
		var reader = new StringReader( source );
		var builder = new StringBuilder();

		// Add a header to the source
		builder.AppendLine( $"#line 1 \"{relativePath.Replace( "\\", "/" )}\"" );
		builder.AppendLine( "#include \"system.fxc\"" );

		string line;
		while ( (line = reader.ReadLine()) != null )
		{
			var match = IncludePattern.Match( line );
			if ( !Options.ExpandIncludes || !match.Success )
			{
				builder = builder.AppendLine( line );
				continue;
			}

			var file = match.Groups[1].Value;

			if ( HandleInclude( file, relativePath, out var includeContents ) )
			{
				builder.AppendLine( includeContents );
				continue;
			}

			builder = builder.AppendLine( line );
		}

		return builder.ToString();
	}
}
