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
using Kona.UILogic.Services;

namespace Kona.UILogic.Tests.Mocks
{
    public class MockTileService : ITileService
    {
        public Func<string, bool> SecondaryTileExistsDelegate { get; set; }
        public Func<string, string, string, string, Task<bool>> PinSquareSecondaryTileDelegate { get; set; }
        public Func<string, string, string, string, Task<bool>> PinWideSecondaryTileDelegate { get; set; }
        public Func<string, Task<bool>> UnpinTileDelegate { get; set; }

        public bool SecondaryTileExists(string tileId)
        {
            return SecondaryTileExistsDelegate(tileId);
        }

        public Task<bool> PinSquareSecondaryTile(string tileId, string shortName, string displayName, string arguments)
        {
            return PinSquareSecondaryTileDelegate(tileId, shortName, displayName, arguments);
        }

        public Task<bool> PinWideSecondaryTile(string tileId, string shortName, string displayName, string arguments)
        {
            return PinWideSecondaryTileDelegate(tileId, shortName, displayName, arguments);
        }

        public Task<bool> UnpinTile(string tileId)
        {
            return UnpinTileDelegate(tileId);
        }
    }
}
