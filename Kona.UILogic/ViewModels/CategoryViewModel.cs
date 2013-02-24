// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Windows.Input;
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Kona.UILogic.ViewModels
{
    public class CategoryViewModel
    {
        private readonly Category _category;
        private ImageSource _image;
        private List<ProductViewModel> _productsViewModels;
        private INavigationService _navigationService;

        public CategoryViewModel(Category category, INavigationService navigationService)
        {
            _category = category;
            _navigationService = navigationService;
            CategoryNavigationCommand = new DelegateCommand(NavigateToCategory);
            _productsViewModels = new List<ProductViewModel>();
            if (category != null && category.Products != null)
            {
                var position = 0;
                foreach (var product in category.Products)
                {
                    _productsViewModels.Add(new ProductViewModel(product) { ItemPosition = position });
                    position++;
                }
                Products = _productsViewModels;
            }

        }

        public int CategoryId { get { return _category.Id; } }

        public int ParentCategoryId { get { return _category.ParentId; } }

        public string Title { get { return _category.Title; } }

        public IEnumerable<ProductViewModel> Products { get; private set; }

        public int TotalNumberOfItems { get { return _category.TotalNumberOfItems; } }

        public ICommand CategoryNavigationCommand { get; private set; }

        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._category.ImageUri != null)
                {
                    this._image = new BitmapImage(this._category.ImageUri);
                }
                return this._image;
            }
        }

        private void NavigateToCategory()
        {
            _navigationService.Navigate("GroupDetail", CategoryId);
        }
    }
}
