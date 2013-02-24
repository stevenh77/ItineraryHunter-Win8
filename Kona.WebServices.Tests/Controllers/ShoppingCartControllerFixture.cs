// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Web.Http;
using Kona.WebServices.Controllers;
using Kona.WebServices.Models;
using Kona.WebServices.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kona.WebServices.Tests.Controllers
{
    [TestClass]
    public class ShoppingCartControllerFixture
    {
        //[TestMethod]
        //public void Get_Shopping_Cart_Items_For_An_User()
        //{
        //    var shoppingCart = new ShoppingCart(new ObservableCollection<ShoppingCartItem>()) { FullPrice = 200, TotalDiscount = 100 };
        //    var repository = new MockShoppingCartRepository();
        //    repository.GetDelegate = (userId) =>
        //                                 {
        //                                     ShoppingCart myshoppingCart = null;

        //                                     if (userId == "JohnDoe")
        //                                     {
        //                                         myshoppingCart = shoppingCart;
        //                                     }

        //                                     return myshoppingCart;
        //                                 };

        //    var controller = new ShoppingCartController(repository);
        //    var result = controller.Get("JohnDoe");
        //    Assert.AreEqual(result, shoppingCart);
        //}

        //[TestMethod]
        //public void Get_Null_Shopping_Cart_Items_For_Unknown_User()
        //{
        //    var shoppingCart = new ShoppingCart(new ObservableCollection<ShoppingCartItem>()) { FullPrice = 200, TotalDiscount = 100 };
        //    var repository = new MockShoppingCartRepository();
        //    repository.GetDelegate = (userId) =>
        //    {
        //        ShoppingCart myshoppingCart = null;

        //        if (userId == "JohnDoe")
        //        {
        //            myshoppingCart = shoppingCart;
        //        }

        //        return myshoppingCart;
        //    };

        //    var controller = new ShoppingCartController(repository);

        //    var result = controller.Get("UnknownUser");
        //    Assert.AreEqual(result, null);
        //}

        //[TestMethod]
        //public void Delete_ShoppingCartItem_For_An_User()
        //{
        //    var myGuid = new Guid();
        //    var shoppingCart = new ShoppingCart(new ObservableCollection<ShoppingCartItem>() {new ShoppingCartItem() {Id = myGuid}})
        //        { FullPrice = 200, TotalDiscount = 100 };
        //    var repository = new MockShoppingCartRepository();
        //    repository.GetDelegate = (userId) =>
        //    {
        //        ShoppingCart myshoppingCart = null;

        //        if (userId == "JohnDoe")
        //        {
        //            myshoppingCart = shoppingCart;
        //        }

        //        return myshoppingCart;
        //    };

        //    repository.RemoveDelegate = (userId, itemId) =>
        //     {
        //        var myshoppingCart = repository.GetShoppingCart(userId);

        //        if (myshoppingCart != null)
        //        {
        //            var item = myshoppingCart.ShoppingCartItems.FirstOrDefault(i => i.Id == Guid.Parse(itemId));
        //            return myshoppingCart.ShoppingCartItems.Remove(item);
        //        }
        //       else
        //        {
        //            return false;
        //        }
        //    };

        //    var controller = new ShoppingCartController(repository);
        //    controller.DeleteShoppingCartItem("JohnDoe", myGuid.ToString());
        //    var resultShoppingCart = controller.Get("JohnDoe");
        //    Assert.IsNotNull(resultShoppingCart);
        //    Assert.AreEqual(resultShoppingCart.ShoppingCartItems.Count, 0);
        //}

        //[TestMethod]
        //public void Delete_ShoppingCartItem_For_Unknown_User()
        //{
        //    var myGuid = new Guid();
        //    var shoppingCart = new ShoppingCart(new ObservableCollection<ShoppingCartItem>() { new ShoppingCartItem() { Id = myGuid } }) { FullPrice = 200, TotalDiscount = 100 };
        //    var repository = new MockShoppingCartRepository();
        //    repository.GetDelegate = (userId) =>
        //    {
        //        ShoppingCart myshoppingCart = null;

        //        if (userId == "JohnDoe")
        //        {
        //            myshoppingCart = shoppingCart;
        //        }

        //        return myshoppingCart;
        //    };

        //    repository.RemoveDelegate = (userId, itemId) =>
        //    {
        //        var myshoppingCart = repository.GetShoppingCart(userId);

        //        if (myshoppingCart != null)
        //        {
        //            var item = myshoppingCart.ShoppingCartItems.FirstOrDefault(i => i.Id == Guid.Parse(itemId));
        //            return myshoppingCart.ShoppingCartItems.Remove(item);
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    };

        //    HttpResponseException caughtException = null;
        //    var controller = new ShoppingCartController(repository);
        //    try
        //    {
        //        controller.DeleteShoppingCartItem("UnknownUser", myGuid.ToString());
        //    }
        //    catch (HttpResponseException ex)
        //    {
        //        caughtException = ex;
        //    }
        //    Assert.AreEqual(HttpStatusCode.NotFound, caughtException.Response.StatusCode);

        //    var resultShoppingCart = controller.Get("JohnDoe");
        //    Assert.IsNotNull(resultShoppingCart);
        //    Assert.AreEqual(resultShoppingCart.ShoppingCartItems.Count, 1);
        //}

        //[TestMethod]
        //public void Delete_ShoppingCartItem_For_NonExistent_Item()
        //{
        //    var myGuid = new Guid();
        //    var shoppingCart = new ShoppingCart(new ObservableCollection<ShoppingCartItem>() { new ShoppingCartItem() { Id = myGuid } }) { FullPrice = 200, TotalDiscount = 100 };
        //    var repository = new MockShoppingCartRepository();
        //    repository.GetDelegate = (userId) =>
        //    {
        //        ShoppingCart myshoppingCart = null;

        //        if (userId == "JohnDoe")
        //        {
        //            myshoppingCart = shoppingCart;
        //        }

        //        return myshoppingCart;
        //    };

        //    repository.RemoveDelegate = (userId, itemId) =>
        //    {
        //        var myshoppingCart = repository.GetShoppingCart(userId);

        //        if (myshoppingCart != null)
        //        {
        //            var item = myshoppingCart.ShoppingCartItems.FirstOrDefault(i => i.Id == Guid.Parse(itemId));
        //            return myshoppingCart.ShoppingCartItems.Remove(item);
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    };

        //    var controller = new ShoppingCartController(repository);
        //    try
        //    {
        //        controller.DeleteShoppingCartItem("JohnDoe", Guid.NewGuid().ToString());
        //    }
        //    catch (HttpResponseException ex)
        //    {
        //        Assert.AreEqual(HttpStatusCode.NotFound, ex.Response.StatusCode);
        //    }
        //    var resultShoppingCart = controller.Get("JohnDoe");
        //    Assert.IsNotNull(resultShoppingCart);
        //    Assert.AreEqual(resultShoppingCart.ShoppingCartItems.Count, 1);
        //}

        //[TestMethod]
        //public void Delete_ShoppingCart()
        //{
        //    var repository = new MockShoppingCartRepository();
        //    repository.DeleteCartDelegate = (userId) => true;

        //    var controller = new ShoppingCartController(repository);

        //    controller.DeleteShoppingCart("JohnDoe");
        //}

        //[TestMethod]
        //public void Delete_ShoppingCart_For_NonExistentCart_Throws()
        //{
        //    var exceptionCaught = false;
        //    var repository = new MockShoppingCartRepository();
        //    repository.DeleteCartDelegate = (userId) => false;

        //    var controller = new ShoppingCartController(repository);
        //    try
        //    {
        //        controller.DeleteShoppingCart("JohnDoe");
        //    }
        //    catch (HttpResponseException ex)
        //    {
        //        exceptionCaught = true;
        //        Assert.AreEqual(HttpStatusCode.NotFound, ex.Response.StatusCode);
        //    }

        //    Assert.IsTrue(exceptionCaught);
        //}
    }
}
