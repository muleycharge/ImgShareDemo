namespace ImgShareDemo.DAL
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public interface IImageDemoStorageContext
    {
        Task<Uri> AddUpdateBlobFile(Stream request, int userId, int assetId, string contentType);
        Task DeleteBlobByUrl(string blobUrl);
        string ImageBlobStorageBaseUrL();
        bool IsAssetsBlobStorageUrl(string blobUrl, int userId, int assetId);
    }
}