// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.Tests.Mocks;
using Kona.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI.Xaml.Navigation;

namespace Kona.UILogic.Tests.ViewModels
{
    [TestClass]
    public class SearchResultsPageViewModelFixture
    {
        [TestMethod]
        public void OnNavigatingTo_Search_Results_Page_With_Search_Term()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();
            repository.GetFilteredProductsAsyncDelegate = (queryString) =>
                {
                    ReadOnlyCollection<Category> categories;
                    if (queryString == "bike")
                        categories = new ReadOnlyCollection<Category>(new List<Category>
                        {
                            new Category() {Products = new List<Product>() {new Product(){Title = "bike1", ProductNumber = "1", ImageUri = new Uri("http://image")}}},
                            new Category() {Products = new List<Product>() {new Product(){Title = "bike2", ProductNumber = "2", ImageUri = new Uri("http://image")}}},
                        });
                    else
                    {
                        categories = new ReadOnlyCollection<Category>(new List<Category>
                        {
                            new Category() {Products = new List<Product>() {new Product(){Title = "bike1", ProductNumber = "1", ImageUri = new Uri("http://image")}}},
                            new Category() {Products = new List<Product>() {new Product(){Title = "bike2", ProductNumber = "2", ImageUri = new Uri("http://image")}}},
                            new Category() {Products = new List<Product>() {new Product(){Title = "product3", ProductNumber = "3", ImageUri = new Uri("http://image")}}}
                        });
                    }

                    return Task.FromResult(categories);
                };

            var target = new SearchResultsPageViewModel(repository, navigationService, new MockSearchPaneService());
            const string searchTerm = "bike";
            target.OnNavigatedTo(searchTerm, NavigationMode.New, null);
            Assert.AreEqual("bike", target.SearchTerm);
            Assert.IsNotNull(target.Results);
            Assert.AreEqual(2, target.Results.Count);
            var resultsThatDontMatch = target.Results.Any(c => c.Products.Any(p => !p.Title.Contains(searchTerm)));
            Assert.IsFalse(resultsThatDontMatch);
        }

        [TestMethod]
        public void OnNavigatingTo_Search_Results_Page_Without_Search_Term()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();
            repository.GetFilteredProductsAsyncDelegate = (queryString) =>
            {
                ReadOnlyCollection<Category> categories;
                if (queryString == "bike")
                    categories = new ReadOnlyCollection<Category>(new List<Category>
                        {
                            new Category() {Products = new List<Product>() {new Product(){Title = "Bike1", ProductNumber = "1", ImageUri = new Uri("http://image")}}},
                            new Category() {Products = new List<Product>() {new Product(){Title = "Bike2", ProductNumber = "2", ImageUri = new Uri("http://image")}}},
                        });
                else
                {
                    categories = new ReadOnlyCollection<Category>(new List<Category>
                        {
                            new Category() {Products = new List<Product>() {new Product(){Title = "Bike1", ProductNumber = "1", ImageUri = new Uri("http://image")}}},
                            new Category() {Products = new List<Product>() {new Product(){Title = "Bike2", ProductNumber = "2", ImageUri = new Uri("http://image")}}},
                            new Category() {Products = new List<Product>() {new Product(){Title = "Product3", ProductNumber = "3", ImageUri = new Uri("http://image")}}}
                        });
                }

                return Task.FromResult(categories);
            };

            var target = new SearchResultsPageViewModel(repository, navigationService, new MockSearchPaneService());
            var searchTerm = string.Empty;
            target.OnNavigatedTo(searchTerm, NavigationMode.New, null);
            Assert.AreEqual(string.Empty, target.SearchTerm);
            Assert.IsNotNull(target.Results);
            Assert.AreEqual(3, target.Results.Count);
        }

        [TestMethod]
        public void ProductNav_With_Valid_Parameter_Does_Navigate()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();
            var productToNavigate = new ProductViewModel(new Product() { ListPrice = 100, ProductNumber = "p1", ImageUri = new Uri("http://image"), Currency = "USD", Title = "My Title", Description = "My Description", });
            navigationService.NavigateDelegate = (pageName, productId) =>
            {
                Assert.AreEqual("ItemDetail", pageName);
                Assert.AreEqual(productToNavigate.ProductNumber, productId);
                return true;
            };

            var viewModel = new SearchResultsPageViewModel(repository, navigationService, null);
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

            var viewModel = new SearchResultsPageViewModel(repository, navigationService, null);
            viewModel.ProductNavigationAction.Invoke(null);
        }
    }
}
