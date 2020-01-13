namespace LiveDo.Profile.Domain.Profiles.ValueObjects
{
	public interface IDayOfBirthParser
	{
		DayOfBirth Parse(string value);
	}
}
