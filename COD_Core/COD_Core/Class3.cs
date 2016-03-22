using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace COD_Base
{
    class Class3
    {
        public void print()
        {
            StackTrace st = new StackTrace();
            string ni = st.GetFrame(1).ToString();
            Console.Out.WriteLine(st.GetFrame(1).GetType() + " " + st.GetFrame(1).GetMethod());
        }
    }
}
