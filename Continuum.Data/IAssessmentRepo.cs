﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Continuum.Data
{
    public interface IAssessmentRepo : IRepository<Data.Assessment>
    {
        Data.Assessment GetCurrentAssessmentForTeam(int teamId);

        void UpdateCapabilityForAssessment(Assessment assessment, IEnumerable<AssessmentItem> assessmentItems);

        void StartModeration(Assessment assessment);

        void CloseAssessment(Assessment assessment);
    }
}
