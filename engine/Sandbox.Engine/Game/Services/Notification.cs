namespace Sandbox.Services;

/// <summary>
/// Player notification
/// </summary>
public sealed class Notification
{
	public DateTimeOffset Created { get; init; }
	public DateTimeOffset Updated { get; init; }
	public int Count { get; init; }
	public DateTimeOffset? Read { get; init; }
	public string NoticeType { get; init; }
	public string Url { get; init; }
	public string Icon { get; init; }
	public string Text { get; init; }
	public Dictionary<string, object> Data { get; init; }

	internal static async Task<int> GetCount()
	{
		try
		{
			return await Sandbox.Backend.Notifications.GetCount();
		}
		catch
		{
			return default;
		}
	}

	internal static async Task<int> MarkRead()
	{
		try
		{
			return await Sandbox.Backend.Notifications.MarkRead();
		}
		catch
		{
			return default;
		}
	}

	internal static async Task<Notification[]> Get( int count )
	{
		try
		{
			var t = await Sandbox.Backend.Notifications.Get( count );
			if ( t is null ) return Array.Empty<Notification>();

			return t.Select( x => From( x ) ).ToArray();
		}
		catch
		{
			return default;
		}
	}


	internal static Notification From( NotificationDto p )
	{
		if ( p is null ) return default;

		return new Notification
		{
			Created = p.Created,
			Updated = p.Updated,
			Count = p.Count,
			Read = p.Read,
			NoticeType = p.NoticeType,
			Url = p.Url,
			Icon = p.Icon,
			Text = p.Text,
			Data = p.Data
		};
	}
}
