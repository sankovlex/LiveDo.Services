using LiveDo.Auth.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiveDo.Auth.UsersDbContext.Mappings
{
	internal class LoginMap : IEntityTypeConfiguration<IdentityUserLogin<string>>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
		{
			builder.HasKey(l => new { l.LoginProvider, l.ProviderKey });
			
			builder.Property(l => l.LoginProvider)
				.HasMaxLength(255);
			
			builder.Property(l => l.ProviderKey)
				.HasMaxLength(255);

			builder.ToTable("UserLogins");
		}
	}
}
