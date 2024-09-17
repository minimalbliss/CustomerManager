using CustomerManager.Api.Data;
using CustomerManager.Api.Infrastructure.EF;
using CustomerManager.Api.Middleware;
using CustomerManager.Api.Services;
using CustomerManager.Api.Services.Interfaces;
using CustomerManager.Api.Services.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Logs;

namespace CustomerManager.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Services.AddLogging();
            builder.Logging.ClearProviders();
            builder.Logging.AddOpenTelemetry(x => x.AddConsoleExporter());
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Customer Manager Api",
                        Version = "v1"
                    }
                 );

                string filePath = Path.Combine(AppContext.BaseDirectory, "CustomerManager.Api.xml");
                c.IncludeXmlComments(filePath);
            });
            builder.Services.AddDbContext<ApiDbContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddLogging();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddValidatorsFromAssemblyContaining<CustomerValidator>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Manager Api");
                    c.RoutePrefix = "";
                });
            }

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
