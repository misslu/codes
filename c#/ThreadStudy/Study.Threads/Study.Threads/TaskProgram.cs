using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Study.Threads
{
    class TaskProgram
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 5; i++)
            {
                new Thread(Run1).Start();
            }
            for (int i = 0; i < 5; i++)
            {
                Task.Run(() => { Run2(); });
            }

            Console.ReadLine();
        }
        static void Run1()
        {
            Console.WriteLine("Thread Id =" + Thread.CurrentThread.ManagedThreadId);
        }
        static void Run2()
        {
            Console.WriteLine("Task调用的Thread Id =" + Thread.CurrentThread.ManagedThreadId);
        }


        /// <summary>
        /// 后台线程属性测试
        /// </summary>
        private static void IsBackgroundTest()
        {
            BackgroundTest shortTest = new BackgroundTest(10);
            Thread foregroundThread =
                new Thread(new ThreadStart(shortTest.RunLoop));
            foregroundThread.Name = "ForegroundThread";

            BackgroundTest longTest = new BackgroundTest(50);
            Thread backgroundThread =
                new Thread(new ThreadStart(longTest.RunLoop));
            backgroundThread.Name = "BackgroundThread";
            backgroundThread.IsBackground = true;

            foregroundThread.Start();
            backgroundThread.Start();
        }
    }


    class BackgroundTest
    {
        int maxIterations;

        public BackgroundTest(int maxIterations)
        {
            this.maxIterations = maxIterations;
        }

        public void RunLoop()
        {
            String threadName = Thread.CurrentThread.Name;

            for (int i = 0; i < maxIterations; i++)
            {
                Console.WriteLine("{0} count: {1}",
                    threadName, i.ToString());
                Thread.Sleep(250);
            }
            Console.WriteLine("{0} finished counting.", threadName);
        }
    }
}
