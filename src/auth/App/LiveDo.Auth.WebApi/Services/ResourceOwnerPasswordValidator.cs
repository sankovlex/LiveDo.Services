using System;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Validation;
using LiveDo.Auth.Domain.Users;
using LiveDo.Auth.Domain.Users.Services;
using Microsoft.AspNetCore.Identity;

namespace LiveDo.Auth.WebApi.Services
{
	/// <inheritdoc />
	internal class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
	{
		private readonly IUserRepository _userRepository;
		private readonly IPasswordHasher _passwordHasher;

		/// <summary>
		/// Initialize instance of <see cref="ResourceOwnerPasswordValidator"/>
		/// </summary>
		/// <param name="userRepository"></param>
		/// <param name="passwordHasher"></param>
		public ResourceOwnerPasswordValidator(
			IUserRepository userRepository,
			IPasswordHasher passwordHasher)
		{
			_userRepository = userRepository 
				?? throw new ArgumentNullException(nameof(userRepository));
			_passwordHasher = passwordHasher 
				?? throw new ArgumentNullException(nameof(passwordHasher));
		}

		/// <inheritdoc />
		public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
		{
			string passwordHash = _passwordHasher.HashPassword(context.Password);
			
			User user = await _userRepository.GetByUsernameAndPasswordAsync(
				context.UserName,
				passwordHash,
				CancellationToken.None);

			if (user == null)
			{
				return;
			}

			context.Result = new GrantValidationResult(
				subject: user.Id.ToString(),
				authenticationMethod: OidcConstants.AuthenticationMethods.Password,
				authTime: DateTime.UtcNow,
				claims: user.Claims);
		}
	}
}
