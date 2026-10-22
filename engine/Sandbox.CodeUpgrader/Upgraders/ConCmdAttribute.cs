namespace Sandbox.CodeUpgrader;

[DiagnosticAnalyzer( LanguageNames.CSharp )]
public partial class ConCmdAnalyzer : Analyzer
{
	public override DiagnosticDescriptor Rule => Diagnostics.ConCmdAttribute;

	public override void Init( AnalysisContext context )
	{
		context.RegisterSyntaxNodeAction( AnalyzeMethod, SyntaxKind.MethodDeclaration );
	}

	void AnalyzeMethod( SyntaxNodeAnalysisContext context )
	{
		var methodDeclaration = (MethodDeclarationSyntax)context.Node;

		bool hasConCmdAttribute = methodDeclaration.AttributeLists
			.SelectMany( attrList => attrList.Attributes )
			.Any( attr => attr.Name.ToString() == "Sandbox.ConCmd" || attr.Name.ToString() == "ConCmd" );

		if ( !hasConCmdAttribute ) return;
		if ( methodDeclaration.Modifiers.Any( SyntaxKind.StaticKeyword ) ) return;

		// Report diagnostic on the attribute name instead of method identifier
		var attribute = methodDeclaration.AttributeLists
			.SelectMany( attrList => attrList.Attributes )
			.FirstOrDefault( attr => attr.Name.ToString() == "Sandbox.ConCmd" || attr.Name.ToString() == "ConCmd" );

		if ( attribute is not null )
		{
			var diagnostic = Diagnostic.Create(
				Rule,
				attribute.GetLocation(),
				methodDeclaration.Identifier.Text
			);
			context.ReportDiagnostic( diagnostic );
		}
	}

	public override async Task RunTests( IAnalyzerTest tester )
	{
		await tester.TestWithMarkup( """
            using Sandbox;
            public class MyClass
            {
                [[|ConCmd|]]
                public void MyMethod() { }
            }
            """ );
	}
}

[ExportCodeFixProvider( LanguageNames.CSharp ), Shared]
public class ConCmdAttributeFix : Fixer<ConCmdAnalyzer>
{
	public override async Task RegisterCodeFixesAsync( CodeFixContext context )
	{
		var root = await context.Document.GetSyntaxRootAsync( context.CancellationToken ).ConfigureAwait( false );

		var diagnostic = context.Diagnostics.First();
		var diagnosticSpan = diagnostic.Location.SourceSpan;

		var methodNode = root.FindToken( diagnosticSpan.Start ).Parent.AncestorsAndSelf()
			.OfType<MethodDeclarationSyntax>()
			.FirstOrDefault();

		if ( methodNode == null ) return;

		var action = CodeAction.Create(
			title: "Add static modifier",
			createChangedDocument: c => AddStaticModifierAsync( context.Document, methodNode ),
			equivalenceKey: "AddStatic",
			priority: CodeActionPriority.High );

		// Register a code action that will invoke the fix
		context.RegisterCodeFix( action, diagnostic );
	}

	private async Task<Document> AddStaticModifierAsync( Document document, MethodDeclarationSyntax node )
	{
		var staticToken = SyntaxFactory.Token( SyntaxKind.StaticKeyword ).WithTrailingTrivia( SyntaxFactory.Space );
		var newModifiers = node.Modifiers.Add( staticToken );

		var newNode = node.WithModifiers( newModifiers );

		var root = await document.GetSyntaxRootAsync( default );
		var newRoot = root.ReplaceNode( node, newNode );

		return document.WithSyntaxRoot( newRoot );
	}

	public override async Task RunTests( IFixerTest tester )
	{
		await tester.Test( """
            using Sandbox;
            public class MyClass
            {
                [[|ConCmd|]]
                public void MyConCmd() { }
            }
            """,
			"""
            using Sandbox;
            public class MyClass
            {
                [[|ConCmd|]]
                public static void MyConCmd() { }
            }
            """ );
	}
}
