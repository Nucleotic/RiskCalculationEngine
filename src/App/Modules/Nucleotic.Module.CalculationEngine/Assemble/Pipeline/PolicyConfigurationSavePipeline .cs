using Nucleotic.Framework.Engine.Assemble;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Configuration;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;

namespace Nucleotic.Module.CalculationEngine.Assemble.Pipeline
{
    public class PolicyConfigurationSavePipeline : AbstractPipeline<LoadingsContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyConfigurationSavePipeline"/> class.
        /// </summary>
        public PolicyConfigurationSavePipeline()
        {
            //Configuration
            AddCommand(new PolicyEntityConfigurationCommand());

            //Save Policy configuration
            AddCommand(new SavePolicyConfigurationCommand());
        }
    }
}