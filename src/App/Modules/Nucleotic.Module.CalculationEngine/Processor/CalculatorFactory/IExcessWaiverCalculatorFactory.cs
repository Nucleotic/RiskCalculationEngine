using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Interfaces;

namespace Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory
{
    public interface IExcessWaiverCalculatorFactory : IRatingsCalculatorFactory
    {
        /// <summary>
        /// Creates the specified calculator type.
        /// </summary>
        /// <param name="calculatorType">Type of the calculator.</param>
        /// <param name="waiverContext">The waiver context.</param>
        /// <returns></returns>
        ICalculator<ExcessWaiverContext> Create(string calculatorType, ExcessWaiverContext waiverContext);
    }
}
