namespace Nucleotic.DataContracts.CalculationEngine.Model.RuleConfigurations
{
    public class MotorRuleConfiguration : IRuleConfiguration
    {
        public RiskItemRatingType RuleConfigRatingType { get; }

        public MotorRuleConfiguration()
        {
            RuleConfigRatingType = RiskItemRatingType.MotorVehicle;
        }
    }
}