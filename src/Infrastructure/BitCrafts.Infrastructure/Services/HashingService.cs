using BitCrafts.Infrastructure.Abstraction.Services;

namespace BitCrafts.Infrastructure.Services;

public sealed class HashingService : IHashingService
{
    public string HashPassword(string password)
    {
        var salt = BCrypt.Net.BCrypt.GenerateSalt();
        return BCrypt.Net.BCrypt.HashPassword(password, salt);
    }

    public bool VerifyPassword(string password, string hashedPassword, string salt)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }


    public string GenerateSalt()
    {
        return BCrypt.Net.BCrypt.GenerateSalt();
    }
}