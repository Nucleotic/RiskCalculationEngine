namespace Nucleotic.Framework.Engine.CommandChain
{
    public abstract class CommandBase
    {
        /// <summary>
        /// PROCESSING_COMPLETED should be returned when context execution is completed.
        /// </summary>
        public const bool PROCESSING_COMPLETED = true;

        /// <summary>
        /// CONTINUE_PROCESSING should be returned when context execution should be passed to other commands that exist in chain.
        /// </summary>
        public const bool CONTINUE_PROCESSING = false;
    }
}