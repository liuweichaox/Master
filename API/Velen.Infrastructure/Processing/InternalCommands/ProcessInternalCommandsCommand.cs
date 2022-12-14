using MediatR;
using Velen.Infrastructure.Commands;
using Velen.Infrastructure.Processing.Outbox;

namespace Velen.Infrastructure.Processing.InternalCommands
{
    public class ProcessInternalCommandsCommand : CommandBase<Unit>, IRecurringCommand
    {

    }
}