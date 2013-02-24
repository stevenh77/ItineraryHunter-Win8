// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.Services;
using Kona.UILogic.Tests.Mocks;
using Kona.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI.Xaml.Navigation;
using System.Collections.Generic;
using System.Net.Http;

namespace Kona.UILogic.Tests.ViewModels
{
    [TestClass]
    public class ItemDetailPageViewModelFixture
    {
        [TestMethod]
        public void OnNavigatedTo_Fill_Items_And_SelectedProduct()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();

            repository.GetProductAsyncDelegate = (productNumber) =>
                {
                    Product product = null;

                    if (productNumber == "1")
                    {
                        product = new Product { ProductNumber = productNumber, SubcategoryId = 1 };
                    }

                    return Task.FromResult(product);
                };

            repository.GetProductsAsyncDelegate = (subCategoryId) =>
                {
                    ReadOnlyCollection<Product> products = null;

                    if (subCategoryId == 1)
                    {
                        products = new ReadOnlyCollection<Product>(new List<Product>
                        {
                            new Product(){ ProductNumber = "1", ImageUri = new Uri("http://image") },
                            new Product(){ ProductNumber = "2", ImageUri = new Uri("http://image") },
                            new Product(){ ProductNumber = "3", ImageUri = new Uri("http://image") }
                        });
                    }

                    return Task.FromResult(products);
                };

            var target = new ItemDetailPageViewModel(repository, navigationService, new MockShoppingCartRepository(), null, null, null, new MockSearchPaneService());
            target.OnNavigatedTo("1", NavigationMode.New, null);

            Assert.IsNotNull(target.Items);
            Assert.AreEqual(3, ((IReadOnlyCollection<ProductViewModel>)target.Items).Count);
            Assert.AreEqual(target.Items.First(), target.SelectedProduct);
        }

        [TestMethod]
        public void OnNavigatedTo_When_Service_Not_Available_Then_Pops_Alert()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();
            var alertService = new MockAlertMessageService();
            var resourceLoader = new MockResourceLoader();

            bool alertCalled = false;
            repository.GetProductAsyncDelegate = (productNumber) =>
                {
                    throw new HttpRequestException();
                };

            repository.GetProductsAsyncDelegate = (subCategoryId) =>
                {
                    throw new HttpRequestException();
                };

            alertService.ShowAsyncDelegate = (msg, title) =>
                {
                    alertCalled = true;
                    return Task.FromResult(string.Empty);
                };

            var target = new ItemDetailPageViewModel(repository, navigationService, new MockShoppingCartRepository(), alertService, resourceLoader, null, new MockSearchPaneService());
            target.OnNavigatedTo("1", NavigationMode.New, null);

