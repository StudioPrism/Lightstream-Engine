using System.Runtime.InteropServices;
using System.Threading;
using Steamworks;
using Steamworks.Data;

namespace Sandbox.Services;

public static class Auth
{
	/// <summary>
	/// Get an auth token, which can be passed to the backend
	/// </summary>
	public static async Task<string> GetToken( string serviceName, CancellationToken token = default )
	{
		var package = Application.GameIdent;
		if ( package is null ) return default;
		if ( Backend.Account is null ) return default;

		try
		{
			var tt = await Backend.Account.GetAuthToken( Api.SessionId.ToString(), package, serviceName );
			return tt?.Trim( '"', '\'', ' ' );
		}
		catch ( Exception e )
		{
			Log.Warning( e, $"Error getting token for {serviceName}" );
			return default;
		}
	}

	/// <summary>
	/// Get a Steam authentication ticket intended for a target with a particular Steam Id.
	/// </summary>
	/// <param name="targetSteamId"></param>
	/// <param name="ticket"></param>
	/// <returns></returns>
	internal static unsafe HAuthTicket GetAuthTicket( ulong targetSteamId, out byte[] ticket )
	{
		var buffer = stackalloc byte[1024];
		HAuthTicket handle;
		uint ticketLength;

		if ( Application.IsDedicatedServer )
		{
			var user = NativeEngine.Steam.SteamGameServer();
			handle = user.GetAuthSessionTicket( targetSteamId, (IntPtr)buffer, out ticketLength );
		}
		else
		{
			var user = NativeEngine.Steam.SteamUser();
			handle = user.GetAuthSessionTicket( targetSteamId, (IntPtr)buffer, out ticketLength );
		}

		ticket = new byte[ticketLength];
		Marshal.Copy( (IntPtr)buffer, ticket, 0, (int)ticketLength );
		return handle;
	}

	/// <summary>
	/// End an authentication session with a particular user.
	/// </summary>
	/// <param name="steamId"></param>
	internal static void EndAuthSession( ulong steamId )
	{
		if ( Application.IsDedicatedServer )
		{
			var user = NativeEngine.Steam.SteamGameServer();
			user.EndAuthSession( steamId );
		}
		else
		{
			var user = NativeEngine.Steam.SteamUser();
			user.EndAuthSession( steamId );
		}
	}

	/// <summary>
	/// Begin an authentication session with a particular user, using the ticket they provided you.
	/// </summary>
	/// <param name="senderSteamId"></param>
	/// <param name="ticket"></param>
	internal static unsafe BeginAuthResult BeginAuthSession( ulong senderSteamId, byte[] ticket )
	{
		fixed ( byte* ptr = ticket )
		{
			if ( Application.IsDedicatedServer )
			{
				var user = NativeEngine.Steam.SteamGameServer();
				return user.BeginAuthSession( senderSteamId, (IntPtr)ptr, ticket.Length );
			}
			else
			{
				var user = NativeEngine.Steam.SteamUser();
				return user.BeginAuthSession( senderSteamId, (IntPtr)ptr, ticket.Length );
			}
		}
	}

	/// <summary>
	/// Cancel an authentication ticket from a handle obtained by <see cref="GetAuthTicket"/>.
	/// </summary>
	/// <param name="handle"></param>
	internal static void CancelAuthTicket( HAuthTicket handle )
	{
		if ( Application.IsDedicatedServer )
		{
			var user = NativeEngine.Steam.SteamGameServer();
			user.CancelAuthTicket( handle );
		}
		else
		{
			var user = NativeEngine.Steam.SteamUser();
			user.CancelAuthTicket( handle );
		}
	}
}
