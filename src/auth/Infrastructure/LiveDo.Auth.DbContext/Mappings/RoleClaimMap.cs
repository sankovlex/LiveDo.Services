using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiveDo.Auth.UsersDbContext.Mappings
{
	internal class RoleClaimMap : IEntityTypeConfiguration<IdentityRoleClaim<string>>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
		{
			builder.HasKey(rc => rc.Id);
			
			builder.ToTable("RoleClaims");
		}
	}
}
