//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Grid02.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Mapped_Floor
    {
        public int ID { get; set; }
        public Nullable<int> Row { get; set; }
        public Nullable<int> Col { get; set; }
        public string Room { get; set; }
        public Nullable<int> Nearest_stairX { get; set; }
        public Nullable<int> Nearest_stairY { get; set; }
        public string Floor { get; set; }
        public Nullable<int> Nearest_EmergencyStairX { get; set; }
        public Nullable<int> Nearest_EmergencyStairY { get; set; }
    }
}