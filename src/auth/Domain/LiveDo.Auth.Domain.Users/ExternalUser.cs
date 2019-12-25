using System;

namespace LiveDo.Auth.Domain.Users
{
	public sealed class ExternalUser : User
	{
		/// <inheritdoc />
		public ExternalUser(Guid id, string email)
			: base(id, email)
		{
		}
	}
}
