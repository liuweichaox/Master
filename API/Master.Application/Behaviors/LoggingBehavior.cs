using System.Text.Json;
using Master.Infrastructure.Commands;
using MediatR;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace Master.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger _logger;

    public LoggingBehavior(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(@"LoggingBehavior Handle command type: " + request.GetType().Name + @"request json: " +
                          JsonSerializer.Serialize(request));
        var command = request as ICommand<TResponse>;

        using (
            LogContext.Push(new CommandLogEnricher(command)))
        {
            try
            {
                _logger.Information(
                    "Executing command {Command}",
                    request.GetType().Name);

                var response = await next();

                _logger.Information("Command {Command} processed successful", command.GetType().Name);

                Console.WriteLine(@"LoggingBehavior Handle Success Command type: " + command.GetType().Name);
                return response;
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Command {Command} processing failed", command.GetType().Name);
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
            logEvent.AddOrUpdateProperty(new LogEventProperty("Context",
                new ScalarValue($"Command:{_command.Id.ToString()}")));
        }
    }
}