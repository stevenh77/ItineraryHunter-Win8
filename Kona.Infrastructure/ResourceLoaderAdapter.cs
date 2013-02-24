// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Windows.ApplicationModel.Resources;

namespace Kona.Infrastructure
{
    public class ResourceLoaderAdapter : IResourceLoader
    {
        private readonly ResourceLoader _resourceLoader;
        
        public ResourceLoaderAdapter(ResourceLoader resourceLoader)
        {
            _resourceLoader = resourceLoader;
        }

        public string GetString(string resource)
        {
            return _resourceLoader.GetString(resource);
        }
    }
}
