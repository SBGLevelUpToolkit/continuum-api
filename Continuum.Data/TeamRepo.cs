using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Continuum.Data
{
    public class TeamRepo : IRepository
    {
        private readonly Data.IContinuumDataContainer _container;

        public TeamRepo()
        {
            _container = new ContinuumDataContainer();
        }

        public TeamRepo(IContinuumDataContainer container)
        {
            _container = container;
        }

        public IEnumerable<Team> All()
        {
            return _container.Teams.Include(i=>i.AvatarType).AsEnumerable();
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

        public AvatarType GetAvatar(string name)
        {
            var avatar = _container.Lookups.OfType<Data.AvatarType>().Where(i => i.Value == name).FirstOrDefault();
            return avatar;
        }

        public AvatarType GetDefaultAvatar()
        {
            return _container.Lookups.OfType<Data.AvatarType>().FirstOrDefault();
        }

        public IEnumerable<Data.AvatarType> ListAvatars()
        {
            return _container.Lookups.OfType<Data.AvatarType>().AsEnumerable();
        }

        public void DeleteTeam(int id)
        {
            var team = _container.Teams.Find(id);
            RemoveTeamMembers(team);

            _container.Teams.Remove(team);
            RemoveAssessments(id);
            RemoveGoals(id);
        }

        private void RemoveGoals(int id)
        {
            var goals = _container.Goals.Where(i => i.TeamId == id);
            foreach (var goal in goals)
            {
                _container.Goals.Remove(goal);
            }
        }

        private void RemoveAssessments(int id)
        {
            var assessments = _container.Assessments.Where(i => i.TeamId == id).ToArray();

            foreach (var assessment in assessments)
            {
                RemoveAssessmentResult(assessment);
                RemoveAssessmentItems(assessment);

                _container.Assessments.Remove(assessment);
            }
        }

        private void RemoveAssessmentItems(Assessment assessment)
        {
            var assessmentItems = _container.AssessmentItems.Where(i => i.AssessmentId == assessment.Id);
            foreach (var assessmentItem in assessmentItems)
            {
                _container.AssessmentItems.Remove(assessmentItem);
            }
        }

        private void RemoveAssessmentResult(Assessment assessment)
        {
            var assessmentResults = _container.AssessmentResults.Where(i => i.AssessmentId == assessment.Id);
            foreach (var assessmentResult in assessmentResults)
            {
                _container.AssessmentResults.Remove(assessmentResult);
            }
        }

        private void RemoveTeamMembers(Team team)
        {
            var teamMembers = team.TeamMembers.ToArray();

            foreach (var teamMember in teamMembers)
            {
                _container.TeamMembers.Remove(teamMember);
            }
        }

        public bool TeamMemberExists(int teamMemberId)
        {
            return _container.TeamMembers.Any(i => i.Id == teamMemberId);
        }

        public void DeleteTeamMember(int teamMemberId)
        {
            var teamMember = _container.TeamMembers.Find(teamMemberId);

            var assessmentItems = _container.AssessmentItems.Where(i => i.TeamMemberId == teamMember.Id).ToArray();

            foreach (var assessmentItem in assessmentItems)
            {
                _container.AssessmentItems.Remove(assessmentItem);
            }

            _container.TeamMembers.Remove(teamMember);

        }
    }
}
