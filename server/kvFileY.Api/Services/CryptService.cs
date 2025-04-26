namespace kvFileY.Api.Services;

public static class CryptService
{
    public static bool PasswordVerify(string inputPassword, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(inputPassword, passwordHash);
    }

    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}