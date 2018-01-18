using Nucleotic.Framework.Engine.Assemble;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Configuration;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Underwriting;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;

namespace Nucleotic.Module.CalculationEngine.Assemble.Pipeline
{
    public class FlatRateLoadingsCalculationPipeline : AbstractPipeline<LoadingsContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlatRateLoadingsCalculationPipeline"/> class.
        /// </summary>
        public FlatRateLoadingsCalculationPipeline()
        {
            //Configuration
            AddCommand(new LoadingsEngineConfigurationCommand());

            //Rate and Loadings
            AddCommand(new FlatRateLoadingsCalculationCommand());
        }
    }
}
