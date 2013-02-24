// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using Kona.WebServices.Models;

namespace Kona.WebServices.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetTodaysDealsProducts();
        IEnumerable<Product> GetProductsFromCategory(int subcategoryId);
        IEnumerable<Product> GetAll();
    }
}