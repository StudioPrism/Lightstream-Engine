using System;

namespace Editor;

/// <summary>
/// When using <see cref="InspectorAttribute"/> with a type that inherits from InspectorWidget, when you inspect an object of that class, it will create an instance of the widget and display it in the inspector.
/// </summary>
[Expose]
public abstract class InspectorWidget : Widget
{
	public SerializedObject SerializedObject { get; init; }

	public InspectorWidget( SerializedObject so ) : base( null )
	{
		ArgumentNullException.ThrowIfNull( so, "SerializedObject" );
		SerializedObject = so;
	}

	/// <summary>
	/// Closes the inspector
	/// </summary>
	/// <param name="newObj"></param>
	/// <returns></returns>
	public bool CloseInspector( object newObj = null )
	{
		return OnInspectorClose( newObj );
	}

	/// <summary>
	/// Called when the inspector is about to be closed.
	/// Can return false to prevent closing.
	/// </summary>
	protected virtual bool OnInspectorClose( object newObj = null )
	{
		return true;
	}

	/// <summary>
	/// Creates an inspector widget for the given serialized object.
	/// </summary>
	/// <param name="obj"></param>
	/// <param name="ignore"></param>
	/// <returns></returns>
	public static InspectorWidget Create( SerializedObject obj, Type ignore = null )
	{
		var t = obj.Targets.FirstOrDefault()?.GetType();

		while ( t != null )
		{
			var ed = EditorTypeLibrary.GetTypesWithAttribute<InspectorAttribute>( false )
					.Where( x => x.Type.TargetType.IsAssignableTo( typeof( InspectorWidget ) ) )
					.Where( x => x.Type.TargetType != ignore )
					.Where( x => x.Attribute.Type.Name == t.Name )
					.FirstOrDefault();

			if ( ed.Type != null )
				return ed.Type.Create<InspectorWidget>( [obj] );

			t = t.BaseType;
		}

		var editor = EditorTypeLibrary.GetTypesWithAttribute<InspectorAttribute>( false )
					.Where( x => x.Type.TargetType.IsAssignableTo( typeof( InspectorWidget ) ) )
					.Where( x => x.Type.TargetType != ignore )
					.Where( x => x.Attribute.Type.Name == obj.TypeName )
					.FirstOrDefault();

		if ( editor.Type == null ) return null;
		return editor.Type.Create<InspectorWidget>( [obj] );
	}
}
