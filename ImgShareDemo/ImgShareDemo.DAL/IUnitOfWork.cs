namespace ImgShareDemo.DAL
{
    using ImgShareDemo.DAL.Repositories;
    using System.Threading.Tasks;

    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        ILinkedInUserRepository LinkedInUserRepository { get; }
        IAssetRepository AssetRepository { get; }
        ITagRepository TagRepository { get; }
        
        int SaveChanges();

        Task<int> SaveChangesAsync();

        void Dispose();
    }
}