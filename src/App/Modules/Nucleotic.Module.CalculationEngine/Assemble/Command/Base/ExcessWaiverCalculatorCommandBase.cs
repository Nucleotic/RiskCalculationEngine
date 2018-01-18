using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.BaseVersion.Interfaces;

namespace Nucleotic.Module.CalculationEngine.Assemble.Command.Base
{
    public abstract class ExcessWaiverCalculatorCommandBase : CalculatorCommandBase<ExcessWaiverContext>
    {
        internal static IExcessWaiverCalculator GetCalculatorFromFactory(ExcessWaiverContext assembly)
        {
            var factory = assembly.ExcessWaiversCalculatorFactory;
            var calculator = factory?.Create("ExcessWaivers", assembly) as IExcessWaiverCalculator;
            return calculator;
        }
    }
}