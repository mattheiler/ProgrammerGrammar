using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityGateway.Core.Behaviors
{
    public class RequestLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<RequestLoggingBehavior<TRequest, TResponse>> _logger;

        public RequestLoggingBehavior(ILogger<RequestLoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation($"Starting '{typeof(TRequest).Name}'...");

            var response = await next();

            _logger.LogInformation($"Finished '{typeof(TRequest).Name}'.");

            return response;
        }
    }
}