namespace Sandbox.CodeUpgrader;

[DiagnosticAnalyzer( LanguageNames.CSharp )]
public partial class ConVarAnalyzer : Analyzer
{
	public override DiagnosticDescriptor Rule => Diagnostics.ConVarAttribute;

	public override void Init( AnalysisContext context )
	{
		context.RegisterSyntaxNodeAction( AnalyzeProperty, SyntaxKind.PropertyDeclaration );
	}

	void AnalyzeProperty( SyntaxNodeAnalysisContext context )
	{
		var propertyDeclaration = (PropertyDeclarationSyntax)context.Node;

		bool hasConCmdAttribute = propertyDeclaration.AttributeLists
			.SelectMany( attrList => attrList.Attributes )
			.Any( attr => attr.Name.ToString() == "Sandbox.ConVar" || attr.Name.ToString() == "ConVar" );

		if ( !hasConCmdAttribute ) return;
		if ( propertyDeclaration.Modifiers.Any( SyntaxKind.StaticKeyword ) ) return;

		// Report diagnostic on the attribute name instead of property identifier
		var attribute = propertyDeclaration.AttributeLists
			.SelectMany( attrList => attrList.Attributes )
			.FirstOrDefault( attr => attr.Name.ToString() == "Sandbox.ConVar" || attr.Name.ToString() == "ConVar" );

		if ( attribute is not null )
		{
			var diagnostic = Diagnostic.Create(
				Rule,
				attribute.GetLocation(),
				propertyDeclaration.Identifier.Text
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
                [[|ConVar|]]
                public bool MyConVar { get; set; }
            }
            """ );
	}
}

[ExportCodeFixProvider( LanguageNames.CSharp ), Shared]
public class ConVarAttributeFix : Fixer<ConVarAnalyzer>
{
	public override async Task RegisterCodeFixesAsync( CodeFixContext context )
	{
		var root = await context.Document.GetSyntaxRootAsync( context.CancellationToken ).ConfigureAwait( false );

		// Locate the diagnostic in the source code
		var diagnostic = context.Diagnostics.First();
		var diagnosticSpan = diagnostic.Location.SourceSpan;

		// Find the property syntax node
		var propertyNode = root.FindToken( diagnosticSpan.Start ).Parent.AncestorsAndSelf()
			.OfType<PropertyDeclarationSyntax>()
			.FirstOrDefault();

		if ( propertyNode == null ) return;

		var action = CodeAction.Create(
			title: "Add static modifier",
			createChangedDocument: c => AddStaticModifierAsync( context.Document, propertyNode ),
			equivalenceKey: "AddStatic",
			priority: CodeActionPriority.High );

		// Register a code action that will invoke the fix
		context.RegisterCodeFix( action, diagnostic );
	}

	private async Task<Document> AddStaticModifierAsync( Document document, PropertyDeclarationSyntax propertyNode )
	{
		// Add the static modifier
		var staticToken = SyntaxFactory.Token( SyntaxKind.StaticKeyword ).WithTrailingTrivia( SyntaxFactory.Space );
		var newModifiers = propertyNode.Modifiers.Add( staticToken );

		var newPropertyNode = propertyNode.WithModifiers( newModifiers );

		// Replace the old property with the new one
		var root = await document.GetSyntaxRootAsync( default );
		var newRoot = root.ReplaceNode( propertyNode, newPropertyNode );

		// Return the updated document
		return document.WithSyntaxRoot( newRoot );
	}

	public override async Task RunTests( IFixerTest tester )
	{
		await tester.Test( """
            using Sandbox;
            public class MyClass
            {
                [[|ConVar|]]
                public bool MyConVar { get; set; }
            }
            """,
			"""
            using Sandbox;
            public class MyClass
            {
                [[|ConVar|]]
                public static bool MyConVar { get; set; }
            }
            """ );

		await tester.Test( """
            using Sandbox;
            public class MyClass
            {
                [[|ConVar|]]
                public int MyOtherConVar { get; set; }
            }
            """,
			"""
            using Sandbox;
            public class MyClass
            {
                [[|ConVar|]]
                public static int MyOtherConVar { get; set; }
            }
            """ );
	}
}
