using System.Collections;

namespace Nucleotic.Framework.Engine.CommandChain.Interfaces
{
    /// <summary>
	/// IContext represents information that should be processed by ICommand or IChain.
	/// Consider using Facade pattern for your particular IContext to get typesafe access to IContext data.
	/// </summary>
    public interface IContext : IDictionary
    {
    }
}