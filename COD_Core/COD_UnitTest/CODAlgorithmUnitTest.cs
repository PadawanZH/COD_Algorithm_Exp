using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ContinuousOutlierDetection;
using COD_Base.Core;
using COD_Base;

namespace COD_UnitTest
{
    [TestFixture]
    public class CODAlgorithmUnitTest
    {
        AlgorithmMgr algHandler = new AlgorithmMgr();
        [Test]
        public void TestEventQueue()
        {
            /*algHandler._algorithm = new BasicContinuousOutlierDetection();
            Configuration configuration = new Configuration();
            configuration.SetProperty(PropertiesType.DataFilePath, @"E:\Workspace\C#\COD_Algorithm_Exp\COD_Core\NormalizeData\bin\Debug\newData.txt");
            configuration.SetProperty(PropertiesType.DataDimension, 2);
            configuration.SetProperty(PropertiesType.Delimiter, ' ');
            configuration.SetProperty(PropertiesType.WindowSize, 10000);
            configuration.SetProperty(PropertiesType.SlideSpan, 1);
            configuration.SetProperty(PropertiesType.QueryRange, 1.41);
            configuration.SetProperty(PropertiesType.KNeighbourThreshold, 50);
            if (algHandler.InitComponent(configuration))
            {
                algHandler.Start();
                Console.ReadKey();
            }*/

            BasicContinuousOutlierDetection basic = new BasicContinuousOutlierDetection();
            SortedSet<CODEvent> CODEventQueue = new SortedSet<CODEvent>(new CODEventComparor());

            for(int j = 10; j > 0; j--)
            {
                for (int i = 10; i > 0; i--)
                {
                    COD_Base.Dynamic.Entity.Tuple t = new COD_Base.Dynamic.Entity.Tuple();
                    t.ArrivalStep = i;
                    ContinuousOutlierDetection.CODTuple codtuple = new CODTuple(t);
                    CODEvent codE = new CODEvent(codtuple, new CODEventTrigger(j,i));
                    CODEventQueue.Add(codE);
                }
            }

            return;

        }

        public static void Main()
        {
            CODAlgorithmUnitTest c = new CODAlgorithmUnitTest();
            c.TestEventQueue();
        }
    }
}
