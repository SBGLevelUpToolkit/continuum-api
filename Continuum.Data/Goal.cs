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
    
    public partial class Goal
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public System.DateTime DueDate { get; set; }
        public bool Completed { get; set; }
        public int TeamId { get; set; }
        public int CapabiltyId { get; set; }
    
        public virtual Team Team { get; set; }
        public virtual Capability Capabilty { get; set; }
    }
}
