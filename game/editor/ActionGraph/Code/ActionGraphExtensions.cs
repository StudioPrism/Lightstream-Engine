using System.Collections.Generic;
using System.Linq;
using Facepunch.ActionGraphs;
using Sandbox;

namespace Editor.ActionGraphs;

public static class ActionGraphExtensions
{
	public static Sandbox.DisplayInfo GetDisplayInfo( this Node node )
	{
		var displayInfo = new Sandbox.DisplayInfo
		{
			Name = node.DisplayInfo.Title,
			Description = node.DisplayInfo.Description,
			Icon = node.DisplayInfo.Icon,
			Tags = node.DisplayInfo.Tags
		};

		if ( node.Definition.Identifier == "resource.ref" && node.Properties["value"].Value is Resource resource )
		{
			var dispInfo = Sandbox.DisplayInfo.For( resource );

			displayInfo.Name = resource.ResourceName;
			displayInfo.Icon = dispInfo.Icon ?? displayInfo.Icon;
			displayInfo.Description = $"References a {dispInfo.Name}.<br/><code>{resource.ResourcePath}</code>";
		}

		if ( node.Definition == EditorNodeLibrary.Input && node.ActionGraph.Title is { } title )
		{
			displayInfo.Name = title;
		}
		else if ( node.Definition == EditorNodeLibrary.InputValue
			&& node.Properties.Name.Value is string name
			&& node.ActionGraph.Inputs.TryGetValue( name, out var input )
			&& input.IsTarget )
		{
			displayInfo.Name = "This";
			displayInfo.Description = "Object this graph is currently running on.";
		}
		else if ( displayInfo.Name.LastIndexOf( '→' ) is var index and not -1
			&& node.Inputs.Values.FirstOrDefault( x => x.IsTarget )?.Link is { Source: { } output } )
		{
			if ( node.ActionGraph.TargetOutput is { } targetSource && output == targetSource )
			{
				displayInfo.Name = $"This → {displayInfo.Name[(index + 1)..].TrimStart()}";
			}
			else if ( output.Node is { Parent: { } parent } && parent == node )
			{
				displayInfo.Name = $"{output.Node.DisplayInfo.Title} → {displayInfo.Name[(index + 1)..].TrimStart()}";
			}
		}

		if ( node.Kind == NodeKind.Action && node.Binding.IsAsync )
		{
			displayInfo.Name += " \u29d7";
		}

		return displayInfo;
	}

	public static bool GetVisible( this Node node )
	{
		return node.UserData["Visible"]?.GetValue<bool>() ?? true;
	}

	public static bool IsUsed( this Node.Output output )
	{
		return output.Links.Any( x => x.Target.Node.Parent != output.Node );
	}

	public static bool IsAnyExpandedOutputVisible( this Node.Output output )
	{
		return output.GetExpandedOutputs( true ).Any( x => x != output );
	}

	public static IEnumerable<Node.Output> GetExpandedOutputs( this Node.Output output, bool visibleOnly )
	{
		if ( !visibleOnly || output.Node.GetVisible() || output.IsUsed() )
		{
			yield return output;
		}

		foreach ( var child in output.Links.Select( x => x.Target.Node ).Where( x => x.Parent == output.Node ) )
		{
			if ( child.Kind != NodeKind.Expression ) continue;

			foreach ( var childOutput in child.Outputs.Values )
			{
				foreach ( var expandedChildOutput in GetExpandedOutputs( childOutput, visibleOnly ) )
				{
					yield return expandedChildOutput;
				}
			}
		}
	}

	public static void SetVisible( this Node node, bool value )
	{
		if ( !value )
		{
			node.UserData["Visible"] = false;
		}
		else
		{
			node.UserData.Remove( "Visible" );
		}
	}

	public static bool IsOperator( this NodeDefinition nodeDef )
	{
		return nodeDef.GetType().Name == "ExpressionNodeDefinition"
			|| nodeDef.Attributes.OfType<ActionGraphOperatorAttribute>().Any();
	}

	public static string GetNextInputName( this ActionGraph graph )
	{
		const string prefix = "_in";

		for ( var i = 0; ; ++i )
		{
			if ( graph.Inputs.ContainsKey( $"{prefix}{i}" ) )
			{
				continue;
			}

			return $"{prefix}{i}";
		}
	}

	public static string GetNextOutputName( this ActionGraph graph )
	{
		const string prefix = "_out";

		for ( var i = 0; ; ++i )
		{
			if ( graph.Outputs.ContainsKey( $"{prefix}{i}" ) )
			{
				continue;
			}

			return $"{prefix}{i}";
		}
	}
}
