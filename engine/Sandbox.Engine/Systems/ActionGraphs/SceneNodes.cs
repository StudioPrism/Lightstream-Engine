#nullable enable

using Facepunch.ActionGraphs;
using System.Linq.Expressions;
using System.Reflection;
using Facepunch.ActionGraphs.Compilation;

// ReSharper disable InconsistentNaming

namespace Sandbox.ActionGraphs;

internal static class SceneNodes
{
	[Obsolete, ActionGraphNode( "scene.getscene" ), Pure, Title( "Get Scene" ), Category( "Scene" ), Description( "Gets the scene containing the object this action is running on." ), Icon( "perm_media" )]
	public static Scene GetScene( [Target] Component _this )
	{
		return _this.Scene;
	}

	[ActionGraphNode( "scene.get" ), Pure, Hide, Title( "Get {T|Component}" ), Category( "Scene" ), Description( "Gets a component of the given type from a target object or component." ), Icon( "check_box_outline_blank" )]
	public static T Get<[HasImplementation( typeof( Component ) )] T>( [Target, Title( "Target" )] IComponentLister _this )
	{
		return _this.Get<T>( FindMode.Enabled | FindMode.Disabled | FindMode.InSelf );
	}

	[ActionGraphNode( "scene.get.inscene" ), Pure, Title( "Get {T|Component} in Scene" ), Category( "Scene" ), Description( "Finds the first instance of an active component of a given type in the current scene." ), Icon( "travel_explore" )]
	public static T? Find<[HasImplementation( typeof( Component ) )] T>()
	{
		return Game.ActiveScene.GetAllComponents<T>().FirstOrDefault();
	}

	[ActionGraphNode( "scene.getall.inscene" ), Pure, Title( "Get All {T|Component} in Scene" ), Category( "Scene" ), Description( "Finds all active components of a given type in the current scene." ), Icon( "travel_explore" )]
	public static IEnumerable<T> FindAll<[HasImplementation( typeof( Component ) )] T>()
	{
		return Game.ActiveScene.GetAllComponents<T>();
	}

	public static GameObject Instantiate( PrefabFile prefab,
		Transform? transform = null,
		GameObject? parent = null,
		bool startEnabled = true,
		string? name = null )
	{
		var scene = SceneUtility.GetPrefabScene( prefab );

		return scene.Clone( transform ?? Transform.Zero, parent, startEnabled, name );
	}

	[ActionGraphNode( "scene.instantiate" ), Title( "Instantiate Prefab" ), Category( "Scene" ), Description( "Creates an instance of the given prefab in this scene." ), Icon( "ballot" )]
	public static GameObject Instantiate( PrefabFile prefab,
		Vector3? position = null,
		Rotation? rotation = null,
		Vector3? scale = null,
		GameObject? parent = null,
		bool startEnabled = true,
		string? name = null )
	{
		return Instantiate( prefab,
			new Transform( position ?? default, rotation ?? Rotation.Identity, scale ?? Vector3.One ),
			parent,
			startEnabled, name );
	}

	[ActionGraphNode( "scene.clone" ), Title( "Clone Game Object" ), Category( "Scene" ), Description( "Create a unique copy of the GameObject." ), Icon( "content_copy" )]
	public static GameObject Clone( GameObject target,
		Vector3? position = null,
		Rotation? rotation = null,
		Vector3? scale = null,
		GameObject? parent = null,
		bool startEnabled = true,
		string? name = null )
	{
		return target.Clone( new Transform( position ?? target.WorldPosition, rotation ?? target.WorldRotation, scale ?? target.WorldScale ), parent, startEnabled, name );
	}

	[ActionGraphNode( "scene.find" ), Pure, Title( "Find Object by Name" ), Category( "Scene" ), Description( "Find a GameObject by name." ), Icon( "search" )]
	public static GameObject? FindByName( string name, bool ignoreCase = true )
	{
		return Game.ActiveScene.Directory.FindByName( name, ignoreCase ).FirstOrDefault();
	}

	[ActionGraphNode( "scene.findall" ), Pure, Title( "Find Objects by Name" ), Category( "Scene" ), Description( "Find all GameObjects by name." ), Icon( "manage_search" )]
	public static IEnumerable<GameObject> FindAllByName( string name, bool ignoreCase = true )
	{
		return Game.ActiveScene.Directory.FindByName( name, ignoreCase );
	}

	/// <summary>
	/// Plays a sound at the position of the target object.
	/// </summary>
	[ActionGraphNode( "sound.play" ), Title( "Play Sound" ), Category( "Audio" ), Icon( "volume_up" )]
	internal static SoundHandle PlaySound( [Description( "Target" )] this GameObject _this, SoundEvent soundEvent )
	{
		return Sound.Play( soundEvent, _this.WorldPosition );
	}

	/// <inheritdoc cref="Scene.Trace"/>
	[ActionGraphNode( "scene.trace" ), Title( "Scene Trace" ), Category( "Scene" ), Icon( "keyboard_tab" )]
	internal static SceneTrace Trace => Game.ActiveScene.Trace;

