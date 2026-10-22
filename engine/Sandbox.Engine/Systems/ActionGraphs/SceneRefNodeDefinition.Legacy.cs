
#nullable enable

using System.Linq.Expressions;
using Facepunch.ActionGraphs;
using Facepunch.ActionGraphs.Compilation;

namespace Sandbox.ActionGraphs;

partial class SceneRefNodeDefinition
{
	public PropertyDefinition LegacyTypeProperty { get; } = new( "T", typeof( Type ),
		PropertyFlags.Required, new Facepunch.ActionGraphs.DisplayInfo( "Type", "Type of object to reference." ) );
	public PropertyDefinition LegacyValueProperty { get; } = new( "value", typeof( Either<GameObject, Component> ),
		PropertyFlags.Required, new Facepunch.ActionGraphs.DisplayInfo( "Value", "Value this node will output." ) );

	public NodeBinding LegacyBinding { get; private set; } = null!;

	public void InitLegacy()
	{
		LegacyBinding = DefaultBinding with { Properties = new[] { LegacyTypeProperty, LegacyValueProperty } };
	}

	private (NodeBinding Binding, Target? Target)? BindTargetLegacy( BindingSurface surface )
	{
		if ( !surface.Properties.TryGetValue( LegacyTypeProperty.Name, out var typeObj ) || typeObj is not Type type )
		{
			return null;
		}

		var binding = DefaultBinding
			.Without( ComponentProperty, GameObjectProperty )
			.With( LegacyValueProperty with { Type = type } )
			.With( new NodeBinding.ValidationMessage( LegacyValueProperty, MessageLevel.Warning, "Legacy scene reference, please replace." ) );

		if ( !surface.Properties.TryGetValue( LegacyValueProperty.Name, out var valueObj ) || !type.IsInstanceOfType( valueObj ) )
		{
			return (binding, null);
		}

		var comp = valueObj as Component;
		var go = comp?.GameObject ?? valueObj as GameObject;

		if ( go is null )
		{
			return (binding, null);
		}

		return (binding, new Target(
			GameObjectReference.FromInstance( go ),
			comp is not null ? ComponentReference.FromInstance( comp ) : null,
			go, comp, true ));
	}

	private Expression ResolveReferenceExpressionLegacy( INodeExpressionBuilder builder, Target target )
	{
		return builder.GetPropertyValue( LegacyValueProperty );
	}
}
