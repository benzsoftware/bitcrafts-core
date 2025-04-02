using BitCrafts.Infrastructure.Abstraction.Services;
using BitCrafts.Infrastructure.Services;

// ReSharper disable ExpressionIsAlwaysNull

namespace BitCrafts.Infrastructure.Tests.Services;

[TestClass]
[TestCategory("Services")]
public class HashingServiceTests
{
    private IHashingService _hashingService;

    [TestInitialize]
    public void Initialize()
    {
        _hashingService = new HashingService();
    }

    #region HashPassword Tests

    [TestMethod]
    public void HashPassword_WithValidInput_ShouldReturnHashedString()
    {
        // Arrange
        var password = "MySecurePassword123";
        var salt = _hashingService.GenerateSalt();

        // Act
        var hashedPassword = _hashingService.HashPassword(password, salt);

        // Assert
        Assert.IsNotNull(hashedPassword);
        Assert.AreNotEqual(password, hashedPassword);
        Assert.IsTrue(hashedPassword.Length > 0);
    }

    [TestMethod]
    public void HashPassword_SamePasswordAndSalt_ShouldReturnSameHash()
    {
        // Arrange
        var password = "MySecurePassword123";
        var salt = _hashingService.GenerateSalt();

        // Act
        var hashedPassword1 = _hashingService.HashPassword(password, salt);
        var hashedPassword2 = _hashingService.HashPassword(password, salt);

        // Assert
        Assert.AreEqual(hashedPassword1, hashedPassword2);
    }

    [TestMethod]
    public void HashPassword_DifferentPasswords_ShouldReturnDifferentHashes()
    {
        // Arrange
        var password1 = "MySecurePassword123";
        var password2 = "MySecurePassword124";
        var salt = _hashingService.GenerateSalt();

        // Act
        var hashedPassword1 = _hashingService.HashPassword(password1, salt);
        var hashedPassword2 = _hashingService.HashPassword(password2, salt);

        // Assert
        Assert.AreNotEqual(hashedPassword1, hashedPassword2);
    }

