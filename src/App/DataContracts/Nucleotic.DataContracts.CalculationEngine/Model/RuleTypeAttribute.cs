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
    
    public partial class RuleTypeAttribute
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RuleTypeAttribute()
        {
            this.BandValues = new HashSet<BandValue>();
            this.RuleTypeExtensions = new HashSet<RuleTypeExtension>();
            this.UnderwritingExclusions = new HashSet<UnderwritingExclusion>();
        }
    
        public int Id { get; set; }
        public int RuleTypeId { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime EffectiveFrom { get; set; }
        public Nullable<System.DateTime> EffectiveTo { get; set; }
        public string AttributeKey { get; set; }
        public string AttributeValue { get; set; }
        public int Version { get; set; }
    
        public virtual RuleType RuleType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BandValue> BandValues { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RuleTypeExtension> RuleTypeExtensions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnderwritingExclusion> UnderwritingExclusions { get; set; }
    }
}
