using Microsoft.CodeAnalysis;
using Sandbox.Generator;

namespace Sandbox.Utility
{
	internal class CodeWriter
	{
		public int Indent { get; set; }
		public System.Text.StringBuilder sb { get; protected set; } = new System.Text.StringBuilder();
		public bool Empty => sb.Length == 0;

		public void Write( string line, bool indent = false )
		{
			if ( indent )
				sb.Append( new string( '\t', Indent ) );

			sb.Append( line );
		}

		public void WriteLine( string line = "" )
		{
			if ( !string.IsNullOrWhiteSpace( line ) ) sb.Append( new string( '\t', Indent ) );
			sb.AppendLine( line );
		}

		public void WriteLines( string text )
		{
			var lines = text.Split( '\n' );

			foreach ( var line in lines )
			{
				WriteLine( line.TrimEnd() );
			}
		}

		public void StartBlock( string line )
		{
			if ( line != null )
				WriteLine( line );

			WriteLine( "{" );

			Indent++;
		}

		public void EndBlock( string line = "" )
		{
			Indent--;
			WriteLine( $"}}{line}" );
		}

		public override string ToString() => sb.ToString();

		internal void StartClass( ISymbol key )
		{
			if ( key.ContainingType != null )
			{
				StartClass( key.ContainingType );
			}
			else
			{
				if ( !key.ContainingNamespace.IsGlobalNamespace )
				{
					StartBlock( $"namespace {key.ContainingNamespace}" );
				}
			}

			var accessibility = key.DeclaredAccessibility.ToDisplayString();
			var modifiers = $"{(key.IsStatic ? "static " : "")}{(accessibility != null ? $"{accessibility} " : "")}";

			StartBlock( $"{modifiers}partial class {key.ToDisplayString( SymbolDisplayFormat.MinimallyQualifiedFormat )}" );
		}

		internal void EndClass( ISymbol key )
		{
			EndBlock();

			if ( key.ContainingType != null )
			{
				EndClass( key.ContainingType );
			}
			else
			{
				if ( !key.ContainingNamespace.IsGlobalNamespace )
				{
					EndBlock();
				}
			}
		}
	}
}
