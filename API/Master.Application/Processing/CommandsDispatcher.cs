﻿using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Master.Infrastructure.Domain;
using Master.Infrastructure.Processing;

namespace Master.Application.Processing
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
            var internalCommand = await _appDbContext.InternalCommands.SingleOrDefaultAsync(x => x.Id == id);
            var type = ApplicationModule.Assembly.GetType(internalCommand?.Type);
            dynamic command = JsonSerializer.Deserialize(internalCommand.Data, type);

            internalCommand.ProcessedDate = DateTime.Now;

            await this._mediator.Send(command);
        }
    }
}