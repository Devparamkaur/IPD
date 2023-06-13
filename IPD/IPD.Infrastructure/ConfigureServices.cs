using IPD.Application.Interfaces.Repositories;
using IPD.Application.Interfaces.Services;
using IPD.Application.Interfaces.Services.Identity;
using IPD.Infrastructure.Emails.SendGrid;
using IPD.Infrastructure.Helpers;
using IPD.Infrastructure.Identity;
using IPD.Infrastructure.Identity.Interfaces;
using IPD.Infrastructure.Identity.Services;
using IPD.Infrastructure.JWT;
using IPD.Infrastructure.Persistance;
using IPD.Infrastructure.Repositories;
using IPD.Infrastructure.Services;
using IPD.Shared.Wrapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        AppSettingsHelper.AppSettingsConfigure(configuration);


        // Add services to the container.
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            // Default Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 7;
        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<JwtHandler>();


        var jwtSettings = configuration.GetSection("JwtSettings");
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(bearer =>
        {
            bearer.RequireHttpsMetadata = false;
            bearer.SaveToken = true;
            bearer.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["validIssuer"],
                ValidAudience = jwtSettings["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(jwtSettings.GetSection("securityKey").Value)),
                RoleClaimType = ClaimTypes.Role
            };
            bearer.Events = new JwtBearerEvents()
            {
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    var result = JsonConvert.SerializeObject(Result.Fail("You are not Authorized."));
                    return context.Response.WriteAsync(result);
                },
                OnForbidden = context =>
                {
                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "application/json";
                    var result = JsonConvert.SerializeObject(Result.Fail("You are not authorized to access this resource."));
                    return context.Response.WriteAsync(result);
                },
            };
        }).AddCookie();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IAccountService, AccountService>();
        
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

        return services;
    }
}
