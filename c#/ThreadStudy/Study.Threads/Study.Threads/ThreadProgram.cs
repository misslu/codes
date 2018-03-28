using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Study.Threads
{
    class ThreadProgram
    {
        static void Main(string[] args)
        {
            // 存入一个数据到Main线程的逻辑调用上下文中
            CallContext.LogicalSetData("Name", "luxiang");

            var thread1 = new Thread(NoArgMethod);
            thread1.Start();

            var thread2 = new Thread(ArgMethod);
            thread2.Start("luxiang");

            var thread3 = new Thread(() => Console.WriteLine("hello, thread3, current threadId:{0}", Thread.CurrentThread.ManagedThreadId));
            thread3.Start();

            // 可以阻止执行上下文流动以提升应用程序的性能
            System.Threading.ExecutionContext.SuppressFlow();

            // 恢复上下文流动
            ExecutionContext.RestoreFlow();

            Console.ReadLine();
        }

        static void NoArgMethod()
        {
            Console.WriteLine("hello, thread1, current threadId:{0}", Thread.CurrentThread.ManagedThreadId);
        }

        static void ArgMethod(object n)
        {
            Console.WriteLine("hello, thread2, current threadId:{0}, arg:{1}", Thread.CurrentThread.ManagedThreadId, n.ToString());
        }
    }
}
