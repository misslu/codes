using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Study.Threads
{
    /// <summary>
    /// 信号量测试
    /// </summary>
    class SemaphoreProgram
    {
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);   // 1,同一时刻只允许一个线程
        private static int count = 0;
        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                Task.Run(() => { Run(); });
            }

            Console.ReadLine();
        }

        static void Run()
        {
            semaphoreSlim.Wait();
            count++;
            Console.WriteLine("current thread id:{0},count:{1}", Thread.CurrentThread.ManagedThreadId, count);
            semaphoreSlim.Release();
        }
    }
}
