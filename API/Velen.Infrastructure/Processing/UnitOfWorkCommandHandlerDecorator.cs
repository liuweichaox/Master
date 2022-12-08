using MediatR;
using Microsoft.EntityFrameworkCore;
using Velen.Domain.SeedWork;
using Velen.Infrastructure.Commands;
using Velen.Infrastructure.Domain;

namespace Velen.Infrastructure.Processing
{
    public class UnitOfWorkCommandHandlerDecorator<T> : ICommandHandler<T> where T:ICommand
    {
        private readonly ICommandHandler<T> _decorated;

        private readonly IUnitOfWork _unitOfWork;

        private readonly AppDbContext _appDbContext;

        public UnitOfWorkCommandHandlerDecorator(
            ICommandHandler<T> decorated, 
            IUnitOfWork unitOfWork, 
            AppDbContext appDbContext)
        {
            _decorated = decorated;
            _unitOfWork = unitOfWork;
            _appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(T command, CancellationToken cancellationToken)
        {
            await this._decorated.Handle(command, cancellationToken);

            if (command is InternalCommandBase)
            {
                var internalCommand =
                    await _appDbContext.InternalCommands.FirstOrDefaultAsync(x => x.Id == command.Id,
                        cancellationToken: cancellationToken);

                if (internalCommand != null)
                {
                    internalCommand.ProcessedDate = DateTime.UtcNow;
                }
            }

            await this._unitOfWork.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}