using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Study.Threads
{
    class ThreadProgram
    {
        static void Main(string[] args)
        {
            var thread1 = new Thread(NoArgMethod);
            thread1.Start();

            var thread2 = new Thread(ArgMethod);
            thread2.Start("luxiang");

            var thread3 = new Thread(() => Console.WriteLine("hello, thread3, current threadId:{0}", Thread.CurrentThread.ManagedThreadId));
            thread3.Start();

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
