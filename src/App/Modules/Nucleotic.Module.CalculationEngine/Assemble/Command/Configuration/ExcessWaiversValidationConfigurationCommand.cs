using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.DataContracts.CalculationEngine.Repository;
using Nucleotic.Framework.Engine.Assemble;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Assemble.Helpers;
using System;
using System.Collections.Generic;

namespace Nucleotic.Module.CalculationEngine.Assemble.Command.Configuration
{
    public class ExcessWaiversValidationConfigurationCommand : AssemblorCommand<BaseContext>
    {
        private readonly IList<string> _validationTypes;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExcessWaiversValidationConfigurationCommand" /> class.
        /// </summary>
        /// <param name="validationTypes">The validation types.</param>
        public ExcessWaiversValidationConfigurationCommand(IList<string> validationTypes)
        {
            _validationTypes = validationTypes;
        }

        /// <summary>
        ///     Fires of the specified assembly into the pipeline.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public override void Assemble(BaseContext assembly)
        {
            try
            {
                //Initialise Providers and Factories
                var catalog = new ProviderCatalog {RatingsDataProvider = !string.IsNullOrEmpty(assembly.EntityConnectionString) ? new RatingsRepository(assembly.EntityConnectionString) : new RatingsRepository()};
                assembly.RatingsProviderCatalog = catalog;
                var provider = assembly.RatingsProviderCatalog.RatingsDataProvider;

                //Setup Base Configuration
                CommandHelpers.SetupBaseConfiguration(provider, assembly);

                //Setup Evaluation Criteria
                assembly.UnderwritingExclusions = provider.GetAllUnderwritingExceptionsByRuleName(assembly.Version, _validationTypes);
            }
            catch (Exception ex)
            {
                Logger.LogFatalErrorMessage(ex.Message, ex);
                assembly.Errors.Add(ex);
                throw;
            }
        }
    }
}