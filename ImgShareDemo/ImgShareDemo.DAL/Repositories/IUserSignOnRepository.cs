namespace ImgShareDemo.DAL.Repositories
{
    using ImgShareDemo.BO.Entities;

    public interface IUserSignOnRepository : IGenericRepository<UserSignOn>
    {
        UserSignOn GenerateNew(string returnUrl);
    }
}
