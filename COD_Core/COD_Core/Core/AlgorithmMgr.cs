using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COD_Base.Interface;
using COD_Base.Util;

namespace COD_Base.Core
{
    /// <summary>
    /// 需要解决的问题：改成多线程模式，Metric的计算维护，是不是还要一个Unregist的方法，Error事件还没有处理者
    /// </summary>
    class AlgorithmMgr : IAlgorithmManager
    {
        private static AlgorithmMgr algMgr = new AlgorithmMgr();
        private AlgorithmMgr()
        {
            _algorithmList = new ArrayList();
            _metricsForAlgorithm = new Hashtable();
            MaxAlgorithmID = -1;
        }
        public AlgorithmMgr GetInstance()
        {
            return algMgr;
        }

        private ArrayList _algorithmList;
        private Hashtable _metricsForAlgorithm;
        private int MaxAlgorithmID;
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

                default:
                    //无法处理的Event种类
                    HandleUnknownEventType(anEvent.Type.ToString());
                    break;
            }
        }

        public void OnNewTupleArriveEvent(ITuple p)
        {
            foreach(IAlgorithm algorithm in _algorithmList)
            {
                algorithm.Arrive(p, p.ArrivalStep);
            }
        }

        public void OnOldTupleDepartEvent(ITuple p)
        {
            foreach (IAlgorithm algorithm in _algorithmList)
            {
                algorithm.Departure(p, p.ArrivalStep);
            }
        }

        public void OnWindowSlide(IWindow newWindow)
        {
            foreach (IAlgorithm algorithm in _algorithmList)
            {
                algorithm.WindowSlide(newWindow);
            }
        }

        public void RegistAlgorithm(IAlgorithm newAlgorithm)
        {
            _algorithmList.Add(newAlgorithm);
            int algorithmKey = _algorithmList.LastIndexOf(newAlgorithm);
            _metricsForAlgorithm.Add(algorithmKey, new ExpMetrics());
        }
        
        /// <summary>
        /// 由于采用单例模式，不需要注册了
        /// </summary>
        /// <param name="eventDistributor"></param>
        public void RegistToDistributor(IEventDIstributor eventDistributor)
        {
            throw new NotImplementedException();
        }

        private void HandleUnknownEventType(string eventType)
        {
            Event error = new Event(this.ToString(), EventType.Error);
            string errorMsg = "A Unknown type of event was send to algorithmMgr,EventType : " + eventType;
            error.AddAttribute(AttributeType.ErrorMessage, errorMsg);
            EventDistributor.GetInstance().SendEvent(error);
            Logger.GetInstance().Warn(errorMsg);
        }

    }
}
