using Sandbox.Network;

namespace Sandbox;

public abstract partial class Connection
{
	public struct Filter
	{
		public enum FilterType
		{
			/// <summary>
			/// Only include the connections in the filter when sending a message.
			/// </summary>
			Include,

			/// <summary>
			/// Exclude the connections in the filter when sending a message.
			/// </summary>
			Exclude
		}

		private IEnumerable<Connection> Connections { get; set; }
		private Predicate<Connection> Predicate { get; set; }
		private FilterType Type { get; set; }

		public Filter( FilterType type, Predicate<Connection> predicate )
		{
			Predicate = predicate;
			Type = type;
		}

		public Filter( FilterType type, IEnumerable<Connection> connections )
		{
			Connections = connections;
			Type = type;
		}

		/// <summary>
		/// Is the specified <see cref="Connection"/> a valid recipient?
		/// </summary>
		public bool IsRecipient( Connection connection )
		{
			if ( Type == FilterType.Exclude )
				return !Predicate?.Invoke( connection ) ?? !Connections.Contains( connection );

			return Predicate?.Invoke( connection ) ?? Connections.Contains( connection );
		}
	}
}
