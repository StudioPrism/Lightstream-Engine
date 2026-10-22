namespace Sandbox.Menu;

partial struct LoadingProgress
{
	public readonly TimeSpan CalculateETA()
	{
		if ( Mbps <= 0 || Fraction >= 1 || TotalSize <= 0 )
			return TimeSpan.MaxValue;

		if ( Fraction < 0.25 )
			return TimeSpan.MaxValue;

		var remainingBytes = TotalSize * (1 - Fraction);
		var bytesPerSecond = Mbps * 1_000_000 / 8; // Convert Mbps to bytes per second
		var secondsRemaining = remainingBytes / bytesPerSecond;

		var roundedSeconds = Math.Max( 1, (int)Math.Round( secondsRemaining ) );

		return TimeSpan.FromSeconds( roundedSeconds );
	}
}
