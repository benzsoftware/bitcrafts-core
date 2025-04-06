using System.ComponentModel.DataAnnotations;
using BitCrafts.Infrastructure.Abstraction.Entities;

namespace BitCrafts.Infrastructure.Tests.Entities;

[TestClass]
public class BaseEntityTests
{
    [TestMethod]
    public void BaseEntity_DefaultId_IsZero()
    {
        // Arrange
        var entity = new TestEntity();

        // Assert
        Assert.AreEqual(0, entity.Id);
    }

    [TestMethod]
    public void BaseEntity_SetId_StoresCorrectValue()
    {
        // Arrange
        var entity = new TestEntity();

        // Act
        entity.Id = 42;

        // Assert
        Assert.AreEqual(42, entity.Id);
    }
}

[TestClass]
public class BaseSoftDeletableEntityTests
{
    [TestMethod]
    public void BaseSoftDeletableEntity_Constructor_IsDeletedIsFalse()
    {
        // Arrange & Act
        var entity = new TestSoftDeletableEntity();

        // Assert
        Assert.IsFalse(entity.IsDeleted);
    }

    [TestMethod]
    public void BaseSoftDeletableEntity_SetDeletedProperties_StoresCorrectValues()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity();
        var deletedAt = new DateTimeOffset(2023, 1, 3, 0, 0, 0, TimeSpan.Zero);

        // Act
        entity.IsDeleted = true;
        entity.DeletedAt = deletedAt;
        entity.DeletedBy = "Deleter";
        entity.DeletedReason = "No longer needed";

