using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using COD_Base.Algorithms;

namespace COD_UnitTest
{
    [TestFixture]
    public class CODAlgorithmUnitTest
    {
        ContinuousOutlierDetection algorithm;
        [Test]
        public void TestEventQueue()
        {
            algorithm = new ContinuousOutlierDetection();
            algorithm.Initialize();

            algorithm.CODEventQueue.Add(new CODEvent(new CODTuple(null), 30));
            algorithm.CODEventQueue.Add(new CODEvent(new CODTuple(null), 20));
            algorithm.CODEventQueue.Add(new CODEvent(new CODTuple(null), 100));
            algorithm.CODEventQueue.Add(new CODEvent(new CODTuple(null), 10));

            Assert.AreEqual(algorithm.CODEventQueue.ElementAt(0).eventTime, 10);
            Assert.AreEqual(algorithm.CODEventQueue.ElementAt(1).eventTime, 20);
            Assert.AreEqual(algorithm.CODEventQueue.ElementAt(2).eventTime, 30);
            Assert.AreEqual(algorithm.CODEventQueue.ElementAt(3).eventTime, 100);
        }
    }
}
