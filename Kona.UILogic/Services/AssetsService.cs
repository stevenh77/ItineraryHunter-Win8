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

namespace Kona.UILogic.Services
{
    public class AssetsService : IAssetsService
    {
        private readonly Uri _baseUri = new Uri("ms-appx:///Assets/");
        private readonly Uri _squareLogoUri;
        private readonly Uri _wideLogoUri;

        public AssetsService(string squareLogoImageName, string wideLogoImageName)
        {
            _squareLogoUri = new Uri(_baseUri, squareLogoImageName);
            _wideLogoUri = new Uri(_baseUri, wideLogoImageName);
        }

        public Uri GetSquareLogoUri()
        {
            return _squareLogoUri;
        }

        public Uri GetWideLogoUri()
        {
            return _wideLogoUri;
        }
    }
}
