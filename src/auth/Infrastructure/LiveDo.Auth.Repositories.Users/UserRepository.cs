using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiveDo.Abstractions.Repository.EntityFrameworkCore;
using LiveDo.Auth.Domain.Users;
using LiveDo.Auth.UsersDbContext;
using Microsoft.EntityFrameworkCore;

namespace LiveDo.Auth.Repositories.Users
{
	public class UserRepository : Repository<User, Guid>, IUserRepository
	{
		private readonly AuthDbContext _dbContext;

		/// <inheritdoc />
		public UserRepository(AuthDbContext dbContext)
			: base(dbContext)
		{
			_dbContext = dbContext 
				?? throw new ArgumentNullException(nameof(dbContext));
		}

		/// <inheritdoc />
		public async Task<User> GetByUsernameAndPasswordAsync(
			string username,
			string passwordHash,
			CancellationToken cancellationToken)
		{
			var user = await _dbContext.Users
				.OfType<InternalUser>()
				.FirstOrDefaultAsync(
					u => 
						u.Email == username && 
						u.PasswordHash == passwordHash, 
				cancellationToken);

			return user;
		}
	}
}
