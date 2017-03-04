namespace ImgShareDemo.BLL.Extensions
{
    using BO.DataTransfer;
    using BO.Entities;
    using System.Collections.Generic;
    using System.Linq;

    public static class EntityToDtoExtensions
    {
        #region Asset Extensions
        public static AssetDto ToAssetDto(this Asset asset)
        {
            return new AssetDto
            {
                Id = asset.Id,
                Name = asset.Name,
                Description = asset.Description,
                SourceUrl = asset.SourceUrl,
                UserId = asset.UserId,
                Tags = asset.AssetTags?.Select(at => at.Tag.ToTagDto()) ?? new List<TagDto>()
            };
        }

        public static TagDto ToTagDto(this Tag tag)
        {
            return new TagDto
            {
                Id = tag.Id,
                Name = tag.TagValue
            };
        }
        #endregion
    }
}
