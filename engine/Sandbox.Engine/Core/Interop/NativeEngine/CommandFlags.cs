[Flags]
internal enum ConVarFlags_t : long
{
	// The default, no flags at all
	FCVAR_NONE = 0,

	// Command to ConVars and ConCommands
	// ConVar Systems
	FCVAR_HIDDEN = (1L << 4),    // Hidden. Doesn't appear in find or auto complete. Like DEVELOPMENTONLY, but can't be compiled out.

	// ConVar only
	FCVAR_ARCHIVE = (1L << 7),   // set to cause it to be saved to vars.rc

	// It's a ConVar that's shared between the client and the server.
	// At signon, the values of all such ConVars are sent from the server to the client (skipped for local
	//  client, of course )
	// If a change is requested it must come from the console (i.e., no remote client changes)
	// If a value is changed while a server is active, it's replicated to all connected clients
	FCVAR_REPLICATED = (1L << 13), // server setting enforced on clients, TODO rename to FCAR_SERVER at some time
	FCVAR_CHEAT = (1L << 14), // Only useable in singleplayer / debug / multiplayer & sv_cheats

	FCVAR_LINKED_CONCOMMAND = (1L << 26), // ConCommands can only be linked if this is specified
}
