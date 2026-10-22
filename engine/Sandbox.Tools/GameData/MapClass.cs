using System;
using System.Reflection;
using Sandbox;

namespace Editor;

//
// Technically the GameData classes aren't limited to maps, we can derive this from a base class in the future.
// But do it like this for now to keep the API simple.
//
// We're not exposing everything on purpose, lots of stuff is redundant or doesn't make sense in managed context.
//

/// <summary>
/// Represents an entity class used by the map editor
/// </summary>
public partial class MapClass
{
	/// <summary>
	/// Class name e.g prop_physics
	/// </summary>
	public string Name { get; internal set; }

	/// <summary>
	/// Display name e.g Physics Prop
	/// </summary>
	public string DisplayName { get; internal set; }

	/// <summary>
	/// Human readable name e.g Physics Prop
	/// </summary>
	public string Description { get; internal set; }

	/// <summary>
	/// Icon ( Material )
	/// </summary>
	public string Icon { get; internal set; }

	/// <summary>
	/// Category
	/// </summary>
	public string Category { get; internal set; }

	/// <summary>
	/// C# Type of this class
	/// </summary>
	public Type Type { get; internal set; }

	/// <summary>
	/// Point, Solid, etc..
	/// </summary>
	internal GameDataClassType ClassType { get; set; }

	// Maybe stupid accessors

	/// <summary>
	/// A point entity, i.e. a model entity, etc.
	/// </summary>
	public bool IsPointClass => ClassType == GameDataClassType.GenericPointClass;

	/// <summary>
	/// A solid class entity, triggers, etc., entities that are tied to from a mesh in Hammer
	/// </summary>
	public bool IsSolidClass => ClassType == GameDataClassType.GenericSolidClass;

	/// <summary>
	/// A path entity, will appear in the Path Tool.
	/// </summary>
	public bool IsPathClass => ClassType == GameDataClassType.PathClass;

	/// <summary>
	/// A cable entity, will appear in the Path Tool.
	/// </summary>
	public bool IsCableClass => ClassType == GameDataClassType.CableClass;

	/// <summary>
	/// List of properties exposed to tools for this class.
	/// </summary>
	public List<MapClassVariable> Variables { get; internal set; } = new();

	/// <summary>
	/// List of inputs for this class.
	/// </summary>
	public List<Input> Inputs { get; internal set; } = new();

	/// <summary>
	/// List of outputs for this class.
	/// </summary>
	public List<Output> Outputs { get; internal set; } = new();

	/// <summary>
	/// General purpose tags, some with special meanings within Hammer and map compilers.
	/// </summary>
	public List<string> Tags { get; internal set; } = new();

	/// <summary>
	/// In-editor helpers for this class, such as box visualizers for certain properties, etc.
	/// </summary>
	public List<Tuple<string, string[]>> EditorHelpers { get; internal set; } = new();

	/// <summary>
	/// General purpose key-value store to alter functionality of UI, map compilation, editor helpers, etc.
	/// </summary>
	public Dictionary<string, object> Metadata { get; internal set; } = new();

	/// <summary>
	/// What game does this belong to? ( TODO: Might not be best place for this? )
	/// </summary>
	public string GameIdent { get; internal set; }

	/// <summary>
	/// What package did this entity come from?
	/// </summary>
	public Package Package { get; internal set; }

	/// <summary>
	/// What Assembly did this entity come from?
	/// </summary>
	internal Assembly Assembly { get; set; }

	public MapClass( string name )
	{
		Name = name;
	}

	public override string ToString()
	{
		return $"MapClass( {Name} )";
	}
}
