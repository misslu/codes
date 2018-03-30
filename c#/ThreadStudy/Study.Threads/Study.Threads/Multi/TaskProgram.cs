using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Study.Threads.Multi
{
    /// <summary>
    /// Task类
    /// 所有Task运行的线程都是线程池线程
    /// </summary>
    class TaskProgram
    {
        static void Main(string[] args)
        {
            #region // 启动一个Task

            Console.WriteLine("-----------------创建Task-----------------");

            // 启动Task第一种方式
            // 还支持泛型版本, 以泛型类型作为任务返回类型
            new Task(str => Console.WriteLine("这是一个 new Task().Start(),IsThreadPoolThread:{0}, {1}", Thread.CurrentThread.IsThreadPoolThread , str), "哈哈哈").Start();

            // 启动Task第二种方式
            Task.Run(() => Console.WriteLine("这是一个 Task.Run(),IsThreadPoolThread:{0}", Thread.CurrentThread.IsThreadPoolThread));

            // 启动Task第三种方式 任务工厂
            //new TaskFactory();
            Task.Factory.StartNew(() => Console.WriteLine("这是一个 Task.Factory.StartNew(),IsThreadPoolThread:{0}", Thread.CurrentThread.IsThreadPoolThread));
            
            Console.ReadLine();

            #endregion

            #region // 获取Task的结果

            Console.WriteLine("--------------获取Task的结果-------------");

            // 获取Task的返回值
            var r = Task.Run(() =>
            {
                Thread.Sleep(2000);
                return "hello world!";
            });

            Console.WriteLine("-------------单个任务运行中,等待结果输出-");

            //r.Wait();   // 等待线程完成
            //Console.WriteLine(r.Result);  // 直接取Task的结果会发生阻塞

            // 不阻塞获取任务的返回结果
            r.ContinueWith(t =>
            {
                Console.WriteLine("单个任务运行结果:" + t.Result);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            Console.WriteLine("-------------主线程继续执行--------------");

            // 获取Task的返回值
            var tasks = new Task<int>[5];
            var rdm = new Random();

            for (int i = 0; i < 5; i++)
            {
                var task = new Task<int>(() =>
                {
                    Thread.Sleep(100);
                    return rdm.Next();
                });
                tasks[i] = task;
                task.Start();
            }

            Console.WriteLine("-------------多个任务运行中,等待结果输出-");

            //Task.WaitAll(tasks);   // 等待所有任务完成

            // 多个任务并行任意一个任务完成回调
            Task.WhenAny(tasks).Unwrap().ContinueWith(t =>
            {
                Console.WriteLine("多个任务任一任务完成结果:" + string.Join(",", t.Result));
            });


            // 不阻塞获取多任务并行运行结果
            Task.WhenAll(tasks).ContinueWith(t =>
            {
                Console.WriteLine("多个任务运行结果:" + string.Join(",", t.Result));
            });

            Console.ReadLine();

            #endregion

            

            

            Console.WriteLine("-------------主线程执行完毕--------------");
            Console.ReadLine();
        }
    }
}
