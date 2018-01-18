using Nucleotic.Framework.Engine.CommandChain;

namespace Nucleotic.Framework.Engine.Assemble
{
    /// <summary>
    /// Base assemblor context class
    /// </summary>
    /// <seealso cref="ContextBase" />
    public abstract class AssemblorContext : ContextBase
    {
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            if (!ContainsKey(key)) return default(T);
            var obj = this[key];
            if (obj != null)
                return (T)obj;
            return default(T);
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Set(string key, object value)
        {
            this[key] = value;
        }
    }
}