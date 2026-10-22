namespace Sandbox;

internal class Avatar
{
	const string DefaultAvatar = "{\"Items\":[{\"id\":1939743225,\"t\":null},{\"id\":-1925751926,\"t\":0.9444444},{\"id\":226909407,\"t\":null},{\"id\":1594058106,\"t\":null},{\"id\":1772984322,\"t\":null},{\"id\":-791583293,\"t\":null},{\"id\":-1688334362,\"t\":null}],\"Height\":1}";

	[ConVar( "avatar", ConVarFlags.Saved | ConVarFlags.UserInfo | ConVarFlags.Protected )]
	public static string AvatarJson { get; set; } = DefaultAvatar;
}
