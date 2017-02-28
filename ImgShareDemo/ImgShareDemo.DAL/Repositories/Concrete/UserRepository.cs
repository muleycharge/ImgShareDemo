namespace ImgShareDemo.DAL.Repositories.Concrete
{
    using ImgShareDemo.BO.Entities;

    internal class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ImgShareDemoContext context) : base(context) { }
    }
}
