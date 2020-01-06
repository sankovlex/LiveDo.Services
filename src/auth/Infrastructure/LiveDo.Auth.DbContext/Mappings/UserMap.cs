using System;
using LiveDo.Auth.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiveDo.Auth.UsersDbContext.Mappings
{
	internal class UserMap : IEntityTypeConfiguration<User>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.HasKey(u => u.Id);
			
			builder.HasIndex(u => u.NormalizedUserName)
				.HasName("IX_UserName")
				.IsUnique();
			
			builder.HasIndex(u => u.NormalizedEmail)
				.HasName("IX_Email");
			
			builder.ToTable("Users");
			
			builder.Property(u => u.ConcurrencyStamp)
				.IsConcurrencyToken();

			builder.Property(u => u.UserName)
				.HasMaxLength(256);
			
			builder.Property(u => u.NormalizedUserName)
				.HasMaxLength(256);
			builder.Property(u => u.Email)
				.HasMaxLength(256);
			builder.Property(u => u.NormalizedEmail)
				.HasMaxLength(256);

			builder
				.HasMany<IdentityUserClaim<string>>()
				.WithOne()
				.HasForeignKey(uc => uc.UserId)
				.IsRequired();
			
			builder
				.HasMany<IdentityUserLogin<string>>()
				.WithOne()
				.HasForeignKey(ul => ul.UserId)
				.IsRequired();
			
			builder.HasMany<IdentityUserLogin<string>>()
				.WithOne()
				.HasForeignKey(ut => ut.UserId)
				.IsRequired();
			
			builder
				.HasMany<IdentityUserRole<string>>()
				.WithOne()
				.HasForeignKey(ur => ur.UserId)
				.IsRequired();
		}
	}
}
