using System;
using System.Collections.Generic;
using Nucleotic.DataContracts.CalculationEngine;
using Nucleotic.DataContracts.CalculationEngine.Model;
using Nucleotic.DataContracts.CalculationEngine.Repository;
using Nucleotic.Framework.Engine.Assemble;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Assemble.Helpers;

namespace Nucleotic.Module.CalculationEngine.Assemble.Command.Configuration
{
    public class AttributeValidationConfigurationCommand : AssemblorCommand<LoadingsContext>
    {
        /// <summary>
        /// Fires of the specified assembly into the pipeline.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public override void Assemble(LoadingsContext assembly)
        {
            try
            {
                //Initialise Providers and Factories
                var catalog = new ProviderCatalog { RatingsDataProvider = !string.IsNullOrEmpty(assembly.EntityConnectionString)
                    ? new RatingsRepository(assembly.EntityConnectionString)
                    : new RatingsRepository() };
                assembly.RatingsProviderCatalog = catalog;
                var provider = assembly.RatingsProviderCatalog.RatingsDataProvider;

                //Setup Base Configuration
                CommandHelpers.SetupBaseConfiguration(provider, assembly);

                //Setup Evaluation Criteria
                var criteria = new List<string>();
                switch (assembly.FlatRateCalculationType)
                {
                    case FlatRateCalculationTypes.None:
                    case FlatRateCalculationTypes.GoodsInTransit:
                    case FlatRateCalculationTypes.CoverAdditions:
                        break;
                    case FlatRateCalculationTypes.Fleet:
                        assembly.UnderwritingExclusions = provider.GetAllUnderwritingExceptionsByRuleAttribute(assembly.Version,
                            new List<string> { assembly.RiskItemCoverType.ToString() });
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
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
