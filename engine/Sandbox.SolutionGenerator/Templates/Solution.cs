using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Xml;

namespace Sandbox.SolutionGenerator;

/// <summary>
/// Writes a .slnx file for the project and its references
/// </summary>
internal sealed class Solution
{
	private readonly List<string> _rootProjects = new();
	private readonly Dictionary<string, List<string>> _folderProjects = new( StringComparer.OrdinalIgnoreCase );

	public List<string> Platforms { get; } = new() { "Any CPU" };

	public void AddProject( string projectPath, string folder )
	{
		if ( string.IsNullOrWhiteSpace( projectPath ) )
			return;

		if ( string.IsNullOrWhiteSpace( folder ) )
		{
			_rootProjects.Add( projectPath );
			return;
		}

		// Take only first segment, ignore deeper nesting
		var firstSegment = folder
			.Split( ['/', '\\'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries )
			.FirstOrDefault();

		if ( string.IsNullOrEmpty( firstSegment ) )
		{
			_rootProjects.Add( projectPath );
			return;
		}

		var normalizedFolder = "/" + firstSegment + "/"; // required leading & trailing slash for .slnx folders

		if ( !_folderProjects.TryGetValue( normalizedFolder, out var list ) )
		{
			list = new List<string>();
			_folderProjects[normalizedFolder] = list;
		}

		list.Add( projectPath );
	}

	public string Generate()
	{
		var sb = new StringBuilder();

		try
		{
			var platforms = Platforms
				.Distinct()
				.OrderBy( p => p, StringComparer.OrdinalIgnoreCase )
				.ToList();

			using ( var writer = XmlWriter.Create( sb, new XmlWriterSettings
			{
				Encoding = new UTF8Encoding( false ),
				Indent = true,
				NewLineChars = "\n",
				OmitXmlDeclaration = true
			} ) )
			{
				writer.WriteStartElement( "Solution" );
				writer.WriteStartElement( "Configurations" );

				foreach ( var plat in platforms )
				{
					writer.WriteStartElement( "Platform" );
					writer.WriteAttributeString( "Name", plat );
					writer.WriteEndElement();
				}
				writer.WriteEndElement();

				foreach ( var p in _rootProjects.OrderBy( x => x, StringComparer.OrdinalIgnoreCase ) )
					WriteProject( writer, p );

				foreach ( var folder in _folderProjects.Keys.OrderBy( k => k, StringComparer.OrdinalIgnoreCase ) )
				{
					writer.WriteStartElement( "Folder" );
					writer.WriteAttributeString( "Name", folder );
					foreach ( var proj in _folderProjects[folder].OrderBy( x => x, StringComparer.OrdinalIgnoreCase ) )
						WriteProject( writer, proj );
					writer.WriteEndElement();
				}

				writer.WriteEndElement();
				writer.Flush();
			}
		}
		catch ( Exception ex )
		{
			return $"<Solution><!-- Generation failed: {ex.Message} --></Solution>";
		}

		if ( sb.Length == 0 )
			return "<Solution />";

		return sb.ToString();
	}

	private static void WriteProject( XmlWriter writer, string path )
	{
		writer.WriteStartElement( "Project" );
		writer.WriteAttributeString( "Path", path.Replace( '\\', '/' ) );
		writer.WriteEndElement();
	}
}
