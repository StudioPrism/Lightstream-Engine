namespace Sandbox;

/// <summary>
/// Automatically synchronize a property of a networked object from the host to other clients.
/// Obsolete: 11/12/2024
/// </summary>
[CodeGenerator( CodeGeneratorFlags.Instance | CodeGeneratorFlags.WrapPropertySet, "__sync_SetValue" )]
[CodeGenerator( CodeGeneratorFlags.Instance | CodeGeneratorFlags.WrapPropertyGet, "__sync_GetValue" )]
[Obsolete( "Use [Sync] with SyncFlags.FromHost" )]
public class HostSyncAttribute : SyncAttribute
{
	public HostSyncAttribute() : base( SyncFlags.FromHost )
	{

	}
}
