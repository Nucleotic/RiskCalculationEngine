//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Nucleotic.DataContracts.CalculationEngine.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class RuleTypeExtension
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RuleTypeExtension()
        {
            this.BandValues = new HashSet<BandValue>();
        }
    
        public int Id { get; set; }
        public int RuleTypeId { get; set; }
        public string Criteria { get; set; }
        public decimal CriteriaFactors { get; set; }
        public Nullable<int> RuleAttributeId { get; set; }
    
        public virtual RuleType RuleType { get; set; }
        public virtual RuleTypeAttribute RuleTypeAttribute { get; set; }
        public virtual RuleTypeExtension RuleTypeExtension1 { get; set; }
        public virtual RuleTypeExtension RuleTypeExtension2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BandValue> BandValues { get; set; }
    }
}
