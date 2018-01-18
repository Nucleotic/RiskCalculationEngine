using Nucleotic.Framework.Engine.CommandChain;
using Nucleotic.Framework.Engine.CommandChain.Interfaces;
using Nucleotic.Framework.Logging;
using System.Diagnostics;

namespace Nucleotic.Framework.Engine.Assemble
{
    public abstract class AbstractPipeline<T> : ChainBase where T : AssemblorContext
    {
        public readonly Logger<T> Logger = new Logger<T>();

        /// <summary>
        /// Assembles the specified context in the pipeline.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Assemble(T context)
        {
            Execute(context);
        }

        /// <summary>
        /// Runs the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public T Run(T context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Execute((IContext)context);
            stopwatch.Stop();
            Logger.LogDebugMessage($"{GetType().FullName} - Total pipeline run time : " + stopwatch.ElapsedMilliseconds);
            return context;
        }
    }
}