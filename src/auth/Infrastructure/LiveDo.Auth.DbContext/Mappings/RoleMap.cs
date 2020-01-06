using LiveDo.Auth.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiveDo.Auth.UsersDbContext.Mappings
{
	internal class RoleMap : IEntityTypeConfiguration<IdentityRole>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<IdentityRole> builder)
		{
			builder.HasKey(r => r.Id);
			
			builder.HasIndex(r => r.NormalizedName)
				.HasName("IX_RoleName")
				.IsUnique();
			
			builder.ToTable("Roles");
			
			builder.Property(r => r.ConcurrencyStamp)
				.IsConcurrencyToken();

			builder.Property(u => u.Name)
				.HasMaxLength(256);
			builder.Property(u => u.NormalizedName)
				.HasMaxLength(256);

			builder
				.HasMany<IdentityUserRole<string>>()
				.WithOne()
				.HasForeignKey(ur => ur.RoleId)
				.IsRequired();
			
			builder
				.HasMany<IdentityRoleClaim<string>>()
				.WithOne()
				.HasForeignKey(rc => rc.RoleId)
				.IsRequired();
		}
	}
}
