// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kona.WebServices.Models;

namespace Kona.WebServices.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private static string ImageServerPath = System.Configuration.ConfigurationManager.AppSettings["ImageServerPath"];
        private static IEnumerable<Category> _categories = PopulateCategories();

        public IEnumerable<Category> GetAll()
        {
            lock (_categories)
            {
                // Return new collection so callers can iterate independently on separate threads
                return _categories.ToArray();
            }
        }

        public Category GetItem(int id)
        {
            lock (_categories)
            {
                return _categories.FirstOrDefault(c => c.Id == id);
            }
        }

        public Category Create(Category item)
        {
            throw new NotImplementedException();
        }

        public bool Update(Category item)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<Category> PopulateCategories()
        {
            return new List<Category>
             {
                 // main categories
                 //----------------
                 new Category {Title = "Recommended", Id = 0, ImageUri = new Uri(ImageServerPath + "/Images/water_bottle_cage_small.gif", UriKind.Absolute) },
                 new Category {Title = "Adventure", Id = 1000, ImageUri = new Uri(ImageServerPath + "/Images/water_bottle_cage_small.gif", UriKind.Absolute) },
                 new Category {Title = "Beach", Id = 2000, ImageUri = new Uri(ImageServerPath + "/Images/racer02_yellow_f_small.gif", UriKind.Absolute) },
                 new Category {Title = "City Breaks", Id = 3000, ImageUri = new Uri(ImageServerPath + "/Images/water_bottle_cage_small.gif", UriKind.Absolute) },
                 new Category {Title = "Festivals", Id = 4000, ImageUri = new Uri(ImageServerPath + "/Images/water_bottle_cage_small.gif", UriKind.Absolute) },
                 new Category {Title = "Safari", Id = 5000, ImageUri = new Uri(ImageServerPath + "/Images/water_bottle_cage_small.gif", UriKind.Absolute) },

                 // sub categories
                 //---------------

                 // Adventure
                new Category { Title = "Hiking", Id=1, ParentId=1000, ImageUri = new Uri(ImageServerPath + "/Images/hotrodbike_f_large.gif", UriKind.Absolute) },
                new Category { Title = "Scuba Diving", Id=2, ParentId=1000, ImageUri = new Uri(ImageServerPath + "/Images/roadster_black_large.gif", UriKind.Absolute) },
                new Category { Title = "Bikes", Id=3, ParentId=1000, ImageUri = new Uri(ImageServerPath + "/Images/julianax_r_02_blue_large.gif", UriKind.Absolute) },
                new Category { Title = "Snow", Id=4, ParentId=1000, ImageUri = new Uri(ImageServerPath + "/Images/handlebar_large.gif", UriKind.Absolute) },
                
                // Beach
                new Category { Title = "Exotic", Id=5, ParentId=2000, ImageUri = new Uri(ImageServerPath + "/Images/no_image_available_large.gif", UriKind.Absolute) },
                new Category { Title = "Best Value", Id=6, ParentId=2000, ImageUri = new Uri(ImageServerPath + "/Images/no_image_available_large.gif", UriKind.Absolute) },
                new Category { Title = "Caribbean", Id=7, ParentId=2000, ImageUri = new Uri(ImageServerPath + "/Images/silver_chain_large.gif", UriKind.Absolute) },
                new Category { Title = "All Inclusive", Id=8, ParentId=2000, ImageUri = new Uri(ImageServerPath + "/Images/no_image_available_large.gif", UriKind.Absolute) },
                
                // City Breaks
                new Category { Title = "Europe", Id=9, ParentId=3000, ImageUri = new Uri(ImageServerPath + "/Images/no_image_available_large.gif", UriKind.Absolute) },
                
                // Festivals
                new Category { Title = "Music", Id=10, ParentId=4000, ImageUri = new Uri(ImageServerPath + "/Images/awc_tee_male_yellow_large.gif", UriKind.Absolute) },
                new Category { Title = "Parties", Id=11, ParentId=4000, ImageUri = new Uri(ImageServerPath + "/Images/no_image_available_large.gif", UriKind.Absolute) },
                new Category { Title = "Cultural", Id=12, ParentId=4000, ImageUri = new Uri(ImageServerPath + "/Images/no_image_available_large.gif", UriKind.Absolute) },

                // Safari
                new Category { Title = "Big Game", Id=13, ParentId=5000, ImageUri = new Uri(ImageServerPath + "/Images/no_image_available_large.gif", UriKind.Absolute) },
                new Category { Title = "Jungle", Id=14, ParentId=5000, ImageUri = new Uri(ImageServerPath + "/Images/no_image_available_large.gif", UriKind.Absolute) },
            };
        }


        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}