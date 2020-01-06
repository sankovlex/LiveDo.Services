using LiveDo.Auth.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiveDo.Auth.UsersDbContext.Mappings
{
	internal class UserClaimsMap : IEntityTypeConfiguration<IdentityUserClaim<string>>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
		{
			builder.HasKey(uc => uc.Id);
			
			builder.ToTable("UserClaims");
		}
	}
}
