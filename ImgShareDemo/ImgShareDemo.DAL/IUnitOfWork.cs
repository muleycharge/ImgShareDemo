using ImgShareDemo.DAL.Repositories;
using System.Threading.Tasks;

namespace ImgShareDemo.DAL
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        IUserSignOnRepository UserSignOnRepository { get; }

        ILinkedInUserRepository LinkedInUserRepository { get; }
        
        int SaveChanges();

        Task<int> SaveChangesAsync();

        void Dispose();
    }
}