using Refit;

namespace Sandbox.Services;

public partial class ServiceApi
{
	public interface INotificationApi
	{
		[Get( "/notifications" )]
		Task<NotificationDto[]> Get( int take );

		[Get( "/notifications/count" )]
		Task<int> GetCount();

		[Post( "/notifications/read/" )]
		Task<int> MarkRead();
	}
}
