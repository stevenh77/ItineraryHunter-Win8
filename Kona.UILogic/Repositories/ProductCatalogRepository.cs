// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Kona.UILogic.Services;

namespace Kona.UILogic.Repositories
{
    public class ProductCatalogRepository : IProductCatalogRepository
    {
        private readonly IProductCatalogService _productCatalogService;
        private readonly ICacheService _cacheService;
        private bool useCacheIfAvailable = false;

        public ProductCatalogRepository(IProductCatalogService productCatalogService, ICacheService cacheService)
        {
            _productCatalogService = productCatalogService;
            _cacheService = cacheService;
        }

        // <snippet512>
        public async Task<ReadOnlyCollection<Category>> GetCategoriesAsync(int maxAmountOfProducts)
        {
            string cacheFileName = String.Format("{0}{1}", "Categories", maxAmountOfProducts);

            if (useCacheIfAvailable && await _cacheService.DataExistsAndIsValidAsync(cacheFileName))
            {
                // Retrieve the items from the cache
                return await _cacheService.GetDataAsync<ReadOnlyCollection<Category>>(cacheFileName);
            }
            else
            {
                // Retrieve the items from the service
                var items = await _productCatalogService.GetCategoriesAsync(maxAmountOfProducts);

                var cacheTask = SaveCategoriesToCache(items, cacheFileName)
                    .ContinueWith(t => { var exeptionToBeLogged = t.Exception; }, TaskContinuationOptions.OnlyOnFaulted);

                return items;
            }
        }
        // </snippet512>

        public async Task<ReadOnlyCollection<Category>> GetFilteredProductsAsync(string queryString)
        {
            // Retrieve the items from the service
            var items = await _productCatalogService.GetFilteredProductsAsync(queryString);
            return items;
        }

        public async Task<ReadOnlyCollection<Category>> GetSubcategoriesAsync(int categoryId)
        {
            string cacheFileName = string.Format("SubCategoriesOfCategoryId{0}", categoryId);

            if (useCacheIfAvailable && await _cacheService.DataExistsAndIsValidAsync(cacheFileName))
            {
                // Retrieve the items from the cache
                return await _cacheService.GetDataAsync<ReadOnlyCollection<Category>>(cacheFileName);
            }
            else
            {
                // Retrieve the items from the service
                var categories = await _productCatalogService.GetSubcategoriesAsync(categoryId);

                var cacheTask = SaveCategoriesToCache(categories, cacheFileName)
                    .ContinueWith(t => { var exeptionToBeLogged = t.Exception; }, TaskContinuationOptions.OnlyOnFaulted);

                return categories;
            }
        }



        public async Task<ReadOnlyCollection<Product>> GetProductsAsync(int categoryId)
        {
            string cacheFileName = string.Format("SubProductsOfCategoryId{0}", categoryId);

            if (useCacheIfAvailable && await _cacheService.DataExistsAndIsValidAsync(cacheFileName))
            {
                // Retrieve the items from the cache
                return await _cacheService.GetDataAsync<ReadOnlyCollection<Product>>(cacheFileName);
            }
            else
            {
                // Retrieve the items from the service
                var products  = await _productCatalogService.GetProductsAsync(categoryId);

                var cacheTask = SaveProductsToCache(products, cacheFileName)
                    .ContinueWith(t => { var exeptionToBeLogged = t.Exception; }, TaskContinuationOptions.OnlyOnFaulted);

                return products;
            }
        }

        public async Task<Category> GetCategoryAsync(int categoryId)
        {
            string cacheFileName = string.Format("CategoryId{0}", categoryId);

            if (useCacheIfAvailable && await _cacheService.DataExistsAndIsValidAsync(cacheFileName))
            {
                // Retrieve the items from the cache
                return await _cacheService.GetDataAsync<Category>(cacheFileName);
            }
            
            // Retrieve the items from the service
            var category = await _productCatalogService.GetCategoryAsync(categoryId);

            // Save the items in the cache
            await _cacheService.SaveDataAsync(cacheFileName, category);

            return category;
        }

        public async Task<Product> GetProductAsync(string productNumber)
        {
            string cacheFileName = string.Format("Product{0}", productNumber);

            if (useCacheIfAvailable && await _cacheService.DataExistsAndIsValidAsync(cacheFileName))
            {
                // Retrieve the items from the cache
                return await _cacheService.GetDataAsync<Product>(cacheFileName);
            }
            
            // Retrieve the items from the service
            var product = await _productCatalogService.GetProductAsync(productNumber);

            if (product.ImageUri != null)
            {
                product.ImageUri = await _cacheService.SaveExternalDataAsync(product.ImageUri);
            }

            // Save the items in the cache
            await _cacheService.SaveDataAsync(cacheFileName, product);

            return product;
        }

        private async Task SaveCategoriesToCache(ReadOnlyCollection<Category> categories, string cacheFileName)
        {
            // Save the images locally
            // Update the item's local URI
            foreach (var category in categories)
            {
                if (category.ImageUri != null)
                {
                    category.ImageUri = await _cacheService.SaveExternalDataAsync(category.ImageUri);
                }
                if (category.Products != null)
                {
                    foreach (var product in category.Products)
                    {
                        if (product.ImageUri != null)
                        {
                            product.ImageUri =
                                await
                                _cacheService.SaveExternalDataAsync(product.ImageUri);
                        }
                    }
                }
            }

            if (categories.Count > 0)
            {
                // Save the items in the cache
                await _cacheService.SaveDataAsync(cacheFileName, categories);
            }
        }

        private async Task SaveProductsToCache(ReadOnlyCollection<Product> products, string cacheFileName)
        {
            // Save the images locally
            // Update the item's local URI
            foreach (var product in products)
            {
                if (product.ImageUri != null)
                {
                    product.ImageUri = await _cacheService.SaveExternalDataAsync(product.ImageUri);
                }
            }

            // Save the items in the cache
            await _cacheService.SaveDataAsync(cacheFileName, products);
        }

    }
}
