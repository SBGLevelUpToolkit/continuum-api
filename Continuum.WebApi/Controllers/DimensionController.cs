using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Continuum.WebApi.Filters;

namespace Continuum.WebApi.Controllers
{
    public class DimensionController : ControllerBase
    {
        private readonly Data.DimensionRepo _dimensionRepo;
        private readonly Logic.DimensionLogic _dimensionLogic;

        public DimensionController(Data.DimensionRepo dimensionRepo)
        {
            _dimensionRepo = dimensionRepo;
            _dimensionLogic = new Logic.DimensionLogic(_dimensionRepo, CurrentUser == null ? this.User : CurrentUser);
    
        }

        public IEnumerable<Models.Dimension> Get()
        {
            return _dimensionLogic.ListDimensions();
        }

        [ApplicationExceptionFilter]
        public Models.Dimension Get(int id)
        {
            if (_dimensionLogic.DimensionExists(id))
            {
                return _dimensionLogic.GetDimensionById(id);
            }
            else
            {
                throw ExceptionBuilder.CreateException("Dimension does not exist.", "Dimension not found.", HttpStatusCode.NotFound);
            }
            
        }

    }
}
