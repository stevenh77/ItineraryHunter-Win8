// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Kona.UILogic.Models;

namespace Kona.UILogic.Repositories
{
    public interface IProductCatalogRepository
    {
        Task<ReadOnlyCollection<Category>> GetCategoriesAsync(int maxAmountOfProducts);

        Task<ReadOnlyCollection<Category>> GetFilteredProductsAsync(string productsQueryString);

        Task<ReadOnlyCollection<Category>> GetSubcategoriesAsync(int categoryId);

        Task<ReadOnlyCollection<Product>> GetProductsAsync(int categoryId);

        Task<Category> GetCategoryAsync(int categoryId);

        Task<Product> GetProductAsync(string productNumber);
    }
}