using System;
using System.Threading;
using System.Threading.Tasks;
using LiveDo.Abstractions.Domain;

namespace LiveDo.Auth.Domain.Users
{
	public interface IUserRepository : IRepository<User, Guid>
	{
		Task<User> GetByUsernameAndPasswordAsync(
			string username,
			string passwordHash,
			CancellationToken cancellationToken);
	}
}
