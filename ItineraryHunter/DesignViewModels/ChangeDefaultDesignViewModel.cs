// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Kona.Infrastructure;
using Kona.Infrastructure.Flyouts;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Kona.UILogic.ViewModels;
using Windows.ApplicationModel.Resources;

namespace Kona.AWShopper.DesignViewModels
{
    public class ChangeDefaultsDesignViewModel
    {
        public ChangeDefaultsDesignViewModel()
        {
            FillWithDummyData();
        }

        public IEnumerable<CheckoutDataViewModel> ShippingAddresses { get; private set; }

        public IEnumerable<CheckoutDataViewModel> BillingAddresses { get; private set; }

        public IEnumerable<CheckoutDataViewModel> PaymentMethods { get; private set; }

        public CheckoutDataViewModel SelectedShippingAddress { get; set; }

        public CheckoutDataViewModel SelectedBillingAddress{ get; set; }

        public CheckoutDataViewModel SelectedPaymentMethod{ get; set; }

        private void FillWithDummyData()
        {
            var resourceLoader = new ResourceLoader();

            ShippingAddresses = new List<CheckoutDataViewModel>()
                {
                    new CheckoutDataViewModel("1", resourceLoader.GetString("ShippingAddress"), "12345 Main St NE",
                                              "Seattle, WA 54321", "Name Lastname",
                                              new Uri("ms-appx:///Assets/shippingAddressLogo.png", UriKind.Absolute),
                                              null, null),
                    new CheckoutDataViewModel("2", resourceLoader.GetString("ShippingAddress"), "6789 Main St SE",
                                              "Seattle, WA 12345", "Name Lastname",
                                              new Uri("ms-appx:///Assets/shippingAddressLogo.png", UriKind.Absolute),
                                              null, null),
                    new CheckoutDataViewModel("3", resourceLoader.GetString("ShippingAddress"), "101112 Main St NW",
                                              "Seattle, WA 3578", "Name Lastname",
                                              new Uri("ms-appx:///Assets/shippingAddressLogo.png", UriKind.Absolute),
                                              null, null)
                };

            BillingAddresses = new List<CheckoutDataViewModel>()
                {
                    new CheckoutDataViewModel("1", resourceLoader.GetString("BillingAddress"), "12345 Main St NE",
                                              "Seattle, WA 54321", "Name Lastname",
                                              new Uri("ms-appx:///Assets/shippingAddressLogo.png", UriKind.Absolute),
                                              null, null),
                    new CheckoutDataViewModel("2", resourceLoader.GetString("BillingAddress"), "6789 Main St SE",
                                              "Seattle, WA 12345", "Name Lastname",
                                              new Uri("ms-appx:///Assets/shippingAddressLogo.png", UriKind.Absolute),
                                              null, null),
                    new CheckoutDataViewModel("3", resourceLoader.GetString("BillingAddress"), "101112 Main St NW",
                                              "Seattle, WA 3578", "Name Lastname",
                                              new Uri("ms-appx:///Assets/shippingAddressLogo.png", UriKind.Absolute),
                                              null, null)
                };

            PaymentMethods = new List<CheckoutDataViewModel>()
                {
                    new CheckoutDataViewModel("3", resourceLoader.GetString("PaymentMethod"), "Card ending in 1234",
                                              "Card expiring in 10/2014", "Name Lastname",
                                              new Uri("ms-appx:///Assets/paymentMethodLogo.png", UriKind.Absolute), null,
                                              null),
                    new CheckoutDataViewModel("3", resourceLoader.GetString("PaymentMethod"), "Card ending in 1234",
                                              "Card expiring in 11/2015", "Name Lastname",
                                              new Uri("ms-appx:///Assets/paymentMethodLogo.png", UriKind.Absolute), null,
                                              null),
                    new CheckoutDataViewModel("3", resourceLoader.GetString("PaymentMethod"), "Card ending in 1234",
                                              "Card expiring in 12/2016", "Name Lastname",
                                              new Uri("ms-appx:///Assets/paymentMethodLogo.png", UriKind.Absolute), null,
                                              null),
                };

            SelectedShippingAddress = ShippingAddresses.First();
            SelectedBillingAddress = BillingAddresses.First();
            SelectedPaymentMethod = PaymentMethods.First();
        }
    }
}