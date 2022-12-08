using System.Reflection;
using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Velen.Infrastructure.Domain;
using Velen.Infrastructure.Processing;

namespace Velen.Application.Processing
{
    public class CommandsDispatcher : ICommandsDispatcher
    {
        private readonly IMediator _mediator;
        private readonly AppDbContext _appDbContext;

        public CommandsDispatcher(
            IMediator mediator, 
            AppDbContext appDbContext)
        {
            this._mediator = mediator;
            this._appDbContext = appDbContext;
        }

        public async Task DispatchCommandAsync(Guid id)
        {
            var internalCommand = await this._appDbContext.InternalCommands.SingleOrDefaultAsync(x => x.Id == id);
            Type type = ApplicationModule.Assembly.GetType(internalCommand.Type);
            dynamic command = JsonSerializer.Deserialize(internalCommand.Data, type);

            internalCommand.ProcessedDate = DateTime.UtcNow;

            await this._mediator.Send(command);
        }
    }
}