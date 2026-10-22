using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sandbox;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Sandbox.Generator;

/// <summary>
/// If we're calling this from the server, throw an assertion.
/// Add a transport functions so it can be called from the server.
/// </summary>
internal static class CloudAssetProvider
{
	internal static void VisitInvocation( ref InvocationExpressionSyntax node, Location location, ImmutableArray<ISymbol> symbols, Worker worker )
	{
		if ( !worker.IsFullGeneration )
			return;

		if ( worker.Processor.PackageAssetResolver == null )
			return;

		var symbol = symbols.OfType<IMethodSymbol>().FirstOrDefault();

		if ( symbol is not IMethodSymbol methodSymbol )
			return;

		if ( !methodSymbol.HasAttribute( "CloudAssetProviderAttribute" ) )
			return;

		if ( node.ArgumentList.Arguments.Count != 1 )
		{
			// for now always expect them to have 1 arg
			worker.AddError( location, "Wrong argument count for a CloudAssetProvider" );
			return;
		}

		var argString = node.ArgumentList.Arguments.First();
		if ( argString.Expression is not LiteralExpressionSyntax ex )
		{
			worker.AddError( location, "Must use a string literal for a CloudAssetProvider" );
			return;
		}

		var packageIdent = ex.Token.ValueText;
		var path = worker.Processor.PackageAssetResolver( packageIdent );

		if ( path == null )
		{
			worker.AddError( location, $"Could not resolve package asset {packageIdent}" );
			return;
		}

		if ( packageIdent.Contains( '\n' ) || packageIdent.Contains( '"' ) ) return;
		if ( path.Contains( '\n' ) || path.Contains( '"' ) ) return;

		worker.AddedCode += $"[assembly:Sandbox.Cloud.Asset( \"{packageIdent}\", \"{path}\" )]\n";

	}
}
