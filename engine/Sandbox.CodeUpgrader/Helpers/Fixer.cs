using System;

namespace Sandbox.CodeUpgrader;

/// <summary>
/// Wraps CodeFixProvider to make it less of a pain in the ass
/// </summary>
public abstract partial class Fixer : CodeFixProvider
{
	public virtual Task RunTests( IFixerTest tester ) => Task.CompletedTask;

	public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create( Target.Rule.Id );
	public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;


	Analyzer _analyzer;

	public Analyzer Target
	{
		get
		{
			return _analyzer ??= (Analyzer)Activator.CreateInstance( Analyzer );
		}
	}

	public abstract Type Analyzer { get; }

}

/// <summary>
/// Wraps CodeFixProvider to make it less of a pain in the ass
/// </summary>
public abstract partial class Fixer<T> : Fixer where T : Analyzer
{
	public override Type Analyzer => typeof( T );

}


public interface IFixerTest
{
	public Task Test( string oldcode, string fixedcode );
}
