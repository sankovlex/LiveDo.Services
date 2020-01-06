using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Validation;
using LiveDo.Auth.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace LiveDo.Auth.WebApp.Services
{
	/// <inheritdoc />
	internal class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
	{
		private readonly UserManager<User> _userManager;

		/// <summary>
		/// Initialize instance of <see cref="ResourceOwnerPasswordValidator"/>
		/// </summary>
		public ResourceOwnerPasswordValidator(UserManager<User> userManager)
		{
			_userManager = userManager 
				?? throw new ArgumentNullException(nameof(userManager));
		}

		/// <inheritdoc />
		public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
		{
			User user = await _userManager.FindByNameAsync(context.UserName);

			if (user == null)
			{
				return;
			}

			bool passwordVerified = await _userManager
				.CheckPasswordAsync(
					user,
					context.Password);
			
			if (!passwordVerified)
			{
				return;
			}

			IList<Claim> claims = await _userManager.GetClaimsAsync(user);
			
			context.Result = new GrantValidationResult(
				subject: user.Id,
				authenticationMethod: OidcConstants.AuthenticationMethods.Password,
				authTime: DateTime.UtcNow,
				claims: claims);
		}
	}
}
