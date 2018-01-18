using Nucleotic.Framework.Engine.Assemble;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Configuration;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Underwriting;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;

namespace Nucleotic.Module.CalculationEngine.Assemble.Pipeline
{
    public class FlatRateLoadingsValidationPipeline : AbstractPipeline<LoadingsContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadingsValidationPipeline"/> class.
        /// </summary>
        public FlatRateLoadingsValidationPipeline()
        {
            //Configuration
            AddCommand(new AttributeValidationConfigurationCommand());

            //Validation
            AddCommand(new ValidateLoadingExclusionsCommand());
        }
    }
}