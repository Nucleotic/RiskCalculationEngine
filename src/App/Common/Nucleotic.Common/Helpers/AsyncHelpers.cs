using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Nucleotic.Common.Helpers
{
    public static class AsyncHelpers
    {
        private static readonly TaskFactory AsyncTaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

        /// <summary>
        /// Runs the synchronize.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">The function.</param>
        /// <returns></returns>
        /// <example>AsyncHelpers.RunSync(new Func<Task<T>>(async () => await AsyncCallGoesHere(parameter)));</example>
        public static T RunSync<T>(Func<Task<T>> func)
        {
            var cultureUi = CultureInfo.CurrentUICulture;
            var culture = CultureInfo.CurrentCulture;
            return AsyncTaskFactory.StartNew(delegate
            {
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = cultureUi;
                return func();

            }).Unwrap().GetAwaiter().GetResult();
        }
    }
}
