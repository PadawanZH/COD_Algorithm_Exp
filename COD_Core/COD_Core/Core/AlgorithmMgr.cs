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
    /// 需要解决的问题：Metric的计算维护，Error事件还没有处理者(这是前台的问题了)
    /// </summary>
    class AlgorithmMgr : IAlgorithmManager
    {
        private static AlgorithmMgr instance;
        private AlgorithmMgr()
        {
            _algorithmList = new ArrayList();
            _metricsForAlgorithm = new Hashtable();
            ID = GetType().ToString();
            Logger.GetInstance().Info(ID, " instantiated.");
        }
        public static AlgorithmMgr GetInstance()
        {
            if(instance == null)
            {
                instance = new AlgorithmMgr();
                instance.Initialize();
            }
            return instance;
        }

        private ArrayList _algorithmList;
        /// <summary>
        /// 算法线程引用列表，方便对线程进行操作，目前暂时没有实现的需求
        /// </summary>
        private ArrayList _algorithmThreadList;
        private Hashtable _metricsForAlgorithm;
        private int MaxAlgorithmID;
        public string ID;

        /// <summary>
        /// 在<see cref="GetInstance"/>的lazy实例化后调用，用以初始化一些变量
        /// </summary>
        public void Initialize()
        {
            MaxAlgorithmID = -1;
            EventType[] etList = { EventType.NewTupleArrive, EventType.OldTupleDepart, EventType.WindowSlide, EventType.NoMoreTuple };
            RegistToDistributor(EventDistributor.GetInstance(), etList);
        }

        public ArrayList Algorithms
        {
            get
            {
                return _algorithmList;
            }
        }

        public Hashtable MetricsForAlgorithms
        {
            get
            {
                return _metricsForAlgorithm;
            }
        }

        /// <summary>
        /// 响应并处理事件
        /// </summary>
        /// <param name="anEvent"></param>
        public void OnEvent(IEvent anEvent)
        {
            switch (anEvent.Type)
            {
                case EventType.NewTupleArrive:
                    OnNewTupleArriveEvent( (ITuple)anEvent.GetAttribute(AttributeType.Tuple) );
                    break;

                case EventType.OldTupleDepart:
                    OnOldTupleDepartEvent((ITuple)anEvent.GetAttribute(AttributeType.Tuple));
                    break;

                case EventType.WindowSlide:
                    OnWindowSlide((IWindow)anEvent.GetAttribute(AttributeType.Window));
                    break;

                case EventType.NoMoreTuple:
                    OnDisposal();
                    break;

                default:
                    //无法处理的Event种类
                    HandleUnknownEventType(anEvent.Type.ToString());
                    break;
            }
        }

        /// <summary>
        /// 对于Metric的问题，我想在Algorithm里面实现
        /// </summary>
        /// <param name="p"></param>
        public void OnNewTupleArriveEvent(ITuple p)
        {
            foreach(IAlgorithm algorithm in _algorithmList)
            {
                Thread thread = new Thread(() => algorithm.Arrive(p, StreamSimulator.GetInstance().CurrentStep));
                thread.IsBackground = true;
                thread.Start();
                thread.Join();
            }
        }

        public void OnOldTupleDepartEvent(ITuple p)
        {
            foreach (IAlgorithm algorithm in _algorithmList)
            {
                Thread thread = new Thread(() => algorithm.Departure(p, StreamSimulator.GetInstance().CurrentStep));
                thread.IsBackground = true;
                thread.Start();
                thread.Join();
            }
        }

        public void OnWindowSlide(IWindow newWindow)
        {
            foreach (IAlgorithm algorithm in _algorithmList)
            {
                Thread thread = new Thread(() => algorithm.WindowSlide(newWindow));
                thread.IsBackground = true;
                thread.Start();
                thread.Join();
            }
        }

        /// <summary>
        /// 释放引用算法分配的内存及自己的内存，在<see cref="EventType.NoMoreTuple"/>事件发出后调用
        /// </summary>
        private void OnDisposal()
        {
            foreach (IAlgorithm algorithm in _algorithmList)
            {
                algorithm.Dispose();
            }
        }

        public void RegistAlgorithm(IAlgorithm newAlgorithm)
        {
            _algorithmList.Add(newAlgorithm);
            int algorithmKey = _algorithmList.LastIndexOf(newAlgorithm);
            _metricsForAlgorithm.Add(algorithmKey, new ExpMetrics());
        }
        

        private void HandleUnknownEventType(string eventType)
        {
            Event error = new Event(this.ToString(), EventType.Error);
            string errorMsg = "A Unknown type of event was send to algorithmMgr,EventType : " + eventType;
            error.AddAttribute(AttributeType.ErrorMessage, errorMsg);
            EventDistributor.GetInstance().SendEvent(error);
            Logger.GetInstance().Warn(ID, errorMsg);
        }

        public void RegistToDistributor(IEventDIstributor eventDistributor, EventType[] acceptedEventType)
        {
            eventDistributor.SubcribeListenerWithFullAcceptedTypeList(this, acceptedEventType);
        }
    }
}