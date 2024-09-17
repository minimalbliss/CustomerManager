using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Logs;

namespace CustomerManger.UI;

public class Program
{
    private static string APIBaseAddress = "APIBaseAddress";

    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddLogging();
        builder.Logging.ClearProviders();
        builder.Logging.AddOpenTelemetry(x => x.AddConsoleExporter());
        builder.Services.AddRazorPages();
        builder.Services.AddControllersWithViews();
        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        builder.Services.AddHttpClient();
        builder.Services.AddHttpClient("CustomerManager", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration[APIBaseAddress]!);
        });
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();
    }
}
