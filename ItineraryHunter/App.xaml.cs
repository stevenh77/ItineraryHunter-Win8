// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure;
using Kona.Infrastructure.Interfaces;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Kona.UILogic.Services;
using Kona.UILogic.ViewModels;
using Microsoft.Practices.PubSubEvents;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.UI.Notifications;
using Windows.UI.Xaml;

namespace Kona.AWShopper
{
    sealed partial class App : MvvmAppBase
    {
        // Create the singleton container that will be used for type resolution in the app
        IUnityContainer _container = new UnityContainer();

        //Bootstrap: App singleton service declarations
        private IEventAggregator _eventAggregator;
        private TileUpdater _tileUpdater;

        public App()
        {
            this.InitializeComponent();
            this.RequestedTheme = ApplicationTheme.Dark;
        }

        // <snippet812>
        public override void OnLaunchApplication(LaunchActivatedEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Arguments))
            {
                // Navigate to the initial page
                NavigationService.Navigate("Hub", null);
            }
            else
            {
                // The app was launched from a Secondary Tile
                // Navigate to the item's page
                NavigationService.Navigate("ItemDetail", args.Arguments);
            }
        }
        // </snippet812>

        public override void OnSearchApplication(SearchEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.QueryText))
            {
                NavigationService.Navigate("SearchResults", args.QueryText);
            }
            else
            {
                NavigationService.Navigate("Hub", null);
            }
        }

        public override void OnRegisterKnownTypesforSerialization()
        {
            // Set up the list of known types for the SuspensionManager
            SuspensionManager.KnownTypes.Add(typeof(Address));
            SuspensionManager.KnownTypes.Add(typeof(PaymentMethod));
            SuspensionManager.KnownTypes.Add(typeof(UserInfo));
            SuspensionManager.KnownTypes.Add(typeof(ShippingMethod));
            SuspensionManager.KnownTypes.Add(typeof(ReadOnlyDictionary<string, ReadOnlyCollection<string>>));
            SuspensionManager.KnownTypes.Add(typeof(Order));
            SuspensionManager.KnownTypes.Add(typeof(UserInfo));
        }

        public override void OnInitialize(IActivatedEventArgs args)
        {
            _eventAggregator = new EventAggregator();

            _container.RegisterInstance<INavigationService>(NavigationService);
            _container.RegisterInstance<ISuspensionManagerState>(SuspensionManagerState);
            _container.RegisterInstance<IFlyoutService>(FlyoutService);
            _container.RegisterInstance<IEventAggregator>(_eventAggregator);
            _container.RegisterInstance<ISettingsStoreService>(new SettingsStoreService());
            _container.RegisterInstance<IAssetsService>(new AssetsService("Logo.png", "WideLogo.scale-100.png"));
            _container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));

            _container.RegisterType<IRequestService, RequestService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IAccountService, AccountService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICredentialStore, RoamingCredentialStore>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICacheService, TemporaryFolderCacheService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ITileService, TileService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IAlertMessageService, AlertMessageService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISearchPaneService, SearchPaneService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IEncryptionService, EncryptionService>(new ContainerControlledLifetimeManager());

            // Register repositories
            _container.RegisterType<IProductCatalogRepository, ProductCatalogRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IShoppingCartRepository, ShoppingCartRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICheckoutDataRepository, CheckoutDataRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IOrderRepository, OrderRepository>(new ContainerControlledLifetimeManager());

            // Register web service proxies
            _container.RegisterType<IProductCatalogService, ProductCatalogServiceProxy>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IOrderService, OrderServiceProxy>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IShoppingCartService, ShoppingCartServiceProxy>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IShippingMethodService, ShippingMethodServiceProxy>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IIdentityService, IdentityServiceProxy>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILocationService, LocationServiceProxy>(new ContainerControlledLifetimeManager());
            
            // Register child view models
            _container.RegisterType<IShippingAddressUserControlViewModel, ShippingAddressUserControlViewModel>();
            _container.RegisterType<IBillingAddressUserControlViewModel, BillingAddressUserControlViewModel>();
            _container.RegisterType<IPaymentMethodUserControlViewModel, PaymentMethodUserControlViewModel>();

            // <snippet302>
            ViewModelLocator.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
                {
                    var viewModelTypeName = string.Format("Kona.UILogic.ViewModels.{0}ViewModel, Kona.UILogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=634ac3171ee5190a", viewType.Name);
                    var viewModelType = Type.GetType(viewModelTypeName);
                    return viewModelType;
                });
            //</snippet302>

            // <snippet800>
            _tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            _tileUpdater.EnableNotificationQueue(true);
            _tileUpdater.StartPeriodicUpdate(new Uri(Kona.UILogic.Constants.ServerAddress + "/api/TileNotification"), PeriodicUpdateRecurrence.HalfHour);
            // </snippet800>
        }

        public override object Resolve(Type type)
        {
            // Use the container to resolve types (e.g. ViewModels and Flyouts)
            // so their dependencies get injected
            return _container.Resolve(type);
        }

        public override IList<SettingsCharmItem> GetSettingsCharmItems()
        {
            var settingsCharmItems = new List<SettingsCharmItem>();
            var accountService = _container.Resolve<IAccountService>();
            if (accountService.SignedInUser == null)
            {
                settingsCharmItems.Add(new SettingsCharmItem("Login", "SignIn"));
            }
            else
            {
                settingsCharmItems.Add(new SettingsCharmItem("Logout", "SignOut"));
            }
            settingsCharmItems.Add(new SettingsCharmItem("Add Shipping Address", "ShippingAddress"));
            settingsCharmItems.Add(new SettingsCharmItem("Add Billing Address", "BillingAddress"));
            settingsCharmItems.Add(new SettingsCharmItem("Add Payment Information", "PaymentMethod"));
            settingsCharmItems.Add(new SettingsCharmItem("Change Defaults", "ChangeDefaults"));
            return settingsCharmItems;
        }
    }
}