        // Assert
        Assert.IsTrue(entity.IsDeleted);
        Assert.AreEqual(deletedAt, entity.DeletedAt);
        Assert.AreEqual("Deleter", entity.DeletedBy);
        Assert.AreEqual("No longer needed", entity.DeletedReason);
    }

    [TestMethod]
    public void BaseSoftDeletableEntity_IsDeletedSetter_SetsDeletedAtAutomatically()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity();
        var defaultDate = default(DateTimeOffset);
        entity.DeletedAt = defaultDate;

        // Act
        entity.IsDeleted = true;

        // Assert
        Assert.IsTrue(entity.IsDeleted);
        Assert.AreNotEqual(defaultDate, entity.DeletedAt,
            "DeletedAt devrait être modifié lorsque IsDeleted est défini à true");
    }

    [TestMethod]
    public void BaseSoftDeletableEntity_ValidWhenAllRequiredPropertiesProvided()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity
        {
            IsDeleted = true,
            DeletedAt = DateTimeOffset.UtcNow,
            DeletedBy = "Deleter"
        };
        var validationResults = new System.Collections.Generic.List<ValidationResult>();
        var validationContext = new ValidationContext(entity);

        // Act
        var isValid = Validator.TryValidateObject(entity, validationContext, validationResults, true);

        // Assert
        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void IsDeleted_WhenSetToTrue_SetsDeletedAtAutomatically()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity();
        entity.DeletedAt = default; // Réinitialiser pour garantir qu'il n'y a pas de valeur initiale

        // Act
        entity.IsDeleted = true;

        // Assert
        Assert.IsTrue(entity.IsDeleted);
        Assert.AreNotEqual(default(DateTimeOffset), entity.DeletedAt);
    }

    [TestMethod]
    public void IsDeleted_WhenSetToTrue_DoesNotOverrideExistingDeletedAt()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity();
        var specificDate = new DateTimeOffset(2023, 5, 15, 10, 30, 0, TimeSpan.Zero);
        entity.DeletedAt = specificDate;

        // Act
        entity.IsDeleted = true;

        // Assert
        Assert.IsTrue(entity.IsDeleted);
        Assert.AreEqual(specificDate, entity.DeletedAt);
    }

    [TestMethod]
    public void IsDeleted_WhenSetToFalse_ResetsDeletedProperties()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity
        {
            IsDeleted = true,
            DeletedAt = DateTimeOffset.UtcNow,
            DeletedBy = "User",
            DeletedReason = "Test reason"
        };

        // Act
        entity.IsDeleted = false;

        // Assert
        Assert.IsFalse(entity.IsDeleted);
        Assert.AreEqual(default(DateTimeOffset), entity.DeletedAt);
        Assert.AreEqual(string.Empty, entity.DeletedBy);
        Assert.AreEqual(string.Empty, entity.DeletedReason);
    }

    [TestMethod]
    public void IsDeleted_WhenToggledMultipleTimes_BehavesCorrectly()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity();

        // Act & Assert - Premier toggle
        entity.IsDeleted = true;
        Assert.IsTrue(entity.IsDeleted);
        Assert.AreNotEqual(default(DateTimeOffset), entity.DeletedAt);

        var initialDeletedAt = entity.DeletedAt;
        entity.DeletedBy = "User";
        entity.DeletedReason = "First deletion";

        // Act & Assert - Deuxième toggle
        entity.IsDeleted = false;
        Assert.IsFalse(entity.IsDeleted);
        Assert.AreEqual(default(DateTimeOffset), entity.DeletedAt);
        Assert.AreEqual(string.Empty, entity.DeletedBy);
        Assert.AreEqual(string.Empty, entity.DeletedReason);

        // Act & Assert - Troisième toggle
        entity.IsDeleted = true;
        Assert.IsTrue(entity.IsDeleted);
        Assert.AreNotEqual(default(DateTimeOffset), entity.DeletedAt);
        Assert.AreNotEqual(initialDeletedAt, entity.DeletedAt); // Nouvelle date
        Assert.AreEqual(string.Empty, entity.DeletedBy);
        Assert.AreEqual(string.Empty, entity.DeletedReason);
    }

    

    #region Tests ValidateDeletionProperties

    [TestMethod]
    public void ValidateDeletionProperties_WhenNotDeleted_ReturnsTrue()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity
        {
            IsDeleted = false
        };

        // Act
        var isValid = entity.ValidateDeletionProperties();

        // Assert
        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void ValidateDeletionProperties_WhenDeletedWithoutBy_ReturnsFalse()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity();
        entity.IsDeleted = true; // Ceci définit automatiquement DeletedAt
        entity.DeletedBy = string.Empty;

        // Act
        var isValid = entity.ValidateDeletionProperties();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void ValidateDeletionProperties_WhenDeletedWithoutAt_ReturnsFalse()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity();
        entity.IsDeleted = true;
        entity.DeletedAt = default; // Forcer à reset la date même si le setter l'a définie
        entity.DeletedBy = "User";

        // Act
        var isValid = entity.ValidateDeletionProperties();

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void ValidateDeletionProperties_WhenDeletedWithAllProperties_ReturnsTrue()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity
        {
            IsDeleted = true,
            DeletedBy = "User"
            // DeletedAt est défini automatiquement par le setter
        };

        // Act
        var isValid = entity.ValidateDeletionProperties();

        // Assert
        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void ValidateDeletionProperties_WhenDeletedReasonIsEmpty_StillReturnsTrue()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity
        {
            IsDeleted = true,
            DeletedBy = "User",
            DeletedReason = string.Empty
            // DeletedAt est défini automatiquement par le setter
        };

        // Act
        var isValid = entity.ValidateDeletionProperties();

        // Assert
        Assert.IsTrue(isValid, "DeletedReason n'est pas requis pour la validation");
    }

    [TestMethod]
    public void ValidateDeletionProperties_AllFieldsWork()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity
        {
            IsDeleted = true,
            DeletedBy = "User",
            DeletedReason = "Test reason"
            // DeletedAt est défini automatiquement par le setter
        };

        // Act
        var isValid = entity.ValidateDeletionProperties();

        // Assert
        Assert.IsTrue(isValid);
        Assert.AreEqual("User", entity.DeletedBy);
        Assert.AreEqual("Test reason", entity.DeletedReason);
        Assert.AreNotEqual(default(DateTimeOffset), entity.DeletedAt);
    }

    #endregion

    #region Scénarios d'intégration

    [TestMethod]
    public void SoftDelete_CompleteScenario()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity();

        // Act - Supprimer l'entité
        entity.IsDeleted = true;
        entity.DeletedBy = "Admin";
        entity.DeletedReason = "No longer needed";

        // Assert - Vérifier l'état après suppression
        Assert.IsTrue(entity.IsDeleted);
        Assert.AreNotEqual(default(DateTimeOffset), entity.DeletedAt);
        Assert.AreEqual("Admin", entity.DeletedBy);
        Assert.AreEqual("No longer needed", entity.DeletedReason);
        Assert.IsTrue(entity.ValidateDeletionProperties());

        // Act - Restaurer l'entité
        entity.IsDeleted = false;

        // Assert - Vérifier que tout est réinitialisé
        Assert.IsFalse(entity.IsDeleted);
        Assert.AreEqual(default(DateTimeOffset), entity.DeletedAt);
        Assert.AreEqual(string.Empty, entity.DeletedBy);
        Assert.AreEqual(string.Empty, entity.DeletedReason);
        Assert.IsTrue(entity.ValidateDeletionProperties()); // Toujours valide car non supprimé
    }

    [TestMethod]
    public void DeletedBy_HandleNullInput()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity
        {
            IsDeleted = true
        };

        // Act
        entity.DeletedBy = null;

        // Assert
        Assert.AreEqual(string.Empty, entity.DeletedBy);
        Assert.IsFalse(entity.ValidateDeletionProperties());
    }

    [TestMethod]
    public void DeletedReason_HandleNullInput()
    {
        // Arrange
        var entity = new TestSoftDeletableEntity
        {
            IsDeleted = true,
            DeletedBy = "User"
        };

        // Act
        entity.DeletedReason = null;

        // Assert
        Assert.AreEqual(string.Empty, entity.DeletedReason);
        Assert.IsTrue(entity.ValidateDeletionProperties());
    }

    #endregion
}

// Classes de test pour les tests
public class TestEntity : BaseEntity
{
}

public class TestSoftDeletableEntity : BaseSoftDeletableEntity
{
    public TestSoftDeletableEntity()
    {
    }
}