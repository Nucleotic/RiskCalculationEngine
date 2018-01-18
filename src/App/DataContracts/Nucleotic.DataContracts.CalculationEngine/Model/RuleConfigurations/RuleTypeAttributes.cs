using System;

namespace Nucleotic.DataContracts.CalculationEngine.Model.RuleConfigurations
{
    public class RuleTypeAttributes
    {
        public int Id { get; set; }

        public int RuleTypeId { get; set; }

        public int Version { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public string AttributeKey { get; set; }

        public string AttributeValue { get; set; }
    }
}