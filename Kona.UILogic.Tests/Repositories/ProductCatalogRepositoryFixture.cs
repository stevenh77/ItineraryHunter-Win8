// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Kona.UILogic.Tests.Mocks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Kona.UILogic.Tests.Repositories
{
    [TestClass]
    public class ProductCatalogRepositoryFixture
    {
        [TestMethod]
        public async Task GetCategories_Calls_Service_When_Cache_Miss()
        {
            var cacheService = new MockCacheService();
            cacheService.DataExistsAndIsValidAsyncDelegate = s => Task.FromResult(false);
            cacheService.SaveExternalDataAsyncDelegate = s => Task.FromResult(new Uri("http://test.org"));
            cacheService.SaveDataAsyncDelegate = (s, c) => Task.FromResult(new Uri("http://test.org"));

            var productCatalogService = new MockProductCatalogService();
            var categories = new List<Category>
            {
                new Category{ Id = 1},
                new Category{ Id = 2}
            };

            productCatalogService.GetCategoriesAsyncDelegate = (depth) => Task.FromResult(new ReadOnlyCollection<Category>(categories));
            productCatalogService.GetSubcategoriesAsyncDelegate = (i) => Task.FromResult(new ReadOnlyCollection<Category>(null));
            
            var target = new ProductCatalogRepository(productCatalogService, cacheService);
            var returnedCategories = await target.GetCategoriesAsync(0);

            Assert.AreEqual(2, returnedCategories.Count);
            Assert.AreEqual(1, returnedCategories[0].Id);
            Assert.AreEqual(2, returnedCategories[1].Id);
        }

        [TestMethod]
        public async Task GetCategories_Uses_Cache_When_Data_Available()
        {
            var cacheService = new MockCacheService();
            cacheService.DataExistsAndIsValidAsyncDelegate = s => Task.FromResult(true);
            
            var categories = new List<Category>
            {
                new Category{ Id = 1},
                new Category{ Id = 2}
            };

            cacheService.GetDataDelegate = (string s) =>
            {
                if (s == "Categories0")
                    return new ReadOnlyCollection<Category>(categories);

                return new ReadOnlyCollection<Category>(null);
            };

            var productCatalogService = new MockProductCatalogService();
            productCatalogService.GetCategoriesAsyncDelegate = (depth) => Task.FromResult(new ReadOnlyCollection<Category>(null));

            var target = new ProductCatalogRepository(productCatalogService, cacheService);

            var returnedCategories = await target.GetCategoriesAsync(0);

            Assert.AreEqual(2, returnedCategories.Count);
            Assert.AreEqual(1, returnedCategories[0].Id);
            Assert.AreEqual(2, returnedCategories[1].Id);
        }

        [TestMethod]
        public async Task GetCategories_Saves_Data_To_Cache()
        {
            var cacheService = new MockCacheService();
            cacheService.DataExistsAndIsValidAsyncDelegate = s => Task.FromResult(false);
            cacheService.SaveExternalDataAsyncDelegate = s => Task.FromResult(new Uri("http://test.org"));

            cacheService.SaveDataAsyncDelegate = (s, o) =>
            {
                var collection = (ReadOnlyCollection<Category>)o;
                Assert.AreEqual("Categories0", s);
                Assert.AreEqual(2, collection.Count);
                Assert.AreEqual(1, collection[0].Id);
                Assert.AreEqual(2, collection[1].Id);
                return Task.FromResult(new Uri("http://test.org"));
            };

            var productCatalogService = new MockProductCatalogService();
            var categories = new List<Category>
                                 {
                                     new Category{ Id = 1},
                                     new Category{ Id = 2}
                                 };
            productCatalogService.GetCategoriesAsyncDelegate = (depth) => Task.FromResult(new ReadOnlyCollection<Category>(categories));
            productCatalogService.GetSubcategoriesAsyncDelegate =
                i => Task.FromResult(new ReadOnlyCollection<Category>(null));

            var target = new ProductCatalogRepository(productCatalogService, cacheService);

            await target.GetCategoriesAsync(0);
        }

        [TestMethod]
        public async Task GetSubcategories_Calls_Service_When_Cache_Miss()
        {
            var cacheService = new MockCacheService();
            cacheService.DataExistsAndIsValidAsyncDelegate = s => Task.FromResult(false);
            cacheService.SaveExternalDataAsyncDelegate = s => Task.FromResult(new Uri("http://test.org"));
            cacheService.SaveDataAsyncDelegate = (s, c) => Task.FromResult(new Uri("http://test.org"));

            var productCatalogService = new MockProductCatalogService();
            var subCategories = new List<Category>
                                 {
                                     new Category{ Id = 10},
                                     new Category{ Id = 11}
                                 };
            productCatalogService.GetSubcategoriesAsyncDelegate = (i) => Task.FromResult(new ReadOnlyCollection<Category>(subCategories));

            var target = new ProductCatalogRepository(productCatalogService, cacheService);

            var returnedSubcategories = await target.GetSubcategoriesAsync(1);

            Assert.AreEqual(2, returnedSubcategories.Count);
            Assert.AreEqual(10, returnedSubcategories[0].Id);
            Assert.AreEqual(11, returnedSubcategories[1].Id);
        }

        [TestMethod]
        public async Task GetSubcategories_Uses_Cache_When_Data_Available()
        {
            var cacheService = new MockCacheService();
            cacheService.DataExistsAndIsValidAsyncDelegate = s => Task.FromResult(true);
            var categories = new List<Category>
                                 {
                                     new Category{ Id = 10},
                                     new Category{ Id = 11}
                                 };
            cacheService.GetDataDelegate = s =>
            {
                if (s == "SubCategoriesOfCategoryId1")
                    return new ReadOnlyCollection<Category>(categories);

                return new ReadOnlyCollection<Category>(null);
            };

            var productCatalogService = new MockProductCatalogService();
            productCatalogService.GetSubcategoriesAsyncDelegate = (i) => Task.FromResult(new ReadOnlyCollection<Category>(null));

            var target = new ProductCatalogRepository(productCatalogService, cacheService);

            var returnedCategories = await target.GetSubcategoriesAsync(1);

            Assert.AreEqual(2, returnedCategories.Count);
            Assert.AreEqual(10, returnedCategories[0].Id);
            Assert.AreEqual(11, returnedCategories[1].Id);
        }

        [TestMethod]
        public async Task GetSubcategories_Saves_Data_To_Cache()
        {
            var cacheService = new MockCacheService();
            cacheService.DataExistsAndIsValidAsyncDelegate = s => Task.FromResult(false);
            cacheService.SaveExternalDataAsyncDelegate = s => Task.FromResult(new Uri("http://test.org"));
            cacheService.SaveDataAsyncDelegate = (s, o) => 
            {
                var collection = (ReadOnlyCollection<Category>)o;
                Assert.AreEqual("SubCategoriesOfCategoryId1", s);
                Assert.AreEqual(2, collection.Count);
                Assert.AreEqual(10, collection[0].Id);
                Assert.AreEqual(11, collection[1].Id);
                return Task.FromResult(new Uri("http://test.org"));
            };

            var productCatalogService = new MockProductCatalogService();
            var subCategories = new List<Category>
                                 {
                                     new Category{ Id = 10},
                                     new Category{ Id = 11}
                                 };
            productCatalogService.GetSubcategoriesAsyncDelegate = (i) => Task.FromResult(new ReadOnlyCollection<Category>(subCategories));

            var target = new ProductCatalogRepository(productCatalogService, cacheService);

            await target.GetSubcategoriesAsync(1);
        }
    }
}
