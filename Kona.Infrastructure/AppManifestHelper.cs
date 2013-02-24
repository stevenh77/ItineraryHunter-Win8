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
using System.Xml.Linq;

namespace Kona.Infrastructure
{
    public class AppManifestHelper
    {
        private static readonly XDocument manifest;
        private static readonly XNamespace xNamespace;

        static AppManifestHelper()
        {
            manifest = XDocument.Load("AppxManifest.xml", LoadOptions.None);
            xNamespace = XNamespace.Get("http://schemas.microsoft.com/appx/2010/manifest");
        }

        public static bool IsSearchDeclared()
        {
            // Get the SplashScreen node located at Package/Applications/Application/VisualElements/SplashScreen
            var extensions = manifest.Descendants(xNamespace + "Extension");
            foreach (var extension in extensions)
            {
                if (extension.Attribute("Category") != null && extension.Attribute("Category").Value == "windows.search")
                {
                    return true;
                }
            }
            return false;
        }

    }
}
