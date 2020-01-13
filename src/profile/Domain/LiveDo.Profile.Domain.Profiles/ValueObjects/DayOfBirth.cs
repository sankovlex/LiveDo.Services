using System;
using System.Text.RegularExpressions;
using LiveDo.Abstractions.Domain;

namespace LiveDo.Profile.Domain.Profiles.ValueObjects
{
	public class DayOfBirth : ValueObject
	{
		/// <inheritdoc />
		public DayOfBirth(int day, int month, int year)
		{
			Day = day;
			Month = month;
			Year = year;
		}
		
		public int Day { get; private set; }
		
		public int Month { get; private set; }
		
		public int Year { get; private set; }

		public static DayOfBirth Parse(string value)
		{
			var parser = new RegexDayOfBirthParser();

			return parser.Parse(value);
		}

		public static DayOfBirth Parse(string value, IDayOfBirthParser parser)
		{
			return parser.Parse(value);
		}
		
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
