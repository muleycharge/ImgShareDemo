namespace ImgShareDemo.Tests.Mock.DAL
{
    using ImgShareDemo.DAL;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ImgShareDemo.DAL.Repositories;
    using BO.Entities;

    public class MockUnitOfWork : IUnitOfWork
    {
        public Dictionary<int, Asset> AssetDb { get; protected set; } = new Dictionary<int, Asset>();
        public Dictionary<int, Tag> TagDb { get; protected set; } = new Dictionary<int, Tag>();
        public Dictionary<int, LinkedInUser> LinkedInUserDb { get; protected set; } = new Dictionary<int, LinkedInUser>();
        public Dictionary<int, User> UserDb { get; protected set; } = new Dictionary<int, User>();
        
        public MockUnitOfWork()
        {
            AssetDb = new Dictionary<int, Asset>();
            TagDb = new Dictionary<int, Tag>();
            LinkedInUserDb = new Dictionary<int, LinkedInUser>();
            UserDb = new Dictionary<int, User>();
        }
        public IAssetRepository AssetRepository
        {
            get
            {
                return new MockAssetRepository(AssetDb);
            }
        }

        public ILinkedInUserRepository LinkedInUserRepository
        {
            get
            {
                return new MockLinkedInUserRepository(LinkedInUserDb);
            }
        }

        public ITagRepository TagRepository
        {
            get
            {
                return new MockTagRepository(TagDb);
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                return new MockUserRepository(UserDb);
            }
        }

        public void Dispose()
        {
        }

        public int SaveChanges()
        {
            return 1;
        }

        public async Task<int> SaveChangesAsync()
        {
            await Task.CompletedTask.ConfigureAwait(false);
            return 1;
        }
    }
}
