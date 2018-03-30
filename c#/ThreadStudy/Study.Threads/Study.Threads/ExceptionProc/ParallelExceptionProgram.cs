using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study.Threads.ExceptionProc
{
    /// <summary>
    /// 并行类异常处理
    /// </summary>
    class ParallelExceptionProgram
    {
        static void Main(string[] args)
        {
            // 1. 直接try catch 可能发生异常的部分，然后进行处理
            Parallel.For(0, 5, i =>
            {
                try
                {
                    throw new Exception("并行任务发生了个错误");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("handled an exception...");
                }
            });

            Console.ReadLine();

            // 2. 将并发异常包装到主线程中，统一处理
            var parallelExceptions = new ConcurrentQueue<Exception>();  // 线程安全的泛型集合ConcurrentQueue<T>
            try
            {
                Parallel.For(0, 5, i =>
                {
                    try
                    {
                        throw new Exception("并行任务发生了个错误");
                    }
                    catch (Exception ex)
                    {

                        parallelExceptions.Enqueue(ex);
                    }

                    //if (parallelExceptions.Count > 0)
                    //    throw new AggregateException(parallelExceptions);
                });

                foreach (var ex in parallelExceptions)
                {
                    Console.WriteLine(@"异常类型：{0}{1}来自：
                    {2}{3}异常内容：{4}", ex.GetType(),
                        Environment.NewLine, ex.Source,
                        Environment.NewLine, ex.Message);
                }
            }
            catch (AggregateException err)
            {
                foreach (Exception item in err.InnerExceptions)
                {
                    Console.WriteLine(@"异常类型：{0}{1}来自：  
                    {2}{3}异常内容：{4}", item.InnerException.GetType(),
                    Environment.NewLine, item.InnerException.Source,
                    Environment.NewLine, item.InnerException.Message);
                }
            }
            Console.WriteLine("主线程马上结束");
            Console.ReadKey();
        }
    }
}
