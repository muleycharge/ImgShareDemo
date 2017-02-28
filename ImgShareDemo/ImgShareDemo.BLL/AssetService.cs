namespace ImgShareDemo.BLL
{
    using BO.DataTransfer;
    using BO.Entities;
    using Extensions;
    using ImgShareDemo.DAL;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AssetService : IDisposable
    {
        #region Fields
        IUnitOfWork _uow;
        IImageDemoStorageContext _imageStorage;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public AssetService()
        {
            _uow = new UnitOfWork();
            _imageStorage = new ImageDemoStorageContext();
        }

        public AssetService(IUnitOfWork unitOfWork, IImageDemoStorageContext imageStorage)
        {
            _uow = unitOfWork;
            _imageStorage = imageStorage;
        }
        #endregion

        #region Methods
        #region Public Methods
        public async Task<PagedResponse<AssetDto>> GetUserAssets(int userId, string search = null, int take = 100, int offset = 0)
        {
            IEnumerable<Asset> assets = (await _uow.AssetRepository.GetUserAssets(userId, search, take, offset).ConfigureAwait(false));
            bool last = assets.Count() < take;

            PagedResponse<AssetDto> response = new PagedResponse<AssetDto>
            {
                Items = assets.Select(a => a.ToAssetDto()).ToList(),
                LastPage = last
            };

            return response;
        }

        public async Task<AssetDto> GetUserAsset(int userId, int assetId)
        {
            Asset asset = await _uow.AssetRepository.GetByIdAsync(assetId, "User").ConfigureAwait(false);
            if (asset == null || asset.User.Id != userId)
            {
                throw new InvalidOperationException($"Invalid Asset ID. Asset ID {assetId} not found.");
            }

            return asset.ToAssetDto();
        }

        public async Task<AssetDto> AddUpdateAsset(AssetDto asset)
        {
            Asset assetData = new Asset
            {
                Id = asset.Id ?? 0,
                Name = asset.Name,
                Description = asset.Description,
                UserId = asset.UserId,
            };
            _uow.AssetRepository.InsertOrUpdate(assetData);

            await _uow.SaveChangesAsync();

            return assetData.ToAssetDto();
        }

        public async Task<AssetDto> UpdateAssetImage(Stream stream, int userId, int assetId, string contentType)
        {
            Asset asset = await _uow.AssetRepository.GetByIdAsync(assetId, "User").ConfigureAwait(false);
            if (asset == null || asset.User.Id != userId)
            {
                throw new InvalidOperationException($"Invalid Asset ID. Asset ID {assetId} not found.");
            }

            Uri uri = await _imageStorage.AddUpdateBlobFile(stream, userId, assetId, contentType).ConfigureAwait(false);

            asset.SourceUrl = uri.AbsoluteUri;
            await _uow.AssetRepository.UpdateAsync(asset).ConfigureAwait(false);
            await _uow.SaveChangesAsync().ConfigureAwait(false);

            return asset.ToAssetDto();
        }

        public async Task DeleteAssetImage(int assetId, int userId)
        {
            Asset asset = await _uow.AssetRepository.GetByIdAsync(assetId, "User").ConfigureAwait(false);
            if (asset == null || asset.User.Id != userId)
            {
                throw new InvalidOperationException($"Invalid Asset ID. Asset ID {assetId} not found.");
            }

            await _uow.AssetRepository.DeleteAsync(assetId).ConfigureAwait(false);

            if(!String.IsNullOrEmpty(asset.SourceUrl) && _imageStorage.IsAssetsBlobStorageUrl(asset.SourceUrl, userId, assetId))
            {
                await _imageStorage.DeleteBlobByUrl(asset.SourceUrl).ConfigureAwait(false);
            }
        }

        public void Dispose() => _uow.Dispose();
        #endregion

        #region Private Methods
        #endregion
        #endregion

        #region Static Declaration
        #endregion
    }
}
