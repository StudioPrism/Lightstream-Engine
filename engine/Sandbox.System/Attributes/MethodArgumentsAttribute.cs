using System;

namespace Sandbox;

/// <summary>
/// Specify the types of arguments a method should have. Typically used with event attributes to throw an exception
/// if an event attribute is added to a method with incorrect arguments.
/// </summary>
[AttributeUsage( AttributeTargets.Class, Inherited = true )]
public class MethodArgumentsAttribute : System.Attribute
{
	public Type[] ArgumentTypes { get; set; }

	public MethodArgumentsAttribute( params Type[] argumentTypes )
	{
		ArgumentTypes = argumentTypes;
	}
}
