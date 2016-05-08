using System;
using System.IO;
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
    public class AlgorithmMgr : IListener
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
        public string ID
        {
            get
            {
                return _algorithm.GetType().ToString();
            }
        }

        public IAlgorithm _algorithm;
        public IDataAdapter _dataSource;
        public Configuration _config;

        public int processedTupleCount;
        public int outlierHistoryCount;
        public double avgOutlierRateAgainstWindowSize;
        public int windowSlideCount;
        public int outliersCountInCurrentWindow;

        protected int windowSize;

        protected Dictionary<int, IExpMetrics> _metricsForAlgorithm;

        /// <summary>
        /// 作为检测出的outlier的行数的list，作为算法间比较的依据
        /// </summary>
        public List<int> OutlierIndexList;

        public long startTime;
        public long endTime;

        public AlgorithmMgr(IAlgorithm algorithm, IDataAdapter dataSource, Configuration _config)
        {
            Init();

            _algorithm = algorithm;
            _dataSource = dataSource;
            windowSize = (int)_config.GetProperty(PropertiesType.WindowSize);
        }

        public AlgorithmMgr()
        {
            Init();
        }

        protected void Init()
        {
            EventType[] acceptedEventTypeList = { EventType.NewTupleArrive, EventType.NoMoreTuple, EventType.InlierBecomeOutlier, EventType.OutlierBecomeInlier, EventType.WindowSlide };
            EventDistributor.GetInstance().SubcribeListenerWithFullAcceptedTypeList(this, acceptedEventTypeList);

            processedTupleCount = 0;
            outlierHistoryCount = 0;
            avgOutlierRateAgainstWindowSize = 0;
            windowSlideCount = 0;
            windowSize = 0;
            outliersCountInCurrentWindow = 0;
            OutlierIndexList = new List<int>();
        }

        public bool AddAlgorithm(Dictionary<string, string> modelInfo)
        {
            string oldDirectory = Directory.GetCurrentDirectory();
            try
            {
                Directory.SetCurrentDirectory(modelInfo["workingDirectory"]);
                AssemblySupport.LoadAssembly(modelInfo["workingDirectory"], modelInfo["assemblyPath"]);
                object obj = AssemblySupport.GetNewInstance(modelInfo["algorithm"]);
                if (obj is IAlgorithm)
                {
                    _algorithm = (IAlgorithm)obj;
                }
                else
                {
                    throw new Exception("Error occur while adding algorithm");
                }
                return true;
            }
            catch(Exception e)
            {
                ExceptionUtil.SendErrorEventAndLog(GetType().ToString(), e.Message);
                throw e;
            }
            finally
            {
                Directory.SetCurrentDirectory(oldDirectory);
            }
        }

        public void AddDataDource(IDataAdapter dataSource)
        {
            _dataSource = dataSource;
        }

        public void AddConfiguration(Configuration config)
        {
            _config = config;
            ReadConfig(config);
        }

        protected void ReadConfig(Configuration config)
        {
            windowSize = (int)_config.GetProperty(PropertiesType.WindowSize);
        }

        public double TotalTimeInSec
        {
            get
            {
                return (endTime - startTime) / 10000000.0;
            }
        }

        public Dictionary<int, IExpMetrics> MetricsForAlgorithms
        {
            get
            {
                return _metricsForAlgorithm;
            }
        }

        /// <summary>
        /// 返回是否可以开始运行，若为true，则调用方应当可以直接调用该类中Start函数而不会出错
        /// </summary>
        /// <returns></returns>
        public bool IsReadyToRun()
        {
            if (_config != null && _config.GetProperty(PropertiesType.WindowSize) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {

        }

        public void OnDisposal()
        {

        }

        public static bool ValidateAlgorithmClassWithFullDLLPath(string workingDirectory, string assemblyPath, string classname)
        {
            string oldDirectory = Directory.GetCurrentDirectory();
            try
            {
                Directory.SetCurrentDirectory(workingDirectory);
                AssemblySupport.LoadAssembly(workingDirectory, assemblyPath);
                object obj = AssemblySupport.GetNewInstance(classname);
                return (obj is IAlgorithm);
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                Directory.SetCurrentDirectory(oldDirectory);
            }
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

                    ITuple outlierTuple = (ITuple)anEvent.GetAttribute(EventAttributeType.Tuple);
                    if(outlierTuple != null)
                    {
                        OutlierIndexList.Add(outlierTuple.ArrivalStep);
                    }
                    break;

                case EventType.WindowSlide:
                    windowSlideCount++;
                    double outlierRateAgainstWindowSize = outliersCountInCurrentWindow / (double)windowSize;
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