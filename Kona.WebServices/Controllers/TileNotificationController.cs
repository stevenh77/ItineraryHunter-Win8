// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace Kona.WebServices.Controllers
{
    public class TileNotificationController : ApiController
    {
        // <snippet801>
        public HttpResponseMessage GetTileNotification()
        {
            var tileXml = @"<tile>
                              <visual>
                                <binding template=""TileWidePeekImage01"">
                                  <image id=""1"" src=""http://localhost:2112/Images/hotrodbike_f_large.gif"" alt=""alt text""/>
                                  <text id=""1"">Mountain-400-W Silver, 42</text>
                                  <text id=""2"">Updated: {0} {1}</text>
                                </binding>
                                <binding template=""TileSquarePeekImageAndText02"">
                                  <image id=""1"" src=""http://localhost:2112/Images/hotrodbike_f_large.gif"" alt=""alt text""/>
                                  <text id=""1"">Mountain-400-W Silver, 42</text>
                                  <text id=""2"">Updated: {0} {1}</text>
                                </binding> 
                              </visual>
                            </tile>";
            tileXml = string.Format(tileXml, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());

            // create HTTP response
           var response = new HttpResponseMessage();

            // format response
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Content = new StringContent(tileXml);

            //Need to return xml format to TileUpdater.StartPeriodicUpdate
            response.Content.Headers.ContentType = 
                new System.Net.Http.Headers.MediaTypeHeaderValue("text/xml");
            return response;
        } 
        // </snippet801>
    }
}
