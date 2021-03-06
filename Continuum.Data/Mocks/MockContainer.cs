﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data.Mocks
{
    public class MockContainer : IContinuumDataContainer
    {
        public bool ChangesSaved { get; private set; }

        public MockContainer()
        {
            Assessments = new Mocks.MockAssessmentDBSet();
            Lookups = new Mocks.MockLookupDBSet();
            Teams = new Mocks.MockTeamDBSet();
            Organisations = new Mocks.MockOrganisationDBSet();
            TeamMembers = new Mocks.MockTeamMembersDBSet();
            Goals = new Mocks.MockGoalsDBSet();
            Capabilities = new Mocks.MockCapabilityDBSet();
            AssessmentItems = new Mocks.MockAssessmentItemDBSet();
        }

        public System.Data.Entity.IDbSet<Dimension> Dimensions { get; set; }
        public System.Data.Entity.IDbSet<Capability> Capabilities { get; set; }
        public System.Data.Entity.IDbSet<Level> Levels { get; set; }
        public System.Data.Entity.IDbSet<Organisation> Organisations { get; set; }
        public System.Data.Entity.IDbSet<Team> Teams { get; set; }
        public System.Data.Entity.IDbSet<Assessment> Assessments { get; set; }
        public System.Data.Entity.IDbSet<Lookup> Lookups { get; set; }
        public System.Data.Entity.IDbSet<Goal> Goals { get; set; }
        public System.Data.Entity.IDbSet<AssessmentItem> AssessmentItems { get; set; }
        public System.Data.Entity.IDbSet<TeamMember> TeamMembers { get; set; }
        public System.Data.Entity.IDbSet<AssessmentResult> AssessmentResults { get; set; }
        public System.Data.Entity.IDbSet<CapabilityRequirement> CapabilityRequirements { get; set; }

        public int SaveChanges()
        {
            ChangesSaved = true;
            return 0; 
        }

        public virtual void SetStateForEntity<T>(T entity, System.Data.Entity.EntityState state) where T : class
        {
            if (state == System.Data.Entity.EntityState.Modified)
            {
                if (entity is Data.Goal)
                {
                    var goal = entity as Data.Goal;
                    if (this.Goals.Any(i => i.Id == goal.Id))
                    {
                        var currentGoal = this.Goals.First(i => i.Id == goal.Id);
                        this.Goals.Remove(currentGoal);
                        this.Goals.Add(goal);
                    }
                    else
                    {
                        throw new ApplicationException("Entity not found.");
                    }
                }
            }
        }


        public void SetStateForEntityProperty<TEntity, TProperty>(TEntity entity, System.Linq.Expressions.Expression<Func<TEntity, TProperty>> property, bool modified) where TEntity : class
        {
        }
    }
}
