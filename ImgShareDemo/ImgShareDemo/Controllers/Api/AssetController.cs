namespace ImgShareDemo.Controllers.Api
{
    using Attributes;
    using BO.DataTransfer;
    using ImgShareDemo.BLL;
    using ImgShareDemo.Controllers.Base;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    [Authorize]
    [ApiException]
    public class AssetController : BaseApiController
    {
        private AssetService _assetService;

        public AssetController()
        {
            _assetService = new AssetService();
        }

        public AssetController(AssetService assetService)
        {
            _assetService = assetService;
        }

        [Route("api/Asset/Tag/{id}")]
        [HttpPost]
        public async Task Tag([FromUri] int id, [FromBody] TagDto tag)
        {
            await _assetService.AddTagToAsset(UserId, id, tag.Id);
        }

        [Route("api/Asset/RemoveTag/{id}")]
        [HttpPost]
        public async Task RemoveTag([FromUri] int id, [FromBody] TagDto tag)
        {
            await _assetService.RemoveTagFromAsset(UserId, id, tag.Id);
        }

        [Route("api/Asset/Upload/{id}")]
        [HttpPost]
        public async Task<ApiResponse<AssetDto>> Upload(int id)
        {
            IEnumerable<string> values;
            if (!Request.Content.Headers.TryGetValues("Content-Type", out values) || !values.Any())
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

        // GET: api/Asset
        [HttpGet]
        [Route("api/Asset")]
        public async Task<ApiResponse<PagedResponse<AssetDto>>> Get(string search = "", int? take = null, int? offset = null)
        {
            PagedResponse<AssetDto> assets = await _assetService.GetUserAssets(UserId, search, take ?? 100, offset ?? 0).ConfigureAwait(false);

            return new ApiResponse<PagedResponse<AssetDto>>
            {
                Data = assets
            };
        }

        // GET: api/Asset/5
        [HttpGet]
        [Route("api/Asset")]
        public async Task<ApiResponse<AssetDto>> Get(int id)
        {
            return new ApiResponse<AssetDto>
            {
                Data = (await _assetService.GetUserAsset(UserId, id).ConfigureAwait(false))
            };
        }

        // POST: api/Asset
        [HttpPost]
        [Route("api/Asset")]
        public async Task<ApiResponse<AssetDto>> Post([FromBody]AssetDto value)
        {
            value.Id = null; // Make sure that this is a insert operation.
            AssetDto asset = await _assetService.AddUpdateAsset(UserId, value).ConfigureAwait(false);
            return new ApiResponse<AssetDto>
            {
                Data = asset
            };
        }

        // PUT: api/Asset/5
        [HttpPut]
        [Route("api/Asset")]
        public async Task<ApiResponse<AssetDto>> Put([FromBody]AssetDto value)
        {
            if (!value.Id.HasValue)
            {
                throw new InvalidOperationException("Asset Id is null");
            }
            AssetDto asset = await _assetService.AddUpdateAsset(UserId, value).ConfigureAwait(false);
            return new ApiResponse<AssetDto>
            {
                Data = asset
            };
        }

        // DELETE: api/Asset/5
        [HttpDelete]
        [Route("api/Asset")]
        public async Task Delete(int id)
        {
            await _assetService.DeleteAssetImage(id, UserId);
        }
    }
}
