namespace GeminiCustomer.Application.Common.Interfaces.Authentication;

public interface IPasswordHasher
{
    string HashPasword(string password, out byte[] salt);
    bool VerifyPassword(string password, string hash, byte[] salt);
}