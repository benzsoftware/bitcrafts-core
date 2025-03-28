using System.ComponentModel.DataAnnotations;
using BitCrafts.Infrastructure.Abstraction.Data;

namespace BitCrafts.Infrastructure.Data;

public sealed class DataValidator : IDataValidator
{
    /// <inheritdoc />
    public bool TryValidate<T>(T entity, bool validateAllProperties, out List<ValidationResult> validationResults)
        where T : class
    {
        if (entity == null)
        {
            validationResults = new List<ValidationResult>();
            return false;
        }

        var validationContext = new ValidationContext(entity, serviceProvider: null, items: null);
        validationResults = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(
            instance: entity,
            validationContext: validationContext,
            validationResults: validationResults,
            validateAllProperties: validateAllProperties
        );

        return isValid;
    }
}