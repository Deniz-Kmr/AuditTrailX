using System.Text;
using AuditTrailX.Core.Interfaces;
using AuditTrailX.Core.Interfaces.Repositories;
using AuditTrailX.Data.Context;
using AuditTrailX.Data.Repositories;
using AuditTrailX.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AuditTrailX.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
    {
        // burada ef core'u postgresql ile bağlıyorum connection string appsettings'ten geliyor
        services.AddDbContext<AuditTrailXDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // burada interface ile implementasyonları eşleştiriyorum
        // scoped her http isteği için yeni bir instance oluşturulur
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<IAuditLogService, AuditLogService>();

        return services;
    }
}