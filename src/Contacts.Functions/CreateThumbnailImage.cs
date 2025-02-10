using System;
using Azure.Storage.Queues.Models;
using Contacts.Domain.Interfaces;
using Contacts.Functions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Contacts.Functions;

public class CreateThumbnailImage
{
    private readonly ISettings _settings;
    private readonly IImageManager _imageManager;

    public CreateThumbnailImage(ISettings settings, IImageManager imageManager)
    {
        _settings = settings;
        _imageManager = imageManager;
    }

    [Function(nameof(CreateThumbnailImage))]
    public async Task RunAsync([QueueTrigger("thumbnail-create")]
        Domain.Models.Messages.ImageToConvert imageToConvert)
    {
    
        var imageUrl = await _imageManager.CreateThumbnailImageAsync(imageToConvert.ImageName,
            _settings.ResizeWidthSize, _settings.ResizeHeightSize);

        // TODO: Do something with the Thumbnail Url
    }
}