using System;

namespace LiveDo.Auth.Domain.Users
{
	public sealed class InternalUser : User
	{
		/// <inheritdoc />
		public InternalUser(Guid id, string email, string passwordHash)
			: base(id, email)
		{
			PasswordHash = passwordHash;
		}

		public string PasswordHash { get; private set; }
	}
}
