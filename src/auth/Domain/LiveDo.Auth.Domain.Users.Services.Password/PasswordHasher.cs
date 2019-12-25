using System;
using System.Linq;
using System.Security.Cryptography;

namespace LiveDo.Auth.Domain.Users.Services.Password
{
	/// <inheritdoc />
	public class PasswordHasher : IPasswordHasher
	{
		private const int IterationsCount = 5;
		private readonly RandomNumberGenerator _randomNumberGenerator 
			= new RNGCryptoServiceProvider();

		/// <inheritdoc />
		public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
		{
			if (string.IsNullOrEmpty(hashedPassword))
			{
				throw new ArgumentException(
					message: "Value cannot be null or empty.",
					paramName: nameof(hashedPassword));
			}

			if (string.IsNullOrEmpty(providedPassword))
			{
				throw new ArgumentException(
					message: "Value cannot be null or empty.",
					paramName: nameof(providedPassword));
			}

			byte[] base64HashedPassword = Convert.FromBase64String(hashedPassword);

			if (!base64HashedPassword.Any())
			{
				return false;
			}

			return PasswordHashChecker.VerifyHashedPassword(
				base64HashedPassword,
				providedPassword,
				IterationsCount);
		}

		/// <inheritdoc />
		public string HashPassword(string password)
		{
			if (string.IsNullOrEmpty(password))
			{
				throw new ArgumentException(
					message: "Value cannot be null or empty.",
					paramName: nameof(password));
			}

			byte[] hash = PasswordHashGenerator.HashPasswordV3(
				password,
				_randomNumberGenerator,
				IterationsCount);

			return Convert.ToBase64String(hash);
		}
	}
}
