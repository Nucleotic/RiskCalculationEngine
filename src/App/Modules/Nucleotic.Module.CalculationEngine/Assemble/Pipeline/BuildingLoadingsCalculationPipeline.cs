using Nucleotic.Framework.Engine.Assemble;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Configuration;
using Nucleotic.Module.CalculationEngine.Assemble.Command.RiskTypes;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Underwriting;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;

namespace Nucleotic.Module.CalculationEngine.Assemble.Pipeline
{
    public class BuildingLoadingsCalculationPipeline : AbstractPipeline<LoadingsContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingLoadingsCalculationPipeline"/> class.
        /// </summary>
        public BuildingLoadingsCalculationPipeline()
        {
            //Configuration
            AddCommand(new LoadingsEngineConfigurationCommand());

            //Rate and Loadings
            AddCommand(new BasePremiumRateCalculationCommand());
            AddCommand(new ClaimsLoadingCalculationCommand());
            AddCommand(new BuildingLoadingsCalculationCommand());
            AddCommand(new BrokerLoadingCommand());

            //Finalization
            AddCommand(new LoadedFactorPremiumCalculationCommand());
        }
    }
}
