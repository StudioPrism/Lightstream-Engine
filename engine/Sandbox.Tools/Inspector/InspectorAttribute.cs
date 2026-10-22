using System;

namespace Editor;

[AttributeUsage( AttributeTargets.Class, AllowMultiple = true )]
public class InspectorAttribute : Attribute, ITypeAttribute, IUninheritable
{
	public System.Type TargetType { get; set; }
	public System.Type Type { get; init; }

	public InspectorAttribute( System.Type type )
	{
		Type = type;
	}
}
