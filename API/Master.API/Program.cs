var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((_, lc) => lc
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Context}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.Seq("http://localhost:5341"));

// Add services to the container.
builder.Services.AddHttpLogging(options => options.LoggingFields = HttpLoggingFields.All);
builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(ApiModule.Assembly, ApplicationModule.Assembly, DomainModule.Assembly, InfrastructureModule.Assembly);
});
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(ApplicationModule.Assembly);
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<AppDbContext>();
builder.Services.AddDbContextPool<AppDbContext>((_, options) =>
{
    var connectionString = builder.Configuration.GetConnectionString("AppDbContext");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IExecutionContextAccessor, ExecutionContextAccessor>();
builder.Services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
builder.Services.AddScoped<ICommandsDispatcher, CommandsDispatcher>();
builder.Services.AddScoped<ICommandsScheduler, CommandsScheduler>();
builder.Services.AddScoped(typeof(ISqlConnectionFactory),
    _ => new SqlConnectionFactory(builder.Configuration.GetConnectionString("AppDbContext")));
builder.Services.AddQuartz(q => { q.UseMicrosoftDependencyInjectionJobFactory(); });
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

builder.Services.AddControllers(options =>
    {
        options.Filters.Add<ValidateModelAttribute>();
    })
    .AddJsonOptions(options =>
    {
        //格式化日期时间格式
        options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd HH:mm:ss"));
        //数据格式首字母小写
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        //数据格式原样输出
        // options.JsonSerializerOptions.PropertyNamingPolicy = null;
        //取消Unicode编码
        options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
        //忽略空值
        options.JsonSerializerOptions.IgnoreNullValues = false;
        //允许额外符号
        options.JsonSerializerOptions.AllowTrailingCommas = true;
        //反序列化过程中属性名称是否使用不区分大小写的比较
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddLocalization();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(_=>true);
    });
});
builder.Services.AddResponseCaching();
var app = builder.Build();
ServiceProviderLocator.SetProvider(app.Services);
await ApplicationStartup.Initialize(app.Services);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

IList<CultureInfo> supportedCultures = new List<CultureInfo>
{
    new CultureInfo("en-US"),
    new CultureInfo("zh-CN"),
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("zh-CN"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    ApplyCurrentCultureToResponseHeaders = true
});

app.UseResponseCaching();

app.UseMiddleware<ExceptionMiddleware>();

app.UseMiddleware<CorrelationMiddleware>();

app.UseHttpLogging();

app.UseCors();

app.UseForwardedHeaders();

app.UseAuthorization();

app.MapControllers();

app.UseSerilogRequestLogging();

app.Run();