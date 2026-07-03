using Ambev.DeveloperEvaluation.IoC.DependencyInjection;
using Serilog;

namespace Ambev.DeveloperEvaluation.WebApi;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Logging.AddFilter("LuckyPennySoftware.MediatR.License", LogLevel.None);

        builder.Host.UseSerilog((context, services, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration)
                         .ReadFrom.Services(services)
                         .Enrich.FromLogContext()
                         .WriteTo.Console());

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSales(builder.Configuration);

        var app = builder.Build();

        app.UseMiddleware<Middleware.ExceptionHandlingMiddleware>();
        app.UseSerilogRequestLogging();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseSalesDatabaseAsync().GetAwaiter().GetResult();
        app.MapControllers();
        app.Run();
    }
}

