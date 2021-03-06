﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data
{
    public interface IContinuumDataContainer
    {
        IDbSet<Dimension> Dimensions { get; set; }
        IDbSet<Capability> Capabilities { get; set; }
        IDbSet<Level> Levels { get; set; }
        IDbSet<Organisation> Organisations { get; set; }
        IDbSet<Team> Teams { get; set; }
        IDbSet<Assessment> Assessments { get; set; }
        IDbSet<Lookup> Lookups { get; set; }
        IDbSet<Goal> Goals { get; set; }
        IDbSet<AssessmentItem> AssessmentItems { get; set; }
        IDbSet<TeamMember> TeamMembers { get; set; }
        IDbSet<AssessmentResult> AssessmentResults { get; set; }
        IDbSet<CapabilityRequirement> CapabilityRequirements { get; set; }

        int SaveChanges();

        void SetStateForEntity<T>(T entity, System.Data.Entity.EntityState state) where T : class;
        void SetStateForEntityProperty<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> property,bool modified) where TEntity : class;
    }
}
