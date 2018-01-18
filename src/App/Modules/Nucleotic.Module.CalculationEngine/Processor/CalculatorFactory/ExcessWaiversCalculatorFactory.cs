using System;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Interfaces;

namespace Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory
{
    public class ExcessWaiversCalculatorFactory : BaseFactory<ExcessWaiversCalculatorFactory>, IExcessWaiverCalculatorFactory
    {
        /// <summary>
        /// Creates the specified calculator type.
        /// </summary>
        /// <param name="calculatorType">Type of the calculator.</param>
        /// <param name="waiverContext">The waiverContext.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Unrecognized calculator: " + key</exception>
        public ICalculator<ExcessWaiverContext> Create(string calculatorType, ExcessWaiverContext waiverContext)
        {
            try
            {
                string key;
                var calculator = LoadCalculatorFromAssembly(calculatorType, waiverContext, out key);
                if (calculator == null)
                {
                    throw new Exception("Unrecognized calculator: " + key);
                }
                return calculator as ICalculator<ExcessWaiverContext>;
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage(ex.Message, ex);
                throw;
            }
        }
    }
}
