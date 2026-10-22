namespace Sandbox.CodeUpgrader;

[DiagnosticAnalyzer( LanguageNames.CSharp )]
public partial class GpuBufferAnalyzer : Analyzer
{
	public override DiagnosticDescriptor Rule => Diagnostics.GpuBuffer;

	public override void Init( AnalysisContext context )
	{
		context.RegisterSyntaxNodeAction( AnalyzeNode, SyntaxKind.GenericName );
	}

	void AnalyzeNode( SyntaxNodeAnalysisContext context )
	{
		if ( context.Node is not GenericNameSyntax genericName )
			return;

		if ( genericName.Identifier.Text == "ComputeBuffer" )
		{
			var diagnostic = Diagnostic.Create( Rule, genericName.GetLocation() );
			context.ReportDiagnostic( diagnostic );
		}
	}

	public override async Task RunTests( IAnalyzerTest tester )
	{
		await tester.TestWithMarkup( """
				using Sandbox;

				public class MyClass
				{
					[|ComputeBuffer<float>|] MyBuffer;
				}
				""" );

		await tester.TestWithMarkup( """
				using Sandbox;

				public class MyClass
				{
					[|ComputeBuffer<float>|] MyBuffer { get; set; }
				}
				""" );

		await tester.TestWithMarkup( """
				using Sandbox;

				public class MyClass
				{
					public void DoSomething()
					{
						var buffer = new [|ComputeBuffer<float>|]( 4 );
					}
				}
				""" );

		await tester.TestWithMarkup( """
				using Sandbox;
				
				public class MyClass
				{
					public void DoSomething()
					{
						[|ComputeBuffer<float>|] buffer = new( 8 );
					}
				}
				""" );
	}
}

[ExportCodeFixProvider( LanguageNames.CSharp ), Shared]
public class GpuBufferFix : Fixer<GpuBufferAnalyzer>
{
	public override async Task RegisterCodeFixesAsync( CodeFixContext context )
	{
		var root = await context.Document.GetSyntaxRootAsync( context.CancellationToken ).ConfigureAwait( false );

		// Locate the diagnostic in the source code
		var diagnostic = context.Diagnostics.First();
		var diagnosticSpan = diagnostic.Location.SourceSpan;

		// Find the attribute syntax node
		var genericNameNode = root.FindToken( diagnosticSpan.Start ).Parent.AncestorsAndSelf()
			.OfType<GenericNameSyntax>()
			.FirstOrDefault();

		if ( genericNameNode == null ) return;

		var action = CodeAction.Create(
				title: "Upgrade To GpuBuffer",
				createChangedDocument: c => ChangeNameAsync( context.Document, genericNameNode ),
				equivalenceKey: "ChangeComputeBufferToGpuBuffer",
				priority: CodeActionPriority.High );

		// Register a code action that will invoke the fix
		context.RegisterCodeFix( action, diagnostic );
	}

	private async Task<Document> ChangeNameAsync( Document document, GenericNameSyntax genericNameNode )
	{
		var gpubufferName = genericNameNode.WithIdentifier( SyntaxFactory.Identifier( "GpuBuffer" ) );

		// Replace the old attribute with the new one
		var root = await document.GetSyntaxRootAsync( default );
		var newRoot = root.ReplaceNode( genericNameNode, gpubufferName );

		// Return the updated document
		return document.WithSyntaxRoot( newRoot );
	}

	public override async Task RunTests( IFixerTest tester )
	{
		await tester.Test( """
				using Sandbox;

				public class MyClass
				{
				    [|ComputeBuffer<float>|] MyBuffer { get; set; }
				}
				""",
				"""
				using Sandbox;

				public class MyClass
				{
				    [|GpuBuffer<float>|] MyBuffer { get; set; }
				}
				""" );
	}
}
