namespace CleanArchitectureTemplate.Application.Interfaces;

public interface ICurrentUserService<TId> where TId : struct
{
    public TId UserId { get; set; }
}