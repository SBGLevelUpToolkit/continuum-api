using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data
{
    public class TeamRepository : ITeamRepo
    {
        private readonly Data.IContinuumDataContainer _container;

        public TeamRepository()
        {
            _container = new ContinuumDataContainer();
        }

        public TeamRepository(IContinuumDataContainer container)
        {
            _container = container;
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
        }

        public Team FindById(int id)
        {
            return _container.Teams.Find(id);
        }


        public void SaveChanges()
        {
            _container.SaveChanges();
        }

        public IEnumerable<Team> GetTeamForUser(string userId)
        {
            return _container.TeamMembers.Where(i => i.UserId == userId).Select(j => j.Team).AsEnumerable();
        }

        public bool IsUserTeamAdmin(Team team, string userId)
        {
            return _container.TeamMembers.Any(i => i.TeamId == team.Id && i.IsAdmin);
        }

        public AvatarType GetAvatar(int id)
        {
            var avatar = _container.Lookups.OfType<Data.AvatarType>().Where(i => i.Id == id).FirstOrDefault();
            return avatar;
        }

        public AvatarType GetDefaultAvatar()
        {
            return _container.Lookups.OfType<Data.AvatarType>().FirstOrDefault();
        }
    }
}
