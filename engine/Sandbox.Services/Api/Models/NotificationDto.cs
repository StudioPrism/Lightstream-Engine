namespace Sandbox.Services;

public class NotificationDto
{
	public DateTimeOffset Created { get; set; }
	public DateTimeOffset Updated { get; set; }
	public int Count { get; set; }
	public DateTimeOffset? Read { get; set; }
	public string NoticeType { get; set; }
	public string Url { get; set; }
	public string Icon { get; set; }
	public string Text { get; set; }
	public Dictionary<string, object> Data { get; set; }
}
