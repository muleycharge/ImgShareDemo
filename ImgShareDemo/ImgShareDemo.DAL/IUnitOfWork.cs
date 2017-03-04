namespace ImgShareDemo.DAL
{
    using ImgShareDemo.DAL.Repositories;
    using System.Threading.Tasks;

    public interface IUnitOfWork
    {
        IAssetRepository AssetRepository { get; }
        IUserRepository UserRepository { get; }
        ILinkedInUserRepository LinkedInUserRepository { get; }
        ITagRepository TagRepository { get; }
        
        int SaveChanges();

        Task<int> SaveChangesAsync();

        void Dispose();
    }
}