namespace Sandbox;

/// <summary>
/// When applied to an attribute, which is them applied to a member..
/// This will make <see cref="MemberDescription"/> set on the attribute upon load.
/// <para>This provides a convenient way to know which member the attribute was attached to.</para>
/// </summary>
public interface IMemberAttribute
{
	/// <summary>
	/// Description of the member this attribute was attached to.
	/// </summary>
	MemberDescription MemberDescription { get; set; }
}
