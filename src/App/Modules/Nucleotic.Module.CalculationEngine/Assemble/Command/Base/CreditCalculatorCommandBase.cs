using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.BaseVersion.Interfaces;

namespace Nucleotic.Module.CalculationEngine.Assemble.Command.Base
{
    public abstract class CreditCalculatorCommandBase : CalculatorCommandBase<LoadingsContext>
    {
        internal static ICreditRelatedLoadingsCalculator GetCalculatorFromFactory(LoadingsContext assembly)
        {
            var factory = assembly.LoadingsCalculatorFactory;
            var calculator = factory?.Create("CreditRelated", assembly) as ICreditRelatedLoadingsCalculator;
            return calculator;
        }
    }
}