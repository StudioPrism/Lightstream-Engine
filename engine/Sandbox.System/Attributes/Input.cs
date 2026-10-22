namespace Sandbox
{
	/// <summary>
	/// Makes this method available as a Map Logic Input, for use in the Hammer Editor. This is only applicable to entities.
	/// </summary>
	[AttributeUsage( AttributeTargets.Method )]
	public class InputAttribute : Attribute
	{
		/// <summary>
		/// Desired name of this input. If not set, the method's name will be used.
		/// </summary>
		public string Name { get; set; }

		public InputAttribute() : base()
		{
		}

		public InputAttribute( string name )
		{
			Name = name.Replace( " ", "" ).Replace( "\t", "" );
		}
	}

}
