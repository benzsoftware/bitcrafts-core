namespace BitCrafts.Infrastructure.Abstraction.Services;

/// <summary>
///     Defines an interface for hashing services.
///     Hashing services are used for securely storing passwords and other sensitive data.
/// </summary>
public interface IHashingService
{
    /// <summary>
    ///     Hashes a password using a secure hashing algorithm.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>The hashed password.</returns>
    string HashPassword(string password);

    /// <summary>
    ///     Verifies a password against a stored hash.
    /// </summary>
    /// <param name="password">The password to verify.</param>
    /// <param name="hashedPassword">The stored hash to compare against.</param>
    /// <param name="salt">The salt used to hash the password.</param>
    /// <returns>
    ///     True if the password matches the hash; otherwise, false.
    /// </returns>
    bool VerifyPassword(string password, string hashedPassword, string salt);

    /// <summary>
    ///     Generates a salt value for use in password hashing.
    /// </summary>
    /// <returns>The generated salt.</returns>
    string GenerateSalt();
}