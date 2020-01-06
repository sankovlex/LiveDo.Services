using LiveDo.Auth.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiveDo.Auth.UsersDbContext.Mappings
{
	internal class TokenMap : IEntityTypeConfiguration<IdentityUserToken<string>>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder)
		{
			builder.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

			builder.Property(t => t.LoginProvider)
				.HasMaxLength(255);
			
			builder.Property(t => t.Name)
				.HasMaxLength(255);

			builder.ToTable("UserTokens");
		}
	}
}
