using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using LiveDo.Auth.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace LiveDo.Auth.WebApp.Services
{
	/// <inheritdoc />
	internal class ProfileService : IProfileService
	{
		private readonly IUserStore<User> _userStore;
		private readonly UserManager<User> _userManager;

		/// <summary>
		/// Initialize instance of <see cref="ProfileService"/>
		/// </summary>
		/// <param name="userStore">User repository.</param>
		/// <param name="userManager">User manage service.</param>
		public ProfileService(
			IUserStore<User> userStore,
			UserManager<User> userManager)
		{
			_userStore = userStore 
				?? throw new ArgumentNullException(nameof(userStore));
			_userManager = userManager 
				?? throw new ArgumentNullException(nameof(userManager));
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

				User user = await _userStore.FindByIdAsync(
					subjectClaim.Value, 
					CancellationToken.None);

				if (user != null)
				{
					IList<Claim> claims = await _userManager.GetClaimsAsync(user);
					context.AddRequestedClaims(claims);
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

			User user = await _userStore.FindByIdAsync(
				subjectId, 
				CancellationToken.None);

			context.IsActive = user != null;
		}
	}
}
