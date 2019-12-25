using System.Security.Claims;

namespace LiveDo.Auth.Domain.Users
{
	public class UserClaim : Claim
	{
		private UserClaim() : this(null, null) {}
		
		/// <inheritdoc />
		public UserClaim(string type, string value)
			: base(type, value)
		{
		}
	}
}
