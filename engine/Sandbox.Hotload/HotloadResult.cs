using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sandbox
{
	/// <summary>
	/// Used to categorize messages emitted when performing a hotload.
	/// </summary>
	public enum HotloadEntryType
	{
		/// <summary>
		/// Used for messages related to debugging or profiling.
		/// </summary>
		Trace,

		/// <summary>
		/// Used for general messages.
		/// </summary>
		Information,

		/// <summary>
		/// Hotload couldn't upgrade an instance, so you should reload the game to avoid runtime bugs.
		/// </summary>
		Warning,

		/// <summary>
		/// Something went wrong during the hotload that Facepunch should fix.
		/// </summary>
		Error
	}

	/// <summary>
	/// Contains information for an individual hotload result message or error.
	/// </summary>
	public sealed class HotloadResultEntry : IEquatable<HotloadResultEntry>
	{
		/// <summary>
		/// Hotload result category.
		/// </summary>
		public HotloadEntryType Type { get; set; }

		/// <summary>
		/// Contains the main information of the result.
		/// </summary>
		public FormattableString Message { get; set; }

		/// <summary>
		/// If the result type is <see cref="HotloadEntryType.Error"/>, contains the
		/// exception thrown.
		/// </summary>
		public Exception Exception { get; set; }

		/// <summary>
		/// When relevant, contains the member that this result relates to.
		/// </summary>
		public MemberInfo Member { get; set; }

		public Hotload.ReferencePath Path { get; set; }

		public HotloadResultEntry()
		{

		}

		internal HotloadResultEntry( Exception exception, FormattableString message = null, MemberInfo member = null, Hotload.ReferencePath path = null )
			: this( HotloadEntryType.Error, message ?? $"{exception.Message}", member, path )
		{
			Exception = exception;
		}

		internal HotloadResultEntry( HotloadEntryType type, FormattableString message, MemberInfo member = null, Hotload.ReferencePath path = null )
		{
			Type = type;
			Message = message;
			Member = member;
			Path = path;
		}

		public string MemberString => Member is Type type
			? $"{type.ToSimpleString()}.{Member.Name} ({Hotload.FormatAssemblyName( type.Assembly )})"
			: Member != null ? $"{Member.DeclaringType.ToSimpleString()}.{Member.Name} ({Hotload.FormatAssemblyName( Member.Module.Assembly )})"
				: null;

		public string Context
		{
			get
			{
				var ctx = MemberString;

				if ( Path != null )
				{
					ctx = ctx == null ? $"Path: {Path}" : $"{ctx}{Environment.NewLine}  Path: {Path}";
				}

				return ctx;
			}
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		public override string ToString()
		{
			var context = Context;

			if ( string.IsNullOrEmpty( context ) )
			{
				return Message.ToString();
			}

			return $"{Message}{Environment.NewLine}  {Context}";
		}

#pragma warning disable CS1591
		public override int GetHashCode()
		{
			return Member == null && Message == null ? 0 : Member == null ? Message.GetHashCode() : Member.GetHashCode();
		}

		public override bool Equals( object obj )
		{
			return obj is HotloadResultEntry && Equals( (HotloadResultEntry)obj );
		}

		public bool Equals( HotloadResultEntry other )
		{
			return ReferenceEquals( other.Member, Member ) && other.Type == Type;
		}
#pragma warning restore CS1591
	}

	public class TimingEntry
	{
		/// <summary>
		/// Total number of instances processed.
		/// </summary>
		public int Instances { get; set; }

		/// <summary>
		/// Total time taken processing instances.
		/// </summary>
		public double Milliseconds { get; set; }

		internal TimingEntry( int instances, TimeSpan timeSpan )
		{
			Instances = instances;
			Milliseconds = timeSpan.TotalMilliseconds;
		}

		public TimingEntry() { }
	}

	/// <summary>
	/// Holds information about the number of instances and total time taken when
	/// processing instances in a certain category.
	/// </summary>
	public class InstanceTimingEntry : TimingEntry
	{
		/// <summary>
		/// The full names and instance count for each static field that instances were found under.
		/// Only populated if <see cref="Hotload.TraceRoots"/> is set to true.
		/// </summary>
		public Dictionary<string, TimingEntry> Roots { get; }

		internal InstanceTimingEntry( bool traceRoots )
		{
			Roots = traceRoots ? new Dictionary<string, TimingEntry>() : null;
		}

		public InstanceTimingEntry() { }
	}

	/// <summary>
	/// Contains information about an assembly hotload event, including any warnings or errors emitted,
	/// the time taken to process instances of different types, and the total number of instances processed.
	/// </summary>
	public sealed class HotloadResult
	{
		public DateTime Created { get; set; } = DateTime.UtcNow;

		internal static HotloadResult NoActionSingleton { get; } = new HotloadResult { NoAction = true };

		/// <summary>
		/// Contains timing information for each type processed during the hotload.
		/// </summary>
		public Dictionary<string, InstanceTimingEntry> TypeTimings { get; set; } = new Dictionary<string, InstanceTimingEntry>();

		/// <summary>
		/// Contains timing information for each IInstanceProcessor during the hotload.
		/// </summary>
		public Dictionary<string, InstanceTimingEntry> ProcessorTimings { get; set; } = new Dictionary<string, InstanceTimingEntry>();

		/// <summary>
		/// If true, at least one error was emitted during the hotload. Information about the error(s) can
		/// be found in <see cref="Errors"/>.
		/// </summary>
		public bool HasErrors { get; internal set; }

		/// <summary>
		/// If true, at least one warning was emitted during the hotload. Information about the error(s) can
		/// be found in <see cref="Errors"/>.
		/// </summary>
		public bool HasWarnings { get; internal set; }

		/// <summary>
		/// If true, the hotload was skipped because no replacement assemblies were specified since the last
		/// hotload.
		/// </summary>
		public bool NoAction { get; set; }

		/// <summary>
		/// Total time elapsed during the hotload (in milliseconds)
		/// </summary>
		public double ProcessingTime { get; set; }

		public double InstanceQueueTime { get; set; }

		public double StaticFieldTime { get; set; }

		public double WatchedInstanceTime { get; set; }

		public double DiagnosticsTime { get; set; }

		/// <summary>
		/// If true, no errors were emitted during the hotload.
		/// </summary>
		public bool Success => !HasErrors;

		/// <summary>
		/// Total number of instances processed during the hotload.
		/// </summary>
		public int InstancesProcessed { get; set; }

		/// <summary>
		/// Retrieves all warnings, errors and other messages emitted during the hotload.
		/// </summary>
		public List<HotloadResultEntry> Entries { get; set; } = new List<HotloadResultEntry>();

		/// <summary>
		/// Types that were automatically determined to be safely skippable.
		/// </summary>
		public List<string> AutoSkippedTypes { get; set; } = new List<string>();

		/// <summary>
		/// Retrieves all error messages emitted during the hotload.
		/// </summary>
		public IEnumerable<HotloadResultEntry> Errors => Entries.Where( x => x.Type == HotloadEntryType.Error );

		/// <summary>
		/// Retrieves all warning messages emitted during the hotload.
		/// </summary>
		public IEnumerable<HotloadResultEntry> Warnings => Entries.Where( x => x.Type == HotloadEntryType.Warning );

		internal void AddEntry( HotloadResultEntry entry )
		{
			Entries.Add( entry );

			HasWarnings |= entry.Type == HotloadEntryType.Warning;
			HasErrors |= entry.Type == HotloadEntryType.Error;
		}
	}
}
