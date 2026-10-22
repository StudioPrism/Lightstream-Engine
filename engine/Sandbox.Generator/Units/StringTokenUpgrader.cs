using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Linq;

namespace Sandbox.Generator;

static class StringTokenUpgrader
{
	static bool IsValidTarget( IParameterSymbol symbol )
	{
		// If this parameter a StringToken?
		if ( symbol.Type.Name == "StringToken" ) return true;

		// Does this parameter have a tag?
		foreach ( var attr in symbol.GetAttributes() )
		{
			if ( "global::Sandbox.StringToken.ConvertAttribute" == attr.AttributeClass.FullName() )
				return true;
		}

		return false;
	}

	internal static void VisitInvocation( ref InvocationExpressionSyntax node, Location location, ImmutableArray<ISymbol> symbols, Worker worker )
	{
		ISymbol symbol = symbols.OfType<IMethodSymbol>().FirstOrDefault();

		if ( symbol is null )
			return;

		if ( symbol is not IMethodSymbol methodSymbol )
			return;

		if ( node.ArgumentList.Arguments.Count == 0 ) return;

		bool changes = false;
		ArgumentSyntax[] newArgs = new ArgumentSyntax[node.ArgumentList.Arguments.Count];

		for ( int i = 0; i < node.ArgumentList.Arguments.Count; i++ )
		{
			newArgs[i] = node.ArgumentList.Arguments[i];
			if ( i >= methodSymbol.Parameters.Length ) continue;

			var param = methodSymbol.Parameters[i];

			if ( newArgs[i].Expression is not LiteralExpressionSyntax stringLiteral || stringLiteral.Kind() != SyntaxKind.StringLiteralExpression )
				continue;

			if ( !IsValidTarget( param ) ) continue;

			var text = stringLiteral.Token.ValueText;
			var hash = text.MurmurHash2( true );

			//Console.WriteLine( $"{text}=>{hash}" );

			var newArg = SyntaxFactory.Argument(
				 SyntaxFactory.InvocationExpression(
					 SyntaxFactory.ParseExpression( "global::Sandbox.StringToken.Literal" ) )
				 .WithArgumentList(
					 SyntaxFactory.ArgumentList(
						 SyntaxFactory.SeparatedList<ArgumentSyntax>( new[]
						 {
							 SyntaxFactory.Argument( SyntaxFactory.LiteralExpression( SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal( text ) ) ),
							 SyntaxFactory.Argument( SyntaxFactory.LiteralExpression( SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal( hash ) ) )
						 }
							 ) ) ) );

			newArgs[i] = newArg;

			//worker.AddedCode += $"[assembly:Sandbox.StringToken( \"{stringLiteral}\" )]\n";
			changes = true;
		}

		if ( changes )
		{
			node = node.WithArgumentList( node.ArgumentList.WithArguments( SyntaxFactory.SeparatedList( newArgs ) ) );
		}
	}
}
