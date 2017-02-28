namespace ImgShareDemo.DAL
{
    using Microsoft.WindowsAzure.Storage;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;
    using MimeTypeMap.List;
    using Microsoft.WindowsAzure.Storage.Blob;
    using System.Text.RegularExpressions;
    using System.IO;

    public class ImageDemoStorageContext : IImageDemoStorageContext
    {
        #region Fields
        private string _containerName;
        private string _storageConnectionString;
        private CloudStorageAccount _storageAccount;
        private readonly string[] _supportedMimeTypes =
        {
            // Any items added to this list should also be added to _assetTypeToContentTypeMap.
            ".png",
            ".jpg",
            ".doc",
            ".gif"
        };
        #endregion

        #region Properties        
        #endregion

        #region Constructors
        /// <summary>
        /// Convention over configuration constructor
        /// </summary>
        public ImageDemoStorageContext()
        {
            _storageConnectionString = ConfigurationManager.ConnectionStrings[nameof(ImageDemoStorageContext)].ConnectionString;
            _storageAccount = CloudStorageAccount.Parse(_storageConnectionString);
            _containerName = ConfigurationManager.AppSettings["ImageStorageContainerName"];
        }

        public ImageDemoStorageContext(string connectionString)
        {
            _storageConnectionString = connectionString;
            _storageAccount = CloudStorageAccount.Parse(_storageConnectionString);
            _containerName = ConfigurationManager.AppSettings["ImageStorageContainerName"];
        }
        #endregion

        #region Methods
        #region Public Methods
        public string ImageBlobStorageBaseUrL()
        {
            return $"{_storageAccount.BlobEndpoint.AbsoluteUri}{_containerName}/";
        }

        public async Task<Uri> AddUpdateBlobFile(Stream request, int userId, int assetId, string contentType)
        {
            ValidateMimeType(contentType);
            string fileName = GetImageFileName(userId, assetId, contentType);
            Uri uri = await UploadAssetContentToBlobStorage(request, fileName, contentType).ConfigureAwait(false);
            return uri;
        }

        public async Task DeleteBlobByUrl(string blobUrl)
        {
            CloudBlob toDelete = await GetBlobFromUrl(blobUrl).ConfigureAwait(false);
            if (toDelete != null)
            {
                await toDelete.DeleteAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Determine whether url points to this environments blob storage server and
        /// that the name is formatted exactly as it would be for this asset and for
        /// this blob storage context.
        /// </summary>
        /// <param name="blobUrl"></param>
        /// <param name="userId"></param>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public bool IsAssetsBlobStorageUrl(string blobUrl, int userId, int assetId)
        {
            return !String.IsNullOrEmpty(blobUrl)
                && blobUrl.StartsWith(ImageBlobStorageBaseUrL())
                && Regex.IsMatch(blobUrl, $"{userId}/{assetId}.\\w+$");
        }
        #endregion

        #region Private Methods
        private async Task<CloudBlob> GetBlobFromUrl(string url)
        {
            string baseUrl = ImageBlobStorageBaseUrL();
            if (!String.IsNullOrEmpty(url) && url.StartsWith(baseUrl))
            {
                CloudBlobContainer blobContainer = GetContainer(_containerName);
                string blobName = url.Substring(baseUrl.Length, url.Length - baseUrl.Length);
                CloudBlob blob = blobContainer.GetBlobReference(blobName);
                if (await blob.ExistsAsync().ConfigureAwait(false))
                {
                    return blob;
                }
            }
            return null;
        }

        private async Task<Uri> UploadAssetContentToBlobStorage(Stream fileStream, string blobName, string contentMimeType)
        {

            CloudBlobContainer blobContainer = GetContainer(_containerName);

            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(blobName);
            blob.Properties.ContentType = contentMimeType;
            await blob.UploadFromStreamAsync(fileStream);
            return blob.Uri;
        }

        private CloudBlobContainer GetContainer(string containerName)
        {
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();

            CloudBlobContainer blobContainer = blobClient.GetContainerReference(containerName);
            return blobContainer;
        }

        private void ValidateMimeType(string mimeType)
        {
            IEnumerable<string> extensions = MimeTypeMap.GetExtension(mimeType).Intersect(_supportedMimeTypes);

            if (!extensions.Any())
            {
                throw new NotSupportedException($"File type is not supported.");
            }
        }

        private string GetImageFileName(int userId, int assetId, string mimeType)
        {
            return $"{userId}/{assetId}{GetExtensionFromMimeType(mimeType)}";
        }

        private string GetExtensionFromMimeType(string mimeType)
        {
            try
            {
                return MimeTypeMap.GetExtension(mimeType).Intersect(_supportedMimeTypes).First();
            }
            catch (ArgumentNullException ex)
            {
                throw new InvalidOperationException($"Mime type is required", ex);
            }
            catch (ArgumentException)
            {
                throw new NotSupportedException($"file type {mimeType ?? "NULL"} is not supported.");
            }
        }
        #endregion
        #endregion

        #region Static Declaration
        #endregion
    }
}
