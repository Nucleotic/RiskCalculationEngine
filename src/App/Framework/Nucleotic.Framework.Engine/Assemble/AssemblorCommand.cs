using Nucleotic.Framework.Engine.CommandChain;
using Nucleotic.Framework.Engine.CommandChain.Interfaces;
using Nucleotic.Framework.Logging;
using System;

namespace Nucleotic.Framework.Engine.Assemble
{
    public abstract class AssemblorCommand<T> : CommandBase, ICommand, IAssemblorCommand<T> where T : AssemblorContext
    {
        public readonly Logger<T> Logger = new Logger<T>();

        /// <summary>
        /// Fires of the specified assembly into the pipeline.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public abstract void Assemble(T assembly);

        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public bool Execute(IContext context)
        {
            Log(LogLevel.Debug, "Applying assemblor command : " + this.GetType().Name);
            try
            {
                this.Assemble(context as T);
            }
            catch (Exception ex)
            {
                Log("Error executing command", ex);
                throw;
            }
            return false;
        }

        /// <summary>
        /// Logs the specified level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="message">The message.</param>
        protected virtual void Log(LogLevel level, string message)
        {
            Logger.LogMessage(level, message);
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        protected virtual void Log(string message, Exception ex)
        {
            Logger.LogErrorMessage(message, ex);
        }
    }
}