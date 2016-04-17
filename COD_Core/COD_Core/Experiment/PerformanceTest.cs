using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using COD_Base.Interface;
using COD_Base.Core;
using COD_Base.Util;

namespace COD_Base.Experiment
{
    /// <summary>
    /// 由PerformTest来维护算法列表，AlgorithmMgr只维护一个算法的运行，从读文件到计算，再到出结果
    /// </summary>
    class PerformanceTest : IListener
    {
        protected List<IAlgorithm> _algorithmList;
        /// <summary>
        /// 算法线程引用列表，方便对线程进行操作，目前暂时没有实现的需求
        /// </summary>
        protected List<Thread> _algorithmThreadList;
        protected Dictionary<int,Metrics> _metricsForAlgorithm;
        protected int MaxAlgorithmID;

        /// <summary>
        /// 在<see cref="GetInstance"/>的lazy实例化后调用，用以初始化一些变量
        /// </summary>
        public void Initialize()
        {
            MaxAlgorithmID = -1;
            EventType[] acceptedEventTypeList = { EventType.NewTupleArrive, EventType.NoMoreTuple };
            EventDistributor.GetInstance().SubcribeListenerWithFullAcceptedTypeList(this, acceptedEventTypeList);
        }

        public List<IAlgorithm> Algorithms
        {
            get
            {
                return _algorithmList;
            }
        }

        public Dictionary<int, Metrics> MetricsForAlgorithms
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
                    OnNewTupleArriveEvent((ITuple)anEvent.GetAttribute(EventAttributeType.Tuple));
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
            foreach (IAlgorithm algorithm in _algorithmList)
            {
                Thread thread = new Thread(() => algorithm.ReceiveNewTupe(p));
                thread.IsBackground = true;
                thread.Start();
                thread.Join();
            }
        }

        /// <summary>
        /// 释放引用算法分配的内存及自己的内存，在<see cref="EventType.NoMoreTuple"/>事件发出后调用
        /// </summary>
        protected void OnDisposal()
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
            _metricsForAlgorithm.Add(algorithmKey, new Metrics());
        }


        protected void HandleUnknownEventType(string eventType)
        {
            ExceptionUtil.SendErrorEventAndLog(GetType().ToString(), "A Unknown type of event was send to algorithmMgr,EventType : " + eventType);
        }
    }
}
