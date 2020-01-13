using System;
using LiveDo.Abstractions.Domain;

namespace LiveDo.Profile.Domain.Profiles
{
	// from another app
	public class Interest : ValueObject
	{
		
		
		/// <inheritdoc />
		protected override bool Equals(ValueObject other)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		protected override int GetValueObjectHashCode()
		{
			throw new NotImplementedException();
		}
	}
}
