using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using COD_Base.Core;


namespace COD_Base
{
    class Class1
    {
        public void ppp(test1 t)
        {
            t.p();
        }
        
        public static void Main(string[] Args)
        {
            Class1 cc1 = new Class1();
            cc1.ppp(new c1());
            cc1.ppp(new c2());
            Console.ReadKey();
        }
    }



    public class c1 : test1
    {
        public void p()
        {
            Console.WriteLine("I am One");
        }
    }

    public class c2 : test1
    {
        public void p()
        {
            Console.WriteLine("I am Two");
        }
    }

    interface test1
    {
        void p();
    }
}
