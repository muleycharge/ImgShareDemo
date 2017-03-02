namespace ImgShareDemo.DAL.Repositories.Concrete
{
    using System;
    using ImgShareDemo.BO.Entities;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data.Entity;
    using System.Threading.Tasks;

    internal class AssetRepository : GenericRepository<Asset>, IAssetRepository
    {
        public AssetRepository(ImgShareDemoContext context) : base(context) { }

        public async Task<IEnumerable<Asset>> GetUserAssets(int userId, string search, int take, int offset)
        {
            IQueryable<Asset> userAssetTags = from at in context.AssetTags
                                               where at.Tag.User.Id == userId 
                                                && (String.IsNullOrEmpty(search) || at.Tag.TagValue.StartsWith(search))
                                               select at.Asset;

            IQueryable<Asset> userAssets = from a in context.Assets
                                            where a.User.Id == userId 
                                                && (String.IsNullOrEmpty(search) || a.Description.Contains(search) || a.Name.Contains(search))
                                            select a;

            IQueryable<Asset> searchedAssets = userAssetTags.Union(userAssets).Distinct().OrderBy(a => a.Name).Skip(offset).Take(take);

            return await searchedAssets.ToListAsync().ConfigureAwait(false);
        }
    }
}
