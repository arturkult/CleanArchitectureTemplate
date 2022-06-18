using System.Diagnostics;
using CleanArchitectureTemplate.Application.Interfaces;
using CleanArchitectureTemplate.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitectureTemplate.Application.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
{
    private readonly ICurrentUserService<Guid> _currentUserService;
    private readonly ILogger<TRequest> _logger;
    private readonly Stopwatch _timer;

    public LoggingBehaviour(
        ILogger<TRequest> logger,
        ICurrentUserService<Guid> currentUserService)
    {
        _timer = new Stopwatch();
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogWarning("CleanArchitecture Running Request: {Name} {Request}", requestName, request);
        try
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500)
                _logger.LogWarning(
                    "CleanArchitecture Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}",
                    requestName, elapsedMilliseconds, request);
            return response;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "CleanArchitecture Request: Unhandled Exception for Request {Name} {@Request}",
                requestName, request);
            throw;
        }
    }
}