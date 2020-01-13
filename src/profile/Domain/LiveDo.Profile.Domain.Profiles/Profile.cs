using System;
using System.Collections.Generic;
using System.Linq;
using LiveDo.Abstractions.Domain;
using LiveDo.Profile.Domain.Profiles.ValueObjects;

namespace LiveDo.Profile.Domain.Profiles
{
	public class Profile : Entity<Guid>, IAggregateRoot
	{
		private readonly IList<Interest> _interests;
		
		/// <inheritdoc />
		protected Profile()
			: base(Guid.Empty)
		{
			_interests = new List<Interest>();
		}

		public Profile(Account account, DayOfBirth dayOfBirth, Gender gender)
			: this()
		{
			Account = account;
			DayOfBirth = dayOfBirth;
			Gender = gender;
		}

		public Account Account { get; private set; }

		public DayOfBirth DayOfBirth { get; private set; }

		public Gender Gender { get; private set; }

		public IEnumerable<Interest> Interests
			=> _interests.AsEnumerable();

		public void AddInterest(Interest interest)
		{
			if (interest == null)
			{
				throw new ArgumentNullException(nameof(interest));
			}

			_interests.Add(interest);
		}
	}
}
