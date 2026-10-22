namespace Sandbox.Services;

/// <summary>
/// Implements SteamUgc, lets us edit/create workshop items
/// </summary>
internal static partial class Ugc
{
	internal static Task<UgcPublisher> CreateCommunityItem()
	{
		var item = NativeEngine.CUgcUpdate.CreateCommunityItem();
		return FinishCreatingItem( item );
	}

	internal static Task<UgcPublisher> CreateMtxItem()
	{
		var item = NativeEngine.CUgcUpdate.CreateMtxItem();
		return FinishCreatingItem( item );
	}

	static async Task<UgcPublisher> FinishCreatingItem( NativeEngine.CUgcUpdate item )
	{
		while ( item.m_creating )
		{
			await Task.Delay( 10 );
		}

		if ( !item.m_created )
		{
			Log.Warning( $"Couldn't create item {item.m_resultCode}\n" );
			((IDisposable)item).Dispose();
			return null;
		}

		return new UgcPublisher( item );
	}

	internal static UgcPublisher OpenItem( ulong itemId )
	{
		var item = NativeEngine.CUgcUpdate.OpenCommunityItem( itemId );
		return new UgcPublisher( item );
	}

}
