using FluentValidation;
using MediatR;
using Serilog;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using CrystalQuartz.AspNetCore;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Velen.API.Configuration;
using Velen.API.Extensions;
using Velen.API.Middlewares;
using Velen.Application;
using Velen.Application.Behaviors;
using Velen.Application.Processing;
using Velen.Domain.Data;
using Velen.Domain.DomainEvents;
using Velen.Domain.SeedWork;
using Velen.Infrastructure;
using Velen.Infrastructure.Commands;
using Velen.Infrastructure.Database;
using Velen.Infrastructure.Domain;
using Velen.Infrastructure.Emails;
using Velen.Infrastructure.Logging;
using Velen.Infrastructure.Processing;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((ctx, lc) => lc
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Context}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.Seq("http://localhost:5341"));

// Add services to the container.
builder.Services.AddHttpLogging(options=>options.LoggingFields = HttpLoggingFields.All);
builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR(ApplicationModule.Assembly);
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
builder.Services.AddValidatorsFromAssemblyContaining<ApplicationModule>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped<AppDbContext>();
builder.Services.AddDbContextPool<AppDbContext>((serviceProvider,options) =>
{
    var connectionString=builder.Configuration.GetConnectionString("AppDbContext");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IExecutionContextAccessor, ExecutionContextAccessor>();
builder.Services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
builder.Services.AddScoped(typeof(IDomainEventNotification<>), typeof(DomainNotificationBase<>));
builder.Services.AddScoped(typeof(INotificationHandler<>), typeof(DomainEventsDispatcherNotificationHandlerDecorator<>));
builder.Services.AddScoped(typeof(ICommandHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));
builder.Services.AddScoped(typeof(ICommandHandler<,>), typeof(UnitOfWorkCommandHandlerWithResultDecorator<,>));
builder.Services.AddScoped(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
builder.Services.AddScoped(typeof(ICommandHandler<,>), typeof(LoggingCommandHandlerWithResultDecorator<,>));
builder.Services.AddScoped<ICommandsDispatcher, CommandsDispatcher>();
builder.Services.AddScoped<ICommandsScheduler, CommandsScheduler>();
builder.Services.AddScoped(typeof(ISqlConnectionFactory),_=>new SqlConnectionFactory(builder.Configuration.GetConnectionString("AppDbContext")));
builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
});
builder.Services.AddQuartzServer(q => q.WaitForJobsToComplete = true);

builder.Services.AddControllers().AddJsonOptions(options => 
{
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();


var app = builder.Build();
ServiceProviderLocator.SetProvider(app.Services);
ApplicationStartup.Initialize(app.Services);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseMiddleware<CorrelationMiddleware>();

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSerilogRequestLogging();

app.Run();

