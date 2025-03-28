namespace BitCrafts.Infrastructure.Abstraction.Entities;

/// <summary>
///     Defines an interface for entities that support soft deletion.
///     Soft-deletable entities are not physically deleted from the data store
///     but are marked as deleted.
/// </summary>
public interface ISoftDeletableEntity
{
    /// <summary>
    ///     Gets or sets a value indicating whether the entity is deleted.
    /// </summary>
    bool IsDeleted { get; set; }

    /// <summary>
    ///     Gets or sets the timestamp of when the entity was deleted.
    /// </summary>
    DateTimeOffset DeletedAt { get; set; }

    /// <summary>
    ///     Gets or sets the user who deleted the entity.
    /// </summary>
    string DeletedBy { get; set; }

    /// <summary>
    ///     Gets or sets the reason for deleting the entity.
    /// </summary>
    string DeletedReason { get; set; }
}