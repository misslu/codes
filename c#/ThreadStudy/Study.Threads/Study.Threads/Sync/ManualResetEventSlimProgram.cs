using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace Study.Threads.Sync
{
    
    class ManualResetEventSlimProgram
    {
        // 若要将初始状态设置为终止，则为 true；若要将初始状态设置为非终止，则为 false
        // Wait() 线程将阻塞，并等待信号。
        // Set() 发出等待线程可以继续进行的信号
        // Reset() 状态设为非终止，线程阻塞
        static ManualResetEventSlim manualResetEvent = new ManualResetEventSlim(false);

        static void Main(string[] args)
        {
            Thread t1 = new Thread(() =>
            {
                while (true)
                {
                    manualResetEvent.Wait(); // 等待铃声
                    Console.WriteLine("同学1Reading...");
                    Thread.Sleep(500);
                }

            });
            t1.IsBackground = true;
            t1.Start();

            Thread t2 = new Thread(() =>
            {
                while (true)
                {
                    manualResetEvent.Wait(); // 等待铃声
                    Console.WriteLine("同学2Reading...");

                    Thread.Sleep(500);
                }
            });
            t2.IsBackground = true;
            t2.Start();

            Console.WriteLine("上早自习啦...");
            Thread.Sleep(500);
            
            manualResetEvent.Set();  //触发读书

            Thread.Sleep(2000);

            Console.WriteLine("下自习啦...");
            manualResetEvent.Reset();
            Console.WriteLine("同学们停止了读书...");

            Thread.Sleep(2000);

            Console.WriteLine("上课啦...");
            manualResetEvent.Set();  //触发读书

            Thread.Sleep(2000);
            Console.WriteLine("下课啦...");

            manualResetEvent.Reset(); //终止读书

            Console.ReadLine();
        }
    }
}
