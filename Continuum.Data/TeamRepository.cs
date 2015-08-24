using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data
{
    public class TeamRepository : IRepository<Data.Team>
    {
        private readonly Data.ContinuumDataContainer _container;

        public TeamRepository()
        {
            _container = new ContinuumDataContainer();
        }

        public IEnumerable<Team> All()
        {
            return _container.Teams.AsEnumerable();
        }

        public void Create(Team item)
        {
            var organisation = _container.Organisations.Where(i => i.Name == "Standard Bank").FirstOrDefault();
            if(organisation == null)
            {
                organisation = new Organisation() { Name = "Standard Bank" };
                _container.Organisations.Add(organisation);
            }

            item.Organisation = organisation;
            item.AvatarTypeId = 1; //Barbarian

            _container.Teams.Add(item);
            _container.SaveChanges();
        }

        public Team FindById(int id)
        {
            return _container.Teams.Find(id);
        }


        public void SaveChanges()
        {
            _container.SaveChanges();
        }
    }
}
