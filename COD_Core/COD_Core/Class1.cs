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
            StreamSimulator.GetInstance();
            StreamSimulator.GetInstance().Initialize();
            StreamSimulator.GetInstance().StartSimulationInTimerMode();

            Thread.Sleep(3000);
            StreamSimulator.GetInstance().StopTimerModeSimulation();
            StreamSimulator.GetInstance().StartSimulationInTimerMode();
            Thread.Sleep(3000);
            StreamSimulator.GetInstance().StopTimerModeSimulation();
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
