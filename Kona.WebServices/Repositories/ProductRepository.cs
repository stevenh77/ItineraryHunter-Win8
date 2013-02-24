// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using Kona.WebServices.Models;
using System.Linq;

namespace Kona.WebServices.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private static string ImageServerPath = System.Configuration.ConfigurationManager.AppSettings["ImageServerPath"];

        private static ICollection<Product> _products = GetProducts();

        private static IEnumerable<Product> _todaysDealsProducts = GetTodaysDeals();

        public IEnumerable<Product> GetAll()
        {
            lock (_products)
            {
                // Return new collection so callers can iterate independently on separate threads
                return _products.ToArray();
            }
        }

        public IEnumerable<Product> GetProductsFromCategory(int subcategoryId)
        {
            lock (_products)
            {
                return _products.Where(p => p.SubcategoryId == subcategoryId);
            }
        }

        public IEnumerable<Product> GetTodaysDealsProducts()
        {
            lock (_todaysDealsProducts)
            {
                return _todaysDealsProducts.ToArray();
            }
        }

        private static ICollection<Product> GetProducts()
        {
            var products = new List<Product>();
            products.Add(new Product { Title = "Tour du Mont Blanc", ProductNumber = "TMB", SubcategoryId = 1, Description = "The Tour du Mont Blanc or TMB is one of the most popular long distance walks in Europe. It circles the Mont Blanc Massif covering a distance of roughly 170 km with 10 km of ascent/descent and passes through parts of Switzerland, Italy and France.", ListPrice = 1500.00, ImageUri = new Uri(ImageServerPath + "adventure-hiking-tourdumontblanc-large.png"), DurationInDays = 10 });
            products.Add(new Product { Title = "Hiking in Argentina", ProductNumber = "TDP", SubcategoryId = 1, Description = "Torres del Paine National Park is a national park encompassing mountains, a glacier, a lake, and river-rich areas in southern Chilean Patagonia.", DurationInDays = 14, ImageUri = new Uri(ImageServerPath + "adventure-hiking-torresdelpaine-large.png") });
            products.Add(new Product { Title = "Sipadan, Borneo", ProductNumber = "DIB", SubcategoryId = 2, Description = "Walk straight off the beach into a world of underwater magic.", ImageUri = new Uri(ImageServerPath + "adventure-scuba-1-large.png"), DurationInDays = 7 });
            products.Add(new Product { Title = "Great Barrier Reef", ProductNumber = "BRF", SubcategoryId = 2, Description = "Explore a sunken WW2 ship with more fish than your imagination could believe.", ImageUri = new Uri(ImageServerPath + "adventure-scuba-2-large.png"), DurationInDays = 7 });
            products.Add(new Product { Title = "Red Sea Diving", ProductNumber = "RSD", SubcategoryId = 2, Description = ".", ImageUri = new Uri(ImageServerPath + "adventure-scuba-3-large.png"), DurationInDays = 14 });
            products.Add(new Product { Title = "Most Dangerous Road, Peru", ProductNumber = "PERU", SubcategoryId = 3, Description = "Cycle down the worlds most dangerous road in Peru.", ImageUri = new Uri(ImageServerPath + "adventure-bikes-1-large.png"), DurationInDays = 14 });
            products.Add(new Product { Title = "Matterhorn", ProductNumber = "MATT", SubcategoryId = 4, Description = "Ski or snowboard from what seems like the another planet.", ImageUri = new Uri(ImageServerPath + "adventure-snow-1-large.png"), DurationInDays = 14});
            products.Add(new Product { Title = "Thailand", ProductNumber = "BK-R19B-58", SubcategoryId = 5, Description = "This description will be entered shortly....", ImageUri = new Uri(ImageServerPath + "beach-exotic-1-large.png"), DurationInDays = 14 });
            products.Add(new Product { Title = "Egypt", ProductNumber = "BK-R19B-44", SubcategoryId = 6, Description = "This description will be entered shortly....", ImageUri = new Uri(ImageServerPath + "beach-bestvalue-1-large.png"), DurationInDays = 14 });
            products.Add(new Product { Title = "Caribbean Islands", ProductNumber = "BK-R19B-48", SubcategoryId = 7, Description = "This description will be entered shortly....", ImageUri = new Uri(ImageServerPath + "beach-caribbean-1-large.png"), DurationInDays = 14 });
            products.Add(new Product { Title = "Saint Lucia", ProductNumber = "STLUCIA", SubcategoryId = 8, Description = "This description will be entered shortly....", ImageUri = new Uri(ImageServerPath + "beach-allinclusive-1-large.png"), DurationInDays = 14 });
            products.Add(new Product { Title = "New Year in Tallinn", ProductNumber = "BK-T18U-54", SubcategoryId = 9, Description = "This description will be entered shortly....", ImageUri = new Uri(ImageServerPath + "citybreak-europe-1-large.png"), DurationInDays = 14 });
            products.Add(new Product { Title = "Benicàssim", ProductNumber = "BK-T18U-58", SubcategoryId = 10, Description = "This description will be entered shortly....", ImageUri = new Uri(ImageServerPath + "festival-music-1-large.png"), DurationInDays = 14 });
            products.Add(new Product { Title = "Full Moon Party", ProductNumber = "BK-T18U-62", SubcategoryId = 11, Description = "This description will be entered shortly....", ImageUri = new Uri(ImageServerPath + "festival-parties-1-large.png"), DurationInDays = 14 });
            products.Add(new Product { Title = "Holi", ProductNumber = "HOLI", SubcategoryId = 12, Description = "This description will be entered shortly....", ImageUri = new Uri(ImageServerPath + "festival-cultural-1-large.png"), DurationInDays = 14 });
            products.Add(new Product { Title = "Kruger National Park", ProductNumber = "BB-8107", SubcategoryId = 13, Description = "This description will be entered shortly....", ImageUri = new Uri(ImageServerPath + "safari-biggame-1-large.png"), DurationInDays = 14 });
            products.Add(new Product { Title = "Amazon Rain Forest", ProductNumber = "AMAZ", SubcategoryId = 14, Description = "This description will be entered shortly....", ImageUri = new Uri(ImageServerPath + "safari-jungle-1-large.png"), DurationInDays = 14 });
            return products;
        }

        private static IEnumerable<Product> GetTodaysDeals()
        {
            var promotedProducts = new List<Product>();
            promotedProducts.Add(_products.First(p => p.ProductNumber == "AMAZ"));
            promotedProducts.Add(_products.First(p => p.ProductNumber == "HOLI"));
            promotedProducts.Add(_products.First(p => p.ProductNumber == "STLUCIA"));
            return promotedProducts;
        }
    }
}