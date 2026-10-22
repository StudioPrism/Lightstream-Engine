using System;

namespace Sandbox.UI;

/// <summary>
/// A panel's property will be inherited from its parent.
/// </summary>
[AttributeUsage( AttributeTargets.Property )]
public class CascadingParameterAttribute : System.Attribute
{

}
