using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace LiveDo.Auth.Domain.Users.Services.Password
{
	public static class PasswordHashGenerator
	{
		public static byte[] HashPasswordV3(string password, RandomNumberGenerator rng, int iterationsCount)
		{
			return HashPasswordV3(password, rng,
				prf: KeyDerivationPrf.HMACSHA256,
				iterCount: iterationsCount,
				saltSize: 128 / 8,
				numBytesRequested: 256 / 8);
		}

		private static byte[] HashPasswordV3(
			string password,
			RandomNumberGenerator rng,
			KeyDerivationPrf prf,
			int iterCount,
			int saltSize,
			int numBytesRequested)
		{
			var salt = new byte[saltSize];
			rng.GetBytes(salt);
			byte[] subKey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, numBytesRequested);

			var outputBytes = new byte[13 + salt.Length + subKey.Length];
			outputBytes[0] = 0x01; // format marker
			WriteNetworkByteOrder(outputBytes, 1, (uint)prf);
			WriteNetworkByteOrder(outputBytes, 5, (uint)iterCount);
			WriteNetworkByteOrder(outputBytes, 9, (uint)saltSize);
			Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
			Buffer.BlockCopy(subKey, 0, outputBytes, 13 + saltSize, subKey.Length);
			return outputBytes;
		}
		
		private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
		{
			buffer[offset + 0] = (byte)(value >> 24);
			buffer[offset + 1] = (byte)(value >> 16);
			buffer[offset + 2] = (byte)(value >> 8);
			buffer[offset + 3] = (byte)(value >> 0);
		}
	}
}
