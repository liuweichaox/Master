using System.Text.Json;
using MediatR;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using Master.Infrastructure;
using Master.Infrastructure.Commands;
using Master.Infrastructure.Processing.Outbox;

namespace Master.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;
        private readonly IExecutionContextAccessor _executionContextAccessor;
        public LoggingBehavior(ILogger logger, IExecutionContextAccessor executionContextAccessor)
        {
            _logger = logger;
            _executionContextAccessor = executionContextAccessor;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            Console.WriteLine(@"LoggingBehavior Handle command type: " + request.GetType().Name+@"request json: "+JsonSerializer.Serialize(request));
            var command = request as ICommand<TResponse>;
            if (request is IRecurringCommand)
            {
                return  await next();
                Console.WriteLine(@"LoggingBehavior Handle RecurringCommand end");
            }

            using (
                LogContext.Push(
                    new RequestLogEnricher(_executionContextAccessor),
                    new CommandLogEnricher(command)))
            {
                try
                {
                    this._logger.Information(
                        "Executing command {Command}",
                        request.GetType().Name);

                    var response = await next();

                    this._logger.Information("Command {Command} processed successful", command.GetType().Name);

                    Console.WriteLine(@"LoggingBehavior Handle Success Command type: " + command.GetType().Name);
                    return response;
                }
                catch (Exception exception)
                {
                    this._logger.Error(exception, "Command {Command} processing failed", command.GetType().Name);
                    throw;
                }
            }
        }
        private class CommandLogEnricher : ILogEventEnricher
        {
            private readonly ICommand<TResponse> _command;

            public CommandLogEnricher(ICommand<TResponse> command)
            {
                _command = command;
            }
            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                logEvent.AddOrUpdateProperty(new LogEventProperty("Context", new ScalarValue($"Command:{_command.Id.ToString()}")));
            }
        }


        private class RequestLogEnricher : ILogEventEnricher
        {
            private readonly IExecutionContextAccessor _executionContextAccessor;

            public RequestLogEnricher(IExecutionContextAccessor executionContextAccessor)
            {
                _executionContextAccessor = executionContextAccessor;
            }

            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                if (_executionContextAccessor.IsAvailable)
                {
                    logEvent.AddOrUpdateProperty(new LogEventProperty("CorrelationId", new ScalarValue(_executionContextAccessor.CorrelationId)));
                }
            }
        }
    }
}