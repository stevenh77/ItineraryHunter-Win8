// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.Serialization;

namespace Kona.UILogic.ViewModels
{
    [DataContract]
    public class ShoppingCartItemViewModel : ViewModel
    {
        private string _id;
        private string _title;
        private string _description;
        private int _quantity;
        private double _listPrice;
        private double _discountPercentage;
        private Uri _imageUri;
        private CurrencyFormatter _currencyFormatter;

        public ShoppingCartItemViewModel(ShoppingCartItem shoppingCartItem)
        {
            if (shoppingCartItem == null)
            {
                throw new ArgumentNullException("shoppingCartItem", "shoppingCartItem cannot be null");
            }

            _id = shoppingCartItem.Id;
            _title = shoppingCartItem.Product.Title;
            _description = shoppingCartItem.Product.Description;
            _quantity = shoppingCartItem.Quantity;
            _listPrice = shoppingCartItem.Product.ListPrice;
            _imageUri = shoppingCartItem.Product.ImageUri;
            EntityId = shoppingCartItem.Id;
            ProductId = shoppingCartItem.Product.ProductNumber;
            _currencyFormatter = new CurrencyFormatter(shoppingCartItem.Currency);
        }

        public string ProductId { get; private set; }

        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }
        
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (SetProperty(ref _quantity, value))
                {
                    OnPropertyChanged("FullPrice");
                    OnPropertyChanged("TotalPrice");
                    OnPropertyChanged("DiscountedPrice");
                }
            }
        }

        public double FullPriceDouble { get { return Math.Round(Quantity*_listPrice, 2); } }

        public string FullPrice
        {
            get { return _currencyFormatter.FormatDouble(FullPriceDouble); }
        }

        public double DiscountPercentage
        {
            get { return _discountPercentage; }
            set { SetProperty(ref _discountPercentage, value); }
        }

        public ImageSource Image
        {
            get { return new BitmapImage(_imageUri); }
        }

        public double DiscountedPriceDouble { get { return Math.Round(FullPriceDouble*(1 - DiscountPercentage/100), 2); } }

        public string DiscountedPrice 
        {
            get { return _currencyFormatter.FormatDouble(DiscountedPriceDouble); }
        }

        public override string ToString()
        {
            return Title + ProductId;
        }
    }
}
