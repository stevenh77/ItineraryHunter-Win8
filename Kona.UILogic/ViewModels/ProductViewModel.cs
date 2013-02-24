using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;

namespace Kona.UILogic.ViewModels
{
    public class ProductViewModel
    {
        private readonly Product _product;
        private readonly Uri _image;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ProductViewModel(Product product) : this(product, null) { }

        public ProductViewModel(Product product, IShoppingCartRepository shoppingCartRepository)
        {
            _product = product;
            _shoppingCartRepository = shoppingCartRepository;
            _image = product.ImageUri;

            AddToCartCommand = DelegateCommand.FromAsyncHandler(AddToCart);
        }

        public string Title { get { return _product.Title; } }

        public string Description { get { return _product.Description; } }

        public string DurationInDays { get { return _product.DurationInDays.ToString(); } }

        public string ProductNumber { get { return _product.ProductNumber; } }

        public Uri Image { get { return _image; } }

        public int ItemPosition { get; set; }
        
        public string SalePrice
        {
            get { return "0"; }
        }

        public DelegateCommand AddToCartCommand { get; private set; }

        public async Task AddToCart()
        {
            await _shoppingCartRepository.AddProductToShoppingCartAsync(ProductNumber);
        }

        public override string ToString()
        {
            return _product.ProductNumber;
        }
    }
}
