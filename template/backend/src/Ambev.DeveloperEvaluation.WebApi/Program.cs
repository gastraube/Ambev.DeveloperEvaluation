using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Ambev.DeveloperEvaluation.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Log.Information("Starting web application");

            var builder = WebApplication.CreateBuilder(args);

            // Serilog
            builder.AddDefaultLogging();

            // Controllers e Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DeveloperEvaluation API", Version = "v1" });

                var jwtScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Informe **apenas** o JWT (sem o prefixo 'Bearer ').",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(jwtScheme.Reference.Id, jwtScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtScheme, Array.Empty<string>() }
                });
            });

            // Healthchecks
            builder.AddBasicHealthChecks();

            // DbContext
            builder.Services.AddDbContext<DefaultContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
                )
            );

            // JWT
            builder.Services.AddJwtAuthentication(builder.Configuration);

            // Injeção de dependência custom
            builder.RegisterDependencies();

            // MediatR
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(Program).Assembly,
                    typeof(ApplicationLayer).Assembly
                );
            });

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

            // Validation pipeline
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // Repositories
            builder.Services.AddScoped<ISaleRepository, SaleRepository>();

            var app = builder.Build();

            // Middlewares
            app.UseMiddleware<ValidationExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Auth
            app.UseAuthentication();
            app.UseAuthorization();

            // Healthchecks
            app.UseBasicHealthChecks();

            // Controllers
            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
            throw;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
