using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data
{
    public class OrganisationRepo : RepositoryBase
    {
        public OrganisationRepo(IContinuumDataContainer container) : base(container) { }

        public void CreateOrganisation(string organisationName)
        {
            _container.Organisations.Add(new Organisation() { Name = organisationName });
        }

        public void UpdateOrganisation(Data.Organisation organisation)
        {
            _container.Organisations.Attach(organisation);
            _container.SetStateForEntity(organisation, System.Data.Entity.EntityState.Modified);
        }

        public IEnumerable<Data.Organisation> ListOrganisations()
        {
            return _container.Organisations.AsEnumerable();
        }

        public IEnumerable<Data.Organisation> FindOrganisation(Func<Data.Organisation, bool> query)
        {
            return _container.Organisations.Where(query).AsEnumerable();
        }

        public Data.Organisation FindById(int id)
        {
            return _container.Organisations.Find(id);
        }
    }
}
