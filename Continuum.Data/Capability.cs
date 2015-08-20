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
    
    public partial class Capability
    {
        public Capability()
        {
            this.CapabilityRequirements = new HashSet<CapabilityRequirement>();
        }
    
        public int Id { get; set; }
        public string Description { get; set; }
        public int LevelId { get; set; }
        public int DimensionId { get; set; }
        public bool Active { get; set; }
        public int DisplayOrder { get; set; }
        public int CapabilityId { get; set; }
    
        public virtual Level Level { get; set; }
        public virtual Dimension Dimension { get; set; }
        public virtual ICollection<CapabilityRequirement> CapabilityRequirements { get; set; }
    }
}