namespace Nucleotic.Framework.Logging
{
    public abstract class Diagnostics<T> where T : class
    {
        public readonly Logger<T> Logger = new Logger<T>();
    }
}
