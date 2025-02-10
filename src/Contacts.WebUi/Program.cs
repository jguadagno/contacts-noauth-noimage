using System;
using Contacts.Domain.Interfaces;
using Contacts.WebUi.Models;
using Contacts.WebUi.Services;
using JosephGuadagno.AzureHelpers.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using WebApplication = Microsoft.AspNetCore.Builder.WebApplication;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    ConfigureServices(builder.Configuration, builder.Services, builder.Environment);

    var app = builder.Build();
    ConfigureMiddleware(app, builder.Environment);
    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}

void ConfigureServices(IConfiguration configuration, IServiceCollection services, IWebHostEnvironment environment)
{
    var settings = new Settings();
    configuration.Bind("Settings", settings);
            
    //  For Project Tye
    var uri = configuration.GetServiceUri("contacts-api", "https");
    // If the uri is null we assume Tye is not running.
    // If not null, assure Uri ends in /
    if (uri != null)
    {
        var url = uri.AbsoluteUri;
        if (!url.EndsWith("/"))
        {
            url += "/";
        }
        // https://localhost:5001/
        settings.ApiRootUri = url;
    }
    
    services.AddSingleton(settings);
    

    
    services.AddControllersWithViews();
    services.AddContactService();
    services.AddRazorPages();
}

void ConfigureMiddleware(WebApplication app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.MapDefaultControllerRoute();
    app.MapRazorPages();
}