            Assert.IsTrue(alertCalled);
        }

        [TestMethod]
        public async Task GoBack_When_CanGoBack_Is_Not_True()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService
                {
                    CanGoBackDelegate = () => false,
                    GoBackDelegate = Assert.Fail
                };

            var target = new ItemDetailPageViewModel(repository, navigationService, new MockShoppingCartRepository(), null, null, null, null);
            bool canExecute = target.GoBackCommand.CanExecute();

            if (canExecute) await target.GoBackCommand.Execute();
        }

        [TestMethod]
        public async Task GoBack_When_CanGoBack_Is_True()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService
                {
                    CanGoBackDelegate = () => true,
                    GoBackDelegate = () => Assert.IsTrue(true, "I can go back")
                };

            var target = new ItemDetailPageViewModel(repository, navigationService, new MockShoppingCartRepository(), null, null, null, null);
            bool canExecute = target.GoBackCommand.CanExecute();

            if (canExecute)
            {
                await target.GoBackCommand.Execute();
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public async Task PinToStart_FiresOnly_IfProductIsSelected_And_SecondaryTileDoesNotExist()
        {
            bool fired = false;
            var tileService = new MockTileService();
            var target = new ItemDetailPageViewModel(null, new MockNavigationService(), null, null, null, tileService, null);

            // Case 1: Item not selected --> should not be fired
            tileService.SecondaryTileExistsDelegate = (a) => false;
            tileService.PinSquareSecondaryTileDelegate = (a, b, c, d) =>
                {
                    fired = true;
                    return Task.FromResult(true);
                };
            tileService.PinWideSecondaryTileDelegate = (a, b, c, d) =>
                {
                    fired = true;
                    return Task.FromResult(true);
                };

            await target.PinProductCommand.Execute();
            Assert.IsFalse(fired);

            // Case 2: Item selected but SecondaryTile exists --> should not be fired
            tileService.SecondaryTileExistsDelegate = (a) => true;
            target.SelectedProduct = new ProductViewModel(new Product() { ImageUri = new Uri("http://dummy-image-uri.com") });
            
            await target.PinProductCommand.Execute();
            Assert.IsFalse(fired);

            // Case 3: Item selected and SecondaryTile does not exist --> should be fired
            tileService.SecondaryTileExistsDelegate = (a) => false;

            await target.PinProductCommand.Execute();
            Assert.IsTrue(fired);
        }

        [TestMethod]
        public async Task PinToStart_Changes_IsAppBarSticky()
        {
            var tileService = new MockTileService() { SecondaryTileExistsDelegate = (a) => false };
            var target = new ItemDetailPageViewModel(null, new MockNavigationService(), null, null, null, tileService, null);
            target.SelectedProduct = new ProductViewModel(new Product() { ImageUri = new Uri("http://dummy-image-uri.com") });

            // The AppBar should be sticky when the item is being pinned
            tileService.PinSquareSecondaryTileDelegate = (a, b, c, d) =>
                {
                    Assert.IsTrue(target.IsAppBarSticky);
                    return Task.FromResult(true);
                };
            tileService.PinWideSecondaryTileDelegate = (a, b, c, d) =>
                {
                    Assert.IsTrue(target.IsAppBarSticky);
                    return Task.FromResult(true);
                };

            // Check if the AppBar is Sticky before pinning
            Assert.IsFalse(target.IsAppBarSticky);

            await target.PinProductCommand.Execute();

            // Check if the AppBar is Sticky after pinning
            Assert.IsFalse(target.IsAppBarSticky);
        }

        [TestMethod]
        public async Task UnpinFromStart_FiresOnly_IfProductIsSelected_And_SecondaryTileDoesNotExist()
        {
            bool fired = false;
            var tileService = new MockTileService();
            var target = new ItemDetailPageViewModel(null, new MockNavigationService(), null, null, null, tileService, null);

            // Case 1: Item not selected --> should not be fired
            tileService.SecondaryTileExistsDelegate = (a) => true;
            tileService.UnpinTileDelegate = (a) =>
            {
                fired = true;
                return Task.FromResult(true);
            };

            await target.UnpinProductCommand.Execute();
            Assert.IsFalse(fired);

            // Case 2: Item selected but SecondaryTile does not exist --> should not be fired
            tileService.SecondaryTileExistsDelegate = (a) => false;
            target.SelectedProduct = new ProductViewModel(new Product() { ImageUri = new Uri("http://dummy-image-uri.com") });

            await target.UnpinProductCommand.Execute();
            Assert.IsFalse(fired);

            // Case 3: Item selected and SecondaryTile exists --> should be fired
            tileService.SecondaryTileExistsDelegate = (a) => true;

            await target.UnpinProductCommand.Execute();
            Assert.IsTrue(fired);
        }

        [TestMethod]
        public async Task UnpinFromStart_Changes_IsAppBarSticky()
        {
            var tileService = new MockTileService() { SecondaryTileExistsDelegate = (a) => false };
            var target = new ItemDetailPageViewModel(null, new MockNavigationService(), null, null, null, tileService, null);
            target.SelectedProduct = new ProductViewModel(new Product() { ImageUri = new Uri("http://dummy-image-uri.com") });

            // The AppBar should be sticky when the item is being unpinned
            tileService.UnpinTileDelegate = (a) =>
                {
                    Assert.IsTrue(target.IsAppBarSticky);
                    return Task.FromResult(true);
                };

            // Check if the AppBar is Sticky before unpinning
            Assert.IsFalse(target.IsAppBarSticky);

            await target.UnpinProductCommand.Execute();

            // Check if the AppBar is Sticky after unpinning
            Assert.IsFalse(target.IsAppBarSticky);
        }
    }
}
