using System;
using System.Linq;
using Nucleotic.Framework.Logging;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;

namespace Nucleotic.Module.CalculationEngine.Processor.ExtensionFactory
{
    public abstract class BaseFactory<T> where T : class, IExtensionFactory
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
    }
}