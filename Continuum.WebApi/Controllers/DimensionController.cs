﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Continuum.WebApi.Controllers
{
    public class DimensionController : ApiController
    {
        public IEnumerable<Models.Dimension> Get()
        {
            var dimensions = new string[] 
            {
                "Dimension 1",
                "Dimension 2",
                "Dimension 3",
                "Dimension 4",
                "Dimension 5",
                "Dimension 6",
                "Dimension 7"
            };

            return dimensions.Select(i => new Models.Dimension() 
            {
                Name = i 
            }).ToList(); 
        }

        [Route("DimensionList")]
        public IEnumerable<Models.Dimension> GetDimensionList()
        {
            return null; 
        }
    }
}
