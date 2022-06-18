namespace CleanArchitectureTemplate.Domain.Common;

public class BaseAuditableEntity<TId> : BaseEntity<TId> where TId : struct
{
    public DateTime Created { get; set; }
    public TId CreatedBy { get; set; }
    public DateTime Updated { get; set; }
    public TId UpdatedBy { get; set; }
}