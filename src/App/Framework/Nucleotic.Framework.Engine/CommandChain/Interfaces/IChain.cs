using System.Collections;

namespace Nucleotic.Framework.Engine.CommandChain.Interfaces
{
    /// <summary>
    /// IChain is ordered list of commands. It receives particular IContext  that should be processed. IContext is processed by passing it to
    /// commands available in IChain(starting from first ICommand in IChain to the last one). IContext processing is performed until one of the
    /// IChain's commands returns true, one of the IChain's commands throws an exception or end of the IChain's commands list is reached.
    /// </summary>
    /// <seealso cref="ICommand" />
    public interface IChain : ICommand
    {
        /// <summary>
        /// Adds the command.
        /// </summary>
        /// <param name="command">The command.</param>
        void AddCommand(ICommand command);

        /// <summary>
        /// Adds the commands.
        /// </summary>
        /// <param name="commands">The commands.</param>
        void AddCommands(IList commands);
    }
}