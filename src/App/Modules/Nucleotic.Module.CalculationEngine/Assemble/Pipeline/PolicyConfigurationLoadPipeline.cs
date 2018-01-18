using Nucleotic.Framework.Engine.Assemble;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Configuration;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;

namespace Nucleotic.Module.CalculationEngine.Assemble.Pipeline
{
    public class PolicyConfigurationLoadPipeline : AbstractPipeline<LoadingsContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyConfigurationLoadPipeline"/> class.
        /// </summary>
        public PolicyConfigurationLoadPipeline()
        {
            //Configuration
            AddCommand(new PolicyEntityConfigurationCommand());

            //Load Policy configuration
            AddCommand(new LoadPolicyConfigurationCommand());
        }
    }
}