using Facepunch.ActionGraphs;
using Sandbox.Upgraders;

namespace Sandbox.Internal;

[UpgraderGroup( typeof( ReferenceTypeUpgraderGroup ) ), AttemptBefore( typeof( DelegateUpgrader ) )]
internal class ActionGraphImplementedDelegateUpgrader : Hotload.InstanceUpgrader
{
	public override bool ShouldProcessType( Type type )
	{
		return typeof( Delegate ).IsAssignableFrom( type );
	}

	protected override bool OnTryCreateNewInstance( object oldInstance, out object newInstance )
	{
		var oldDelegate = (Delegate)oldInstance;

		if ( oldDelegate.GetActionGraphInstance() is not { } oldDeleg )
		{
			newInstance = null;
			return false;
		}

		newInstance = GetNewInstance( oldDeleg ).Delegate;
		return true;
	}

	protected override bool OnTryUpgradeInstance( object oldInstance, object newInstance, bool createdElsewhere )
	{
		if ( createdElsewhere )
		{
			return false;
		}

		AddCachedInstance( oldInstance, newInstance );

		return true;
	}
}

[UpgraderGroup( typeof( ReferenceTypeUpgraderGroup ) ), AttemptBefore( typeof( DelegateUpgrader ) )]
internal class ActionGraphDelegateUpgrader : Hotload.InstanceUpgrader
{
	public override bool ShouldProcessType( Type type )
	{
		return typeof( IActionGraphDelegate ).IsAssignableFrom( type );
	}

	protected override bool OnTryCreateNewInstance( object oldInstance, out object newInstance )
	{
		var oldDelegate = (IActionGraphDelegate)oldInstance;

		var newGraph = GetNewInstance( oldDelegate.Graph );
		var newDefaults = GetNewInstance( oldDelegate.Defaults );
		var newDelegateType = GetNewType( oldDelegate.DelegateType );

		var newDelegate = newGraph.CreateDelegate( newDelegateType, newDefaults );

		newInstance = newDelegate;

		return true;
	}

	protected override bool OnTryUpgradeInstance( object oldInstance, object newInstance, bool createdElsewhere )
	{
		if ( createdElsewhere )
		{
			return false;
		}

		AddCachedInstance( oldInstance, newInstance );

		return true;
	}
}

[UpgraderGroup( typeof( ReferenceTypeUpgraderGroup ) )]
internal class ActionGraphUpgrader : Hotload.InstanceUpgrader
{
	public override bool ShouldProcessType( Type type )
	{
		return type == typeof( ActionGraph );
	}

	protected override bool OnTryCreateNewInstance( object oldInstance, out object newInstance )
	{
		newInstance = oldInstance;
		return true;
	}

	protected override bool OnTryUpgradeInstance( object oldInstance, object newInstance, bool createdElsewhere )
	{
		AddCachedInstance( oldInstance, newInstance );
		ScheduleProcessInstance( oldInstance, newInstance );

		return true;
	}

	protected override int OnProcessInstance( object oldInstance, object newInstance )
	{
		if ( !ReferenceEquals( oldInstance, newInstance ) )
		{
			throw new ArgumentException();
		}

		var graph = (ActionGraph)oldInstance;

		var inputs = graph.Inputs.Values.Select( GetNewInstance ).ToArray();
		var outputs = graph.Outputs.Values.Select( GetNewInstance ).ToArray();

		graph.SetParameters( inputs, outputs );

		foreach ( var variable in graph.Variables.Values )
		{
			variable.Type = GetNewType( variable.Type );
			variable.DefaultValue = GetNewInstance( variable.DefaultValue );
		}

		foreach ( var node in graph.Nodes.Values )
		{
			foreach ( var property in node.Properties.Values )
			{
				property.Value = GetNewInstance( property.Value );
			}

			node.MarkDirty();
		}

		graph.ClearChanges();

		return 1;
	}
}
