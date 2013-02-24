// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.Services;
using Kona.UILogic.Tests.Mocks;
using Kona.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI.Xaml.Navigation;
using System.Collections.Generic;

namespace Kona.UILogic.Tests.ViewModels
{
    [TestClass]
    public class HubPageModelFixture
    {
        // <snippet1201>
        [TestMethod]
        public void OnNavigatedTo_Fill_RootCategories()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();
            var searchPaneService = new MockSearchPaneService();

            repository.GetCategoriesAsyncDelegate = (maxAmmountOfProducts) =>
            {
                var categories = new ReadOnlyCollection<Category>(new List<Category>{
                    new Category(),
                    new Category(),
                    new Category()
                });

                return Task.FromResult(categories);
            };

            var viewModel = new HubPageViewModel(repository, navigationService, null, null, searchPaneService);
            viewModel.OnNavigatedTo(null, NavigationMode.New, null);

            Assert.IsNotNull(viewModel.RootCategories);
            Assert.AreEqual(((ICollection<CategoryViewModel>)viewModel.RootCategories).Count, 3);
        }
        // </snippet1201>

        [TestMethod]
        public void ProductNav_With_Valid_Parameter()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();
            var productToNavigate =
              new ProductViewModel(new Product()
              {
                  ListPrice = 100,
                  ProductNumber = "p1",
                  ImageUri = new Uri("http://image"),
                  Currency = "USD",
                  Title = "My Title",
                  Description = "My Description",
              });
            navigationService.NavigateDelegate = (pageName, categoryId) =>
            {
                Assert.AreEqual("ItemDetail", pageName);
                Assert.AreEqual(productToNavigate.ProductNumber, categoryId);
                return true;
            };

            var viewModel = new HubPageViewModel(repository, navigationService, null, null, null);
            viewModel.ProductNavigationAction.Invoke(productToNavigate);
        }

        [TestMethod]
        public void ProductNav_With_Null_Parameter_Does_Not_Navigate()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();

            navigationService.NavigateDelegate = (pageName, categoryId) =>
            {
                Assert.Fail();
                return false;
            };
                
            var viewModel = new HubPageViewModel(repository, navigationService, null, null, null);
            viewModel.ProductNavigationAction.Invoke(null);
        }

        [TestMethod]
        public void FailedCallToProductCatalogRepository_ShowsAlert()
        {
            var alertCalled = false;
            var productCatalogRepository = new MockProductCatalogRepository();
            var navService = new MockNavigationService();
            var searchPaneService = new MockSearchPaneService();
            productCatalogRepository.GetCategoriesAsyncDelegate = (maxAmmountOfProducts) =>
            {
                throw new HttpRequestException();
            };
            var alertMessageService = new MockAlertMessageService();
            alertMessageService.ShowAsyncDelegate = (s, s1) =>
            {
                alertCalled = true;
                Assert.AreEqual("Error", s1);
                return Task.FromResult(string.Empty);
            };
            var target = new HubPageViewModel(productCatalogRepository, navService,
                                                                 alertMessageService, new MockResourceLoader(), searchPaneService);
            target.OnNavigatedTo(null, NavigationMode.New, null);
            
            Assert.IsTrue(alertCalled);
        }
    }
}
