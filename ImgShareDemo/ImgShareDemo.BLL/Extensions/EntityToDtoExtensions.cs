namespace ImgShareDemo.BLL.Extensions
{
    using BO.DataTransfer;
    using BO.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
            };
        }
        #endregion
    }
}
