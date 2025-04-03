using System.ComponentModel.DataAnnotations;
using BitCrafts.Infrastructure.Abstraction.Data;
using BitCrafts.Infrastructure.Data;

namespace BitCrafts.Infrastructure.Tests.Data;

[TestClass]
public class DataValidatorTests
{
    private DataValidator _dataValidator;

    [TestInitialize]
    public void Initialize()
    {
        _dataValidator = new DataValidator();
    }

    [TestMethod]
    public void TryValidate_WhenEntityIsNull_ReturnsFalse()
    {
        // Arrange
        Authentication auth = null;

        // Act
        var result = _dataValidator.TryValidate(auth, true, out var validationResults);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual(0, validationResults.Count);
    }

    [TestMethod]
    public void TryValidate_WhenEntityIsValid_ReturnsTrue()
    {
        // Arrange
        var auth = new Authentication
        {
            Login = "testuser",
            Password = "Password123",
            PasswordConfirmation = "Password123"
        };

        // Act
        var result = _dataValidator.TryValidate(auth, true, out var validationResults);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(0, validationResults.Count);
    }

    [TestMethod]
    public void TryValidate_WhenRequiredPropertiesAreMissing_ReturnsFalse()
    {
        // Arrange
        var auth = new Authentication();

        // Act
        var result = _dataValidator.TryValidate(auth, true, out var validationResults);

        // Assert
        Assert.IsFalse(result);
        Assert.IsTrue(validationResults.Count >= 3); // 3 required fields

        // Vérification que chaque champ requis a généré une erreur
        Assert.IsTrue(validationResults.Any(v => v.MemberNames.Contains(nameof(Authentication.Login))));
        Assert.IsTrue(validationResults.Any(v => v.MemberNames.Contains(nameof(Authentication.Password))));
        Assert.IsTrue(validationResults.Any(v => v.MemberNames.Contains(nameof(Authentication.PasswordConfirmation))));
    }

    [TestMethod]
    public void TryValidate_WhenPasswordTooShort_ReturnsFalse()
    {
        // Arrange
        var auth = new Authentication
        {
            Login = "testuser",
            Password = "short", // Moins de 8 caractères
            PasswordConfirmation = "short"
        };

        // Act
        var result = _dataValidator.TryValidate(auth, true, out var validationResults);

        // Assert
        Assert.IsFalse(result);
        Assert.IsTrue(validationResults.Count >= 2); // Les deux champs de mot de passe sont trop courts
        Assert.IsTrue(validationResults.Any(v => v.MemberNames.Contains(nameof(Authentication.Password))));
        Assert.IsTrue(validationResults.Any(v => v.MemberNames.Contains(nameof(Authentication.PasswordConfirmation))));
    }

    [TestMethod]
    public void TryValidate_WhenPasswordTooLong_ReturnsFalse()
    {
        // Arrange
        var auth = new Authentication
        {
            Login = "testuser",
            Password = "ThisPasswordIsMuchTooLongForTheValidation", // Plus de 24 caractères
            PasswordConfirmation = "ThisPasswordIsMuchTooLongForTheValidation"
        };

        // Act
        var result = _dataValidator.TryValidate(auth, true, out var validationResults);

        // Assert
        Assert.IsFalse(result);
        Assert.IsTrue(validationResults.Count >= 2); // Les deux champs de mot de passe sont trop longs
        Assert.IsTrue(validationResults.Any(v => v.MemberNames.Contains(nameof(Authentication.Password))));
        Assert.IsTrue(validationResults.Any(v => v.MemberNames.Contains(nameof(Authentication.PasswordConfirmation))));
    }

    [TestMethod]
    public void TryValidate_WhenLoginTooLong_ReturnsFalse()
    {
        // Arrange
        var loginTooLong = new string('a', 101); // 101 caractères, max est 100
        var auth = new Authentication
        {
            Login = loginTooLong,
            Password = "Password123",
            PasswordConfirmation = "Password123"
        };

        // Act
        var result = _dataValidator.TryValidate(auth, true, out var validationResults);

        // Assert
        Assert.IsFalse(result);
        Assert.IsTrue(validationResults.Any(v => v.MemberNames.Contains(nameof(Authentication.Login))));
    }

    [TestMethod]
    public void TryValidate_WithValidateAllPropertiesFalse_ValidatesOnlyRequiredProperties()
    {
        // Arrange
        var auth = new Authentication
        {
            Login = "testuser",
            Password = "short", // Ne respecte pas MinLength mais n'est pas vérifié si validateAllProperties est false
            PasswordConfirmation = "short"
        };

        // Act - Valider uniquement les propriétés requises (Required)
        var result = _dataValidator.TryValidate(auth, false, out var validationResults);

        // Assert
        // Normalement, ça devrait passer car on a fourni toutes les propriétés requises,
        // même si certaines ne respectent pas les contraintes de longueur
        Assert.IsTrue(result);
        Assert.AreEqual(0, validationResults.Count);
    }

    [TestMethod]
    public void TryValidate_WithCustomModel_ValidatesCorrectly()
    {
        // Arrange - Utilisation d'une classe anonyme pour tester avec un autre type
        var customModel = new CustomTestModel
        {
            Name = "Test",
            Email = "invalid-email" // Format d'email invalide
        };

        // Act
        var result = _dataValidator.TryValidate(customModel, true, out var validationResults);

        // Assert
        Assert.IsFalse(result);
        Assert.IsTrue(validationResults.Any(v => v.MemberNames.Contains(nameof(CustomTestModel.Email))));
    }
}

public class CustomTestModel
{
    [Required] public string Name { get; set; }

    [Required] [EmailAddress] public string Email { get; set; }
}