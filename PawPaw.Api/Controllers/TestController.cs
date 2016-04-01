using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PawPaw.Api.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        [Route("test")]
        public string GetStuff()
        {
            return "HEIEHEI";
        }
    }
}
