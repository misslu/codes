using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Study.Threads.Multi
{
    class ThreadProgram
    {
        static void Main(string[] args)
        {
            // 存入一个数据到Main线程的逻辑调用上下文中
            CallContext.LogicalSetData("Name", "luxiang");

            var thread1 = new Thread(MethodWithoutParam);
            thread1.Start();

            var thread2 = new Thread(MethodWithParam);
            thread2.Start("luxiang");

            var thread3 = new Thread(() => Console.WriteLine("hello, thread3, current threadId:{0}", Thread.CurrentThread.ManagedThreadId));
            thread3.Start();
            

            Console.ReadLine();
        }

        static void MethodWithoutParam()
        {
            Console.WriteLine("hello, thread1, current threadId:{0}", Thread.CurrentThread.ManagedThreadId);
        }

        static void MethodWithParam(object n)
        {
            Console.WriteLine("hello, thread2, current threadId:{0}, arg:{1}", Thread.CurrentThread.ManagedThreadId, n.ToString());
        }
    }
}

