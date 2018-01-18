using Nucleotic.Framework.Logging;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory
{
    internal static class RatingsCalculatorFactoryInitialiser
    {
        /// <summary>
        /// Creates the motor ratings calculator factory.
        /// </summary>
        /// <param name="factoryType">Type of the factory.</param>
        /// <returns></returns>
        public static ILoadingsCalculatorFactory CreateLoadingsCalculatorFactory(string factoryType)
        {
            var factory = CreateCalculatorFactory<ILoadingsCalculatorFactory>(factoryType);
            return factory;
        }

        /// <summary>
        /// Creates the excess waivers calculator factory.
        /// </summary>
        /// <param name="factoryType">Type of the factory.</param>
        /// <returns></returns>
        public static IExcessWaiverCalculatorFactory CreateExcessWaiversCalculatorFactory(string factoryType)
        {
            var factory = CreateCalculatorFactory<IExcessWaiverCalculatorFactory>(factoryType);
            return factory;
        }

        /// <summary>
        /// Creates the calculator factory.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factoryType">Type of the factory.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        private static T CreateCalculatorFactory<T>(string factoryType) where T : class
        {
            try
            {
                string key;
                var factory = LoadFactoryFromAssembly(factoryType, out key);
                if (factory == null)
                {
                    throw new Exception($"Unrecognised calculator factory with key: {key}");
                }
                return (T)factory;
            }
            catch (Exception ex)
            {
                new Logger<T>().LogErrorMessage(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Loads the factory from assembly.
        /// </summary>
        /// <param name="factoryType">Type of the factory.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private static object LoadFactoryFromAssembly(string factoryType, out string key)
        {
            var resource = "Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory." + factoryType + "CalculatorFactory";
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            key = resource;
            return (from assembly in assemblies
                    where assembly.FullName.StartsWith("Nucleotic.Module.CalculationEngine") && !assembly.FullName.Contains("Test")
                    select assembly.CreateInstance(resource, true, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, new object[] {},
                        CultureInfo.InvariantCulture, null)).FirstOrDefault();
        }
    }
}