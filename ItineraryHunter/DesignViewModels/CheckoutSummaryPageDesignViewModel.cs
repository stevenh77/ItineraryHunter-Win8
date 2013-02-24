// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.ViewModels;
using Windows.ApplicationModel.Resources;

namespace Kona.AWShopper.DesignViewModels
{
    public class CheckoutSummaryPageDesignViewModel
    {
        public CheckoutSummaryPageDesignViewModel()
        {
            FillWithDummyData();
        }

        public string OrderSubtotal { get; set; }

        public string ShippingCost { get; set; }

        public string TaxCost { get; set; }

        public string GrandTotal { get; set; }

        public IEnumerable<CheckoutDataViewModel> CheckoutDataViewModels { get; set; }

        public IEnumerable<ShippingMethod> ShippingMethods { get; set; }

        public IEnumerable<ShoppingCartItemViewModel> ShoppingCartItemViewModels { get; set; }

        public IEnumerable<CheckoutDataViewModel> AllCheckoutDataViewModels { get; set; }

        public bool IsSelectCheckoutDataPopupOpened { get; set; }

        public string SelectCheckoutDataLabel { get; set; }

        public bool IsBottomAppBarOpened { get; set; }

        public ShippingMethod SelectedShippingMethod { get; set; }

        public CheckoutDataViewModel SelectedCheckoutData { get; set; }

        public CheckoutDataViewModel SelectedAllCheckoutData { get; set; }

        private void FillWithDummyData()
        {
            OrderSubtotal = "$100.00";
            ShippingCost = "$20.00";
            TaxCost = "$5.00";
            GrandTotal = "$125.00";

            IsBottomAppBarOpened = true;

            IsSelectCheckoutDataPopupOpened = true;
            SelectCheckoutDataLabel = "Select Shipping Address";

            var resourceLoader = new ResourceLoader();
            CheckoutDataViewModels = new List<CheckoutDataViewModel>()
            {
                new CheckoutDataViewModel("1", resourceLoader.GetString("ShippingAddress"), "12345 Main St NE", "Seattle, WA 54321", "Name Lastname", new Uri("ms-appx:///Assets/shippingAddressLogo.png", UriKind.Absolute), null, null),
                new CheckoutDataViewModel("2", resourceLoader.GetString("BillingAddress"), "12345 Main St NE", "Seattle, WA 54321", "Name Lastname", new Uri("ms-appx:///Assets/billingAddressLogo.png", UriKind.Absolute), null, null),
                new CheckoutDataViewModel("3", resourceLoader.GetString("PaymentMethod"), "Card ending in 1234", "Card expiring in 10/2014", "Name Lastname", new Uri("ms-appx:///Assets/paymentMethodLogo.png", UriKind.Absolute), null, null),
            };
            SelectedCheckoutData = CheckoutDataViewModels.First();

            ShippingMethods = new List<ShippingMethod>()
            {
                new ShippingMethod() { Id = 1, Description = "Shipping Method 1", Cost = 1.50, EstimatedTime = "1 year"},
                new ShippingMethod() { Id = 2, Description = "Shipping Method 2", Cost = 15.10, EstimatedTime = "1 month"},
                new ShippingMethod() { Id = 3, Description = "Shipping Method 3", Cost = 151.0, EstimatedTime = "1 day"},
            };
            SelectedShippingMethod = ShippingMethods.First();

            ShoppingCartItemViewModels = new List<ShoppingCartItemViewModel>()
                {
                    new ShoppingCartItemViewModel(new ShoppingCartItem()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Product = new Product() {  Title = "Product 1",  Description = "Description of Product 1", ListPrice = 25.10, ProductNumber = "1", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png")},
                            Quantity = 1,
                            Currency = "USD"
                        }),
                   new ShoppingCartItemViewModel(new ShoppingCartItem()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Product = new Product() {  Title = "Product 2",  Description = "Description of Product 2", ListPrice = 25.10, ProductNumber = "2", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png")},
                            Quantity = 1,
                            Currency = "USD"
                        }), 
                   new ShoppingCartItemViewModel(new ShoppingCartItem()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Product = new Product() {  Title = "Product 3",  Description = "Description of Product 3", ListPrice = 25.10, ProductNumber = "3", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png")},
                            Quantity = 1,
                            Currency = "USD"
                        }), 
                   new ShoppingCartItemViewModel(new ShoppingCartItem()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Product = new Product() {  Title = "Product 4",  Description = "Description of Product 4", ListPrice = 25.10, ProductNumber = "4", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png")},
                            Quantity = 1,
                            Currency = "USD"
                        }), 
                   new ShoppingCartItemViewModel(new ShoppingCartItem()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Product = new Product() {  Title = "Product 5",  Description = "Description of Product 5", ListPrice = 25.10, ProductNumber = "5", ImageUri = new Uri("ms-appx:///Assets/StoreLogo.png")},
                            Quantity = 1,
                            Currency = "USD"
                        }), 
                };
            
            AllCheckoutDataViewModels = new List<CheckoutDataViewModel>()
            {
                new CheckoutDataViewModel("1", resourceLoader.GetString("ShippingAddress"), "12345 Main St NE", "Seattle, WA 54321", "Name Lastname", new Uri("ms-appx:///Assets/shippingAddressLogo.png", UriKind.Absolute), null, null),
                new CheckoutDataViewModel("2", resourceLoader.GetString("ShippingAddress"), "12345 Main St NE", "Seattle, WA 54321", "Name Lastname", new Uri("ms-appx:///Assets/shippingAddressLogo.png", UriKind.Absolute), null, null),
                new CheckoutDataViewModel("3", resourceLoader.GetString("ShippingAddress"), "12345 Main St NE", "Seattle, WA 54321", "Name Lastname", new Uri("ms-appx:///Assets/shippingAddressLogo.png", UriKind.Absolute), null, null),
                new CheckoutDataViewModel("4", resourceLoader.GetString("ShippingAddress"), "12345 Main St NE", "Seattle, WA 54321", "Name Lastname", new Uri("ms-appx:///Assets/shippingAddressLogo.png", UriKind.Absolute), null, null),
            };
            SelectedAllCheckoutData = AllCheckoutDataViewModels.First();
        }
    }
}
