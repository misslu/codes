using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Study.Threads.Sync
{
    public class ReadWrite
    {
        private ReaderWriterLockSlim rwl;
        private int x, y;

        public ReadWrite()
        {
            rwl = new ReaderWriterLockSlim();
        }

        public void ReadInts(ref int a, ref int b)
        {
            rwl.EnterReadLock();
            try
            {
                a = this.x;
                b = this.y;
            }
            finally
            {
                rwl.ExitReadLock();
            }
        }


        public void WriteInts(int a, int b)
        {
            this.rwl.EnterWriteLock();
            try
            {
                this.x = a;
                this.y = b;
                Console.WriteLine("x = " + this.x
                                  + " y = " + this.y
                                  + " ThreadID = " + Thread.CurrentThread.GetHashCode());
            }
            finally
            { 
               rwl.ExitWriteLock();  
            }
        }
    }

    class ReadWriteLockProgram
    {
        private ReadWrite rw = new ReadWrite();

        static void Main(string[] args)
        {
            ReadWriteLockProgram e = new ReadWriteLockProgram();

            //Writer Threads
            Thread wt1 = new Thread(new ThreadStart(e.Write));
            wt1.Start();
            Thread wt2 = new Thread(new ThreadStart(e.Write));
            wt2.Start();

            //Reader Threads
            Thread rt1 = new Thread(new ThreadStart(e.Read));
            rt1.Start();
            Thread rt2 = new Thread(new ThreadStart(e.Read));
            rt2.Start();

            Console.ReadLine();

        }

        private void Write()
        {
            int a = 10;
            int b = 11;
            Console.WriteLine("************** Write *************");

            for (int i = 0; i < 5; i++)
            {
                this.rw.WriteInts(a++, b++);
                Thread.Sleep(1000);
            }
        }

        private void Read()
        {
            int a = 10;
            int b = 11;
            Console.WriteLine("************** Read *************");

            for (int i = 0; i < 5; i++)
            {
                this.rw.ReadInts(ref a, ref b);
                Console.WriteLine("For i = " + i
                                  + " a = " + a
                                  + " b = " + b
                                  + " TheadID = " + Thread.CurrentThread.GetHashCode());
                Thread.Sleep(500);
            }
        }
    }
}
