// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Http;
using Kona.WebServices.Models;
using Kona.WebServices.Repositories;

namespace Kona.WebServices.Controllers
{
    public class LocationController : ApiController
    {
        private IRepository<State> _stateRepository;

        public LocationController()
            : this(new StateRepository())
        { }

        public LocationController(IRepository<State> stateRepository)
        {
            _stateRepository = stateRepository;
        }

        //
        // GET: /api/Location/
        public IEnumerable<string> GetStates()
        {
            return _stateRepository.GetAll().Select(c => c.Name);
        }

        // TODO: add countries here
    }
}
