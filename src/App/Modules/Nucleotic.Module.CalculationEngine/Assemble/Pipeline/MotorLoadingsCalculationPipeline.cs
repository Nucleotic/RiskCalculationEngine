using Nucleotic.Framework.Engine.Assemble;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Configuration;
using Nucleotic.Module.CalculationEngine.Assemble.Command.RiskTypes;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Underwriting;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;

namespace Nucleotic.Module.CalculationEngine.Assemble.Pipeline
{
    public class MotorLoadingsCalculationPipeline : AbstractPipeline<LoadingsContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MotorLoadingsCalculationPipeline"/> class.
        /// </summary>
        public MotorLoadingsCalculationPipeline()
        {
            //Configuration
            AddCommand(new LoadingsEngineConfigurationCommand());

            //Rate and Loadings
            AddCommand(new BasePremiumRateCalculationCommand());
            AddCommand(new AgeBandLoadingCalculationCommand());
            AddCommand(new ClaimsLoadingCalculationCommand());
            AddCommand(new MotorLoadingsCalculationCommand());
            AddCommand(new CoverTypeLoadingCommand());
            AddCommand(new CreditShortfallRateCommand());
            AddCommand(new BrokerLoadingCommand());
            AddCommand(new ExcessBandLoadingCalculationCommand());

            //Finalization
            AddCommand(new LoadedFactorPremiumCalculationCommand());
        }
    }
}