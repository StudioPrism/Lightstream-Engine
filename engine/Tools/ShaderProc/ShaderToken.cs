using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facepunch.ShaderProc
{
	class ShaderToken
	{
		public string Token;
		public ShaderToken( string token )
		{
			Token = token;
		}
		public bool RemoveEverythingBefore( string expression )
		{
			var index = Token.IndexOf( expression );
			if ( index >= 0 )
			{
				Token = Token.Substring( index + expression.Length );
				return true;
			}


			return false;
		}
		public bool RemoveEverythingAfter( string expression )
		{
			var index = Token.IndexOf( expression );
			if ( index >= 0 )
			{
				Token = Token.Substring( 0, index );
				return true;
			}
			return false;
		}
		public bool RemoveEverythingBetween( string expStart, string expEnd, bool includingItself = true )
		{
			int extraTrail = includingItself ? expEnd.Length : 0;
			var iS = Token.IndexOf( expStart );
			var iE = Token.IndexOf( expEnd );

			if ( iE >= 0 && iS >= 0 )
			{
				Token = Token.Remove( iS, iE - iS + extraTrail );
				return true;
			}

			return false;
		}
		public override string ToString()
		{
			return Token;
		}
	}
}
