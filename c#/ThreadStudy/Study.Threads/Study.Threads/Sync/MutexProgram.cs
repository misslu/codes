using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Study.Threads.Sync
{
    class MutexProgram
    {
        private static Mutex mutex = new Mutex();
        public static int account = 1000;//账户
        public static int pocket = 0;//口袋
        static void Main(string[] args)
        {
            Thread t1 = new Thread(DoWork);
            t1.Start();
            Thread t2 = new Thread(DoWork);
            t2.Start();
            t1.Join();
            t2.Join();
            Console.WriteLine("pocket=" + pocket);
        }
        public static void DoWork()
        {
            if (mutex.WaitOne())
            {
                try
                {
                    if (account >= 1000)
                    {
                        Thread.Sleep(1000);//自动取款机打了个小盹
                        account -= 1000;
                        pocket += 1000;
                    }
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }
    }
}
