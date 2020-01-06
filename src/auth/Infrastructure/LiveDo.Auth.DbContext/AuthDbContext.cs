using LiveDo.Auth.Domain.Users;
using LiveDo.Auth.UsersDbContext.Mappings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LiveDo.Auth.UsersDbContext
{
	/// <summary>
	/// Auth database context.
	/// </summary>
	public class AuthDbContext : IdentityDbContext<User>
	{
		/// <inheritdoc />
		public AuthDbContext(DbContextOptions<AuthDbContext> options)
			: base(options)
		{
		}

		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new LoginMap());
			modelBuilder.ApplyConfiguration(new RoleClaimMap());
			modelBuilder.ApplyConfiguration(new RoleMap());
			modelBuilder.ApplyConfiguration(new TokenMap());
			modelBuilder.ApplyConfiguration(new UserClaimsMap());
			modelBuilder.ApplyConfiguration(new UserMap());
			modelBuilder.ApplyConfiguration(new UserRoleMap());
		}
	}
}
