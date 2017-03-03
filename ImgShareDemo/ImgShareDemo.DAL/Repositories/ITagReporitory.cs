namespace ImgShareDemo.DAL.Repositories
{
    using ImgShareDemo.BO.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITagRepository : IGenericRepository<Tag>
    {
        Task<IEnumerable<Tag>> GetTags(int userId, string search = null, int take = 100, int offset = 0, params string[] includes);
    }
}
