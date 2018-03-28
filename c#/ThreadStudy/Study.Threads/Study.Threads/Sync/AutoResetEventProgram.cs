using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Study.Threads.Sync
{
    /// <summary>
    /// 
    /// </summary>
    class AutoResetEventProgram
    {
        // 若要将初始状态设置为终止，则为 true；若要将初始状态设置为非终止，则为 false
        // WaitOne() 线程将阻塞，并等待信号。
        // Set() 发出等待线程可以继续进行的信号
        // Reset() 状态设为非终止，线程阻塞
        private static AutoResetEvent autoResetEvent = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            Console.WriteLine("学校停电了...");

            // 10个学生
            for (int i = 0; i < 10; i++)
            {
                new Task(num =>
                {
                    autoResetEvent.WaitOne();  // 等待通知

                    Console.WriteLine("学生{0}收到了通知,开始转告下一个同学...", num);
                    Thread.Sleep(1000);

                    if (Convert.ToInt32(num) != 8)
                    {
                        autoResetEvent.Set(); // 通知下一个同学
                    }
                    else
                    {
                        Console.WriteLine("学生{0}很皮,不想转告别的同学...", num);
                    }

                }, i).Start();
            }


            Console.WriteLine("老师开始打电话第一个学生，并且请同学相互转告...");
            Thread.Sleep(1000);

            autoResetEvent.Set();  // 老师通知第一个同学

            Console.ReadLine();

            #region //
            //var account = Account.GetInstance("想哥", 200000);
            //var rmd = new Random();
            //var tasks = new Task[100];
            //Stopwatch watch = new Stopwatch();
            //watch.Start();
            //for (int i = 0; i < 100; i++)
            //{
            //    var atm = new Atm(account);

            //    tasks[i] = new Task(() =>
            //    {
            //        mres.WaitOne();
            //        atm.DrawMoney(rmd.Next(1, 10000));
            //        mres.Set();
            //    });
            //    tasks[i].Start();
            //}

            ////mres.Set();
            //Task.WaitAll(tasks);
            //watch.Stop();

            //Console.WriteLine("用时:{0}毫秒", watch.ElapsedMilliseconds);
            //Console.ReadLine();
            #endregion
        }

        private class Atm
        {

            private Atm() { }

            public Atm(Account account)
            {
                this._account = account;
            }

            private Account _account;

            public void SaveMoney(int money)
            {
                try
                {
                    this._account.Increment(money);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("啊，加钱发生了异常," + ex.Message);
                }
            }

            public void DrawMoney(object obj)
            {

                try
                {
                    var money = Convert.ToInt32(obj);
                    this._account.Decrement(money);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("啊，扣钱发生了异常," + ex.Message);
                }

            }

            public void Transfer(Account toAccount, int money)
            {
                this._account.Decrement(money);
                toAccount.Increment(money);
            }
        }

        private class AccountManager
        {
            private Account _account;

            public AccountManager(Account account)
            {
                _account = account;
            }
        }


        private class Account
        {
            private string _name;
            private decimal _balance;
            private int _flag;

            public static Account _account;

            public static Account GetInstance(string name, decimal balance)
            {
                if (_account == null)
                    _account = new Account() { _name = name, _balance = balance };

                return _account;
            }

            private Account() { }

            public void Increment(decimal money)
            {
                this._balance += money;
            }

            public void Decrement(decimal money)
            {
                if (money > this._balance)
                {
                    Console.WriteLine("{0}:{2},本次扣款:{1},余额不足", this._name, money, _balance);
                }
                else
                {
                    Console.WriteLine("{0}:{1},本次扣款:{2},余额:{3}", this._name, this._balance, money, this._balance - money);
                    this._balance -= money;
                }

            }
        }
    }
}
