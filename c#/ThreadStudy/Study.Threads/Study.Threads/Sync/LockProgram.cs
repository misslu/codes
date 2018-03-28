using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Study.Threads.Sync
{
    class LockProgram
    {
        static void Main(string[] args)
        {
            var account = new Account("想哥", 200000);
            var rmd = new Random();
            var tasks = new Task[100];

            for (int i = 0; i < 100; i++)
            {
                var atm = new Atm(account);

                tasks[i] = new Task(() => atm.DrawMoney(rmd.Next(1, 10000)));
                tasks[i].Start();
            }

            //Task.WaitAll(tasks);

            Console.ReadLine();
        }

        private class Atm
        {
            private Atm() { }

            public Atm(Account account)
            {
                this._account = account;
            }

            private Account _account;

            public void SaveMoney(decimal money)
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
                    var money = Convert.ToDecimal(obj);
                    this._account.Decrement(money);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("啊，扣钱发生了异常," + ex.Message);
                }
            }

            //public void Transfer(Account toAccount, decimal money)
            //{
            //    this._account.Decrement(money);
            //    toAccount.Increment(money);
            //}
        }


        private class Account
        {
            private string _name;
            private decimal _balance;
            private static object objLock = new object();


            private Account() { }


            public Account(string name, decimal balance)
            {
                this._name = name;
                this._balance = balance;
            }

            public void Increment(decimal money)
            {
                this._balance += money;
            }

            public void Decrement(decimal money)
            {
                lock (objLock)
                {
                    if (money > this._balance)
                    {
                        Console.WriteLine("{0}:{2},本次扣款:{1},余额不足", this._name, money, _balance);
                    }
                    else
                    {
                        Console.WriteLine("{0}:{1},本次扣款:{2},余额:{3}", this._name, this._balance, money,
                            this._balance - money);
                        this._balance -= money;
                    }
                }
            }
        }
    }
}
