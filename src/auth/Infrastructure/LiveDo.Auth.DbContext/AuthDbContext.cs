using LiveDo.Auth.Domain.Users;
using LiveDo.Auth.UsersDbContext.Mappings;
using Microsoft.EntityFrameworkCore;

namespace LiveDo.Auth.UsersDbContext
{
	/// <summary>
	/// Auth database context.
	/// </summary>
	public class AuthDbContext : DbContext
	{
		/// <inheritdoc />
		public AuthDbContext(DbContextOptions options)
			: base(options)
		{
		}

		/// <summary>
		/// Users.
		/// </summary>
		public DbSet<User> Users { get; set; }

		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new UserMap());
			modelBuilder.ApplyConfiguration(new InternalUserMap());
		}
	}
}
