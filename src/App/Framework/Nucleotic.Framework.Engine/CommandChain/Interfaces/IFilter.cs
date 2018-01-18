using System;

namespace Nucleotic.Framework.Engine.CommandChain.Interfaces
{
    /// <summary>
    /// IFilter is type of ICommand that contains PostProcess method.
    /// In case when this particular IFilter's execute method was executed by IChain then IChain have to execute also this particular
    /// IFilter's PostProcess method.
    /// </summary>
    public interface IFilter : ICommand
    {
        /// <summary>
        /// Posts the process.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        bool PostProcess(IContext context, Exception exception);
    }
}