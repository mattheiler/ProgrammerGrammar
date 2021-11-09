using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityGateway.Core.Behaviors
{
    public class RequestPerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> _logger;
        private readonly Stopwatch _timer = new Stopwatch();

        public RequestPerformanceBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            if (_timer.ElapsedMilliseconds > 500)
                _logger.LogWarning("Long-running request: {Name} ({ElapsedMilliseconds} milliseconds) {request}", typeof(TRequest).Name, _timer.ElapsedMilliseconds, request);

            return response;
        }
    }
}