using FluentValidation;
using MediatR;
using Serilog;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.HttpLogging;
using Velen.API.Configuration;
using Velen.API.Middlewares;
using Velen.Application;
using Velen.Application.Configuration.Validation;
using Velen.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((ctx, lc) => lc
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Context}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.Seq("http://localhost:5341"));

// Add services to the container.
builder.Services.AddHttpLogging(options=>options.LoggingFields = HttpLoggingFields.All);
builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR(typeof(ApplicationModule).Assembly);
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandValidationBehavior<,>));
builder.Services.AddValidatorsFromAssemblyContaining<ApplicationModule>();
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

