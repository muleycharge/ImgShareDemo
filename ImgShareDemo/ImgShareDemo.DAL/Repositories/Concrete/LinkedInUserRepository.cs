namespace ImgShareDemo.DAL.Repositories.Concrete
{
    using ImgShareDemo.BO.Entities;

    internal class LinkedInUserRepository : GenericRepository<LinkedInUser>, ILinkedInUserRepository
    {
        public LinkedInUserRepository(ImgShareDemoContext context) : base(context) { }
    }
}
