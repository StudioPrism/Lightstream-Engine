using System.Reflection;
using Facepunch.ActionGraphs;

namespace Sandbox
{
	/// <inheritdoc cref="IPureAttribute"/>
	[AttributeUsage( AttributeTargets.Method )]
	public sealed class PureAttribute : Attribute, IPureAttribute
	{

	}

	/// <inheritdoc cref="IImpureAttribute"/>
	[AttributeUsage( AttributeTargets.Method )]
	public sealed class ImpureAttribute : Attribute, IImpureAttribute
	{

	}

	/// <inheritdoc cref="ITargetAttribute" />
	[AttributeUsage( AttributeTargets.Parameter )]
	public sealed class ActionGraphTargetAttribute : Attribute, ITargetAttribute
	{

	}

	/// <summary>
	/// In ActionGraph, this type parameter can only be satisfied by a type <c>TArg</c>, such
	/// that there exists at least one non-abstract type that extends / implements both
	/// <c>TArg</c> and <see cref="BaseType"/>.
	/// </summary>
	[AttributeUsage( AttributeTargets.GenericParameter, AllowMultiple = true )]
	public sealed class HasImplementationAttribute : Attribute
	{
		/// <summary>
		/// Base class or interface for which there must exist an extending / implementing type.
		/// </summary>
		public Type BaseType { get; }

		/// <inheritdoc cref="HasImplementationAttribute"/>
		/// <param name="baseType">
		/// Base class or interface for which there must exist an extending / implementing type.
		/// </param>
		public HasImplementationAttribute( Type baseType )
		{
			BaseType = baseType;
		}
	}

	/// <summary>
	/// In ActionGraph, this parameter should only be configurable in the inspector as a property and not have a dedicated input.
	/// </summary>
	[AttributeUsage( AttributeTargets.Parameter )]
	public sealed class ActionGraphPropertyAttribute : Attribute, IPropertyAttribute
	{

	}

	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Constructor )]
	public sealed class ActionGraphIgnoreAttribute : Attribute
	{

	}

	/// <summary>
	/// Don't cache instances of this type when serializing action graph references, force them to be always serialized separately.
	/// We need this for component / game object references so we can update IDs when duplicating objects / instantiating prefabs.
	/// </summary>
	[AttributeUsage( AttributeTargets.Struct | AttributeTargets.Class )]
	public sealed class ActionGraphExposeWhenCachedAttribute : Attribute, IExposeWhenCachedAttribute
	{

	}

	/// <inheritdoc cref="INodeAttribute"/>
	[AttributeUsage( AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Constructor )]
	public class ActionGraphNodeAttribute : Attribute, INodeAttribute
	{
		/// <inheritdoc />
		public string Identifier { get; }

		/// <inheritdoc />
		public bool DefaultInputSignal { get; set; } = true;

		/// <inheritdoc />
		public bool DefaultOutputSignal { get; set; } = true;

		/// <inheritdoc />
		public bool InheritAsync { get; set; } = false;

		public ActionGraphNodeAttribute( string identifier )
		{
			Identifier = identifier;
		}
	}

	/// <summary>
	/// Display this node as an operator, with no header or socket labels, and a big icon in the middle.
	/// </summary>

	[AttributeUsage( AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Constructor )]
	public sealed class ActionGraphOperatorAttribute : Attribute
	{

	}

	[Obsolete( "Please use [ActionGraphNode( id ), Pure]." )]
	[AttributeUsage( AttributeTargets.Method )]
	public sealed class ExpressionNodeAttribute : NodeAttribute, IPureAttribute
	{
		public ExpressionNodeAttribute( string identifier )
			: base( identifier )
		{

		}
	}

	[Obsolete( "Please use [ActionGraphNode( id )]." )]
	[AttributeUsage( AttributeTargets.Method )]
	public sealed class ActionNodeAttribute : NodeAttribute
	{
		public ActionNodeAttribute( string identifier )
			: base( identifier )
		{

		}
	}

	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Constructor )]
	public sealed class ActionGraphIncludeAttribute : Attribute
	{
		/// <summary>
		/// If true, double-clicking on an output of the declaring type will auto-expand this member.
		/// </summary>
		public bool AutoExpand { get; set; }
	}

	/// <summary>
	/// Force a delegate-type property to only have a single attached Action Graph.
	/// </summary>
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property )]
	public sealed class SingleActionAttribute : Attribute
	{

	}
}
