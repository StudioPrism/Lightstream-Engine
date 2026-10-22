using System;

namespace Editor
{
	/// <summary>
	/// Base attribute which allows adding FGD metadata to classes.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class )]
	public abstract class MetaDataAttribute : Attribute
	{
		public virtual void AddTags( List<string> tags ) { }
		public virtual void AddMetaData( Dictionary<string, object> meta_data ) { }
		public virtual void AddHelpers( List<Tuple<string, string[]>> helpers ) { }
	}

	/// <summary>
	/// Base attribute which allows adding metadata to properties.
	/// </summary>
	[AttributeUsage( AttributeTargets.Property )]
	public abstract class FieldMetaDataAttribute : Attribute
	{
		public virtual void AddMetaData( Dictionary<string, string> meta_data ) { }
	}

	/// <summary>
	/// A way to hide properties from parent classes in tools.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = true )]
	public class HidePropertyAttribute : Attribute
	{
		internal string PropertyName;

		/// <param name="internal_name">The internal/fgd name to skip. Usually all lowercase and with underscores (_) instead of spaces.</param>
		public HidePropertyAttribute( string internal_name )
		{
			PropertyName = internal_name;
		}
	}

	/// <summary>
	/// If used on a Color or Color32 property, enables alpha modification in editors.
	/// </summary>
	[AttributeUsage( AttributeTargets.Property )]
	public class EnableColorAlphaAttribute : FieldMetaDataAttribute
	{
		public override void AddMetaData( Dictionary<string, string> meta_data )
		{
			meta_data["alpha"] = "true";
		}
	}
}
