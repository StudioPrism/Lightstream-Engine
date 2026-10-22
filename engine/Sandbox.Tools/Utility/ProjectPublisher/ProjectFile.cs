using System.Text.Json.Serialization;

namespace Editor;


public partial class ProjectPublisher
{
	/// <summary>
	/// A single file in the project revision manifest
	/// </summary>
	public class ProjectFile
	{
		public string Name { get; set; }
		public int Size { get; set; }
		public string Hash { get; set; }

		[JsonIgnore]
		public string AbsolutePath { get; set; }

		[JsonIgnore]
		public byte[] Contents { get; set; }

		[JsonIgnore]
		public bool Skip { get; set; }

		[JsonIgnore]
		public long SizeUploaded { get; set; }

		public override string ToString() => Name;
	}
}
