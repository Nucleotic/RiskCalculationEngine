using Nucleotic.Framework.Engine.Assemble;
using Nucleotic.Module.CalculationEngine.Assemble.Contexts;
using Nucleotic.Module.CalculationEngine.Interfaces;
using System;

namespace Nucleotic.Module.CalculationEngine.Assemble.Command.Base
{
    public abstract class RepositoryCommandBase<T> : AssemblorCommand<T>, IActivity where T : LoadingsContext
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RepositoryCommandBase{T}" /> class.
        /// </summary>
        protected RepositoryCommandBase()
        {
            Key = Guid.NewGuid();
        }

        /// <summary>
        ///     Gets the key.
        /// </summary>
        /// <value>
        ///     The key.
        /// </value>
        public Guid Key { get; }
    }
}