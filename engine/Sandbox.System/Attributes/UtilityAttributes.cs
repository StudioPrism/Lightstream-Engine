
/// <summary>
/// Adds a single or multiple tags for this type or member. Tags can then be retrieved via DisplayInfo library.
/// </summary>
public class TagAttribute : Attribute
{
	/// <summary>
	/// The tags to add for this type or member.
	/// </summary>
	public string[] Value { get; private set; }

	public TagAttribute( params string[] tag )
	{
		Value = tag;
	}

	/// <summary>
	/// Returns all the tags as an enumerable.
	/// </summary>
	/// <returns></returns>
	public IEnumerable<string> EnumerateValues()
	{
		foreach ( var tag in Value )
		{
			yield return tag;
		}
	}
}

/// <summary>
/// Alternate class name(s) for this type to the one specified via LibraryAttribute. This info can then be retrieved via DisplayInfo library.
/// </summary>
public class AliasAttribute : Attribute, IUninheritable
{
	/// <summary>
	/// The aliases for this class.
	/// </summary>
	public string[] Value { get; private set; }

	public AliasAttribute( params string[] tag )
	{
		Value = tag;
	}
}

/// <summary>
/// Tell the tools or gameui property editor which editor we should be using for this property or type.
/// </summary>
public class EditorAttribute : Attribute, IUninheritable
{
	/// <summary>
	/// The editor to use.
	/// </summary>
	public string Value { get; private set; }

	public EditorAttribute( string editorName )
	{
		Value = editorName;
	}
}

/// <summary>
/// This entity is expected to be spawnable in-game, like from Sandbox's spawnmenu.
/// </summary>
public sealed class SpawnableAttribute : TagAttribute, IUninheritable
{
	public SpawnableAttribute() : base( "spawnable" )
	{
	}
}

/// <summary>
/// Hide this in tools/editors.
/// </summary>
[System.Obsolete( "Moved to [Hide]" )]
public sealed class HideInEditorAttribute : TagAttribute, IUninheritable
{
	public HideInEditorAttribute() : base( "hideineditor" )
	{
	}
}

/// <summary>
/// Mark property as having a minimum and maximum value.
/// </summary>
[AttributeUsage( AttributeTargets.Property )]
public class MinMaxAttribute : Attribute
{
	/// <summary>
	/// The minimum value for this property.
	/// </summary>
	public float MinValue { get; set; }

	/// <summary>
	/// The maximum value for this property.
	/// </summary>
	public float MaxValue { get; set; }

	public MinMaxAttribute( float min, float max )
	{
		MinValue = min;
		MaxValue = max;
	}
}

/// <summary>
/// Declare a model to represent this entity in editor. This is a common attribute so it's leaked out of the Editor namespace.
/// </summary>
[AttributeUsage( AttributeTargets.Class )]
public class EditorModelAttribute : Attribute
{
	/// <summary>
	/// The model to display in the editor.
	/// </summary>
	public string Model { get; set; }

	/// <summary>
	/// Whether the model should cast shadows in the editor.
	/// </summary>
	public bool CastShadows { get; set; } = false;

	/// <summary>
	/// Don't reorient bounds. This is used for things that have fixed bounds in the game, like info_player_start.
	/// </summary>
	public bool FixedBounds { get; set; } = false;

	/// <summary>
	/// Tint color for this editor model instance when the entity it represents is static.
	/// </summary>
	public Color StaticColor { get; set; }

	/// <summary>
	/// Tint color for this editor model instance when the entity it represents is dynamic.
	/// </summary>
	public Color DynamicColor { get; set; }

	public EditorModelAttribute( string model, string staticColor = "white", string dynamicColor = "white" )
	{
		Model = model;
		StaticColor = staticColor;
		DynamicColor = dynamicColor;
	}
}


/// <summary>
/// Sometimes with CodeGen we want reflection to be able to get the original initial value
/// of a property (which is set with {get;set;} = initialvalue;). For this reason sometimes
/// we'll drop this attribute on that property.
/// You might want to use this manually for instances where codegen can't define the default
/// value. This will usually happen for structs like vector and color.. if the default value isn't
/// defined as a number or string.
/// </summary>
public sealed class DefaultValueAttribute : Attribute
{
	/// <summary>
	/// The default value.
	/// </summary>
	public object Value { get; internal set; }

	public DefaultValueAttribute( object value )
	{
		Value = value;
	}
}

/// <summary>
/// Allows any task defined in this assembly to continue after a sync context expires.
/// Other tasks that await tasks in this assembly will still be cancelled if their assembly
/// isn't also marked with this attribute.
/// </summary>
[AttributeUsage( AttributeTargets.Assembly )]
internal sealed class TasksPersistOnContextResetAttribute : Attribute { }


namespace Sandbox
{
	/// <summary>
	/// Hide this in tools/editors.
	/// </summary>
	public sealed class HideAttribute : TagAttribute, IUninheritable
	{
		public HideAttribute() : base( "hide" )
		{
		}
	}

}
