﻿using System.Text.Json;
using Dapper;
using MediatR;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using Master.Domain.Data;
using Master.Domain.Events;
using Master.Infrastructure.Commands;
using Master.Infrastructure.Processing.Outbox;

namespace Master.Application.Processing
{
    internal class ProcessOutboxCommandHandler : ICommandHandler<ProcessOutboxCommand, Unit>
    {
        private readonly IMediator _mediator;

        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public ProcessOutboxCommandHandler(IMediator mediator, ISqlConnectionFactory sqlConnectionFactory)
        {
            _mediator = mediator;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
        {
            var connection = this._sqlConnectionFactory.GetOpenConnection();
            const string sql = "SELECT Id,Type,Data FROM OutboxMessages WHERE ProcessedDate  IS NULL";

            var messages = await connection.QueryAsync<OutboxMessageDto>(sql);
            var messagesList = messages.AsList();

            const string sqlUpdateProcessedDate = "UPDATE OutboxMessages " +
                                                  "SET ProcessedDate = @Date " +
                                                  "WHERE Id = @Id";
            if (messagesList.Count > 0)
            {
                foreach (var message in messagesList)
                {
                    var type = ApplicationModule.Assembly
                        .GetType(message.Type);
                    var request = JsonSerializer.Deserialize(message.Data, type) as IDomainEventNotification;

                    using (LogContext.Push(new OutboxMessageContextEnricher(request)))
                    {
                        Console.WriteLine(@"Processing outbox ,data: {0}", message.Data);
                        await this._mediator.Publish(request, cancellationToken);

                        await connection.ExecuteAsync(sqlUpdateProcessedDate, new
                        {
                            Date = DateTime.Now,
                            message.Id
                        });
                    }

                }
            }

            return Unit.Value;
        }

        private class OutboxMessageContextEnricher : ILogEventEnricher
        {
            private readonly IDomainEventNotification _notification;

            public OutboxMessageContextEnricher(IDomainEventNotification notification)
            {
                _notification = notification;
            }
            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
            {
                logEvent.AddOrUpdateProperty(new LogEventProperty("Context", new ScalarValue($"OutboxMessage:{_notification.Id.ToString()}")));
            }
        }
    }
}