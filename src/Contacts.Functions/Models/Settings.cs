namespace Contacts.Functions.Models;

public class Settings: ISettings
{
    public string ContactBlobStorageAccount { get; set; }
    public string ContactBlobStorageAccountName { get;  set;}
    public string ContactImageContainerName  { get;  set;}
    public string ThumbnailQueueName  { get;  set;}
    public string ThumbnailQueueStorageAccount  { get;  set;}
    public string ThumbnailQueueStorageAccountName { get;  set;}
    public string ContactThumbnailBlobStorageAccount  { get;  set;}
    public string ContactThumbnailBlobStorageAccountName  { get;  set;}
    public string ContactThumbnailImageContainerName  { get;  set;}

    public int ResizeHeightSize { get; set; }

    public int ResizeWidthSize { get; set; }
}