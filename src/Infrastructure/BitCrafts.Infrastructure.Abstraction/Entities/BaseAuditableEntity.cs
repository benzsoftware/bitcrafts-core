namespace BitCrafts.Infrastructure.Abstraction.Entities;

/// <summary>
///     Provides an abstract base class for auditable entities with an integer ID.
///     This class implements the IAuditableEntity interface and provides default auditing properties.
/// </summary>
public abstract class BaseAuditableEntity<T> : BaseEntity<T>, IAuditableEntity
{
    /// <inheritdoc />

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    /// <inheritdoc />

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    /// <inheritdoc />
    public string CreatedBy { get; set; } = string.Empty;

    /// <inheritdoc />
    public string UpdatedBy { get; set; } = string.Empty;
}

/// <summary>
///     Provides an abstract base class for auditable entities with a generic ID.
///     This class implements the IAuditableEntity interface and provides default auditing properties.
/// </summary>
/// <typeparam name="T">The type of the entity's unique identifier.</typeparam>
public abstract class BaseAuditableEntity : BaseEntity, IAuditableEntity
{
    /// <inheritdoc />
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    /// <inheritdoc />
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    /// <inheritdoc />
    public string CreatedBy { get; set; } = string.Empty;

    /// <inheritdoc />
    public string UpdatedBy { get; set; } = string.Empty;
}