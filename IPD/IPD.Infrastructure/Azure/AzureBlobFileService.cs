using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using IPD.Infrastructure.Helpers;
using System.Text;

namespace IPD.Infrastructure.Azure
{
    public class FileItem
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsFolder { get; set; }
        public DateTimeOffset LastModifiedDate { get; set; }
        public string Type
        {
            get
            {
                if (IsFolder)
                    return "folder";

                switch (Path.GetExtension(Name).ToLower())
                {
                    case ".jpg":
                    case ".png":
                    case ".gif":
                        return "image";
                    case ".mp3":
                        return "mp3";
                    case ".pdf":
                        return "pdf";
                    default:
                        return "unknown";
                }
            }
        }
    }

    public class AzureBlobFileService
    {
        private static Dictionary<string, AzureBlobFileService> services = new Dictionary<string, AzureBlobFileService>();
        public static AzureBlobFileService ByContainerName(string containerName)
        {
            if (!services.ContainsKey(containerName))
                services[containerName] = new AzureBlobFileService(containerName);
            return services[containerName];
        }
        private BlobContainerClient _container;

        public AzureBlobFileService(string containerName)
        {
            _container = InitBlobContainer(containerName);
        }

   
        private async Task<AsyncPageable<BlobItem>> GetBlobList(string path = null)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return _container.GetBlobsAsync();
            }
            else
            {
                return _container.GetBlobsAsync(prefix: path);
            }
        }

        private static BlobServiceClient blobServiceClient = null;
        public static BlobContainerClient InitBlobContainer(string containerName)
        {
            if (blobServiceClient == null)
            {
                blobServiceClient = new BlobServiceClient(AppSettingsHelper.Setting("AzureStorageConfig:ConnectionString"));
            }

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            containerClient.CreateIfNotExistsAsync();


            return containerClient;
        }


        /// <summary>
        /// Creates folder
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        public async void CreateFolder(string name, string path = "")
        {
            var blobReference = _container.GetBlobClient((string.IsNullOrWhiteSpace(path) ? "" : path + "/") + name + "/__");

            using (var mem = new MemoryStream())
            {
                using (var sw = new StreamWriter(mem))
                {
                    sw.Write(string.Empty);
                    sw.Flush();//otherwise you are risking empty stream
                    mem.Seek(0, SeekOrigin.Begin);
                }
                await blobReference.UploadAsync(mem, true);
            }
        }

        public void DeleteFile(string path)
        {
            var blob = _container.GetBlobClient(path);
            blob.DeleteIfExistsAsync();
        }


        /// <summary>
        /// Gets folder path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IList<FileItem> GetFolderPaths(string path)
        {
            var list = new List<FileItem>
                {
                    new FileItem
                    {
                        IsFolder = true,
                        Name = "Root",
                        Url = string.Empty
                    }
                };

            if (string.IsNullOrWhiteSpace(path))
            {
                return list;
            }

            var paths = path.Trim('/').Split('/');

            FileItem item = null;

            foreach (var p in paths)
            {
                item = new FileItem
                {
                    Name = p,
                    IsFolder = true,
                    Url = item == null ? p : item.Url + "/" + p
                };
                list.Add(item);
            }

            return list;
        }


        /// <summary>
        /// Save file on storage
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="path"></param>
        public void SaveFile(Stream fileStream, string path)
        {
            var blob = _container.GetBlobClient(path);
            blob.UploadAsync(fileStream);
        }
    }
}
