namespace ImgShareDemo.DAL.Repositories
{
    using ImgShareDemo.BO.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAssetRepository : IGenericRepository<Asset>
    {
        Task<IEnumerable<Asset>> GetUserAssets(int userId, string search, int take, int offset, params string[] includes);
    }
}
