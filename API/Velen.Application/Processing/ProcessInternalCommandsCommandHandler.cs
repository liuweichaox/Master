using System.Text.Json;
using Dapper;
using MediatR;
using Velen.Domain.Data;
using Velen.Infrastructure.Commands;
using Velen.Infrastructure.Processing;
using Velen.Infrastructure.Processing.InternalCommands;

namespace Velen.Application.Processing
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