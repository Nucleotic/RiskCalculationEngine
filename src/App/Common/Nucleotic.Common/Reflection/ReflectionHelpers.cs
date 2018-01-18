using System.Linq;
using System.Reflection;

namespace iSixty.Common.Reflection
{
    public static class ReflectionHelpers
    {
        /// <summary>
        /// Gets the method information.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static MethodInfo GetMethodInformation(Assembly assembly)
        {
            var ass = assembly;
            var methodInfos = assembly.GetType().GetMethods(BindingFlags.Public | BindingFlags.Static);
            return methodInfos.First();
        }
    }
}
