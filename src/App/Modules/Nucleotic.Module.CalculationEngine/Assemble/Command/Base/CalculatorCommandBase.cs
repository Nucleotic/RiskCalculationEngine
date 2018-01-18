using Nucleotic.Framework.Engine.Assemble;
using Nucleotic.Module.CalculationEngine.Interfaces;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Nucleotic.Module.CalculationEngine.Assemble.Command.Base
{
    public abstract class CalculatorCommandBase<T> : AssemblorCommand<T>, IActivity where T : AssemblorContext
    {
        /// <summary>
        ///     The function set used for this specific set of calculations
        /// </summary>
        public dynamic FunctionSet;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CalculatorCommandBase{T}" /> class.
        /// </summary>
        protected CalculatorCommandBase()
        {
            Key = Guid.NewGuid();
        }

        public Guid Key { get; }

        /// <summary>
        ///     Gets the calculator function set.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="setName">Name of the set.</param>
        internal void GetCalculatorFunctionSet(int version, string setName)
        {
            try
            {
                var resource = $"Nucleotic.Module.CalculationEngine.Processor.CalculatorFactory.V{version}.Calculation.{setName}FunctionSet";
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                FunctionSet = (from assembly in assemblies
                    where
                    assembly.FullName.StartsWith("Nucleotic.Module.CalculationEngine") &&
                    !assembly.FullName.Contains("Test")
                    select
                    assembly.CreateInstance(resource, true,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null,
                        new object[] { }, CultureInfo.InvariantCulture, null)).FirstOrDefault();
                if (FunctionSet == null)
                    throw new Exception("Unrecognized calculator functionset: " + resource);
            }
            catch (Exception ex)
            {
                Logger.LogErrorMessage(ex.Message, ex);
            }
        }
    }
}