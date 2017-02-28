namespace ImgShareDemo.Controllers.Api
{
    using BO.DataTransfer;
    using ImgShareDemo.BLL;
    using ImgShareDemo.Controllers.Base;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class AssetController : BaseApiController
    {
        private AssetService _assetService;

        private int UserId
        {
            get
            {
                return this.User.Identity.GetUserId();
            }
        }
        public AssetController()
        {
            _assetService = new AssetService();
        }

        public AssetController(AssetService assetService)
        {
            _assetService = assetService;
        }

        // GET: api/Asset
        public async Task<ApiResponse<PagedResponse<AssetDto>>> Get(string search = "" , int? take = null, int? offset = null )
        {
            PagedResponse<AssetDto> assets = await _assetService.GetUserAssets(UserId, search, take ?? 100, offset ?? 0).ConfigureAwait(false);

            return new ApiResponse<PagedResponse<AssetDto>>
            {
                Data = assets
            };
        }

        // GET: api/Asset/5
        public async Task<ApiResponse<AssetDto>> Get(int id)
        {
            return new ApiResponse<AssetDto>
            {
                Data = (await _assetService.GetUserAsset(UserId, id).ConfigureAwait(false))
            };
        }

        // POST: api/Asset
        public async Task Post([FromBody]AssetDto value)
        {
            value.Id = null; // Make sure that this is a insert operation.
            await _assetService.AddUpdateAsset(value).ConfigureAwait(false);
        }

        // PUT: api/Asset/5
        public async Task Put(int id, [FromBody]AssetDto value)
        {
            if (!value.Id.HasValue)
            {
                throw new InvalidOperationException("Asset Id is null");
            }
            await _assetService.AddUpdateAsset(value).ConfigureAwait(false);
        }

        // DELETE: api/Asset/5
        public async Task Delete(int id)
        {
            await _assetService.DeleteAssetImage(id, UserId);
        }

        public async Task<ApiResponse<AssetDto>> Upload(int id)
        {
            IEnumerable<string> values;
            if (!Request.Headers.TryGetValues("Content-Type", out values) || !values.Any())
            {
                throw new InvalidOperationException("Content-Type header is required");
            }
            string contentType = values.First();
            using (Stream stream = await Request.Content.ReadAsStreamAsync().ConfigureAwait(false))
            {
                ApiResponse<AssetDto> response = new ApiResponse<AssetDto>();
                AssetDto asset = await _assetService.UpdateAssetImage(stream, UserId, id, contentType).ConfigureAwait(false);
                response.Data = asset;
                return response;
            }
        }
    }
}
