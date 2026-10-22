namespace Sandbox.Services;

/// <summary>
/// Describes a specific user's interaction with this package
/// </summary>
public struct PackageInteraction
{
	public bool Favourite { get; set; }
	public DateTimeOffset? FavouriteCreated { get; set; }
	public int? Rating { get; set; }
	public DateTimeOffset? RatingCreated { get; set; }
	public bool Used { get; set; }
	public DateTimeOffset? FirstUsed { get; set; }
	public DateTimeOffset? LastUsed { get; set; }
	public long Sessions { get; set; }
	public long Seconds { get; set; }

	/// <summary>
	/// Total achievement score from this package
	/// </summary>
	public int AchievementScore { get; set; }

	/// <summary>
	/// Count of unlocked achievements (allows showing a % unlocked etc)
	/// </summary>
	public int AchievementCount { get; set; }
}
