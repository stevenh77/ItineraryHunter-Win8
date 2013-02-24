// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.UILogic.Models;
using Kona.UILogic.ViewModels;
using System;
using System.Collections.Generic;

namespace Kona.AWShopper.DesignViewModels
{
    public class SearchResultsPageDesignViewModel
    {
        public SearchResultsPageDesignViewModel()
        {
            this.QueryText = '\u201c' + "bike" + '\u201d';
            this.SearchTerm = "bike";
            this.NoResults = false;
            FillWithDummyData();
        }

        public string QueryText { get; set; }

        public string SearchTerm { get; set; }

        public bool NoResults { get; set; }

        public List<CategoryViewModel> Results { get; set; }

        public void FillWithDummyData()
        {
            Results = new List<CategoryViewModel>()
                {
                    new CategoryViewModel(new Category()
                    { 
                        Title = "Category 1", 
                        Products = new List<Product>()
                            {
                                new Product() {  Title = "Bike 1",  Description = "Description of Product 1", ListPrice = 25.10, ProductNumber = "1", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png"), Currency = "USD"},
                                new Product() {  Title = "Bike 2",  Description = "Description of Product 2", ListPrice = 25.10, ProductNumber = "2", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png"), Currency = "USD" },
                                new Product() {  Title = "Bike 3",  Description = "Description of Product 3", ListPrice = 25.10, ProductNumber = "3", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png"), Currency = "USD" },
                            }
                    }, null),
                    new CategoryViewModel(new Category()
                    { 
                        Title = "Category 2", 
                        Products = new List<Product>()
                            {
                                new Product() {  Title = "Bike Lock",  Description = "Description of Product 1", ListPrice = 25.10, ProductNumber = "1", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png"), Currency = "USD" },
                                new Product() {  Title = "Red Mountain Bike with light blue inclusions in the frame.",  Description = "Description of Product 2", ListPrice = 25.10, ProductNumber = "2", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png"), Currency = "USD" },
                            }
                    }, null),
                    new CategoryViewModel(new Category()
                    { 
                        Title = "Category 3", 
                        Products = new List<Product>()
                            {
                                new Product() {  Title = "Blue Bike Cover",  Description = "Description of Product 1", ListPrice = 25.10, ProductNumber = "1", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png"), Currency = "USD" },
                            }
                    }, null)
                };
        }

    }
}
