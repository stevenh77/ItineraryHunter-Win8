// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Kona.Infrastructure;
using Kona.Infrastructure.Interfaces;
using Kona.UILogic.Repositories;
using Windows.UI.Xaml.Navigation;

namespace Kona.UILogic.ViewModels
{
    public class SearchResultsPageViewModel : ViewModel, INavigationAware
    {
        private readonly IProductCatalogRepository _productCatalogRepository;
        private readonly INavigationService _navigationService;
        private readonly ISearchPaneService _searchPaneService;
        private string _searchTerm;
        private string _queryString;
        private bool _noResults;
        private ReadOnlyCollection<CategoryViewModel> _results;

        public SearchResultsPageViewModel(IProductCatalogRepository productCatalogRepository, INavigationService navigationService, ISearchPaneService searchPaneService)
        {
            _productCatalogRepository = productCatalogRepository;
            _navigationService = navigationService;
            _searchPaneService = searchPaneService;
            ProductNavigationAction = NavigateToItem;
            GoBackCommand = new DelegateCommand(_navigationService.GoBack, _navigationService.CanGoBack);
        }

        public string QueryText
        {
            get { return _queryString; }
            set { SetProperty(ref this._queryString, value); }
        }

        public string SearchTerm
        {
            get { return _searchTerm; }
            set { SetProperty(ref this._searchTerm, value); }
        }

        public ReadOnlyCollection<CategoryViewModel> Results {
            get { return _results; }
            set { SetProperty(ref _results, value); }
        } 

        public bool NoResults
        {
            get { return _noResults; }
            set { SetProperty(ref _noResults, value); }
        }

        public Action<object> ProductNavigationAction { get; private set; }

        public ICommand GoBackCommand { get; private set; }

        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            var queryText = navigationParameter as String;

            var rootCategories = await _productCatalogRepository.GetFilteredProductsAsync(queryText);
            var rootCategoryViewModels = new List<CategoryViewModel>();
            foreach (var rootCategory in rootCategories)
            {
                rootCategoryViewModels.Add(new CategoryViewModel(rootCategory, _navigationService));
            }

            // Communicate results through the view model
            this.SearchTerm = queryText;
            this.QueryText = '\u201c' + queryText + '\u201d';
            this.Results = new ReadOnlyCollection<CategoryViewModel>(rootCategoryViewModels);
            this.NoResults = this.Results.Count(c => c.Products.Any()) <= 0;
               
            _searchPaneService.ShowOnKeyboardInput(true);
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewState, bool suspending)
        {
            base.OnNavigatedFrom(viewState, suspending);
            if (!suspending)
            {
                _searchPaneService.ShowOnKeyboardInput(false);
            }
        }

        private void NavigateToItem(object parameter)
        {
            var product = parameter as ProductViewModel;
            if (product != null)
            {
                _navigationService.Navigate("ItemDetail", product.ProductNumber);
            }
        }
    }
}


