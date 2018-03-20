using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Study.WinThreads
{
    public partial class Form1 : Form
    {
        private Thread pBarThread;
        private Thread showNumThread;
        private int i = 0;


        public Form1()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 开始按钮事件，创建线程并为线程指定方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            pBarThread = new Thread(new ThreadStart(this.ExepBarShow)); //创建进度条线程
            showNumThread = new Thread(new ThreadStart(this.ExeShowNum));   //创建显示文本框中的文字线程
                                                                            //开始两个已创建的线程
            this.StartThread(showNumThread);
            this.StartThread(pBarThread);
        }

        /// <summary>
        /// 使用委托执行ShowNumToText方法
        /// </summary>
        private void ExeShowNum()
        {
            try
            {
                MethodInvoker mInvoker = this.ShowNumToText; //声明托管委托，并为委托执行执行的方法

                //执行委托方法，向Text中写入文字
                while (true)
                {
                    this.BeginInvoke(mInvoker);   //异步执行执行的委托
                    Thread.Sleep(1000);     //线程停顿1秒后继续执行 
                }
            }
            catch { }
        }


        /// <summary>
        /// 先文本框txtNum中写入文字
        /// </summary>
        private void ShowNumToText()
        {
            i = i + 1;  //i累加
            txtNum.Text = txtNum.Text + " " + (i + 1).ToString();   //向txtNum中写入文字
        }

        /// <summary>
        /// 执行pBarShow方法，加载进度条，让进度条读取进度
        /// </summary>
        private void ExepBarShow()
        {
            try
            {
                MethodInvoker mInvoker = new MethodInvoker(this.pBarShow);  //声明并创建委托，为委托执行进度

                //异步执行委托
                while (true)
                {
                    this.BeginInvoke((Delegate)mInvoker);
                    Thread.Sleep(10);
                }
            }
            catch { }
        }

        /// <summary>
        /// 执行进度条读取进度
        /// </summary>
        private void pBarShow()
        {
            this.pgBar.PerformStep();
        }

        /// <summary>
        /// 线程开始方法
        /// </summary>
        /// <param name="th">Thread对象，需要开始的线程</param>
        private void StartThread(Thread th)
        {
            th.Start();
        }

        /// <summary>
        /// 线程结束方法
        /// </summary>
        /// <param name="th">Thread对象，需要结束的线程</param>
        private void EndThread(Thread th)
        {
            th.Interrupt(); //中断线程
            th.Abort(); //终止线程
            th = null;
        }

        /// <summary>
        /// 停止线程事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                this.TestThead();   //验证线程是否存在，如果没有存在将会抛错
                this.EndThread(this.pBarThread);    //结束线程
                this.EndThread(this.showNumThread); //结束线程
            }
            catch (Exception ex)
            {
                //提示错误信息
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        /// <summary>
        /// 终止线程事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnd_Click(object sender, EventArgs e)
        {
            try
            {
                this.TestThead();   //验证线程是否创建
                this.EndThread(this.pBarThread);//结束线程
                this.EndThread(this.showNumThread); //结束线程
                txtNum.Text = "";   //清空文本框内容
                i = 0;  //数字充值
                this.pgBar.Value = 0;//进度条重置
            }
            catch (Exception ex)
            {
                //显示错误信息
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 执行指定线程停顿时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStopMinute_Click(object sender, EventArgs e)
        {
            try
            {
                int j = int.Parse(txtNum.Text);   //获取终止的时间
                Thread.Sleep(j);    //将线程暂停指定的时间
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 验证线程是否存在方法
        /// </summary>
        private void TestThead()
        {
            if (pBarThread == null)
            {
                throw new Exception("未创建线程，请创建线程后操作！");
            }

            if (showNumThread == null)
            {
                throw new Exception("未创建线程，请创建线程后操作！");
            }
        }
    }
}
