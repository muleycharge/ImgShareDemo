namespace ImgShareDemo.DAL.Repositories.Concrete
{
    using System;
    using ImgShareDemo.BO.Entities;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data.Entity;
    using System.Threading.Tasks;

    internal class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(ImgShareDemoContext context) : base(context) { }

        public async Task<IEnumerable<Tag>> GetTags(int userId, string search = null, int take = 100, int offset = 0, params string[] includes)
        {
            var query = from t in context.Tags
                       where t.UserId == userId && t.TagValue.StartsWith(search)
                       select t;
            foreach (var includeProperty in includes)
            {
                query = query.Include(includeProperty);
            }
            return await query.OrderByDescending(t => t.Id).Skip(offset).Take(100).ToListAsync();
        }
    }
}