	/// <inheritdoc cref="GameObject.NetworkSpawn(Connection)"/>
	[ActionGraphNode( "scene.netspawn" ), Icon( "share" )]
	internal static bool NetworkSpawn( this GameObject go, Connection? connection = null )
	{
		return go.NetworkSpawn( connection ?? Connection.Local );
	}

	/// <summary>
	/// True if we are the owner of the GameObject
	/// </summary>
	[ActionGraphNode( "go.isowner" ), Pure, Title( "Is Owner" ), Icon( "verified_user" )]
	internal static bool IsOwner( [Description( "Target" )] this GameObject _this )
	{
		return _this.Network.IsOwner;
	}
}

[NodeDefinition, Obsolete( "Please use 'Component -> Game Object -> Get' instead." )]
internal class GetGameObjectNodeDefinition : NodeDefinition
{
	private NodeBinding? _defaultBinding;
	private NodeBinding DefaultBinding
	{
		get
		{
			if ( _defaultBinding is not null ) return _defaultBinding;

			var outputs = new List<OutputDefinition> { ResultOutput };

			_defaultBinding = NodeBinding.Create( DisplayInfo,
				inputs: new[] { TargetInput },
				outputs: outputs,
				attributes: Attributes );

			return _defaultBinding;
		}
	}

	private InputDefinition TargetInput { get; } = InputDefinition.Target( typeof( GameObject ) );

	private OutputDefinition ResultOutput { get; } = new( "_result", typeof( GameObject ), 0,
		new Facepunch.ActionGraphs.DisplayInfo( "Game Object", "The game object this action is running on." ) );

	public GetGameObjectNodeDefinition( NodeLibrary nodeLibrary )
		: base( nodeLibrary, "scene.getobject" )
	{

	}

	protected override void OnDefaultBindingsInvalidated()
	{
		_defaultBinding = null;
	}

	protected override NodeBinding OnBind( BindingSurface surface )
	{
		var targetInput = surface.ActionGraph?.Inputs.Values.FirstOrDefault( x => x.IsTarget );

		if ( targetInput is null || !targetInput.Type.IsAssignableTo( typeof( GameObject ) ) )
		{
			return DefaultBinding with
			{
				Messages = new List<NodeBinding.ValidationMessage>( DefaultBinding.Messages )
				{
					new( null, MessageLevel.Error, $"Unable to find target input of type '{typeof(GameObject)}'." )
				}
			};
		}

		if ( targetInput.Default is not GameObject obj )
		{
			return DefaultBinding;
		}

		var components = obj.Components
			.GetAll<Component>( FindMode.Enabled | FindMode.Disabled | FindMode.InSelf )
			.Where( x => x is not IActionComponent )
			.ToArray();

		var componentTypes = components
			.Select( x => x.GetType() )
			.ToArray();

		return DefaultBinding with
		{
			Outputs = DefaultBinding.Outputs
				.Concat( componentTypes
					.Select( x =>
					{
						var displayInfo = Sandbox.DisplayInfo.ForType( x );

						return new OutputDefinition( x.Name, x, 0,
							new Facepunch.ActionGraphs.DisplayInfo( displayInfo.Name ?? x.Name.ToTitleCase(),
								displayInfo.Description,
								displayInfo.Group,
								displayInfo.Icon,
								null,
								displayInfo.Tags ) );
					} ) )
				.ToArray(),
			Target = components
		};
	}

	protected override Expression OnBuildExpression( INodeExpressionBuilder builder )
	{
		var node = builder.Node;
		var getGameObjectExpr = builder.GetInputValue( TargetInput );

		var statements = new List<Expression>
		{
			builder.GetOutputValue( ResultOutput ).Assign( getGameObjectExpr )
		};

		switch ( node.Binding.Target )
		{
			case Component[] components:
				{
					foreach ( var component in components )
					{
						if ( !node.Outputs.TryGetValue( component.GetType().Name, out var output ) )
						{
							continue;
						}

						if ( !output.IsLinked )
						{
							continue;
						}

						statements.Add( builder.GetOutputValue( output )
							.Assign( Expression.Constant( component, output.Type ) ) );
					}

					break;
				}

			case Type[] componentTypes:
				{
					var getMethod = typeof( IComponentLister )
						.GetMethod( nameof( IComponentLister.Get ), 1, BindingFlags.Public | BindingFlags.Instance,
							null, new[] { typeof( FindMode ) }, null )!;
					var findMode = Expression.Constant( FindMode.Enabled | FindMode.Disabled | FindMode.InSelf );

					foreach ( var type in componentTypes )
					{
						if ( !node.Outputs.TryGetValue( type.Name, out var output ) )
						{
							continue;
						}

						if ( !output.IsLinked )
						{
							continue;
						}

						statements.Add( builder.GetOutputValue( output )
							.Assign( Expression.Call( getGameObjectExpr, getMethod.MakeGenericMethod( type ), findMode ) ) );
					}

					break;
				}
		}

		return Expression.Block( statements );
	}

	public override Facepunch.ActionGraphs.DisplayInfo DisplayInfo => new(
		Title: "Get Game Object",
		Description: "Gets the game object this action is running on.",
		Group: "Game Object",
		Icon: "ballot" );
}
