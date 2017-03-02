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

namespace ImgShareDemo.Tests.BLL
{
    [TestClass]
    public class AssetServiceTest
    {
        HttpClient _client = new HttpClient();
        IImageDemoStorageContext _imageCtx;
        AssetService _assetSvc;
        MockUnitOfWork _mockUow;
        AssetDto _testAsset;
        public AssetServiceTest()
        {
            _client = new HttpClient();
            _imageCtx = new ImageDemoStorageContext();
            _mockUow = new MockUnitOfWork();
            _assetSvc = new AssetService(_mockUow, _imageCtx);
        }


        [TestMethod]
        public void TestAssetService()
        {
            AddAsset().Wait();
            GetAsset().Wait();
            UpdateAsset().Wait();
            DeleteAsset().Wait();
        }

        public async Task AddAsset()
        {
            _mockUow.TestUser.Username = "test@test.com";
            FileStream fs = new FileStream(@"..\..\Data\TestImage.png", FileMode.Open);
            _testAsset = await _assetSvc.AddUpdateAsset(_mockUow.TestUser.Id, new AssetDto
            {
                Name = "Test Asset",
                Description = "Test Asset",
                UserId = _mockUow.TestUser.Id
            }).ConfigureAwait(false);
            _mockUow.AssetRepository.GetById(_testAsset.Id.Value).User = _mockUow.TestUser;

            _testAsset = await _assetSvc.UpdateAssetImage(fs, _mockUow.TestUser.Id, _testAsset.Id.Value, "image/png").ConfigureAwait(false);
        }

        public async Task GetAsset()
        {
            AssetDto asset = (await _assetSvc.GetUserAssets(_mockUow.TestUser.Id).ConfigureAwait(false)).Items.Single();
            HttpResponseMessage response = await _client.GetAsync(asset.SourceUrl).ConfigureAwait(false);

            Assert.IsTrue(asset.Id == _testAsset.Id);
            Assert.IsTrue(response.IsSuccessStatusCode);
            byte[] content = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            Assert.IsTrue(content.Length > 0);
        }
        public async Task UpdateAsset()
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }
        public async Task DeleteAsset()
        {
            await _assetSvc.DeleteAssetImage(_testAsset.Id.Value, _mockUow.TestUser.Id).ConfigureAwait(false);

            HttpResponseMessage response = await _client.GetAsync(_testAsset.SourceUrl).ConfigureAwait(false);

            Assert.IsFalse(response.IsSuccessStatusCode);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.NotFound);
        }
    }
}
