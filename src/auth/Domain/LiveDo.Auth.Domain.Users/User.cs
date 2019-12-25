using System;
using System.Collections.Generic;
using System.Security.Claims;
using LiveDo.Abstractions.Domain;

namespace LiveDo.Auth.Domain.Users
{
	/// <summary>
	/// User.
	/// </summary>
	public abstract class User : Entity<Guid>, IAggregateRoot
	{
		private readonly IList<UserClaim> _claims;

		
		protected User()
			: this(Guid.Empty, null)
		{
			
		}

		/// <inheritdoc />
		protected User(Guid id, string email)
			: base(id)
		{
			_claims = new List<UserClaim>();
		}

		public string Email { get; private set; }
		
		public bool IsActive { get; private set; }

		public IEnumerable<UserClaim> Claims
			=> _claims;

		public void AddClaim(UserClaim claim)
		{
			if (claim == null)
			{
				throw new ArgumentNullException(nameof(claim));
			}
			
			_claims.Add(claim);
		}
	}
}
