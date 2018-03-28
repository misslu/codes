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
        
        static void Main(string[] args)
        {
            Monster monster = new Monster(1000);
            Player youXia = new Player() { Name = "游侠", Weapon = "宝剑", Atk = 150 };
            Player yeManRen = new Player() { Name = "野蛮人", Weapon = "链锤", Atk = 250 };
            Thread t1 = new Thread(youXia.PhysAttack);
            t1.Start(monster);
            Thread t2 = new Thread(yeManRen.PhysAttack);
            t2.Start(monster);
            t1.Join();
            t2.Join();
            Console.ReadKey();
        }

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
    }
}
