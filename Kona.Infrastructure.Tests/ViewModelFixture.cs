// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Linq;
using Kona.Infrastructure.Tests.Mocks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI.Xaml.Navigation;

namespace Kona.Infrastructure.Tests
{
    [TestClass]
    public class ViewModelFixture
    {
        [TestMethod]
        public void OnNavigatedFrom_With_No_RestorableStateAttributes()
        {
            var vm = new MockViewModelWithNoRestorableStateAttributes()
            {
                Title = "MyMock",
                Description = "MyDescription",
                EntityId =  "MyEntityId"
            };

            var result = new Dictionary<string, object>();
            
            vm.OnNavigatedFrom(result, true);

            Assert.IsTrue(result.Keys.Count == 1);
            Assert.IsTrue(result.ContainsKey("MyEntityId"));
            Assert.IsNotNull(result["MyEntityId"]);
            Assert.IsInstanceOfType(result["MyEntityId"], typeof(Dictionary<string, object>));
            Assert.IsTrue(((Dictionary<string, object>)result["MyEntityId"]).Count == 0);
        }

        [TestMethod]
        public void OnNavigatedFrom_With_RestorableStateAttributes()
        {
            var vm = new MockViewModelWithRestorableStateAttributes()
            {
                Title = "MyMock",
                Description = "MyDescription",
                EntityId = "MyEntityId"
            };
            var result = new Dictionary<string, object>();

            vm.OnNavigatedFrom(result, true);

            Assert.IsTrue(result.Keys.Count == 1);
            Assert.IsTrue(result.ContainsKey("MyEntityId"));
            Assert.IsNotNull(result["MyEntityId"]);
            Assert.IsInstanceOfType(result["MyEntityId"], typeof(Dictionary<string, object>));

            var viewState = (Dictionary<string, object>)result["MyEntityId"];

            Assert.IsTrue(viewState.Keys.Count == 2);
            Assert.IsTrue(viewState["Title"].ToString() == "MyMock");
            Assert.IsTrue(viewState["Description"].ToString() == "MyDescription");
        }

        [TestMethod]
        public void OnNavigatedTo_With_No_RestorableStateAttributes()
        {
            var viewModelState = new Dictionary<string, object>();
            viewModelState.Add("Title", "MyMock");
            viewModelState.Add("Description", "MyDescription");

            var viewState = new Dictionary<string, object>();
            viewState.Add("Kona.AWShopper.Tests.Mocks.MockViewModelWithNoResumableStateAttributes1", viewModelState);

            var vm = new MockViewModelWithNoRestorableStateAttributes() { EntityId = "MyEntityId" };
            vm.OnNavigatedTo(null, NavigationMode.Back, viewState);

            Assert.IsNull(vm.Title);
            Assert.IsNull(vm.Description);
        }


        [TestMethod]
        public void OnNavigatedTo_With_RestorableStateAttribute()
        {
            var viewModelState = new Dictionary<string, object>();
            viewModelState.Add("Title", "MyMock");
            viewModelState.Add("Description", "MyDescription");

            var viewState = new Dictionary<string, object>();
            viewState.Add("MyEntityId", viewModelState);

            var vm = new MockViewModelWithRestorableStateAttributes() { EntityId = "MyEntityId" };
            vm.OnNavigatedTo(null, NavigationMode.Back, viewState);

            Assert.AreEqual(vm.Title, viewModelState["Title"]);
            Assert.AreEqual(vm.Description, viewModelState["Description"]);
        }

        [TestMethod]
        public void OnNavigatedTo_With_RestorableStateCollection()
        {
            var childViewModelState = new Dictionary<string, object>();
            childViewModelState.Add("Title", "MyChildMock");
            childViewModelState.Add("Description", "MyChildDescription");

            var viewModelState = new Dictionary<string, object>();
            viewModelState.Add("Kona.AWShopper.Tests.Mocks.MockViewModelWithResumableStateCollection1", childViewModelState);

            var viewState = new Dictionary<string, object>();
            viewState.Add("MyEntityId", viewModelState);

            var vm = new MockViewModelWithRestorableStateCollection()
            {
                EntityId = "MyEntityId",
                ChildViewModels = new List<BindableBase>()
                {
                    new MockViewModelWithRestorableStateAttributes
                    {
                        Title = "MyChildMock",
                        Description = "MyChildDescription"
                    }
                }
            };
            vm.OnNavigatedTo(null, NavigationMode.Back, viewState);

            var childViewModel = (MockViewModelWithRestorableStateAttributes)vm.ChildViewModels.FirstOrDefault();

            Assert.AreEqual(childViewModel.Title, childViewModelState["Title"]);
            Assert.AreEqual(childViewModel.Description, childViewModelState["Description"]);
        }
    }
}
