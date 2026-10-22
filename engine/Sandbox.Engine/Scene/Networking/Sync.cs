namespace Sandbox;

/// <summary>
/// Automatically synchronize a property of a networked object from the owner to other clients.
/// </summary>
[AttributeUsage( AttributeTargets.Property )]
[CodeGenerator( CodeGeneratorFlags.Instance | CodeGeneratorFlags.WrapPropertySet, "__sync_SetValue" )]
[CodeGenerator( CodeGeneratorFlags.Instance | CodeGeneratorFlags.WrapPropertyGet, "__sync_GetValue" )]
public class SyncAttribute : Attribute
{
	/// <summary>
	/// Query this value for changes rather than counting on set being called. This is appropriate
	/// if the value returned by its getter can change without calling its setter.
	///
	/// Obsoleted: 13/12/2024
	/// </summary>
	[Obsolete( "Use SyncFlags.Query" )]
	public bool Query
	{
		set
		{
			if ( value )
				Flags |= SyncFlags.Query;
			else
				Flags &= ~SyncFlags.Query;
		}
	}

	/// <summary>
	/// Flags that describe how this property is synchronized.
	/// </summary>
	public SyncFlags Flags { get; set; }

	public SyncAttribute( SyncFlags flags )
	{
		Flags = flags;
	}

	public SyncAttribute()
	{

	}
}
