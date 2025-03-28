using System.ComponentModel.DataAnnotations;

namespace BitCrafts.Infrastructure.Abstraction.Entities;

/// <summary>
///     Provides an abstract base class for auditable entities with a generic ID.
///     This class implements the IAuditableEntity interface and provides default auditing properties.
/// </summary>
/// <typeparam name="T">The type of the entity's unique identifier.</typeparam>
public abstract class BaseAuditableEntity : BaseEntity, IAuditableEntity
{
    /// <inheritdoc />
    [Required(ErrorMessage = "CreatedAt property is required.")]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    /// <inheritdoc />
    [Required(ErrorMessage = "UpdatedAt property is required.")]
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    /// <inheritdoc />
    [Required(ErrorMessage = "CreatedBy property is required.")]
    public string CreatedBy { get; set; } = string.Empty;

    /// <inheritdoc />
    [Required(ErrorMessage = "UpdatedBy property is required.")]
    public string UpdatedBy { get; set; } = string.Empty;
}