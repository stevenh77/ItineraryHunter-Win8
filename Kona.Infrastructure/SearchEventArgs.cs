// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Search;

namespace Kona.Infrastructure
{
    public class SearchEventArgs
    {

        public SearchEventArgs(SearchActivatedEventArgs args)
        {
            Language = args.Language;
            QueryText = args.QueryText;
        }

        public SearchEventArgs(SearchPaneQuerySubmittedEventArgs args)
        {
            Language = args.Language;
            QueryText = args.QueryText;
        }

        public string Language { get; set; }

        public string QueryText { get; set; }
    }
}
