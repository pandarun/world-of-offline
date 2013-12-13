using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace web.Api
{
    public class BaseApiController : ApiController
    {

        protected readonly woo.data.WooDataContext DataContext;
        public BaseApiController()
        {
            DataContext = new woo.data.WooDataContext();
        }
        
    }
}