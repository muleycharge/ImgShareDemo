namespace ImgShareDemo.Tests.Mock.DAL
{
    using BO.Entities;
    using ImgShareDemo.DAL.Repositories;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MockAssetRepository : MockGenericRepository<Asset>, IAssetRepository
    {
        public MockAssetRepository(Dictionary<int, Asset> mockDb) : base(mockDb) { }
        public async Task<IEnumerable<Asset>> GetUserAssets(int userId, string search, int take, int offset)
        {
            await Task.CompletedTask.ConfigureAwait(false);
            return _mockDb.Values.Skip(offset).Take(take);
        }
    }

    public class MockTagRepository : MockGenericRepository<Tag>, ITagRepository
    {
        public MockTagRepository(Dictionary<int, Tag> mockDb) : base(mockDb) { }

    }

    public class MockLinkedInUserRepository : MockGenericRepository<LinkedInUser>, ILinkedInUserRepository
    {
        public MockLinkedInUserRepository(Dictionary<int, LinkedInUser> mockDb) : base(mockDb) { }

    }

    public class MockUserRepository : MockGenericRepository<User>, IUserRepository
    {

        public MockUserRepository(Dictionary<int, User> mockDb) : base(mockDb) { }
    }
}
