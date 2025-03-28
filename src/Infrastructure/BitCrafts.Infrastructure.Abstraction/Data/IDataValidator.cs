using System.ComponentModel.DataAnnotations;

namespace BitCrafts.Infrastructure.Abstraction.Data;

public interface IDataValidator
{
    /// <summary>
    /// Attempts to validate the specified entity using DataAnnotations.
    /// </summary>
    /// <typeparam name="T">The type of the entity to validate.</typeparam>
    /// <param name="entity">The entity instance to validate.</param>
    /// <param name="validateAllProperties">Idicate if the validation should validate only required properties or all properties of the entity.</param>
    /// <param name="validationResults">When this method returns, contains a list of ValidationResult
    /// objects detailing any validation errors. This list will be empty if validation succeeds.
    /// This parameter is passed uninitialized.</param>
    /// <returns>true if the entity is valid according to its DataAnnotations; otherwise, false.</returns>
    bool TryValidate<T>(T entity, bool validateAllProperties, out List<ValidationResult> validationResults)
        where T : class;
}