using System.Collections.Generic;
using Nucleotic.Framework.Engine.Assemble;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Configuration;
using Nucleotic.Module.CalculationEngine.Assemble.Command.Underwriting;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;

namespace Nucleotic.Module.CalculationEngine.Assemble.Pipeline
{
    public class WaiversValidationPipeline : AbstractPipeline<ExcessWaiverContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WaiversValidationPipeline"/> class.
        /// </summary>
        public WaiversValidationPipeline()
        {
            //Configuration
            AddCommand(new ExcessWaiversValidationConfigurationCommand(new List<string> { "FullLossExcessWaiver" }));

            //Validation
            AddCommand(new ValidateWaiverExclusionsCommand());
        }
    }
}