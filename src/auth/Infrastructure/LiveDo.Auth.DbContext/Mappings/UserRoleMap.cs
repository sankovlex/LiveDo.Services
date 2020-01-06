using LiveDo.Auth.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiveDo.Auth.UsersDbContext.Mappings
{
	internal class UserRoleMap : IEntityTypeConfiguration<IdentityUserRole<string>>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
		{
			builder.HasKey(r => new
			{
				r.UserId, r.RoleId
			});
			
			builder.ToTable("UserRoles");
		}
	}
}
