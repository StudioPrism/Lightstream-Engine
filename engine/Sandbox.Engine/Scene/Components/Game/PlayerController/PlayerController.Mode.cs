using Sandbox.Movement;
namespace Sandbox;


public sealed partial class PlayerController : Component
{
	public MoveMode Mode { get; private set; }

	void ChooseBestMoveMode()
	{
		var best = GetComponents<MoveMode>( false ).MaxBy( x => x.Score( this ) );
		if ( Mode == best ) return;

		Mode?.OnModeEnd( best );

		Mode = best;

		if ( Body?.PhysicsBody is { } body )
		{
			body.Sleeping = false;
		}

		Mode?.OnModeBegin();
	}
}
