// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using Kona.UILogic.Models;
using Kona.UILogic.ViewModels;

namespace Kona.AWShopper.DesignViewModels
{
    public class GroupDetailPageDesignViewModel
    {
        public GroupDetailPageDesignViewModel()
        {
            FillWithDummyData();
        }

        public string Title { get; set; }

        public IEnumerable<object> Items { get; set; }

        private void FillWithDummyData()
        {
            Title = "Daily Deals";
            Items = new List<CategoryViewModel>()
                {
                    new CategoryViewModel(new Category()
                    { 
                        Title = "Category 1", 
                        Products = new List<Product>()
                            {
                                new Product() {  Title = "Product 1",  Description = "Description of Product 1", ListPrice = 25.10, ProductNumber = "1", ImageUri = new Uri("ms-appx:///Assets/WideLogo.scale-100.png")},
                                new Product() {  Title = "Product 2",  Description = "Description of Product 2", ListPrice = 25.10, ProductNumber = "2", ImageUri = new Uri("ms-appx:///Assets/WideLogo.scale-100.png")},
                                new Product() {  Title = "Product 3",  Description = "Description of Product 3", ListPrice = 25.10, ProductNumber = "3", ImageUri = new Uri("ms-appx:///Assets/WideLogo.scale-100.png")},
                            }
                    }, null),
                    new CategoryViewModel(new Category()
                    { 
                        Title = "Category 2", 
                        Products = new List<Product>()
                            {
                                new Product() {  Title = "Product 1",  Description = "Description of Product 1", ListPrice = 25.10, ProductNumber = "1", ImageUri = new Uri("ms-appx:///Assets/WideLogo.scale-100.png")},
                                new Product() {  Title = "Product 2",  Description = "Description of Product 2", ListPrice = 25.10, ProductNumber = "2", ImageUri = new Uri("ms-appx:///Assets/WideLogo.scale-100.pngg")},
                            }
                    }, null),
                    new CategoryViewModel(new Category()
                    { 
                        Title = "Category 3", 
                        Products = new List<Product>()
                            {
                                new Product() {  Title = "Product 1",  Description = "Description of Product 1", ListPrice = 25.10, ProductNumber = "1", ImageUri = new Uri("ms-appx:///Assets/WideLogo.scale-100.png")},
                            }
                    }, null)
                };
        }
    }
}
