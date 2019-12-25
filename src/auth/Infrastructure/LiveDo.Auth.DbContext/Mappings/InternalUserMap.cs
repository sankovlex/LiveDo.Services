using LiveDo.Auth.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiveDo.Auth.UsersDbContext.Mappings
{
	internal class InternalUserMap : IEntityTypeConfiguration<InternalUser>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<InternalUser> builder)
		{
			builder.Property(u => u.PasswordHash)
				.IsFixedLength()
				.HasMaxLength(32);
		}
	}
}
