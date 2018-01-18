using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.DataContracts.CalculationEngine.Repository;
using Nucleotic.Framework.Engine.Assemble;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Assemble.Helpers;
using System;

namespace Nucleotic.Module.CalculationEngine.Assemble.Command.Configuration
{
    public class PolicyEntityConfigurationCommand : AssemblorCommand<LoadingsContext>
    {
        /// <summary>
        ///     Fires of the specified assembly into the pipeline.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Assemble(LoadingsContext assembly)
        {
            try
            {
                //Initialise Providers and Factories
                var catalog = new ProviderCatalog {RatingsDataProvider = !string.IsNullOrEmpty(assembly.EntityConnectionString) ? new RatingsRepository(assembly.EntityConnectionString) : new RatingsRepository()};
                assembly.RatingsProviderCatalog = catalog;
                var provider = assembly.RatingsProviderCatalog.RatingsDataProvider;

                //Setup Base Configuration
                CommandHelpers.SetupBaseConfiguration(provider, assembly);
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