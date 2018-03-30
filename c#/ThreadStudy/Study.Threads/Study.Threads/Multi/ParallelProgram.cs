using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Study.Threads.Multi
{
    /// <summary>
    /// 并行类
    /// 处理数据集，顺序和结果不太重要，或者想要更精确的控制并发度可以使用Parallel
    /// 如果既可以使用For, 也可以使用ForEach, 建议使用For, 它执行的更快
    /// 内部使用Task
    /// </summary>
    class ParallelProgram
    {
        private static ParallelOptions options = new ParallelOptions();

        static void Main(string[] args)
        {
            #region // 使用Parallel

            // for并行循环
            Console.WriteLine("---------------Parallel.For---------------");
            Parallel.For(1, 10, i =>
            {
                Console.WriteLine("线程Id:{0}, IsThreadPoolThread:{1}, i:{2}", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread, i);
            });

            // ForEach并行循环
            Console.WriteLine("---------------Parallel.ForEach---------------");
            Parallel.ForEach(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, (item) =>
            {
                Console.WriteLine("线程Id:{0}, IsThreadPoolThread:{1}, i:{2}", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread, item);
            });

            // 控制线程数  
            Console.WriteLine("---------------ParallelOptions---------------");
            ParallelOptions po = new ParallelOptions();
            po.MaxDegreeOfParallelism = 2;//每次2条线程执行  
            Parallel.For(0, 10, po, i =>
            {
                Console.WriteLine("线程Id:{0}, t:{1}", Thread.CurrentThread.ManagedThreadId, i);
            });


            Console.WriteLine("---------------Parallel.Invoke---------------");
            Parallel.Invoke(() =>
                {
                    Console.WriteLine("method1");
                },
                () =>
                {
                    Console.WriteLine("method2");
                }, () =>
                {
                    Console.WriteLine("method3");
                });

            // Parallel.For()和Paraller.ForEach()方法在每次迭代中调用相同的代码，
            // 而Parallel.Invoke()方法允许同时调用不同的方法。
            // Parallel.ForEach()用于数据并行性，Parallel.Invoke()用于任务并行性；

            #endregion

            

            #region

            // For 和 ForEach 重载版本允许3个委托
            // 1.任务局部初始化委托(localInit), 每次处理都会调用一次
            // 2.主体委托(body), 每次处理都会调用一次
            // 3.任务局部终结委托(localFinally), 每次处理都会调用一次

            //var totalFileBytes = 0L;
            //var files = Directory.GetFiles(@"D:\workarea", "*.*", SearchOption.TopDirectoryOnly);

            //var result = Parallel.ForEach(
            //    files,
            //    () => //localInit
            //    {
            //        return 0L;
            //    },
            //    (file, loopState, index, taskLocalTotal) => //body
            //    {
            //        if (index == 0)
            //        {
            //            //throw new IOException("opened.");
            //        }
            //        long fileLength = 0;
            //        FileStream fs = null;
            //        try
            //        {
            //            Thread.Sleep(1000);
            //            fs = File.OpenRead(file);
            //            fileLength = fs.Length;
            //        }
            //        catch (IOException e)
            //        {

            //        }
            //        finally{if(fs != null) fs.Dispose();}

            //        return taskLocalTotal + fileLength;
            //    },
            //    taskLocalTotal =>  // localFinally
            //    {
            //        Console.WriteLine(taskLocalTotal);
            //        Interlocked.Add(ref totalFileBytes, taskLocalTotal);
            //    });

            //Console.WriteLine(totalFileBytes);

            #endregion
        }
    }
}
