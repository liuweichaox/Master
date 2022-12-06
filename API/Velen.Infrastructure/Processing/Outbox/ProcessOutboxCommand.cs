using MediatR;
using Velen.Infrastructure.Commands;

namespace Velen.Infrastructure.Processing.Outbox
{
    public class ProcessOutboxCommand : CommandBase<Unit>, IRecurringCommand
    {

    }
}