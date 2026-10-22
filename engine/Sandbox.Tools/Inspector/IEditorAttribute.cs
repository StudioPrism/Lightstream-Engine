
namespace Editor
{
	namespace Internal
	{
		public interface IEditorAttributeBase
		{

		}
	}

	/// <summary>
	/// Allows an editor widget to provide an attribute that it can use. Editors with attributes are chosen
	/// over editors without when the target property has the provided attribute.
	/// </summary>
	public interface IEditorAttribute<T> : Internal.IEditorAttributeBase where T : System.Attribute
	{
		public void SetEditorAttribute( T attribute );
	}
}
