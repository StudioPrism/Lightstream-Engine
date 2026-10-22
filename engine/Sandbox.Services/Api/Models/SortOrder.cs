namespace Sandbox.Services;

public record struct SortOrder( string Name, string Title, string Icon );

/// <summary>
/// Returned when favouriting or unfavouriting a package
/// </summary>
public record struct PackageFavouriteResult( bool Success, bool State, int Total );

/// <summary>
/// Returned when rating a package
/// </summary>
public record struct PackageRateResult( bool Success, int VotesUp, int VotesDown );


public enum AggregationType : byte
{
	Sum,
	Highest,
	Lowest,
	Latest,
	Median
}
