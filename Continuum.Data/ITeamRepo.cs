using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data
{
    public interface ITeamRepo : IRepository<Team>
    {
        IEnumerable<Data.Team> GetTeamForUser(string userId);

        bool IsUserTeamAdmin(Team team, string userId);

        AvatarType GetAvatar(int p);

        AvatarType GetDefaultAvatar();

      
    }
}
