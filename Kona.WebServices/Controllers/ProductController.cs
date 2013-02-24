// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.WebServices.Models;
using Kona.WebServices.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System;

namespace Kona.WebServices.Controllers
{
    public class ProductController : ApiController
    {
        private IProductRepository _productRepository;

        public ProductController()
            : this(new ProductRepository())
        { }

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET /api/Product
        public IEnumerable<Product> GetProducts()
        {
            return _productRepository.GetAll();
        }

        // GET /api/Product/id
        public Product GetProduct(string id)
        {
            var item = _productRepository.GetAll().FirstOrDefault(c => c.ProductNumber == id);
            
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return item;
        }

        // GET /api/Product?categoryId={categoryId}
        public IEnumerable<Product> GetProducts(int categoryId)
        {
            return _productRepository.GetAll().Where(c => c.SubcategoryId == categoryId);
        }
    }
}
