using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using onikaplus_server.DB;
using onikaplus_server.Interfaces;
using onikaplus_server.Swager;
using System.Reflection;

namespace onikaplus_server;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
#if DEBUG
            options.AddPolicy("onikaCORS",
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
#else
            options.AddPolicy("onikaCORS",
                builder =>
                {
                        builder.WithOrigins(Configuration["CORS:Origin"])
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                });
#endif
        });

        services.AddMvc();
        services.AddControllers();

        services.AddDbContext<IApplicationDbContext, AppDbContext>(options =>
        {
            options
                .UseSqlite("Data Source=onikaplus.db");
        });

        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddSwaggerDocument(settings =>
        {
            settings.Title = "OnikaPlus API";
            settings.SchemaSettings.SchemaNameGenerator = new SwagerSchemaNameGenerator();
            settings.PostProcess = document =>
            {
                document.Info.Version = "v1";
                document.Info.Title = "OnikaPlus API";
                document.Info.Description = "REST API for web server";
            };
        });

        services.AddHttpContextAccessor();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
    {
        app.UseCors("onikaCORS");
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseStaticFiles();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        if (environment.IsDevelopment())
        {
            app.UseOpenApi();
        }

        AppDbSeeder.SeedAsync(app.ApplicationServices).GetAwaiter().GetResult();
    }
}