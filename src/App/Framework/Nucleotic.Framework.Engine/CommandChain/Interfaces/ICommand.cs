namespace Nucleotic.Framework.Engine.CommandChain.Interfaces
{
    /// <summary>
    /// ICommand represents unit of work that should be performed taking IContext under consideration.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        bool Execute(IContext context);
    }
}