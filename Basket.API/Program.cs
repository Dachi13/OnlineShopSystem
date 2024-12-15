using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

var dbConnectionString = builder.Configuration.GetConnectionString("Database")!;
var redisConnectionString = builder.Configuration.GetConnectionString("Redis")!;

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddMarten(opts =>
    {
        opts.Connection(dbConnectionString);
        opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
    })
    .UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(options => { options.Configuration = redisConnectionString; });

builder.Services.AddHealthChecks();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(dbConnectionString)
    .AddRedis(redisConnectionString);

var app = builder.Build();

app.MapCarter();
app.UseExceptionHandler(_ => { });
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();