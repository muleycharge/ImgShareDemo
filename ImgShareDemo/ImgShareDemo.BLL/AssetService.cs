namespace ImgShareDemo.BLL
{
    using BO.DataTransfer;
    using BO.Entities;
    using Exceptions;
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
            IEnumerable<Asset> assets = (await _uow.AssetRepository.GetUserAssets(userId, search, take, offset, AssetDto.RequiredIncludes).ConfigureAwait(false));
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
            Asset asset = await _uow.AssetRepository.GetByIdAsync(assetId, AssetDto.RequiredIncludes).ConfigureAwait(false);
            if (asset == null || asset.User.Id != userId)
            {
                throw new ServiceLevelException($"Unable to find asset.");
            }

            return asset.ToAssetDto();
        }

        public async Task<AssetDto> AddUpdateAsset(int userId, AssetDto asset)
        {
            Asset toUpdate;
            if (asset.Id.HasValue)
            {
                toUpdate = await _uow.AssetRepository.GetByIdAsync(asset.Id.Value, AssetDto.RequiredIncludes).ConfigureAwait(false);
                if (asset == null || toUpdate.User.Id != userId)
                {
                    throw new ServiceLevelException($"Unable to find asset.");
                }
            }
            else
            {
                toUpdate = new Asset();
            }
            toUpdate.Id = asset.Id ?? 0;
            toUpdate.Name = asset.Name;
            toUpdate.Description = asset.Description;
            toUpdate.UserId = asset.UserId;

            _uow.AssetRepository.InsertOrUpdate(toUpdate);

            await _uow.SaveChangesAsync();

            return toUpdate.ToAssetDto();
        }

        public async Task<AssetDto> UpdateAssetImage(Stream stream, int userId, int assetId, string contentType)
        {
            Asset asset = await _uow.AssetRepository.GetByIdAsync(assetId, AssetDto.RequiredIncludes).ConfigureAwait(false);
            if (asset == null || asset.User.Id != userId)
            {
                throw new ServiceLevelException($"Unable to find asset.");
            }

            Uri uri = await _imageStorage.AddUpdateBlobFile(stream, userId, assetId, contentType).ConfigureAwait(false);
            if(_imageStorage.IsAssetsBlobStorageUrl(asset.SourceUrl, asset.UserId, asset.Id))
            {
                await _imageStorage.DeleteBlobByUrl(asset.SourceUrl).ConfigureAwait(false);
            }

            asset.SourceUrl = uri.AbsoluteUri;
            await _uow.AssetRepository.UpdateAsync(asset).ConfigureAwait(false);
            await _uow.SaveChangesAsync().ConfigureAwait(false);

            return asset.ToAssetDto();
        }

        public async Task DeleteAssetImage(int assetId, int userId)
        {
            Asset asset = await _uow.AssetRepository.GetByIdAsync(assetId).ConfigureAwait(false);
            if (asset == null || asset.UserId != userId)
            {
                throw new ServiceLevelException($"Unable to find asset.");
            }

            await _uow.AssetRepository.DeleteAsync(assetId).ConfigureAwait(false);

            if(!String.IsNullOrEmpty(asset.SourceUrl) && _imageStorage.IsAssetsBlobStorageUrl(asset.SourceUrl, userId, assetId))
            {
                await _imageStorage.DeleteBlobByUrl(asset.SourceUrl).ConfigureAwait(false);
            }
            await _uow.SaveChangesAsync();
        }

        public async Task AddTagToAsset(int userId, int assetId, int tagId)
        {
            Tag tag = await _uow.TagRepository.GetByIdAsync(tagId).ConfigureAwait(false);
            Asset asset = await _uow.AssetRepository.GetByIdAsync(assetId, "AssetTags").ConfigureAwait(false);

            if (tag == null || tag.UserId != userId)
            {
                throw new ServiceLevelException($"Unable to add tag to asset, tag not found.");
            }
            if (asset == null || asset.UserId != userId)
            {
                throw new ServiceLevelException($"Unable to add tag to asset, asset not found.");
            }
            if(asset.AssetTags.Any(at => at.TGT_Id == tag.Id))
            {
                throw new ServiceLevelException($"Unable to add tag to asset, tag is already assigned to asset.");
            }

            asset.AssetTags.Add(new AssetTag
            {
                ASA_Id = assetId,
                TGT_Id = tagId
            });
            await _uow.SaveChangesAsync();
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
