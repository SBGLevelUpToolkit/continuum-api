//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Continuum.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class AssessmentItem
    {
        public int Id { get; set; }
        public int AssessmentId { get; set; }
        public int TeamMemberId { get; set; }
        public int CapabiltyId { get; set; }
        public bool CapabilityAchieved { get; set; }
    
        public virtual Capability Capabilty { get; set; }
        
    }
}
