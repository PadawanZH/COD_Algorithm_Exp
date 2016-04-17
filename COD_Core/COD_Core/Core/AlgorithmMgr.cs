using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COD_Base.Interface;
using COD_Base.Util;
using System.Threading;

namespace COD_Base.Core
{
    /// <summary>
    /// 需要解决的问题：Error事件还没有处理者(这是前台的问题了)
    /// </summary>
    class AlgorithmMgr : IListener
    {
        /*private static AlgorithmMgr instance;
        private AlgorithmMgr()
        {
            _algorithmList = new ArrayList();
            _metricsForAlgorithm = new Hashtable();
            Logger.GetInstance().Info(GetType().ToString(), " instantiated.");
        }
        public static AlgorithmMgr GetInstance()
        {
            if(instance == null)
            {
                instance = new AlgorithmMgr();
                instance.Initialize();
            }
            return instance;
        }*/

        public int processedTupleCount;
        public int outlierHistoryCount;
        public double avgOutlierRateAgainstWindowSize;
        public int windowSlideCount;
        public int outliersCountInCurrentWindow;

        protected bool isConfigurationSet;

        public long startTime;
        public long endTime;

        public AlgorithmMgr()
        {
            EventType[] acceptedEventTypeList = { EventType.NewTupleArrive, EventType.NoMoreTuple, EventType.InlierBecomeOutlier, EventType.OutlierBecomeInlier, EventType.WindowSlide };
            EventDistributor.GetInstance().SubcribeListenerWithFullAcceptedTypeList(this, acceptedEventTypeList);

            processedTupleCount = 0;
            outlierHistoryCount = 0;
            avgOutlierRateAgainstWindowSize = 0;
            windowSlideCount = 0;
            outliersCountInCurrentWindow = 0;

            isConfigurationSet = false;
        }

        public double TotalTimeInSec
        {
            get
            {
                return (endTime - startTime) / 10000000.0;
            }
        }

        public void SetupConfiguration(int WindowSize, int SildeSpan, string DataFilePath, int DataDimension, List<Type> typeListOfDimension, char[] Delimiter, double queryRange, int kNighbourThreshold)
        {

            isConfigurationSet = true;
        }

        public void OnEvent(IEvent anEvent)
        {
            switch (anEvent.Type)
            {
                case EventType.NewTupleArrive:
                    if (processedTupleCount == 0)
                    {
                        startTime = System.DateTime.Now.Ticks;
                    }
                    processedTupleCount++;
                    break;
                case EventType.OutlierBecomeInlier:
                    outliersCountInCurrentWindow--;
                    break;
                case EventType.InlierBecomeOutlier:
                    outlierHistoryCount++;
                    outliersCountInCurrentWindow++;
                    break;
                case EventType.WindowSlide:
                    windowSlideCount++;
                    double outlierRateAgainstWindowSize = outliersCountInCurrentWindow / (double)Configuration.GetInstance().GetProperty(PropertiesType.WindowSize);
                    avgOutlierRateAgainstWindowSize += outlierRateAgainstWindowSize;
                    outliersCountInCurrentWindow = 0;
                    break;
                case EventType.NoMoreTuple:
                    endTime = DateTime.Now.Ticks;
                    if (windowSlideCount > 0)
                    {
                        avgOutlierRateAgainstWindowSize /= (double)windowSlideCount;
                    }
                    else
                    {
                        avgOutlierRateAgainstWindowSize = (double)outliersCountInCurrentWindow / (double)processedTupleCount;
                    }
                    break;
            }
        }
    }
}