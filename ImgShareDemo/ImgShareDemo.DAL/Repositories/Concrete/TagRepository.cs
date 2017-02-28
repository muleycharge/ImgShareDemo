namespace ImgShareDemo.DAL.Repositories.Concrete
{
    using ImgShareDemo.BO.Entities;
    using Repositories;

    internal class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(ImgShareDemoContext context) : base(context) { }
    }
}
