using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Continuum.WebApi.Logic
{
    public class DimensionLogic : LogicBase
    {
        private readonly Data.DimensionRepo _dimensionRepository;

        public DimensionLogic(Data.DimensionRepo dimensionRepository, System.Security.Principal.IPrincipal principal)
            : base(principal)
        {
            _dimensionRepository = dimensionRepository;

        }

        internal IEnumerable<Models.Dimension> ListDimensions()
        {
            return _dimensionRepository.All().Select(i => new Models.Dimension()
            {
                Name = i.Name,
                Id = i.Id,
                ImageName = i.ImageName
            }).ToList();
        }

        internal Models.Dimension GetDimensionById(int id)
        {
            var result = _dimensionRepository.FindById(id);

            var dimension = new Models.Dimension()
            {
                Id = id,
                Name = result.Name,
                ImageName = result.ImageName,
                Capabilities = result.Capabilities.Select(i => new Models.Capability()
                {
                    Id = i.Id,
                    Description = i.Description,
                    Level = i.LevelId,
                    RequiredCapabilities = i.CapabilityRequirements.Select(j => j.RequiredCapabilityId).ToArray()
                }).ToList()
            };

            return dimension; 
        }

        internal bool DimensionExists(int id)
        {
            return _dimensionRepository.FindById(id) != null;
        }
    }
}