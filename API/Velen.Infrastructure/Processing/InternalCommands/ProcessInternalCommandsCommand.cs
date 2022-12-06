using MediatR;
using Velen.Infrastructure.Commands;
using Velen.Infrastructure.Processing.Outbox;

namespace Velen.Infrastructure.Processing.InternalCommands
{
    internal class ProcessInternalCommandsCommand : CommandBase<Unit>, IRecurringCommand
    {

    }
}