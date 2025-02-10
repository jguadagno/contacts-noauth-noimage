namespace Contacts.WebUi.Models;

public interface ISettings
{
    string ApiRootUri { get; set; }
    string ContactBlobStorageAccount { get; set; }
    string ContactBlobStorageAccountName { get; set; }
    string ContactImageContainerName { get; set; }

    string ContactThumbnailBlobStorageAccount { get; set; }
    string ContactThumbnailBlobStorageAccountName { get; set; }
    string ContactThumbnailImageContainerName { get; set; }
    string ContactImageUrl { get; set; }
    
    string ThumbnailQueueName { get; set; }
    string ThumbnailQueueStorageAccount { get; set; }
    string ThumbnailQueueStorageAccountName { get; set; }
}