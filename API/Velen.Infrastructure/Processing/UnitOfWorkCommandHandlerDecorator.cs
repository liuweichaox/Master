using MediatR;
using Velen.Infrastructure.Commands;

namespace Velen.Infrastructure.Processing
{
    public class UnitOfWorkCommandHandlerDecorator<T> : ICommandHandler<T> where T:ICommand
    {
        public Task<Unit> Handle(T request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}