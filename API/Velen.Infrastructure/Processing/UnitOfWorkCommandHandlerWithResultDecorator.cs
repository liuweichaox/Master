using Microsoft.EntityFrameworkCore;
using Velen.Domain.SeedWork;
using Velen.Infrastructure.Commands;
using Velen.Infrastructure.Domain;

namespace Velen.Infrastructure.Processing
{
    public class UnitOfWorkCommandHandlerWithResultDecorator<T, TResult> : ICommandHandler<T, TResult> where T : ICommand<TResult>
    {
        private readonly ICommandHandler<T, TResult> _decorated;

        private readonly IUnitOfWork _unitOfWork;

        private readonly AppDbContext _appDbContext;

        public UnitOfWorkCommandHandlerWithResultDecorator(
            ICommandHandler<T, TResult> decorated, 
            IUnitOfWork unitOfWork, 
            AppDbContext appDbContext)
        {
            _decorated = decorated;
            _unitOfWork = unitOfWork;
            _appDbContext = appDbContext;
        }

        public async Task<TResult> Handle(T command, CancellationToken cancellationToken)
        {
            var result = await this._decorated.Handle(command, cancellationToken);

            if (command is InternalCommandBase<TResult>)
            {
                var internalCommand = await _appDbContext.InternalCommands.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken: cancellationToken);

                if (internalCommand != null)
                {
                    internalCommand.ProcessedDate = DateTime.UtcNow;
                }
            }

            await this._unitOfWork.CommitAsync(cancellationToken);

            return result;
        }
    }
}