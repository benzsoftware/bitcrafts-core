using System.ComponentModel.DataAnnotations;

namespace BitCrafts.Infrastructure.Abstraction.Entities;

/// <summary>
///     Provides an abstract base class for soft-deletable entities with a generic ID.
///     This class implements the ISoftDeletableEntity interface and provides default soft deletion properties.
/// </summary>
public abstract class BaseSoftDeletableEntity : BaseEntity, ISoftDeletableEntity
{
    private bool _isDeleted;
    private DateTimeOffset _deletedAt;
    private string _deletedBy = string.Empty;
    private string _deletedReason = string.Empty;

    /// <inheritdoc />
    public bool IsDeleted 
    { 
        get => _isDeleted;
        set
        {
            _isDeleted = value;
            
            // Si on marque comme non supprimé, on réinitialise les propriétés
            if (!value)
            {
                _deletedAt = default;
                _deletedBy = string.Empty;
                _deletedReason = string.Empty;
            }
            else if (_deletedAt == default)
            {
                // Si on marque comme supprimé et que la date n'est pas définie, on la définit
                _deletedAt = DateTimeOffset.UtcNow;
            }
        }
    }

    /// <inheritdoc />
    public DateTimeOffset DeletedAt 
    { 
        get => _deletedAt;
        set => _deletedAt = value;
    }

    /// <inheritdoc />
    public string DeletedBy 
    { 
        get => _deletedBy;
        set => _deletedBy = value ?? string.Empty;
    }

    /// <inheritdoc />
    public string DeletedReason 
    { 
        get => _deletedReason;
        set => _deletedReason = value ?? string.Empty;
    }
    
    /// <summary>
    /// Valide que les propriétés de suppression sont correctement définies
    /// </summary>
    /// <returns>True si l'entité est valide, sinon false</returns>
    public virtual bool ValidateDeletionProperties()
    {
        if (!IsDeleted)
            return true;
            
        return DeletedAt != default && !string.IsNullOrEmpty(DeletedBy);
    }
}