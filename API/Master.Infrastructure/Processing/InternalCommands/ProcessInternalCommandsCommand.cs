using MediatR;
using Master.Infrastructure.Commands;
using Master.Infrastructure.Processing.Outbox;

namespace Master.Infrastructure.Processing.InternalCommands
{
    public class ProcessInternalCommandsCommand : CommandBase<Unit>, IRecurringCommand
    {

    }
}