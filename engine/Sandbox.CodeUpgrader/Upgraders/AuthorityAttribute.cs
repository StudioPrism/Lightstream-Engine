namespace Sandbox.CodeUpgrader;

[DiagnosticAnalyzer( LanguageNames.CSharp )]
public partial class AuthorityAttributeAnalyzer : Analyzer
{
	public override DiagnosticDescriptor Rule => Diagnostics.AuthorityAttribute;

	public override void Init( AnalysisContext context )
	{
		context.RegisterSyntaxNodeAction( AnalyzeNode, SyntaxKind.Attribute );
	}

	void AnalyzeNode( SyntaxNodeAnalysisContext context )
	{
		var attributeSyntax = (AttributeSyntax)context.Node;

		// Check if the attribute name is "Broadcast"
		if ( attributeSyntax.Name.ToString() == "Authority" || attributeSyntax.Name.ToString() == "Sandbox.Authority" )
		{
			var diagnostic = Diagnostic.Create( Rule, attributeSyntax.GetLocation() );
			context.ReportDiagnostic( diagnostic );
		}
	}

	public override async Task RunTests( IAnalyzerTest tester )
	{
		await tester.TestWithMarkup( """
				using Sandbox;

				public class MyClass
				{
					[[|Authority|]]
					public void RunIt(){}
				}
				""" );

		await tester.TestWithMarkup( """
				using Sandbox;

				public class MyClass
				{
					[[|Sandbox.Authority|]]
					public void RunIt(){}
				}
				""" );

		await tester.TestWithMarkup( """
				using Sandbox;

				public class MyClass
				{
					[[|Authority( NetPermission.OwnerOnly )|]]
					public void RunIt(){}
				}
				""" );

		await tester.TestWithMarkup( """
				using Sandbox;

				public class MyClass
				{
					[Rpc.Owner]
					public void RunIt(){}
				}
				""" );
	}
}



[ExportCodeFixProvider( LanguageNames.CSharp ), Shared]
public class AuthorityAttributeFix : Fixer<AuthorityAttributeAnalyzer>
{
	public override async Task RegisterCodeFixesAsync( CodeFixContext context )
	{
		var root = await context.Document.GetSyntaxRootAsync( context.CancellationToken ).ConfigureAwait( false );

		// Locate the diagnostic in the source code
		var diagnostic = context.Diagnostics.First();
		var diagnosticSpan = diagnostic.Location.SourceSpan;

		// Find the attribute syntax node
		var attributeNode = root.FindToken( diagnosticSpan.Start ).Parent.AncestorsAndSelf()
			.OfType<AttributeSyntax>()
			.FirstOrDefault();

		if ( attributeNode == null ) return;

		var action = CodeAction.Create(
				title: "Upgrade To [Rpc.Authority]",
				createChangedDocument: c => ChangeAttributeAsync( context.Document, attributeNode ),
				equivalenceKey: "ChangeAuthorityToRpc",
				priority: CodeActionPriority.High );

		// Register a code action that will invoke the fix
		context.RegisterCodeFix( action, diagnostic );
	}

	private async Task<Document> ChangeAttributeAsync( Document document, AttributeSyntax attributeNode )
	{
		// Create the new attribute syntax
		var rpcAttribute = SyntaxFactory.Attribute( SyntaxFactory.ParseName( "Rpc.Owner" ) );

		// If we have arguments, convert them to flags
		if ( attributeNode.ArgumentList != null )
		{
			string flags = "";

			foreach ( var a in attributeNode.ArgumentList.Arguments )
			{
				if ( a.ToFullString().Contains( "NetPermission.HostOnly" ) ) flags = "NetFlags.HostOnly";
				if ( a.ToFullString().Contains( "NetPermission.OwnerOnly" ) ) flags = "NetFlags.OwnerOnly";
			}

			if ( flags is not null )
			{
				var flagList = SyntaxFactory.ParseAttributeArgumentList( $"( {flags} )" );
				rpcAttribute = rpcAttribute.WithArgumentList( flagList );
			}
		}

		// Replace the old attribute with the new one
		var root = await document.GetSyntaxRootAsync( default );
		var newRoot = root.ReplaceNode( attributeNode, rpcAttribute );

		// Return the updated document
		return document.WithSyntaxRoot( newRoot );
	}

	public override async Task RunTests( IFixerTest tester )
	{
		await tester.Test( """
				using Sandbox;

				public class MyClass
				{
					[[|Authority|]]
					public void RunIt(){}
				}
				""",
				"""
				using Sandbox;

				public class MyClass
				{
					[[|Rpc.Owner|]]
					public void RunIt(){}
				}
				""" );

		await tester.Test( """
				using Sandbox;

				public class MyClass
				{
					[[|Authority( Permission = NetPermission.OwnerOnly )|]]
					public void RunIt(){}
				}
				""",
				"""
				using Sandbox;

				public class MyClass
				{
					[[|Rpc.Owner( NetFlags.OwnerOnly )|]]
					public void RunIt(){}
				}
				""" );

		await tester.Test( """
				using Sandbox;

				public class MyClass
				{
					[[|Authority( Permission = NetPermission.HostOnly )|]]
					public void RunIt(){}
				}
				""",
				"""
				using Sandbox;

				public class MyClass
				{
					[[|Rpc.Owner( NetFlags.HostOnly )|]]
					public void RunIt(){}
				}
				""" );
	}
}
