using System;
using System.Linq;
using Nucleotic.Framework.Logging;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;

namespace Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory
{
    public abstract class BaseFactory<T> where T : class, IRatingsCalculatorFactory
    {
        /// <summary>
        /// The logger
        /// </summary>
        internal Logger<T> Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFactory{T}"/> class.
        /// </summary>
        protected BaseFactory()
        {
            Logger = new Logger<T>();
        }

        /// <summary>
        /// Loads the calculator from assembly.
        /// </summary>
        /// <param name="calculatorType">Type of the calculator.</param>
        /// <param name="context">The context.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="System.MissingMethodException">No public constructor defined for this object</exception>
        internal static object LoadCalculatorFromAssembly(string calculatorType, BaseContext context, out string key)
        {
            var resource = $"Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.V{context.Version}.Common.{calculatorType}Calculator";
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            key = resource;
            object calculator = null;
            foreach (var assembly in assemblies)
            {
                if (!assembly.FullName.StartsWith("Nucleotic.Module.CalculationEngine") ||
                    assembly.FullName.Contains("Test")) continue;

                var type = assembly.GetType(resource);
                if (type == null) continue;
                var constructor = type.GetConstructors().OrderBy(c => c.GetParameters().Length).FirstOrDefault();
                if (constructor == null)
                    throw new MissingMethodException("No public constructor defined for this object");
                calculator = constructor.Invoke(new object[] { context });
                break;
            }

            return calculator;
        }
    }
}