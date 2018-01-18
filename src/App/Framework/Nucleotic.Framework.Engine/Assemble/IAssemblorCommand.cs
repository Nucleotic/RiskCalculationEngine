namespace Nucleotic.Framework.Engine.Assemble
{
    /// <summary>
    /// Public assemblor command interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAssemblorCommand<in T>
    {
        /// <summary>
        /// Fires of the specified assembly into the pipeline.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        void Assemble(T assembly);
    }
}