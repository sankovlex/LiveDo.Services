using System.Security.Claims;
using LiveDo.Auth.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiveDo.Auth.UsersDbContext.Mappings
{
	internal class ClaimsMap : IEntityTypeConfiguration<UserClaim>
	{
		/// <inheritdoc />
		public void Configure(EntityTypeBuilder<UserClaim> builder)
		{
			builder.ToTable("UserClaims");
			
			builder.Property(c => c.Type)
				.IsRequired();

			builder.Property(c => c.Value)
				.IsRequired();
		}
	}
}
