using System;
using System.Collections.Generic;
using System.Text;

namespace Sandbox
{
	internal static class StyleHelpers
	{
		static internal float RotationDegrees( float val, string unit )
		{
			if ( unit.StartsWith( "grad" ) ) return val.GradiansToDegrees();
			if ( unit.StartsWith( "rad" ) ) return val.RadianToDegree();
			if ( unit.StartsWith( "turn" ) ) return val * 360.0f;
			if ( unit.StartsWith( "deg" ) ) return val;

			return val;
		}
	}
}
