using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Study.Threads.Multi
{
    /// <summary>
    /// 线程池
    /// </summary>
    class ThreadPoolProgram
    {
        static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem(count =>
            {
                Console.WriteLine("Name={0}", CallContext.LogicalGetData("Name"));
                Console.WriteLine(count);
            }, 1000);

            // 可以阻止执行上下文流动以提升应用程序的性能
            System.Threading.ExecutionContext.SuppressFlow();

            ThreadPool.QueueUserWorkItem(count =>
            {
                Console.WriteLine("Name={0}", CallContext.LogicalGetData("Name"));
                Console.WriteLine(count);
            }, 1000);

            // 恢复上下文流动
            ExecutionContext.RestoreFlow();

            CallContext.LogicalSetData("Name", "xiangge");

            ThreadPool.QueueUserWorkItem(count =>
            {
                Console.WriteLine("Name={0}", CallContext.LogicalGetData("Name"));
                Console.WriteLine(count);
            }, 1000);
        }
    }
}
