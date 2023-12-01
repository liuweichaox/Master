using System.Text.Json;
using Dapper;
using MediatR;
using Master.Domain.Data;
using Master.Infrastructure.Commands;
using Master.Infrastructure.Processing;
using Master.Infrastructure.Processing.InternalCommands;

namespace Master.Application.Processing
{
    internal class ProcessInternalCommandsCommandHandler : ICommandHandler<ProcessInternalCommandsCommand, Unit>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public ProcessInternalCommandsCommandHandler(
            ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(ProcessInternalCommandsCommand command, CancellationToken cancellationToken)
        {
            var connection = this._sqlConnectionFactory.GetOpenConnection();

            const string sql = "SELECT Type, Data " +
                               "FROM InternalCommands " +
                               "WHERE ProcessedDate IS NULL";
            var commands = await connection.QueryAsync<InternalCommandDto>(sql);

            var internalCommandsList = commands.AsList();

            foreach (var internalCommand in internalCommandsList)
            {
                Type? type = ApplicationModule.Assembly.GetType(internalCommand.Type);
                if (type == null) continue;
                Console.WriteLine($"Processing internal command: {type.Name}, {internalCommand.Data}");
                dynamic commandToProcess = JsonSerializer.Deserialize(internalCommand.Data, type);
                await CommandsExecutor.Execute(commandToProcess);
            }

            return Unit.Value;
        }

        private class InternalCommandDto
        {
            public string Type { get; set; }

            public string Data { get; set; }
        }
    }
}