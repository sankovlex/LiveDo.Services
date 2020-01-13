using System;
using LiveDo.Abstractions.Domain;

namespace LiveDo.Profile.Domain.Profiles.ValueObjects
{
	public class Account : ValueObject
	{
		/// <inheritdoc />
		public Account(Guid id, string name, string email)
		{
			Id = id;
			Name = name;
			Email = email;
		}

		public Guid Id { get; private set; }

		public string Name { get; private set; }

		public string Email { get; private set; }
		
		/// <inheritdoc />
		protected override bool Equals(ValueObject other)
		{
			throw new System.NotImplementedException();
		}

		/// <inheritdoc />
		protected override int GetValueObjectHashCode()
		{
			throw new System.NotImplementedException();
		}
	}
}
