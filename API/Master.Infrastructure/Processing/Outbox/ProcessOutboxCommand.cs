using MediatR;
using Master.Infrastructure.Commands;

namespace Master.Infrastructure.Processing.Outbox
{
    public class ProcessOutboxCommand : CommandBase<Unit>, IRecurringCommand
    {

    }
}