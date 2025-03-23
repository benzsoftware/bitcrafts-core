namespace BitCrafts.Infrastructure.Abstraction.Entities;

/// <summary>
///     Provides an abstract base class for soft-deletable entities with an integer ID.
///     This class implements the ISoftDeletableEntity interface and provides default soft deletion properties.
/// </summary>
public abstract class BaseSoftDeletableEntity<T> : BaseEntity<T>, ISoftDeletableEntity
{
    /// <inheritdoc />

    public bool IsDeleted { get; set; }

    /// <inheritdoc />
    public DateTimeOffset? DeletedAt { get; set; }

    /// <inheritdoc />
    public string DeletedBy { get; set; }

    /// <inheritdoc />
    public string DeletedReason { get; set; }
}

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
    public DateTimeOffset? DeletedAt { get; set; }

    /// <inheritdoc />
    public string DeletedBy { get; set; }

    /// <inheritdoc />
    public string DeletedReason { get; set; }
}