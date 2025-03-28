using System.ComponentModel.DataAnnotations;

namespace BitCrafts.Infrastructure.Abstraction.Entities;

/// <summary>
///     Provides an abstract base class for entities with a generic ID.
///     This class implements the IEntity&lt;T&gt; interface and provides a default ID property.
/// </summary>
/// <typeparam name="T">The type of the entity's unique identifier.</typeparam>
public abstract class BaseEntity : IEntity
{
    /// <inheritdoc />
    [Key]
    public int Id { get; set; }
}