#nullable enable

using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;
using Facepunch.ActionGraphs;
using Facepunch.ActionGraphs.Compilation;

namespace Sandbox.ActionGraphs;

using DisplayInfo = Facepunch.ActionGraphs.DisplayInfo;

[NodeDefinition]
internal class RunSceneEventNodeDefinition : NodeDefinition
{
	public const string Ident = "scene.run";

	public DisplayInfo DefaultDisplay { get; } =
		new DisplayInfo( "Run Event", "Runs an event on all components implementing an interface.", "Scene", Icon: "bolt", Hidden: true );

	public override DisplayInfo DisplayInfo => DefaultDisplay;

	public PropertyDefinition InterfaceProperty { get; } = new( ParameterNames.Type, typeof( Type ), PropertyFlags.Required,
		new DisplayInfo( "Interface Type", "Scene event interface type.", Hidden: true ) );

	public PropertyDefinition MethodNameProperty { get; } = new( ParameterNames.Name, typeof( string ), PropertyFlags.Required,
		new DisplayInfo( "Method Name", "Scene event method name to invoke.", Hidden: true ) );

	public InputDefinition InputSignal { get; } = InputDefinition.PrimarySignal();

	/// <summary>
	/// Optional target <see cref="GameObject"/> to post this event to.
	/// Only components within this target and its descendants will receive the event.
	/// If not provided, the whole <see cref="Scene"/> will be used.
	/// </summary>
	public InputDefinition TargetInput { get; }
	public OutputDefinition OutputSignal { get; } = OutputDefinition.PrimarySignal();

	public NodeBinding DefaultBinding { get; }

	public RunSceneEventNodeDefinition( NodeLibrary nodeLibrary )
		: base( nodeLibrary, Ident )
	{
		TargetInput = new InputDefinition( "target", typeof( GameObject ), 0,
			DisplayInfo.FromAttributes( typeof( RunSceneEventNodeDefinition ).GetProperty( nameof( TargetInput ) )! ) with
			{
				Title = "Target"
			} );

		DefaultBinding = NodeBinding.Create( DefaultDisplay,
			new[] { InterfaceProperty, MethodNameProperty },
			new[] { InputSignal, TargetInput },
			new[] { OutputSignal } );
	}

	private static IReadOnlyList<MethodInfo> GetEventMethods( Type type )
	{
		if ( !type.IsInterface || !type.GetInterfaces().Any( x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof( ISceneEvent<> ) ) )
		{
			return Array.Empty<MethodInfo>();
		}

		return type.GetMethods( BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly );
	}

	protected override NodeBinding OnBind( BindingSurface surface )
	{
		var binding = DefaultBinding;

		surface = surface with
		{
			Properties = surface.Properties.With( ParameterNames.IsStatic, false ),
			InputTypes = surface.InputTypes.Without( TargetInput.Name )
		};

		if ( !surface.Properties.TryGetValue( InterfaceProperty.Name, out var typeObj ) || typeObj is not Type type )
		{
			return binding;
		}

		if ( GetEventMethods( type ) is not { Count: > 0 } )
		{
			return binding with
			{
				Messages = new[]
				{
					new NodeBinding.ValidationMessage( InterfaceProperty, MessageLevel.Error,
						$"Expected an interface implementing {nameof(ISceneEvent<object>)} with at least one method." )
				}
			};
		}

		var callBinding = NodeLibrary.CallMethod.Bind( surface );
		var properties = callBinding.Properties
			.With( InterfaceProperty, MethodNameProperty )
			.Without( callBinding.Properties.First( x => x.Name == ParameterNames.IsStatic ) );

		binding = callBinding with
		{
			DisplayInfo = callBinding.DisplayInfo with { Title = $"Run {callBinding.DisplayInfo.Title} Event" },
			Inputs = new[] { InputSignal, TargetInput }.With( callBinding.Inputs.Where( x => !x.IsTarget ).ToArray() ),
			Properties = properties
		};

		return binding;
	}

	private static void RunEvent<T>( Action<T> action )
	{
		if ( !Game.ActiveScene.IsValid() ) return;

		Game.ActiveScene.RunEvent( action );
	}

	private static void PostEvent<T>( Action<T> action, GameObject target )
	{
		if ( !target.IsValid() ) return;

		foreach ( var c in target.Components.GetAll<T>() )
		{
			try
			{
				action( c );
			}
			catch ( Exception e )
			{
				Log.Warning( e, e.Message );
			}
		}
	}

	protected override Expression OnBuildExpression( INodeExpressionBuilder builder )
	{
		var node = builder.Node;
		var innerMethod = builder.GetBindingTarget<MethodInfo>();
		var binder = ((MethodCallNodeDefinition)NodeLibrary.CallMethod).GetBinder( innerMethod );
		var targetType = innerMethod.DeclaringType!;
		var lambdaType = typeof( Action<> ).MakeGenericType( targetType );
		var hasTarget = node.Inputs[TargetInput.Name].IsLinked;

		var runMethod = typeof( RunSceneEventNodeDefinition )
			.GetMethod( hasTarget ? nameof( PostEvent ) : nameof( RunEvent ), BindingFlags.Static | BindingFlags.NonPublic )!
			.MakeGenericMethod( targetType );

		var targetParameter = Expression.Parameter( targetType, "x" );
		var innerBuilder = new InnerExpressionBuilder( builder, targetParameter );
		var innerCall = binder.BuildExpression( innerBuilder );
		var innerLambda = Expression.Lambda( lambdaType, innerCall, targetParameter );

		return hasTarget
			? Expression.Call( runMethod, innerLambda, builder.GetInputValue( TargetInput ) )
			: Expression.Call( runMethod, innerLambda );
	}

	private class InnerExpressionBuilder : INodeExpressionBuilder
	{
		public Node Node => _outer.Node;
		public NodeBinding Binding { get; }

		private readonly INodeExpressionBuilder _outer;
		private readonly ParameterExpression _targetParameter;

		public InnerExpressionBuilder( INodeExpressionBuilder outer, ParameterExpression targetParameter )
		{
			_outer = outer;
			_targetParameter = targetParameter;

			Binding = outer.Binding with
			{
				Inputs = outer.Binding.Inputs.With( InputDefinition.Target( targetParameter.Type ) )
			};
		}

		public ParameterExpression CreateLocal( Type type, string? name = null ) => throw new NotImplementedException();

		public Expression GetVariableValue( Variable variable ) => _outer.GetVariableValue( variable );
		public Expression GetPropertyValue( Node.Property property ) => _outer.GetPropertyValue( property );

		public Expression GetInputValue( Node.Input input ) => input.IsTarget
			? _targetParameter
			: _outer.GetInputValue( input );

		public LambdaExpression GetInputValueFunc( Node.Input input ) => input.IsTarget
			? throw new NotImplementedException()
			: _outer.GetInputValueFunc( input );

		public IOutputValue GetOutputValue( Node.Output valueOutput ) => throw new NotImplementedException();
		public IOutputValue GetOutputValue( Node.Output signalOutput, Node.Output valueOutput ) => throw new NotImplementedException();
		public Expression RunOutputSignal( Node.Output signalOutput ) => throw new NotImplementedException();
	}
}
