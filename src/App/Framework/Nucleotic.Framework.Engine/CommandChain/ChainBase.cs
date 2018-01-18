using Nucleotic.Framework.Engine.CommandChain.Interfaces;
using Nucleotic.Framework.Logging;
using System;
using System.Collections;

namespace Nucleotic.Framework.Engine.CommandChain
{
    /// <summary>
    /// Base class for IChain implementations.
    /// </summary>
    public class ChainBase : IChain
    {
        /// <summary>
        /// List of commands that belong to this IChain.
        /// </summary>
        protected IList CommandsList = new ArrayList();

        /// <summary>
        /// The current executing chain is frozen
        /// </summary>
        protected bool IsFrozen;

        /// <summary>
        /// Returns list of commands that are in IChain.
        /// </summary>
        public IList Commands => CommandsList;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainBase"/> class.
        /// </summary>
        public ChainBase() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainBase"/> class.
        /// </summary>
        /// <param name="commmand">The commmand.</param>
        /// <exception cref="System.ArgumentNullException">commmand - Cannot initialise chain with passed command: Command is null.</exception>
        public ChainBase(ICommand commmand)
        {
            if (commmand == null)
            {
                throw new ArgumentNullException(nameof(commmand), "Cannot initialise chain with passed command: Command is null.");
            }
            AddCommand(commmand);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainBase"/> class.
        /// </summary>
        /// <param name="commands">The commands.</param>
        public ChainBase(IList commands)
        {
            AddCommands(commands);
        }

        /// <summary>
        /// Adds the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <exception cref="System.ArgumentNullException">command - Cannot add command: Command is null.</exception>
        /// <exception cref="System.InvalidOperationException">Cannot add command: Chain is frozen.</exception>
        public void AddCommand(ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command), "Cannot add command: Command is null.");
            if (IsFrozen)
                throw new InvalidOperationException("Cannot add command: Chain is frozen.");
            CommandsList.Add(command);
        }

        /// <summary>
        /// Adds the commands.
        /// </summary>
        /// <param name="commands">The commands.</param>
        /// <exception cref="System.ArgumentNullException">commands - Cannot add commands: Commands list is null.</exception>
        public void AddCommands(IList commands)
        {
            if (commands == null)
                throw new ArgumentNullException(nameof(commands), "Cannot add commands: Commands list is null.");
            IEnumerator iterator = commands.GetEnumerator();
            while (iterator.MoveNext())
            {
                AddCommand((ICommand)iterator.Current);
            }
        }

        /// <summary>
        /// Removes the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <exception cref="System.ArgumentNullException">command - Cannot remove command: Command is null.</exception>
        /// <exception cref="System.InvalidOperationException">
        /// Cannot remove command: Chain is frozen.
        /// or
        /// Cannot remove command: Chain is empty.
        /// or
        /// Cannot remove command: Chain dosn't contain specified command.
        /// </exception>
        public void RemoveCommand(ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command), "Cannot remove command: Command is null.");
            if (this.IsFrozen)
                throw new InvalidOperationException("Cannot remove command: Chain is frozen.");
            if (CommandsList.Count == 0)
                throw new InvalidOperationException("Cannot remove command: Chain is empty.");
            if (!CommandsList.Contains((object)command))
                throw new InvalidOperationException("Cannot remove command: Chain dosn't contain specified command.");
            CommandsList.Remove(command);
        }

        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">context - Context is null.</exception>
        public bool Execute(IContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context), "Context is null.");
            IsFrozen = true;
            var flag1 = false;
            var exception = (Exception)null;
            var count = CommandsList.Count;
            int primaryIndex;
            for (primaryIndex = 0; primaryIndex < count; ++primaryIndex)
            {
                try
                {
                    flag1 = ((ICommand)CommandsList[primaryIndex]).Execute(context);
                    if (flag1)
                        break;
                }
                catch (Exception ex)
                {
                    exception = ex;
                    break;
                }
            }
            if (primaryIndex == count)
                --primaryIndex;
            var flag2 = false;
            for (var secondaryIndex = primaryIndex; secondaryIndex >= 0; --secondaryIndex)
            {
                var filter = CommandsList[secondaryIndex] as IFilter;
                if (filter != null)
                {
                    try
                    {
                        if (filter.PostProcess(context, exception))
                            flag2 = true;
                    }
                    catch (Exception ex)
                    {
                        new Logger<ChainBase>().LogErrorMessage($"Error executing Chain Command from Context: {context.GetType().FullName}", ex);
                    }
                }
            }
            if (exception != null && !flag2) throw exception;
            return flag1;
        }
    }
}