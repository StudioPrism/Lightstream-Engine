using Mono.Cecil.Cil;
using Microsoft.CodeAnalysis.Text;
using System;

namespace Sandbox;

internal partial class AssemblyAccess
{
	[ThreadStatic]
	protected static AccessControl.CodeLocation Location;

	private void UpdateLocation( SequencePoint sequencePoint )
	{
		if ( sequencePoint == null )
			return;

		if ( sequencePoint.IsHidden )
		{
			Location = new AccessControl.CodeLocation( $"{sequencePoint.Document.Url}:{sequencePoint.StartLine}" );
			return;
		}

		// Roslyn is zero based, Mono.Cecil is one based
		var start = new LinePosition( sequencePoint.StartLine - 1, sequencePoint.StartColumn - 1 );
		var end = new LinePosition( sequencePoint.EndLine - 1, sequencePoint.EndColumn - 1 );

		var location = Microsoft.CodeAnalysis.Location.Create(
			sequencePoint.Document.Url,
			textSpan: default,
			lineSpan: new LinePositionSpan( start, end )
		);

		Location = new AccessControl.CodeLocation( location );
	}
}
