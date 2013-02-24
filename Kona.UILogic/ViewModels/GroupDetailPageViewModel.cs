// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.ObjectModel;
using Kona.Infrastructure;
using Kona.Infrastructure.Interfaces;
using Kona.UILogic.Repositories;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Kona.UILogic.Services;
using System.Net.Http;

namespace Kona.UILogic.ViewModels
{
    public class GroupDetailPageViewModel : ViewModel, INavigationAware
    {
        private readonly IProductCatalogRepository _productCatalogRepository;
        private readonly INavigationService _navigationService;
        private readonly IAlertMessageService _alertService;
        private readonly IResourceLoader _resourceLoader;
        private readonly ISearchPaneService _searchPaneService;
        private IReadOnlyCollection<object> _items;
        private string _title;

        public GroupDetailPageViewModel(IProductCatalogRepository productCatalogRepository, INavigationService navigationService, IAlertMessageService alertMessageService, IResourceLoader resourceLoader, ISearchPaneService searchPaneService)
        {
            _productCatalogRepository = productCatalogRepository;
            _navigationService = navigationService;
            _alertService = alertMessageService;
            _resourceLoader = resourceLoader;
            _searchPaneService = searchPaneService;
            ProductNavigationAction = NavigateToProduct;
            GoBackCommand = new DelegateCommand(navigationService.GoBack, navigationService.CanGoBack);
        }

        public IReadOnlyCollection<object> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        public Action<object> ProductNavigationAction { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }

        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            try
            {
                var categoryId = navigationParameter is int ? (int)navigationParameter : 0;
                var subcategories = await _productCatalogRepository.GetSubcategoriesAsync(categoryId);
                var categorytViewModels = new List<CategoryViewModel>();
                foreach (var subcategory in subcategories)
                {
                    categorytViewModels.Add(new CategoryViewModel(subcategory, _navigationService));
                }
                Items = new ReadOnlyCollection<CategoryViewModel>(categorytViewModels);

                var category = await _productCatalogRepository.GetCategoryAsync(categoryId);
                Title = category.Title;
                _searchPaneService.ShowOnKeyboardInput(true);
            }
            catch (HttpRequestException)
            {
                var task = _alertService.ShowAsync(_resourceLoader.GetString("ErrorServiceUnreachable"), _resourceLoader.GetString("Error"));
            }
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewState, bool suspending)
        {
            base.OnNavigatedFrom(viewState, suspending);
            if (!suspending)
            {
                _searchPaneService.ShowOnKeyboardInput(false);
            }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        // <snippet607>
        private void NavigateToProduct(object parameter)
        {
            var product = parameter as ProductViewModel;
            if (product != null)
            {
                _navigationService.Navigate("ItemDetail", product.ProductNumber);
            }
        }
        // </snippet607>
    }
}
