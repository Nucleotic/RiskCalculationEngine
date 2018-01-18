using Nucleotic.DataContracts.CalculationEngine.Repository;

namespace Nucleotic.DataContracts.CalculationEngine.Model
{
    public class ProviderCatalog
    {
        public IRatingsRepository RatingsDataProvider { get; set; }
    }
}