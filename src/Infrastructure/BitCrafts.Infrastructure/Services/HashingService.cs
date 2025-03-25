using BitCrafts.Infrastructure.Abstraction.Services;

namespace BitCrafts.Infrastructure.Services;

public sealed class HashingService : IHashingService
{
    public string HashPassword(string password, string salt)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentNullException(nameof(password));
        if (string.IsNullOrWhiteSpace(salt))
            throw new ArgumentNullException(nameof(salt));

        return BCrypt.Net.BCrypt.HashPassword(password, salt);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentNullException(nameof(password));
        if (string.IsNullOrWhiteSpace(hashedPassword))
            throw new ArgumentNullException(nameof(hashedPassword));

        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }


    public string GenerateSalt()
    {
        return BCrypt.Net.BCrypt.GenerateSalt();
    }
}