using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Study.Threads.Sync
{
    class MonitorProgram
    {
        static void Main(string[] args)
        {
            #region case 2

            //Monster monster = new Monster(1000);
            //Player youXia = new Player() { Name = "游侠", Weapon = "宝剑", Atk = 150 };
            //Player yeManRen = new Player() { Name = "野蛮人", Weapon = "链锤", Atk = 250 };
            //Thread t1 = new Thread(youXia.PhysAttack);
            //t1.Start(monster);
            //Thread t2 = new Thread(yeManRen.PhysAttack);
            //t2.Start(monster);
            //t1.Join();
            //t2.Join();
            //Console.ReadKey();

            #endregion

            var account = new Account("想哥", 200000);
            var rmd = new Random();
            var tasks = new Task[100];

            for (int i = 0; i < 100; i++)
            {
                var atm = new Atm(i.ToString(), account);

                tasks[i] = new Task(() => atm.DrawMoney(rmd.Next(1, 10000)));
                tasks[i].Start();
            }

            //Task.WaitAll(tasks);

            Console.ReadLine();
        }

        private class Atm
        {
            private string _name;
            private Atm() { }

            public Atm(string name, Account account)
            {
                this._name = name;
                this._account = account;
            }

            private Account _account;
            private static object objLock = new object();

            public void SaveMoney(decimal money)
            {
                try
                {
                    this._account.Increment(money);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("啊,ATM:{0}，加钱发生了异常,{1}", this._name, ex.Message);
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
                    Console.WriteLine("啊,ATM:{0}，出钱失败了,{1}", this._name, ex.Message);
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
                if (Monitor.TryEnter(objLock, 1000))
                {
                    Thread.Sleep(300);
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
                    Monitor.Exit(objLock);
                }
                else
                {
                    throw new Exception("其他ATM机正在取钱, 请稍后再试!");
                }
            }
        }
    }

    #region // case 2

    /// <summary>
    /// 怪物类
    /// </summary>
    public class Monster
    {
        private int _blood;

        public int Blood => _blood;

        public Monster(int blood)
        {
            this._blood = blood;
            Console.WriteLine("我是怪物，我有{0}点血!", _blood);
        }

        public void Hurt(int akt)
        {
            if (akt >= _blood)
            {
                this._blood = 0;
                Console.WriteLine("我是怪物，我的血量为0，已经死了！");
            }
            else
            {
                this._blood -= akt;
                Console.WriteLine("我是怪物，我受到了{0}点攻击，还剩{1}点血！", akt, this._blood);
            }
        }
    }


    public class Player
    {
        public string Name { get; set; }
        public string Weapon { get; set; }
        public int Atk { get; set; }

        public void PhysAttack(object monster)
        {

            Monster m = monster as Monster;
            while (m.Blood > 0)
            {
                Monitor.Enter(monster);
                if (m.Blood > 0)
                {
                    Console.WriteLine("当前玩家 【{0}】,使用{1}攻击怪物！", this.Name, this.Weapon);
                    m.Hurt(this.Atk);
                }
                Thread.Sleep(500);
                Monitor.Exit(monster);
            }

        }
    }
    #endregion

}
