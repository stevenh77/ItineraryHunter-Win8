// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Threading.Tasks;

namespace Kona.UILogic.Services
{
    public interface ITileService
    {
        bool SecondaryTileExists(string tileId);

        Task<bool> PinSquareSecondaryTile(string tileId, string shortName, string displayName, string arguments);

        Task<bool> PinWideSecondaryTile(string tileId, string shortName, string displayName, string arguments);

        Task<bool> UnpinTile(string tileId);
    }
}
