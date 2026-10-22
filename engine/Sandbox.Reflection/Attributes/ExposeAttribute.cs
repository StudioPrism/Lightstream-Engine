/// <summary>
/// <para>
/// If set on a type, it (and its descendants) can be created
/// and manipulated via the TypeLibrary system, and therefore also in action graphs.
/// </para>
/// <para>
/// Note that this is only useful for our internal libraries because
/// everything in compiled (addons) assemblies is accessible anyway.
/// </para>
/// </summary>
internal sealed class ExposeAttribute : System.Attribute
{
}
