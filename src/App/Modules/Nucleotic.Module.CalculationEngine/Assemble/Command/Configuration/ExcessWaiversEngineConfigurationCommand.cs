using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.DataContracts.CalculationEngine.Repository;
using Nucleotic.Framework.Engine.Assemble;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Assemble.Helpers;
using Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory;
using System;

namespace Nucleotic.Module.CalculationEngine.Assemble.Command.Configuration
{
    public class ExcessWaiversEngineConfigurationCommand : AssemblorCommand<ExcessWaiverContext>
    {
        /// <summary>
        ///     Fires of the specified assembly into the pipeline.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public override void Assemble(ExcessWaiverContext assembly)
        {
            try
            {
                //Initialize Providers and Factories
                var catalog = new ProviderCatalog {RatingsDataProvider = !string.IsNullOrEmpty(assembly.EntityConnectionString) ? new RatingsRepository(assembly.EntityConnectionString) : new RatingsRepository()};
                assembly.RatingsProviderCatalog = catalog;
                assembly.ExcessWaiversCalculatorFactory = RatingsCalculatorFactoryInitialiser.CreateExcessWaiversCalculatorFactory("ExcessWaivers");
                var provider = assembly.RatingsProviderCatalog.RatingsDataProvider;

                //Setup Base Configuration
                CommandHelpers.SetupBaseConfiguration(provider, assembly);

                //Setup Evaluation Criteria
                assembly.BandValues = CommandHelpers.SetupBandValuesRules(provider.GetRuleTypesByCriteriaType(assembly.Version, DataRuleTypes.Banding, assembly.EngineType));
                assembly.ExtendedBandValues = CommandHelpers.SetupExtendedBandValuesRules(provider.GetRuleTypesByCriteriaType(assembly.Version, DataRuleTypes.BandingExtended, assembly.EngineType));
                assembly.Attributes = CommandHelpers.SetupAttributesRules(provider.GetRuleTypesByCriteriaType(assembly.Version, DataRuleTypes.Attributes, assembly.EngineType));
                assembly.ExtendedAttributes = CommandHelpers.SetupExtendedAttributesRules(provider.GetRuleTypesByCriteriaType(assembly.Version, DataRuleTypes.AttributesExtended, assembly.EngineType));
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