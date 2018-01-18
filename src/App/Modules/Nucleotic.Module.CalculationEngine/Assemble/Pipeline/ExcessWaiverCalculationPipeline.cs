using Nucleotic.Framework.Engine.Assemble;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Configuration;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Underwriting;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;

namespace Nucleotic.Module.CalculationEngine.Assemble.Pipeline
{
    public class ExcessWaiverCalculationPipeline : AbstractPipeline<ExcessWaiverContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcessWaiverCalculationPipeline"/> class.
        /// </summary>
        public ExcessWaiverCalculationPipeline()
        {
            //Configuration
            AddCommand(new ExcessWaiversEngineConfigurationCommand());

            //Rate and Loadings
            AddCommand(new ExcessWaiversCalculationCommand());
        }
    }
}