    [TestMethod]
    public void HashPassword_DifferentSalts_ShouldReturnDifferentHashes()
    {
        // Arrange
        var password = "MySecurePassword123";
        var salt1 = _hashingService.GenerateSalt();
        var salt2 = _hashingService.GenerateSalt();

        // Act
        var hashedPassword1 = _hashingService.HashPassword(password, salt1);
        var hashedPassword2 = _hashingService.HashPassword(password, salt2);

        // Assert
        Assert.AreNotEqual(hashedPassword1, hashedPassword2);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void HashPassword_WithNullPassword_ShouldThrowArgumentNullException()
    {
        // Arrange
        string password = null;
        var salt = _hashingService.GenerateSalt();

        // Act
        _hashingService.HashPassword(password, salt);

        // Assert: ExpectedException
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void HashPassword_WithEmptyPassword_ShouldThrowArgumentNullException()
    {
        // Arrange
        var password = "";
        var salt = _hashingService.GenerateSalt();

        // Act
        _hashingService.HashPassword(password, salt);

        // Assert: ExpectedException
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void HashPassword_WithWhitespacePassword_ShouldThrowArgumentNullException()
    {
        // Arrange
        var password = "   ";
        var salt = _hashingService.GenerateSalt();

        // Act
        _hashingService.HashPassword(password, salt);

        // Assert: ExpectedException
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void HashPassword_WithNullSalt_ShouldThrowArgumentNullException()
    {
        // Arrange
        var password = "MySecurePassword123";
        string salt = null;

        // Act
        _hashingService.HashPassword(password, salt);

        // Assert: ExpectedException
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void HashPassword_WithEmptySalt_ShouldThrowArgumentNullException()
    {
        // Arrange
        var password = "MySecurePassword123";
        var salt = "";

        // Act
        _hashingService.HashPassword(password, salt);

        // Assert: ExpectedException
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void HashPassword_WithWhitespaceSalt_ShouldThrowArgumentNullException()
    {
        // Arrange
        var password = "MySecurePassword123";
        var salt = "   ";

        // Act
        _hashingService.HashPassword(password, salt);

        // Assert: ExpectedException
    }

    #endregion

    #region VerifyPassword Tests

    [TestMethod]
    public void VerifyPassword_WithCorrectPassword_ShouldReturnTrue()
    {
        // Arrange
        var password = "MySecurePassword123";
        var salt = _hashingService.GenerateSalt();
        var hashedPassword = _hashingService.HashPassword(password, salt);

        // Act
        var result = _hashingService.VerifyPassword(password, hashedPassword);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void VerifyPassword_WithIncorrectPassword_ShouldReturnFalse()
    {
        // Arrange
        var correctPassword = "MySecurePassword123";
        var incorrectPassword = "MySecurePassword124";
        var salt = _hashingService.GenerateSalt();
        var hashedPassword = _hashingService.HashPassword(correctPassword, salt);

        // Act
        var result = _hashingService.VerifyPassword(incorrectPassword, hashedPassword);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void VerifyPassword_WithNullPassword_ShouldThrowArgumentNullException()
    {
        // Arrange
        string password = null;
        var hashedPassword = "hashedPasswordValue";

        // Act
        _hashingService.VerifyPassword(password, hashedPassword);

        // Assert: ExpectedException
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void VerifyPassword_WithEmptyPassword_ShouldThrowArgumentNullException()
    {
        // Arrange
        var password = "";
        var hashedPassword = "hashedPasswordValue";

        // Act
        _hashingService.VerifyPassword(password, hashedPassword);

        // Assert: ExpectedException
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void VerifyPassword_WithWhitespacePassword_ShouldThrowArgumentNullException()
    {
        // Arrange
        var password = "   ";
        var hashedPassword = "hashedPasswordValue";

        // Act
        _hashingService.VerifyPassword(password, hashedPassword);

        // Assert: ExpectedException
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void VerifyPassword_WithNullHashedPassword_ShouldThrowArgumentNullException()
    {
        // Arrange
        var password = "MySecurePassword123";
        string hashedPassword = null;

        // Act
        _hashingService.VerifyPassword(password, hashedPassword);

        // Assert: ExpectedException
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void VerifyPassword_WithEmptyHashedPassword_ShouldThrowArgumentNullException()
    {
        // Arrange
        var password = "MySecurePassword123";
        var hashedPassword = "";

        // Act
        _hashingService.VerifyPassword(password, hashedPassword);

        // Assert: ExpectedException
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void VerifyPassword_WithWhitespaceHashedPassword_ShouldThrowArgumentNullException()
    {
        // Arrange
        var password = "MySecurePassword123";
        var hashedPassword = "   ";

        // Act
        _hashingService.VerifyPassword(password, hashedPassword);

        // Assert: ExpectedException
    }

    #endregion

    #region GenerateSalt Tests

    [TestMethod]
    public void GenerateSalt_ShouldReturnNonEmptyString()
    {
        // Act
        var salt = _hashingService.GenerateSalt();

        // Assert
        Assert.IsNotNull(salt);
        Assert.IsTrue(salt.Length > 0);
    }

    [TestMethod]
    public void GenerateSalt_MultipleInvocations_ShouldReturnDifferentSalts()
    {
        // Act
        var salt1 = _hashingService.GenerateSalt();
        var salt2 = _hashingService.GenerateSalt();
        var salt3 = _hashingService.GenerateSalt();

        // Assert
        Assert.AreNotEqual(salt1, salt2);
        Assert.AreNotEqual(salt1, salt3);
        Assert.AreNotEqual(salt2, salt3);
    }

    [TestMethod]
    public void GenerateSalt_ShouldReturnValidBCryptSalt()
    {
        // Act
        var salt = _hashingService.GenerateSalt();

        // Assert
        // BCrypt salts typically start with "$2a$", "$2b$", or "$2y$" and are 29 characters long
        Assert.IsTrue(salt.StartsWith("$2") && salt.Length >= 29);
    }

    #endregion
}