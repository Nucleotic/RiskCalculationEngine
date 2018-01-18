using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Interfaces;

namespace Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory
{
    public interface ILoadingsCalculatorFactory : IRatingsCalculatorFactory
    {
        /// <summary>
        /// Creates the specified calculator type.
        /// </summary>
        /// <param name="calculatorType">Type of the calculator.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        ICalculator<LoadingsContext> Create(string calculatorType, LoadingsContext context);
    }
}