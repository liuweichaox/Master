using System.Reflection;
using MediatR;

namespace Velen.Infrastructure.Processing.InternalCommands
{
    public class CommandsDispatcher : ICommandsDispatcher
    {
        public Task DispatchCommandAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}