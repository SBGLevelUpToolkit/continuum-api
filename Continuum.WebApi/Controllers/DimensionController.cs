using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Continuum.WebApi.Controllers
{
    public class DimensionController : ApiController
    {
        private readonly Data.DimensionRepo _dimensionRepo;

        public DimensionController(Data.DimensionRepo dimensionRepo)
        {
            _dimensionRepo = dimensionRepo;
        }

        public IEnumerable<Models.Dimension> Get()
        {
            return _dimensionRepo.All().Select(i => new Models.Dimension() 
            {
                Name = i.Name,
                Id = i.Id
            }).ToList();
        }

        public Models.Dimension Get(int id)
        {
            var result = _dimensionRepo.FindById(id);

            if (result == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound);
                throw new HttpResponseException(resp);
            }

            var dimension = new Models.Dimension()
            {
                Id = id,
                Name = result.Name,
                Capabilities = result.Capabilities.Select(i => new Models.Capability() 
                { 
                    Id = i.Id,  
                    Description = i.Description,
                    Level = i.LevelId,
                    RequiredCapabilities = i.CapabilityRequirements.Select(j=>j.RequiredCapabilityId).ToArray()
                }).ToList()
            };

            return dimension; 
        }

    }
}
