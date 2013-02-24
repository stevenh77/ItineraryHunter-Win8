// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Runtime.Serialization;
using Kona.Infrastructure;

namespace Kona.UILogic.ViewModels
{
    public class CheckoutDataViewModel : ViewModel
    {
        private string _title;
        private string _firstLine;
        private string _secondLine;
        private string _bottomLine;
        private Uri _logoUri;
        private string _dataType;
        private object _context;

        public CheckoutDataViewModel(string entityId, string title, string firstLine, string secondLine, string bottomLine, Uri logoUri, string dataType, object context)
        {
            EntityId = entityId;
            _title = title;
            _firstLine = firstLine;
            _secondLine = secondLine;
            _bottomLine = bottomLine;
            _logoUri = logoUri;
            _dataType = dataType;
            _context = context;
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string FirstLine
        {
            get { return _firstLine; }
            set { SetProperty(ref _firstLine, value); }
        }

        public string SecondLine
        {
            get { return _secondLine; }
            set { SetProperty(ref _secondLine, value); }
        }

        public string BottomLine
        {
            get { return _bottomLine; }
            set { SetProperty(ref _bottomLine, value); }
        }

        public Uri LogoUri
        {
            get { return _logoUri; }
            set { SetProperty(ref _logoUri, value); }
        }

        public string DataType
        {
            get { return _dataType; }
            set { SetProperty(ref _dataType, value); }
        }

        public object Context
        {
            get { return _context; }
            set { SetProperty(ref _context, value); }
        }
    }
}
