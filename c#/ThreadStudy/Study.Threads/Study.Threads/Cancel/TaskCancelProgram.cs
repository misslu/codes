using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Study.Threads.Cancel
{
    /// <summary>
    /// Task取消
    /// </summary>
    class TaskCancelProgram
    {
        static void Main(string[] args)
        {
            #region // 取消任务

            Console.WriteLine("-------------取消任务---------------");

            var cts = new CancellationTokenSource();
            var longTask = new Task(() =>
            {
                Console.WriteLine("这是个task...");
            }, cts.Token);

            Console.WriteLine(longTask.Status);
            cts.Cancel(); // 取消
            Console.WriteLine(longTask.Status);

            cts = new CancellationTokenSource();
            var t2 = Task.Run(() => Sum(100, cts.Token), cts.Token);

            cts.CancelAfter(1); // 1毫秒后取消

            t2.ContinueWith(task =>
            {
                Console.WriteLine("t2 canceled.");
            }, TaskContinuationOptions.OnlyOnCanceled); // TaskContinuationOptions 的应用

            Console.ReadLine();

            #endregion
        }

        private static int Sum(int n, CancellationToken token)
        {
            int sum = 0;
            for (; n > 0; n--)
            {
                token.ThrowIfCancellationRequested();
                Console.WriteLine("Sum:{0}", sum);
                sum += n;
                Thread.Sleep(1);
            }

            return sum;
        }
    }
}
