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
    /// <param name="salt">salt generated with <see cref="GenerateSalt"/>.</param>
    /// <returns>The hashed password.</returns>
    string HashPassword(string password, string salt);

    /// <summary>
    ///     Verifies a password against a stored hash.
    /// </summary>
    /// <param name="password">The password to verify.</param>
    /// <param name="hashedPassword">The stored hash to compare against.</param> 
    /// <returns>
    ///     True if the password matches the hash; otherwise, false.
    /// </returns>
    bool VerifyPassword(string password, string hashedPassword);

    /// <summary>
    ///     Generates a salt value for use in password hashing.
    /// </summary>
    /// <returns>The generated salt.</returns>
    string GenerateSalt();
}