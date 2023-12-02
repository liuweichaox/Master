using System.Text.Json;
using Dapper;
using Master.Domain.Data;
using Master.Infrastructure.Commands;

namespace Master.Infrastructure.Processing
{
    public class CommandsScheduler : ICommandsScheduler
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public CommandsScheduler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task EnqueueAsync<T>(ICommand<T> command)
        {
            Console.WriteLine(@"CommandsScheduler Execute TResult,command json " + JsonSerializer.Serialize(command) + " " + command.GetType().Name);
            var connection = this._sqlConnectionFactory.GetOpenConnection();

            const string sqlInsert = "INSERT INTO InternalCommands (Id, EnqueueDate, Type, Data) VALUES (@Id, @EnqueueDate, @Type, @Data)";
            //将接口类型转换为封闭类型
            var type = command.GetType();

            await connection.ExecuteAsync(sqlInsert, new
            {
                command.Id,
                EnqueueDate = DateTime.Now,
                Type = command.GetType().FullName,
                Data = JsonSerializer.Serialize(command, type)
            });
        }
    }
}