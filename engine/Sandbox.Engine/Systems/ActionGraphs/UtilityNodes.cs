#nullable enable

using System;
using System.Linq.Expressions;
using System.Reflection;
using Facepunch.ActionGraphs;
using Facepunch.ActionGraphs.Compilation;

namespace Sandbox.ActionGraphs
{
	[AttributeUsage( AttributeTargets.Parameter )]
	public sealed class HasConversionFromAttribute : Attribute
	{
		public Type Type { get; }

		public HasConversionFromAttribute( Type type )
		{
			Type = type;
		}
	}

	internal static class UtilityNodes
	{
		/// <summary>
		/// Tests if the given input is of the given type, otherwise returns null.
		/// </summary>
		/// <typeparam name="TIn">Input type.</typeparam>
		/// <typeparam name="TValue">Type to test for.</typeparam>
		/// <param name="input">Input value to test the type of.</param>
		[ActionGraphNode( "op.as" ), Pure, ActionGraphOperator, Title( "As {TValue|Type}" ), Icon( "filter_alt" ), Category( "Operators/Conversion" ), Tags( "common" )]
		public static TValue? As<TIn, TValue>( TIn? input )
			where TIn : class
			where TValue : class, TIn
		{
			return input as TValue;
		}

		[NodeDefinition]
		private sealed class ConversionNode : NodeDefinition
		{
			public PropertyDefinition TypeProperty { get; } = new( ParameterNames.Type, typeof( Type ),
				PropertyFlags.Required,
				new Facepunch.ActionGraphs.DisplayInfo( "Type", "Value type to convert to." ) );

			public InputDefinition ValueInput { get; } = new( ParameterNames.Value, typeof( object ),
				InputFlags.Required,
				new Facepunch.ActionGraphs.DisplayInfo( "Value", "Value to convert." ) );

			public OutputDefinition ResultOutput { get; } = new( ParameterNames.Result, typeof( object ),
				0,
				new Facepunch.ActionGraphs.DisplayInfo( "Result", "Converted value." ) );

			public NodeBinding DefaultBinding { get; }

			public override Facepunch.ActionGraphs.DisplayInfo DisplayInfo { get; } = new(
				"Convert",
				"Converts a value between types.",
				"Operators/Conversion",
				"settings",
				Tags: new[] { "common" } );

			public override IReadOnlyCollection<Attribute> Attributes { get; } = new[]
			{
				new ActionGraphOperatorAttribute()
			};

			public ConversionNode( NodeLibrary nodeLibrary )
				: base( nodeLibrary, "op.convert" )
			{
				DefaultBinding = NodeBinding.Create( DisplayInfo,
					new[] { TypeProperty },
					new[] { ValueInput },
					new[] { ResultOutput },
					attributes: Attributes );
			}

			protected override NodeBinding OnBind( BindingSurface surface )
			{
				if ( surface == BindingSurface.Empty ) return DefaultBinding;

				var valueType = surface.InputTypes.GetValueOrDefault( ValueInput.Name ) ?? typeof( object );
				var resultType = surface.Properties.TryGetValue( TypeProperty.Name, out var typeObj ) && typeObj is Type type
					? type
					: typeof( object );

				var resultDisplay = Sandbox.DisplayInfo.ForType( resultType );
				var icon = resultDisplay.Icon ?? "settings";

				var binding = DefaultBinding with
				{
					DisplayInfo = surface.Properties.ContainsKey( TypeProperty.Name )
						? DisplayInfo with { Title = $"Convert to {resultDisplay.Name}", Icon = icon }
						: DisplayInfo,
					Properties =
					new[]
					{
						TypeProperty with { Attributes = new[] { new HasConversionFromAttribute( valueType ) } }
					},
					Inputs = new[] { ValueInput with { Type = valueType } },
					Outputs = new[] { ResultOutput with { Type = resultType } }
				};

				if ( resultType == typeof( string ) )
				{
					return binding;
				}

				try
				{
					_ = Expression.Convert( Expression.Throw( null, valueType ), resultType );
				}
				catch ( Exception e )
				{
					binding = binding with
					{
						Messages = new[]
						{
							new NodeBinding.ValidationMessage( binding.Inputs.Single(), MessageLevel.Error,
								e.Message )
						}
					};
				}

				return binding;
			}

			protected override Expression OnBuildExpression( INodeExpressionBuilder builder )
			{
				var input = builder.Node.Inputs[ValueInput.Name];
				var output = builder.Node.Outputs[ResultOutput.Name];
				var inputExpr = builder.GetInputValue( input );

				if ( output.Type != typeof( string ) )
				{
					return builder.GetOutputValue()
						.Assign( Expression.Convert( inputExpr, output.Type ) );
				}

				var toString = input.Type.GetMethod( nameof( ToString ), BindingFlags.Public | BindingFlags.Instance, Type.EmptyTypes )!;
				var toStringCall = Expression.Call( inputExpr, toString );

				if ( input.Type.IsValueType )
				{
					return builder.GetOutputValue()
						.Assign( toStringCall );
				}

				return builder.GetOutputValue()
					.Assign( Expression.Condition(
						Expression.ReferenceEqual( inputExpr, Expression.Constant( null, input.Type ) ),
						Expression.Constant( null, typeof( string ) ),
						toStringCall ) );
			}
		}

		/// <summary>
		/// Tests if the given input is null or invalid.
		/// </summary>
		/// <param name="input">Input value to test for null.</param>
		[ActionGraphNode( "op.isnull" ), Pure, ActionGraphOperator, Title( "Is Null" ), Icon( "∄" ), Category( "Operators/Comparison" ), Tags( "common" )]
		public static bool IsNull( object? input )
		{
			return input is null or IValid { IsValid: false };
		}

		/// <summary>
		/// Tests if the given input is not null and not invalid.
		/// </summary>
		/// <param name="input">Input value to test for null.</param>
		[ActionGraphNode( "op.isnotnull" ), Pure, ActionGraphOperator, Title( "Is Not Null" ), Icon( "∃" ), Category( "Operators/Comparison" ), Tags( "common" )]
		public static bool IsNotNull( object? input )
		{
			return input is not (null or IValid { IsValid: false });
		}

		/// <inheritdoc cref="object.ToString"/>
		[ActionGraphNode( "sys.tostring" ), Pure]
		[Title( "To String" ), Icon( "abc" ), Category( "Object:data_object" )]
		public static string? SystemToString<T>( [Target] T? input )
		{
			return input?.ToString();
		}

		/// <inheritdoc cref="object.ToString"/>
		[ActionGraphNode( "sys.tostring.format" ), Pure]
		[Title( "To String (Format)" ), Icon( "abc" ), Category( "Object:data_object" )]
		public static string? SystemToString<T>( [Target] T? input, string format )
			where T : IFormattable
		{
			return input?.ToString( format, null );
		}

		/// <inheritdoc cref="object.GetHashCode"/>
		[ActionGraphNode( "sys.gethashcode" ), Pure]
		[Title( "Get Hash Code" ), Icon( "fingerprint" ), Category( "Object:data_object" )]
		public static int SystemGetHashCode<T>( [Target] T? input )
		{
			return input?.GetHashCode() ?? 0;
		}
	}
}
