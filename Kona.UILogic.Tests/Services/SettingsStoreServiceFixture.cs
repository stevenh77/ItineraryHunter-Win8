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
using Kona.UILogic.Services;
using Kona.UILogic.Tests.Mocks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Kona.UILogic.Tests.Services
{
    [TestClass]
    public class SettingsStoreServiceFixture
    {
        [TestMethod]
        public void GetValue_ReturnsValue()
        {
            var target = new SettingsStoreService();
            SetupTarget(target);

            string value1 = target.GetValue<string>("TestContainer1", "1");
            string value2 = target.GetValue<string>("TestContainer1", "2");
            string value3 = target.GetValue<string>("TestContainer2", "3");
            string invalidValue = target.GetValue<string>("TestContainer2", "-1");

            Assert.AreEqual(value1, "value1");
            Assert.AreEqual(value2, "value2");
            Assert.AreEqual(value3, "value3");
            Assert.IsNull(invalidValue);
        }

        [TestMethod]
        public void GetAllValues_ReturnsAllValues()
        {
            var target = new SettingsStoreService();
            SetupTarget(target);

            var valuesContainer1 = target.GetAllValues<string>("TestContainer1");
            var valuesContainer2 = target.GetAllValues<string>("TestContainer2");

            Assert.IsNotNull(valuesContainer1);
            Assert.IsTrue(valuesContainer1.Count() == 2);
            Assert.IsNotNull(valuesContainer2);
            Assert.IsTrue(valuesContainer2.Count() == 1);
        }

        [TestMethod]
        public void SaveValue_SavesValues()
        {
            var target = new SettingsStoreService();
            SetupTarget(target);

            target.SaveValue("TestContainer1", "100", "NewValue");
            var value = target.GetValue<string>("TestContainer1", "100");

            Assert.IsTrue(value == "NewValue");
        }

        [TestMethod]
        public void GetEntity_ReturnsEntity()
        {
            var target = new SettingsStoreService();
            SetupTarget(target);

            var entity4 = target.GetEntity<MockAddress>("TestContainer3", "4");
            var entity5 = target.GetEntity<MockAddress>("TestContainer3", "5");
            var entity6 = target.GetEntity<MockAddress>("TestContainer4", "7");
            var invalidEntity = target.GetEntity<MockAddress>("-1", "-1");

            Assert.IsNotNull(entity4);
            Assert.AreEqual(entity4.FirstName, "TestFirstName4");
            Assert.IsNotNull(entity5);
            Assert.AreEqual(entity5.FirstName, "TestFirstName5");
            Assert.IsNotNull(entity6);
            Assert.AreEqual(entity6.FirstName, "TestFirstName7");
            Assert.IsNull(invalidEntity);
        }

        [TestMethod]
        public void GetAllEntities_ReturnsAllEntities()
        {
            var target = new SettingsStoreService();
            SetupTarget(target);

            var valuesContainer3 = target.GetAllEntities<MockAddress>("TestContainer3");
            var valuesContainer4 = target.GetAllEntities<MockAddress>("TestContainer4");

            Assert.IsNotNull(valuesContainer3);
            Assert.IsTrue(valuesContainer3.Count() == 3);
            Assert.IsNotNull(valuesContainer4);
            Assert.IsTrue(valuesContainer4.Count() == 1);
        }

        [TestMethod]
        public void SaveEntity_SavesEntity()
        {
            var target = new SettingsStoreService();
            SetupTarget(target);
            
            target.SaveEntity("TestContainer4", "100", new MockAddress() { FirstName = "NewAddress" });
            var entity = target.GetEntity<MockAddress>("TestContainer4", "100");

            Assert.IsNotNull(entity);
            Assert.IsTrue(entity.FirstName == "NewAddress");
        }

        [TestMethod]
        public void DeleteSetting_DeletesSetting()
        {
            var target = new SettingsStoreService();
            SetupTarget(target);

            target.DeleteSetting("TestContainer1", "1");
            target.DeleteSetting("TestContainer2", "3");
            target.DeleteSetting("TestContainer2", "-1");

            target.DeleteSetting("TestContainer3", "4");
            target.DeleteSetting("TestContainer4", "7");
            target.DeleteSetting("TestContainer4", "-1");

            var valuesContainer1 = target.GetAllValues<string>("TestContainer1");
            var valuesContainer2 = target.GetAllValues<string>("TestContainer2");
            var valuesContainer3 = target.GetAllEntities<MockAddress>("TestContainer3");
            var valuesContainer4 = target.GetAllEntities<MockAddress>("TestContainer4");

            Assert.AreEqual(valuesContainer1.Count(), 1);
            Assert.AreEqual(valuesContainer2.Count(), 0);
            Assert.AreEqual(valuesContainer3.Count(), 2);
            Assert.AreEqual(valuesContainer4.Count(), 0);
        }

        private void SetupTarget(SettingsStoreService target)
        {
            // Clear all data
            target.DeleteContainer("TestContainer1");
            target.DeleteContainer("TestContainer2");
            target.DeleteContainer("TestContainer3");
            target.DeleteContainer("TestContainer4");

            target.SaveValue<string>("TestContainer1", "1", "value1");
            target.SaveValue<string>("TestContainer1", "2", "value2");
            target.SaveValue<string>("TestContainer2", "3", "value3");

            target.SaveEntity("TestContainer3", "4", new MockAddress() { FirstName = "TestFirstName4" });
            target.SaveEntity("TestContainer3", "5", new MockAddress() { FirstName = "TestFirstName5" });
            target.SaveEntity("TestContainer3", "6", new MockAddress() { FirstName = "TestFirstName6" });
            target.SaveEntity("TestContainer4", "7", new MockAddress() { FirstName = "TestFirstName7" });
        }
    }
}
