using Nucleotic.Framework.Engine.Assemble;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Configuration;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Underwriting;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;

namespace Nucleotic.Module.CalculationEngine.Assemble.Pipeline
{
    public class LoadingsValidationPipeline : AbstractPipeline<LoadingsContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadingsValidationPipeline"/> class.
        /// </summary>
        public LoadingsValidationPipeline()
        {
            //Configuration
            AddCommand(new PolicyValidationConfigurationCommand());

            //Validation
            AddCommand(new ValidateLoadingExclusionsCommand());
        }
    }
}