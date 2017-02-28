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
        private Dictionary<int, Asset> _assetDb;
        private Dictionary<int, Tag> _tagDb;
        private Dictionary<int, LinkedInUser> _linkedInUserDb;
        private Dictionary<int, User> _user;

        User _testUser = new User
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.test",
            ImageUrl = "https://www.gravatar.com/avatar/205e460b479e2e5b48aec07710c08d50?f=y",
            Username = "test",
            DateCreated = DateTime.Now,
        };

        public User TestUser
        {
            get
            {
                return _testUser;
            }
        }

        public MockUnitOfWork()
        {
            _assetDb = new Dictionary<int, Asset>();
            _tagDb = new Dictionary<int, Tag>();
            _linkedInUserDb = new Dictionary<int, LinkedInUser>();
            _user = new Dictionary<int, User>();
            // Initialize test user.
            UserRepository.Insert(_testUser);
        }
        public IAssetRepository AssetRepository
        {
            get
            {
                return new MockAssetRepository(_assetDb);
            }
        }

        public ILinkedInUserRepository LinkedInUserRepository
        {
            get
            {
                return new MockLinkedInUserRepository(_linkedInUserDb);
            }
        }

        public ITagRepository TagRepository
        {
            get
            {
                return new MockTagRepository(_tagDb);
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                return new MockUserRepository(_user);
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
