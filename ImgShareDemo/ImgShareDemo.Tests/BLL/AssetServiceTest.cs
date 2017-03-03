using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using ImgShareDemo.BLL;
using ImgShareDemo.Tests.Mock.DAL;
using ImgShareDemo.DAL;
using System.IO;
using ImgShareDemo.BO.Entities;
using ImgShareDemo.BO.DataTransfer;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ImgShareDemo.Tests.BLL
{
    [TestClass]
    public class AssetServiceTest
    {
        MockUnitOfWork _mockUow = new MockUnitOfWork();

        HttpClient _client = new HttpClient();
        IImageDemoStorageContext _imageCtx;
        AssetService _assetSvc;
        AssetDto _testAsset;
        public AssetServiceTest()
        {
            _client = new HttpClient();
            _imageCtx = new ImageDemoStorageContext();
            _assetSvc = new AssetService(_mockUow, _imageCtx);
        }

        
        [TestMethod]
        public async Task AddAsset()
        {
            User testUser = AddTestUser();
            Asset testAsset = AddTestAsset(testUser);
            FileStream fs = new FileStream(@"..\..\Data\TestImage.png", FileMode.Open);
            _testAsset = await _assetSvc.AddUpdateAsset(testUser.Id, new AssetDto
            {
                Name = "Test Asset",
                Description = "Test Asset",
                UserId = testUser.Id
            }).ConfigureAwait(false);
            // Need to do this because the mock isn't complete
            _mockUow.AssetDb[_testAsset.Id.Value].User = testUser;

            _testAsset = await _assetSvc.UpdateAssetImage(fs, testUser.Id, _testAsset.Id.Value, "image/png").ConfigureAwait(false);

        }

        [TestMethod]
        public async Task GetAsset()
        {
            User testUser = AddTestUser();
            Asset testAsset = AddTestAsset(testUser);
            AssetDto asset = (await _assetSvc.GetUserAssets(testUser.Id).ConfigureAwait(false)).Items.Single();
            HttpResponseMessage response = await _client.GetAsync(asset.SourceUrl).ConfigureAwait(false);

            Assert.IsTrue(asset.Id == testAsset.Id);
            Assert.IsTrue(response.IsSuccessStatusCode);
            byte[] content = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            Assert.IsTrue(content.Length > 0);
        }
        [TestMethod]
        public async Task UpdateAsset()
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        [TestMethod]
        public async Task DeleteAsset()
        {
            User testUser = AddTestUser();
            Asset testAsset = AddTestAsset(testUser);

            await _assetSvc.DeleteAssetImage(testAsset.Id, testUser.Id).ConfigureAwait(false);

            HttpResponseMessage response = await _client.GetAsync(testAsset.SourceUrl).ConfigureAwait(false);

            Assert.IsFalse(response.IsSuccessStatusCode);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.NotFound);
        }


        private User AddTestUser(int id = 1)
        {
            User user = new User
            {
                Id = id,
                FirstName = "Test",
                LastName = "Test",
                Email = "test@test.test",
                ImageUrl = "https://www.gravatar.com/avatar/205e460b479e2e5b48aec07710c08d50?f=y",
                Username = "test",
                DateCreated = DateTime.Now,
            };

            _mockUow.UserDb.Add(id, user);
            return user;
        }

        private Asset AddTestAsset(User user, int id = 1)
        {
            Asset asset = new Asset
            {
                Id = id,
                Name = "Test Asset",
                Description = "Test Asset Description",
                SourceUrl = null,
                User = user,
                UserId = user.Id,
                AssetTags = new List<AssetTag>()
            };
            _mockUow.AssetDb.Add(id, asset);
            return asset;
        }
    }
}
