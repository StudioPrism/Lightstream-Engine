using Steamworks.Data;
using System.Runtime.InteropServices;


namespace Steamworks
{
	internal unsafe class ISteamUtils : SteamInterface
	{

		internal ISteamUtils( bool IsGameServer )
		{
			SetupInterface( IsGameServer );
		}

		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_SteamUtils_v010", CallingConvention = Platform.CC )]
		internal static extern IntPtr SteamAPI_SteamUtils_v010();
		internal override IntPtr GetUserInterfacePointer() => SteamAPI_SteamUtils_v010();
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_SteamGameServerUtils_v010", CallingConvention = Platform.CC )]
		internal static extern IntPtr SteamAPI_SteamGameServerUtils_v010();
		internal override IntPtr GetServerInterfacePointer() => SteamAPI_SteamGameServerUtils_v010();


		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamUtils_GetSecondsSinceAppActive", CallingConvention = Platform.CC )]
		private static extern uint _GetSecondsSinceAppActive( IntPtr self );

		#endregion
		internal uint GetSecondsSinceAppActive()
		{
			var returnValue = _GetSecondsSinceAppActive( Self );
			return returnValue;
		}

		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamUtils_GetSecondsSinceComputerActive", CallingConvention = Platform.CC )]
		private static extern uint _GetSecondsSinceComputerActive( IntPtr self );

		#endregion
		internal uint GetSecondsSinceComputerActive()
		{
			var returnValue = _GetSecondsSinceComputerActive( Self );
			return returnValue;
		}

		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamUtils_GetConnectedUniverse", CallingConvention = Platform.CC )]
		private static extern Universe _GetConnectedUniverse( IntPtr self );

		#endregion
		internal Universe GetConnectedUniverse()
		{
			var returnValue = _GetConnectedUniverse( Self );
			return returnValue;
		}

		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamUtils_GetServerRealTime", CallingConvention = Platform.CC )]
		private static extern uint _GetServerRealTime( IntPtr self );

		#endregion
		internal uint GetServerRealTime()
		{
			var returnValue = _GetServerRealTime( Self );
			return returnValue;
		}

		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamUtils_GetIPCountry", CallingConvention = Platform.CC )]
		private static extern Utf8StringPointer _GetIPCountry( IntPtr self );

		#endregion
		internal string GetIPCountry()
		{
			var returnValue = _GetIPCountry( Self );
			return returnValue;
		}

		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamUtils_GetImageSize", CallingConvention = Platform.CC )]
		[return: MarshalAs( UnmanagedType.I1 )]
		private static extern bool _GetImageSize( IntPtr self, int iImage, ref uint pnWidth, ref uint pnHeight );

		#endregion
		internal bool GetImageSize( int iImage, ref uint pnWidth, ref uint pnHeight )
		{
			var returnValue = _GetImageSize( Self, iImage, ref pnWidth, ref pnHeight );
			return returnValue;
		}

		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamUtils_GetImageRGBA", CallingConvention = Platform.CC )]
		[return: MarshalAs( UnmanagedType.I1 )]
		private static extern bool _GetImageRGBA( IntPtr self, int iImage, [In, Out] byte[] pubDest, int nDestBufferSize );

		#endregion
		internal bool GetImageRGBA( int iImage, [In, Out] byte[] pubDest, int nDestBufferSize )
		{
			var returnValue = _GetImageRGBA( Self, iImage, pubDest, nDestBufferSize );
			return returnValue;
		}

		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamUtils_GetCurrentBatteryPower", CallingConvention = Platform.CC )]
		private static extern byte _GetCurrentBatteryPower( IntPtr self );

		#endregion
		internal byte GetCurrentBatteryPower()
		{
			var returnValue = _GetCurrentBatteryPower( Self );
			return returnValue;
		}

		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamUtils_GetAppID", CallingConvention = Platform.CC )]
		private static extern uint _GetAppID( IntPtr self );

		#endregion
		internal uint GetAppID()
		{
			var returnValue = _GetAppID( Self );
			return returnValue;
		}

		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamUtils_SetOverlayNotificationPosition", CallingConvention = Platform.CC )]
		private static extern void _SetOverlayNotificationPosition( IntPtr self, NotificationPosition eNotificationPosition );

		#endregion
		internal void SetOverlayNotificationPosition( NotificationPosition eNotificationPosition )
		{
			_SetOverlayNotificationPosition( Self, eNotificationPosition );
		}

		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamUtils_IsAPICallCompleted", CallingConvention = Platform.CC )]
		[return: MarshalAs( UnmanagedType.I1 )]
		private static extern bool _IsAPICallCompleted( IntPtr self, SteamAPICall_t hSteamAPICall, [MarshalAs( UnmanagedType.U1 )] ref bool pbFailed );

		#endregion
		internal bool IsAPICallCompleted( SteamAPICall_t hSteamAPICall, [MarshalAs( UnmanagedType.U1 )] ref bool pbFailed )
		{
			var returnValue = _IsAPICallCompleted( Self, hSteamAPICall, ref pbFailed );
			return returnValue;
		}

		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamUtils_GetAPICallResult", CallingConvention = Platform.CC )]
		[return: MarshalAs( UnmanagedType.I1 )]
		private static extern bool _GetAPICallResult( IntPtr self, SteamAPICall_t hSteamAPICall, IntPtr pCallback, int cubCallback, int iCallbackExpected, [MarshalAs( UnmanagedType.U1 )] ref bool pbFailed );

		#endregion
		internal bool GetAPICallResult( SteamAPICall_t hSteamAPICall, IntPtr pCallback, int cubCallback, int iCallbackExpected, [MarshalAs( UnmanagedType.U1 )] ref bool pbFailed )
		{
			var returnValue = _GetAPICallResult( Self, hSteamAPICall, pCallback, cubCallback, iCallbackExpected, ref pbFailed );
			return returnValue;
		}

		#region FunctionMeta
		[DllImport( Platform.LibraryName, EntryPoint = "SteamAPI_ISteamUtils_IsSteamRunningOnSteamDeck", CallingConvention = Platform.CC )]
		[return: MarshalAs( UnmanagedType.I1 )]
		private static extern bool _IsSteamRunningOnSteamDeck( IntPtr self );

		#endregion
		internal bool IsSteamRunningOnSteamDeck()
		{
			var returnValue = _IsSteamRunningOnSteamDeck( Self );
			return returnValue;
		}

	}
}
