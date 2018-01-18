using Nucleotic.Framework.Engine.Assemble;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Configuration;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Underwriting;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;

namespace Nucleotic.Module.CalculationEngine.Assemble.Pipeline
{
    public class AllRiskLoadingsCalculationPipeline : AbstractPipeline<LoadingsContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllRiskLoadingsCalculationPipeline"/> class.
        /// </summary>
        public AllRiskLoadingsCalculationPipeline()
        {
            //Configuration
            AddCommand(new LoadingsEngineConfigurationCommand());

            //Rate and Loadings
            AddCommand(new BasePremiumRateCalculationCommand());
            AddCommand(new ClaimsLoadingCalculationCommand());
            AddCommand(new AdditionalExcessWaiverCalculationCommand());
            AddCommand(new BrokerLoadingCommand());

            //Finalization
            AddCommand(new LoadedFactorPremiumCalculationCommand());
        }
    }
}
