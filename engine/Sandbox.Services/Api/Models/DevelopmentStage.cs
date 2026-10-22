using System.ComponentModel.DataAnnotations;

namespace Sandbox.Services;

public enum DevelopmentStage : int
{
	[Display( Name = "Hidden" )]
	Hidden = 0,

	[Display( Name = "Released" )]
	Released = 1,
}
