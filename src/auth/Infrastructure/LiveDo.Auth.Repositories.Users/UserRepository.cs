using System;
using System.Threading;
using System.Threading.Tasks;
using LiveDo.Abstractions.Repository.EntityFrameworkCore;
using LiveDo.Auth.Domain.Users;
using LiveDo.Auth.UsersDbContext;
using Microsoft.EntityFrameworkCore;

namespace LiveDo.Auth.Repositories.Users
{
	/// <inheritdoc cref="IUserRepository" />
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
		public async Task<User> GetByUsernameAsync(string username, CancellationToken cancellationToken)
		{
			User user = await _dbContext.Users
				.FirstOrDefaultAsync(
					u => u.Email == username,
					cancellationToken);

			return user;
		}

		/// <inheritdoc />
		public async Task<bool> IsExistedAsync(string email, CancellationToken cancellationToken)
		{
			bool isExists = await _dbContext.Users
				.AnyAsync(
					u => u.Email == email,
					cancellationToken);

			return isExists;
		}
	}
}
