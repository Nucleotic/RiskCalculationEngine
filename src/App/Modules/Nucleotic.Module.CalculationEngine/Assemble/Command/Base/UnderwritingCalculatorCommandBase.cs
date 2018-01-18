using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.BaseVersion.Interfaces;

namespace Nucleotic.Module.CalculationEngine.Assemble.Command.Base
{
    public abstract class UnderwritingCalculatorCommandBase : CalculatorCommandBase<LoadingsContext>
    {
        /// <summary>
        ///     Gets the calculator from factory.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        internal static IGeneralUnderwritingLoadingsCalculator GetCalculatorFromFactory(LoadingsContext assembly)
        {
            var factory = assembly.LoadingsCalculatorFactory;
            var calculator = factory?.Create("GeneralUnderwritingLoadings", assembly) as IGeneralUnderwritingLoadingsCalculator;
            return calculator;
        }
    }
}