namespace LiveDo.Auth.Domain.Users.Services
{
	public interface IPasswordHasher
	{
		bool VerifyHashedPassword(string hashedPassword, string providedPassword);
		
		string HashPassword(string password);
	}
}
