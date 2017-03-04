namespace ImgShareDemo.Controllers.Api
{
    using Attributes;
    using Base;
    using BLL;
    using BO.DataTransfer;
    using System.Threading.Tasks;
    using System.Web.Http;

    [ApiException]
    public class TagController : BaseApiController
    {
        private TagService _tagService;
        public TagController()
        {
            _tagService = new TagService();
        }

        public TagController(TagService tagService)
        {
            _tagService = tagService;
        }


        // GET: api/Tag
        [HttpGet]
        public async Task<ApiResponse<PagedResponse<TagDto>>> Get(string search = "", int? take = null, int? offset = null)
        {
            PagedResponse<TagDto> assets = await _tagService.GetTags(UserId, search, take ?? 100, offset ?? 0).ConfigureAwait(false);

            return new ApiResponse<PagedResponse<TagDto>>
            {
                Data = assets
            };
        }

        // POST: api/Tag
        [HttpPost]
        public async Task<ApiResponse<TagDto>> Post([FromBody]string value)
        {
            TagDto asset = await _tagService.AddTag(UserId, value).ConfigureAwait(false);
            return new ApiResponse<TagDto>
            {
                Data = asset
            };
        }

        // DELETE: api/Tag/5
        [HttpDelete]
        public async Task Delete(int id)
        {
            await _tagService.DeleteTag(UserId, id);
        }
    }
}