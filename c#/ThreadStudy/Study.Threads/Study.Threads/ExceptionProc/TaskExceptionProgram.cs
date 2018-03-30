using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study.Threads.ExceptionProc
{
    /// <summary>
    /// Task异常处理
    /// </summary>
    class TaskExceptionProgram
    {
        static void Main(string[] args)
        {
            #region // 异常处理

            Console.WriteLine("-------------异常处理---------------");

            //// 这一个异常不会被察觉
            var task1 = new Task<int>(() =>
            {
                throw new Exception();
            });

            task1.Start();

            // 1. 通过延续任务来处理这个异常
            task1.ContinueWith(task =>
            {
                foreach (var ex in task.Exception.InnerExceptions)
                {
                    Console.WriteLine("ContinueWith processed task1 exception:{0}", ex.Message);
                }
            }, TaskContinuationOptions.OnlyOnFaulted);  // 可以指定任务失败时才执行延续任务

            // 2. 通过显示等待任务完成或访问任务结果捕获异常
            // 对线程调用Wait方法（或者求Result）不是最好的办法，因为它会阻滞主线程，并且CLR在后台会新起线程池线程来完成额外的工作。
            try
            {
                //task1.Wait();
                //Task.WaitAll(task1);
                //Console.WriteLine("task1.Result:" + task1.Result);
            }
            catch (AggregateException ex)
            {
                Console.WriteLine("catched task1 exception:{0}", ex.Message);
            }

            // 3. 使用事件通知的方式：
            AggregateExceptionCatched += Program_AggregateExceptionCatched;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    throw new InvalidOperationException("任务并行编码中产生的未知异常");
                }
                catch (Exception err)
                {
                    AggregateExceptionArgs errArgs = new AggregateExceptionArgs()
                        { AggregateException = new AggregateException(err) };
                    AggregateExceptionCatched(null, errArgs);
                }
            });

            #endregion
        }



        // 声明处理委托
        static event EventHandler<AggregateExceptionArgs> AggregateExceptionCatched;

        public class AggregateExceptionArgs : EventArgs
        {
            public AggregateException AggregateException { get; set; }
        }

        static void Program_AggregateExceptionCatched(object sender,
            AggregateExceptionArgs e)
        {
            foreach (var item in e.AggregateException.InnerExceptions)
            {
                Console.WriteLine("异常类型：{0}{1}来自：{2}{3}异常内容：{4}",
                    item.GetType(), Environment.NewLine, item.Source,
                    Environment.NewLine, item.Message);
            }
        }
    }
}
