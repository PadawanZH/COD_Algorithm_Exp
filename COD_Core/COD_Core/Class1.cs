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
            Hashtable ht = new Hashtable();
            if(ht["fun"] == null)
            {
                Console.WriteLine("11");
            }
            Console.ReadKey();
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
