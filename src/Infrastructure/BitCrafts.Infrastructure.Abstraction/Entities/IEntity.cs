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

/// <summary>
///     Defines the base interface for all entities in the application with a generic ID type.
///     Entities represent domain objects that are persisted in the data store.
/// </summary>
/// <typeparam name="T">The type of the entity's unique identifier.</typeparam>
public interface IEntity<T>
{
    /// <summary>
    ///     Gets or sets the unique identifier for the entity.
    /// </summary>
    [Key]
    T Id { get; set; }
}