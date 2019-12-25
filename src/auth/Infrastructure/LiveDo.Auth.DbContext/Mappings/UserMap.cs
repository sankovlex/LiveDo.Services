using System;
using LiveDo.Auth.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiveDo.Auth.UsersDbContext.Mappings
{
	internal class UserMap : IEntityTypeConfiguration<User>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("Users");
			
			builder.HasKey(u => u.Id)
				.HasName("Id");

			builder.Property(u => u.Email)
				.HasColumnName("Email")
				.HasMaxLength(320)
				.IsRequired()
				.IsUnicode();

			builder.Property(u => u.IsActive)
				.HasDefaultValue(false)
				.IsRequired();

			builder.HasDiscriminator<string>("AuthType")
				.HasValue<InternalUser>("internal")
				.HasValue<ExternalUser>("external");

			builder.OwnsMany(user => user.Claims, owns =>
			{
				owns.WithOwner()
					.HasForeignKey("UserId");
				
				owns.Property<Guid>("Id");
				
				owns.HasKey("Id");
				
				owns.Property(p => p.Type)
					.HasColumnName("Type")
					.IsRequired();
				
				owns.Property(p => p.Value)
					.HasColumnName("Value")
					.IsRequired();
			});
		}
	}
}
