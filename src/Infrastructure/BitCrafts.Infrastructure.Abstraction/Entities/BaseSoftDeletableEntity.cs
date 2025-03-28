using System.ComponentModel.DataAnnotations;

namespace BitCrafts.Infrastructure.Abstraction.Entities;

/// <summary>
///     Provides an abstract base class for soft-deletable entities with a generic ID.
///     This class implements the ISoftDeletableEntity interface and provides default soft deletion properties.
/// </summary>
/// <typeparam name="T">The type of the entity's unique identifier.</typeparam>
public abstract class BaseSoftDeletableEntity : BaseEntity, ISoftDeletableEntity
{
    /// <inheritdoc />
    public bool IsDeleted { get; set; }

    /// <inheritdoc />
    [Required(ErrorMessage = "DeletedAt property is required.")]
    public DateTimeOffset DeletedAt { get; set; }

    /// <inheritdoc />
    [Required(ErrorMessage = "DeletedBy property is required.")]
    public string DeletedBy { get; set; }

    /// <inheritdoc />
    public string DeletedReason { get; set; }
}