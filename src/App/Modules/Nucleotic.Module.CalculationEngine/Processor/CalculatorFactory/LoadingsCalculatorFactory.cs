using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Interfaces;
using System;
using System.Linq;

namespace Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory
{
    public class LoadingsCalculatorFactory : BaseFactory<LoadingsCalculatorFactory>, ILoadingsCalculatorFactory
    {
        /// <summary>
        /// Creates the specified calculator type.
        /// </summary>
        /// <param name="calculatorType">Type of the calculator.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Unrecognized calculator: " + key</exception>
        public ICalculator<LoadingsContext> Create(string calculatorType, LoadingsContext context)
        {
            try
            {
                string key;
                var calculator = LoadCalculatorFromAssembly(calculatorType, context, out key);
                if (calculator == null)
                {
                    throw new Exception("Unrecognized calculator: " + key);
                }
                return calculator as ICalculator<LoadingsContext>;
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage(ex.Message, ex);
                throw;
            }
        }


    }
}