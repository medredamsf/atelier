using AtelierCleanApp.Application.Contracts;
using AtelierCleanApp.Application.Services;
using AtelierCleanApp.Infrastructure.Contracts;
using AtelierCleanApp.Infrastructure.Persistence;
using AtelierCleanApp.Infrastructure.Repositories;
using AtelierCleanApp.Application.Mappings;
using Microsoft.EntityFrameworkCore;

namespace AtelierCleanApp.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AtelierCleanAppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(AtelierCleanAppDbContext).Assembly.FullName);
                }));
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<IPlayerService, PlayerService>();
        services.AddScoped<IPlayerStatisticsService, PlayerStatisticsService>();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "AtelierCleanApp API", Version = "v1" });
        });
        
        return services;
    }

    public static WebApplication ConfigureRequestPipeline(this WebApplication app)
    {
        
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AtelierCleanApp API v1"));
        app.UseDeveloperExceptionPage();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.MapControllers();
        
        return app;
    }
}
