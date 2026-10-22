using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Sandbox.Generator;

internal static class ComponentSubscriberInterfaces
{
	internal static Dictionary<string, string> Map = new()
	{
		{ "OnUpdate", "Sandbox.Internal.IUpdateSubscriber" },
		{ "OnFixedUpdate", "Sandbox.Internal.IFixedUpdateSubscriber" },
		{ "OnPreRender", "Sandbox.Internal.IPreRenderSubscriber" }
	};

	/// <summary>
	/// Find anything implementing callback methods, and add an interface to them.
	/// </summary>
	internal static void VisitMethod( MethodDeclarationSyntax node, IMethodSymbol symbol, Worker master )
	{
		if ( !symbol.ContainingType.DerivesFrom( "global::Sandbox.Component" ) )
			return;

		if ( symbol.IsVirtual )
			return;

		if ( !Map.ContainsKey( symbol.Name ) )
			return;

		// Must be implemented
		if ( (node.Body == null && node.ExpressionBody == null) || symbol.IsAbstract )
			return;

		//Console.WriteLine( $"AddBaseTypeToCurrentClass: {symbol.Name}" );

		master.AddBaseTypeToCurrentClass( Map[symbol.Name] );
	}
}
