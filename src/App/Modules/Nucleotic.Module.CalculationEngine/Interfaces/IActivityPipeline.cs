using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nucleotic.Module.CalculationEngine.Interfaces
{
    public interface IActivityPipeline
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        Guid Key { get; }
    }
}
