namespace Sandbox
{
	/// <summary>
	/// We might have multiple rich presence systems running (Steam, Discord, ...)
	/// </summary>
	internal interface IRichPresenceSystem
	{
		public void Poll();
	}

	/// <summary>
	/// Rich Presence System - polls rich presence state periodically.
	/// All in one place so we don't update rich presence in 100 places
	/// </summary>
	internal static class RichPresenceSystem
	{
		/// <summary>
		/// The current rich presence system
		/// </summary>
		static IRichPresenceSystem Current { get; set; } = new SteamRichPresenceSystem();

		static RealTimeUntil TimeUntilNextPoll = 0;

		/// <summary>
		/// Called by ClientDll Tick to poll active rich presence systems
		/// </summary>
		internal static void Tick()
		{
			if ( TimeUntilNextPoll )
			{
				Current.Poll();

				// Poll every 5 seconds
				TimeUntilNextPoll = 5;
			}
		}
	}
}
