using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study.Threads.Cancel
{
    class ParallelCancelProgram
    {
        static void Main(string[] args)
        {
            // 退出循环 退出单次循环  
            Console.WriteLine("---------------退出单次循环---------------");
            var singleCancelResult = Parallel.For(0, 50, (i, state) =>//参数这里需要传入state  
            {
                Console.WriteLine(i);  //因为不会保证处理的顺序,结束前可能已经在处理20之后的项,所以还会输出20之后的数字
                if (i == 20)
                {
                    //state.Stop();//退出单次循环   
                    state.Break(); ;//退出单次循环   Break(); 会保证i之前的项会执行完成, 以i=20结束为例, 会保证0-19全部执行完成
                    return;
                }

                // do something...
            });

            Console.WriteLine("是否完成:{0},最小的结束项:{1}", singleCancelResult.IsCompleted, singleCancelResult.LowestBreakIteration);

            //Parallel.ForEach(new[] {1, 2, 3, 4, 5, 6, 7, 8, 9}, (i, state, index) =>
            //{
            //    Console.WriteLine(index);
            //    if (index == 1)
            //    {
            //        state.Break();//退出单次循环  
            //    }
            //});

            Console.ReadLine();
        }
    }
}
