using DeveloperStore.Sales.Infrastructure.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
                 .ReadFrom.Services(services)
                 .Enrich.FromLogContext()
                 .WriteTo.Console());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<DeveloperStore.Sales.Api.Middleware.ExceptionHandlingMiddleware>();
app.UseSerilogRequestLogging();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();

namespace DeveloperStore.Sales.Api;

public partial class Program;
