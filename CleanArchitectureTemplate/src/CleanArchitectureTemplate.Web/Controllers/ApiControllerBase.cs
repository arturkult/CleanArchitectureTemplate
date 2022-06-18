using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanAchitectureTemplate.Web.Controllers;

public class ApiControllerBase: ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    public async Task<IActionResult> Send<T>(T request, CancellationToken cancellationToken) where T : IRequest
    {
        return Ok(await Mediator.Send(request, cancellationToken));
    }
}