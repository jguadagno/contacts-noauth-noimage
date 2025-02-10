using Contacts.Domain.Interfaces;
using Contacts.Functions.Models;
using Contacts.ImageManager;
using JosephGuadagno.AzureHelpers.Storage;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

var settings = new Settings();
builder.Configuration.Bind("Settings", settings);
builder.Services.AddSingleton<ISettings>(settings);

builder.Services.TryAddSingleton<IImageStore>(provider =>
{
    var settings = provider.GetService<ISettings>();
                
    var blobs = IsDevelopment()
        ? new Blobs(settings.ContactBlobStorageAccount, settings.ContactImageContainerName)
        : new Blobs(settings.ContactBlobStorageAccountName, null, settings.ContactImageContainerName);
    return new ImageStore(blobs);
});
            
builder.Services.TryAddSingleton<IThumbnailImageStore>(provider =>
{
    var settings = provider.GetService<ISettings>();
    var blobs = IsDevelopment()
        ? new Blobs(settings.ContactBlobStorageAccount, settings.ContactThumbnailImageContainerName)
        : new Blobs(settings.ContactBlobStorageAccountName, null, settings.ContactThumbnailImageContainerName);
    return new ThumbnailImageStore(blobs);
});

builder.Services.TryAddSingleton<IImageManager, ImageManager>();

builder.Services.AddSingleton(_ => IsDevelopment()
    ? new Queue(settings.ThumbnailQueueStorageAccount, settings.ThumbnailQueueName)
    : new Queue(settings.ThumbnailQueueStorageAccountName, null, settings.ThumbnailQueueName));


builder.Build().Run();


bool IsDevelopment()
{
    return Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == "Development"; 
}