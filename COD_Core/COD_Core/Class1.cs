using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using COD_Base.Core;


namespace COD_Base
{
    class Class1
    {

        public static readonly object mutex = new object();
        public static void Main(string[] Args)
        {
            Class1 c1 = new Class1();
            Thread t1 = new Thread(c1.thread1func);
            Thread t2 = new Thread(c1.thread2Func);
            t1.Start();
            t2.Start();
            t1.Join();
        }

        public void thread1func()
        {
            
        }

        public void thread2Func()
        {
            Thread.Sleep(1000);
            if (Monitor.TryEnter(mutex))
            {
                Console.WriteLine("Try and in");
                Monitor.Exit(mutex);
            }
            else
            {
                Console.WriteLine("Try and cannot get in");
            }
        }
    }
}
