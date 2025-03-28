using System.ComponentModel.DataAnnotations;

namespace BitCrafts.Infrastructure.Abstraction.Entities;

/// <summary>
///     Defines the base interface for all entities in the application.
///     Entities represent domain objects that are persisted in the data store.
/// </summary>
public interface IEntity
{
    /// <summary>
    ///     Gets or sets the unique identifier for the entity.
    /// </summary>
    [Key]
    int Id { get; set; }
}