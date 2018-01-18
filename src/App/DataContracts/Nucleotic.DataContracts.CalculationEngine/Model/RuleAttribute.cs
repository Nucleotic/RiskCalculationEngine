namespace Nucleotic.DataContracts.CalculationEngine.Model
{
    public class RuleAttribute
    {
        public string AttributeName { get; set; }

        public string AttributeKey { get; set; }

        public string AttributeValue { get; set; }

        public CalculationTypes AttributeType { get; set; }
    }
}