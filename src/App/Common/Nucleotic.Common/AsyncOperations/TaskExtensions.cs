using System;
using System.Threading;
using System.Threading.Tasks;

namespace Nucleotic.Common.AsyncOperations
{
    /// <summary>
    /// Asynchronous Task Method Extensions
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Timeouts the after.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="task">The task.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        /// <exception cref="System.TimeoutException">The operation has timed out.</exception>
        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            var timeoutCancellationTokenSource = new CancellationTokenSource();
            var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
            if (completedTask != task) throw new TimeoutException("The operation has timed out.");
            timeoutCancellationTokenSource.Cancel();
            return await task;
        }

        /// <summary>
        /// Timeouts the after.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        /// <exception cref="System.TimeoutException">The operation has timed out.</exception>
        public static async Task TimeoutAfter(this Task task, TimeSpan timeout)
        {
            var timeoutCancellationTokenSource = new CancellationTokenSource();
            var completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));
            if (completedTask != task) throw new TimeoutException("The operation has timed out.");
            timeoutCancellationTokenSource.Cancel();
        }
    }
}
