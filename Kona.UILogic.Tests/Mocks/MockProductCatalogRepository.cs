// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;

namespace Kona.UILogic.Tests.Mocks
{
    public class MockProductCatalogRepository : IProductCatalogRepository
    {
        public Func<int, Task<ReadOnlyCollection<Category>>> GetCategoriesAsyncDelegate { get; set; }
        public Func<string, Task<ReadOnlyCollection<Category>>> GetFilteredProductsAsyncDelegate { get; set; }
        public Func<int, Task<ReadOnlyCollection<Category>>> GetSubcategoriesAsyncDelegate { get; set; }
        public Func<int, Task<ReadOnlyCollection<Product>>> GetProductsAsyncDelegate { get; set; }
        public Func<int, Task<Category>> GetCategoryAsyncDelegate { get; set; }
        public Func<string, Task<Product>> GetProductAsyncDelegate { get; set; }

        public Task<ReadOnlyCollection<Category>> GetCategoriesAsync(int maxAmountOfProducts)
        {
            return this.GetCategoriesAsyncDelegate(maxAmountOfProducts);
        }

        public Task<ReadOnlyCollection<Category>> GetFilteredProductsAsync(string productsQueryString)
        {
            return this.GetFilteredProductsAsyncDelegate(productsQueryString);
        }

        public Task<ReadOnlyCollection<Category>> GetSubcategoriesAsync(int categoryId)
        {
            return this.GetSubcategoriesAsyncDelegate(categoryId);
        }

        public Task<ReadOnlyCollection<Product>> GetProductsAsync(int categoryId)
        {
            return this.GetProductsAsyncDelegate(categoryId);
        }

        public Task<Category> GetCategoryAsync(int categoryId)
        {
            return this.GetCategoryAsyncDelegate(categoryId);
        }

        public Task<Product> GetProductAsync(string productNumber)
        {
            return this.GetProductAsyncDelegate(productNumber);
        }

    }
}
