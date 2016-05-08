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
    class PerformanceTest
    {
        protected List<AlgorithmMgr> _algorithmMgrList;
        /// <summary>
        /// 算法线程引用列表，方便对线程进行操作，目前暂时没有实现的需求
        /// </summary>
        protected List<Thread> _algorithmThreadList;
        
        protected int MaxAlgorithmID;

        /// <summary>
        /// 在<see cref="GetInstance"/>的lazy实例化后调用，用以初始化一些变量
        /// </summary>
        public virtual void Initialize()
        {
            MaxAlgorithmID = -1;
        }

        public List<AlgorithmMgr> AlgorithmMgrs
        {
            get
            {
                return _algorithmMgrList;
            }
        }

        public void StartSimulation()
        {
            foreach (AlgorithmMgr algorithmMgr in _algorithmMgrList)
            {
                if (algorithmMgr.IsReadyToRun())
                {
                    Thread thread = new Thread(() => algorithmMgr.Start());
                    thread.IsBackground = true;
                    thread.Start();
                }
                else
                {
                    ExceptionUtil.SendErrorEventAndLog(GetType().ToString(), "AlgorithmMgr" + algorithmMgr.ID + "is NOT READY TO RUN");
                }
            }
        }

        /// <summary>
        /// 释放引用算法分配的内存及自己的内存，在<see cref="EventType.NoMoreTuple"/>事件发出后调用
        /// </summary>
        protected void OnDisposal()
        {
            foreach (AlgorithmMgr algorithmMgr in _algorithmMgrList)
            {
                algorithmMgr.OnDisposal();
            }
        }

        public void RegistAlgorithm(AlgorithmMgr newAlgorithmMgr)
        {
            _algorithmMgrList.Add(newAlgorithmMgr);
        }

        protected void HandleUnknownEventType(string eventType)
        {
            ExceptionUtil.SendErrorEventAndLog(GetType().ToString(), "A Unknown type of event was send to algorithmMgr,EventType : " + eventType);
        }
    }
}
