//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fingerprint
{
    using System;
    using System.Collections.Generic;
    
    public partial class Attendance
    {
        public int ID { get; set; }
        public Nullable<int> EmpID { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public string Type { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}