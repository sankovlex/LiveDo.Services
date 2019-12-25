using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace LiveDo.Auth.Domain.Users.Services.Password
{
	internal static class PasswordHashChecker
	{
		public static bool VerifyHashedPassword(byte[] hashedPassword, string password, int iterationCount)
		{
			try
			{
				var prf = (KeyDerivationPrf)ReadNetworkByteOrder(hashedPassword, 1);
				int saltLength = (int)ReadNetworkByteOrder(hashedPassword, 9);

				if (!CheckLength(saltLength))
				{
					return false;
				}

				byte[] salt = GetSalt(hashedPassword, saltLength);

				int payload = GetRestPayload(hashedPassword, salt);

				if (!CheckLength(payload))
				{
					return false;
				}

				var expectedSubKey = new byte[payload];
				Buffer.BlockCopy(
					src: hashedPassword,
					srcOffset: 13 + salt.Length,
					dst: expectedSubKey,
					dstOffset: 0, 
					count: expectedSubKey.Length);

				// Hash the incoming password and verify it
				byte[] actualSubKey = KeyDerivation.Pbkdf2(password, salt, prf, iterationCount, payload);
				return ByteArraysEqual(actualSubKey, expectedSubKey);
			}
			catch
			{
				// This should never occur except in the case of a malformed payload, where
				// we might go off the end of the array. Regardless, a malformed payload
				// implies verification failed.
				return false;
			}
		}

		private static int GetRestPayload(
			IReadOnlyCollection<byte> hashedPassword,
			IReadOnlyCollection<byte> salt)
		{
			int subKeyLength = hashedPassword.Count - 13 - salt.Count;

			return subKeyLength;
		}

		private static bool CheckLength(int source)
		{
			// must be >= 128 bits
			if (source < 128 / 8)
			{
				return false;
			}

			return true;
		}

		private static byte[] GetSalt(byte[] hashedPassword, int saltLength)
		{
			var salt = new byte[saltLength];
			Buffer.BlockCopy(
				src: hashedPassword,
				srcOffset: 13,
				dst: salt,
				dstOffset: 0,
				count: salt.Length);

			return salt;
		}

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		private static bool ByteArraysEqual(IReadOnlyList<byte> a, IReadOnlyList<byte> b)
		{
			if (a == null && b == null)
			{
				return true;
			}
			if (a == null || b == null || a.Count != b.Count)
			{
				return false;
			}
			
			bool areSame = true;
			for (int i = 0; i < a.Count; i++)
			{
				areSame &= (a[i] == b[i]);
			}
			return areSame;
		}

		private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
		{
			return ((uint)(buffer[offset + 0]) << 24)
				| ((uint)(buffer[offset + 1]) << 16)
				| ((uint)(buffer[offset + 2]) << 8)
				| ((uint)(buffer[offset + 3]));
		}
	}
}
