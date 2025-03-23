namespace BitCrafts.Infrastructure.Abstraction.Entities;

/// <summary>
///     Defines an interface for entities that require auditing information.
///     Auditable entities track creation and modification timestamps and users.
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    ///     Gets or sets the timestamp of when the entity was created.
    /// </summary>
    DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    ///     Gets or sets the timestamp of when the entity was last updated.
    /// </summary>
    DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    ///     Gets or sets the user who created the entity.
    /// </summary>
    string CreatedBy { get; set; }

    /// <summary>
    ///     Gets or sets the user who last updated the entity.
    /// </summary>
    string UpdatedBy { get; set; }
}