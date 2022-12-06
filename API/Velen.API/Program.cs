using FluentValidation.AspNetCore;
using MediatR;
using Serilog;
using Velen.Application;
using Velen.Application.Configuration.Validation;
using Velen.Infrastructure;
using Hellang.Middleware.ProblemDetails;
using Velen.API.SeedWork;
using Velen.Domain.SeedWork;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((ctx, lc) => lc
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Context}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.Seq("http://localhost:5341"));

// Add services to the container.
builder.Services.AddProblemDetails(x =>
{
    x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
    x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR(typeof(ApplicationModule).Assembly);
builder.Services.AddScoped(typeof(IPipelineBehavior<, >), typeof(CommandValidationBehavior<, >)); builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddControllers().AddFluentValidation(options=>options.RegisterValidatorsFromAssemblyContaining<ApplicationModule>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

    
var app = builder.Build();
ServiceProviderLocator.SetProvider(app.Services);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSerilogRequestLogging();

app.Run();

