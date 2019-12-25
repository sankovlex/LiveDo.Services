using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using LiveDo.Auth.Domain.Users;

namespace LiveDo.Auth.WebApi.Services
{
	/// <inheritdoc />
	internal class ProfileService : IProfileService
	{
		private readonly IUserRepository _userRepository;

		/// <summary>
		/// Initialize instance of <see cref="ProfileService"/>
		/// </summary>
		/// <param name="usersRepository">User repository.</param>
		public ProfileService(IUserRepository usersRepository)
		{
			_userRepository = usersRepository 
				?? throw new ArgumentNullException(nameof(usersRepository));
		}
		
		/// <inheritdoc />
		public async Task GetProfileDataAsync(ProfileDataRequestContext context)
		{
			if (context.RequestedClaimTypes.Any())
			{
				Claim subjectClaim = context.Subject.FindFirst(JwtClaimTypes.Subject);

				if (subjectClaim == null)
				{
					throw new InvalidOperationException("sub claim is missing.");
				}

				if (!Guid.TryParse(subjectClaim.Value, out Guid userId))
				{
					throw new InvalidOperationException("sub claim hasn't GUID type.");
				}

				User user = await _userRepository.GetByIdAsync(
					userId, 
					CancellationToken.None);

				if (user != null)
				{
					context.AddRequestedClaims(user.Claims);
				}
			}
		}

		/// <inheritdoc />
		public async Task IsActiveAsync(IsActiveContext context)
		{
			string subjectId = context.Subject.GetSubjectId();

			if (subjectId == null)
			{
				throw new InvalidOperationException("subjectId cannot be null.");
			}
			
			if (!Guid.TryParse(subjectId, out Guid userId))
			{
				throw new InvalidOperationException("subjectId hasn't GUID type.");
			}

			User user = await _userRepository.GetByIdAsync(
				userId, 
				CancellationToken.None);

			context.IsActive = user != null;
		}
	}
}
