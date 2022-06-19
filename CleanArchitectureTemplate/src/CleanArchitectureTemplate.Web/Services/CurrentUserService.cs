
using CleanArchitectureTemplate.Application.Interfaces;

namespace CleanArchitectureTemplate.Web.Services;

public class CurrentUserService : ICurrentUserService<Guid>
{
    public Guid UserId { get; set; } = Guid.Empty;
}

