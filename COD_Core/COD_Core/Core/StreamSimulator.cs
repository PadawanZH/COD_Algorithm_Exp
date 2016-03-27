using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using COD_Base.Interface;
using COD_Base.Util;

namespace COD_Base.Core
{
    /// <summary>
    /// 生产者消费者的正确性有待检验
    /// </summary>
    class StreamSimulator : IStreamSimulator
    {
        private static StreamSimulator instance;
        private StreamSimulator()
        {
            
        }

        public static StreamSimulator GetInstance()
        {
            if(instance == null)
            {
                instance = new StreamSimulator();
                instance.Initialize();
            }
            return instance;
        }

        /// <summary>
        /// 对变量初始化，在EventDistributor注册listener等工作
        /// </summary>
        public void Initialize()
        {
            ToDoBuffer = new Queue<ITuple>();
            
            millisecondsTimeoutOfWaitLock = 1000;
        }

        //这些变量应该由Configuration处获得
        protected int _curStreamStep;
        protected bool _isProcessOver;
        protected int _slideSpan;
        
        /// <summary>
        /// time interval of get 
        /// </summary>
        protected double _streamRate;
        protected int _windowSize;

        /// <summary>
        /// 当<see cref="UseSteadyStreamingMode"/>为false时需要实现该buffer
        /// </summary>
        protected Queue<ITuple> ToDoBuffer;

        protected int millisecondsTimeoutOfWaitLock;

        /// <summary>
        /// _dataApapter的类型可能需要在界面上赋值（路径 + dataAdapter类型），具体学习SimulationCompution的做法.
        /// 现在为了测试，仅仅直接赋值（在Init中赋值）
        /// </summary>
        protected IDataAdapter _dataAdapter;

        /// <summary>
        /// 用于在Timer和处理每一步的函数之间的互斥关系，若当Timer触发时<see cref="PerformAStep"/>没有处理完成，则Timer将阻塞下一个Tuple到来，即产生BackPressure
        /// </summary>
        protected readonly object mutex = new object();


        /// <summary>
        /// <para>可看做生产者-消费者问题中的生产者</para>
        /// 这里使用lock的办法使得若是由于算法处理慢而跟不上既定StreamRate的tuple产生速度从而出现BackPressure（这里推迟了整个流数据发送，不算是真正的BackPressure），仅仅推迟下一个Tuple的发送。
        /// 还有第二种解决方案（这里没有实现），可以实现一个Buffer，在进入临界区时使用tryLock方式（<see cref="System.Threading.Monitor.TryEnter"/>）,
        /// 若lock已被占用，则将新tuple存入buffer中，产生真正的BackPressure。
        /// </summary>
        protected void TimerThreadFunc()
        {
            ITuple newTuple = _dataAdapter.GetNextTuple();
            if(Monitor.TryEnter(mutex))
            {
                try
                {
                    //critical section start
                    ToDoBuffer.Enqueue(newTuple);
                    //唤醒可能等待的消费者线程
                    Monitor.Pulse(mutex);
                }
                catch (Exception e)
                {
                    ExceptionUtil.SendErrorEventAndLog(GetType().ToString(), "Something occured while timer thread in StreamSimulator try to acquire the mutex lock. Exception Message : " + e.Message);
                }
                finally
                {
                    Monitor.Exit(mutex);
                }
            }
            else
            {
                //没有获得锁表面消费者正在运行，无需唤醒。
                //Queue不是线程安全的，由于这里没有获得锁，所以不会发生死锁的问题
                lock (ToDoBuffer)
                {
                    ToDoBuffer.Enqueue(newTuple);
                }
            }
        }

        /// <summary>
        /// <para>可看做生产者-消费者问题中的消费者</para>
        /// </summary>
        protected void TupleProcessorThreadFunc()
        {
            while (true)
            {
                try
                {
                    Monitor.Enter(mutex);
                    if (ToDoBuffer == null || ToDoBuffer.Count == 0)
                    {
                        //若队列为空，则等待生产者唤醒
                        Monitor.Wait(mutex, millisecondsTimeoutOfWaitLock);
                    }
                    while (ToDoBuffer.Count > 0)
                    {
                        ITuple newTuple;
                        lock (ToDoBuffer)
                        {
                            newTuple = ToDoBuffer.Dequeue();
                        }
                        newTuple.ArrivalStep = CurrentStep;
                        newTuple.DepartStep = GetDepartStep(CurrentStep);
                        newTuple.ID = CurrentStep;

                        PerformAStep(newTuple);
                    }
                }catch(Exception e)
                {
                    ExceptionUtil.SendErrorEventAndLog(GetType().ToString(), e.Message);
                }
                finally
                {
                    Monitor.Exit(mutex);
                }
                
            }
        }

        public int CurrentStep
        {
            get
            {
                return _curStreamStep;
            }
        }

        /// <summary>
        /// 如何获取一个tuple的过期step？
        /// </summary>
        /// <param name="arrivalStep"></param>
        /// <returns></returns>
        public int GetDepartStep(int arrivalStep)
        {
            throw new NotImplementedException();
        }

        public bool IsProcessOver
        {
            get
            {
                return _isProcessOver;
            }
        }

        public int SliedSpan
        {
            get
            {
                return _slideSpan;
            }
        }

        public double StreamRate
        {
            get
            {
                return _streamRate;
            }
            set
            {
                _streamRate = value;
            }
        }

        public int WindowSize
        {
            get
            {
                return _windowSize;
            }
        }

        /// <summary>
        /// 新tuple的到来才导致了streamSimulator的一切状态变化，故以newTuple为参数
        /// </summary>
        /// <param name="newTuple"></param>
        public void PerformAStep(ITuple newTuple)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 负责对Tuple的一些有关流的属性（如到达step，过期step）的初始化，以及事件的发送
        /// </summary>
        public void OnNewTupleArrive()
        {
            throw new NotImplementedException();
        }

        public void OnOldTupleDepart()
        {
            throw new NotImplementedException();
        }

        public void OnWindowSlide()
        {
            throw new NotImplementedException();
        }

        public void SendEvent()
        {
            throw new NotImplementedException();
        }

        public void SendEvent(IEvent anEvent)
        {
            throw new NotImplementedException();
        }

        public void StartStreamSimulate()
        {
            throw new NotImplementedException();
        }

        public void PauseSimulate()
        {

        }

        public void CheckWindow()
        {
            throw new NotImplementedException();
        }

        public void OnEvent(IEvent anEvent)
        {
            throw new NotImplementedException();
        }

        public void RegistToDistributor(IEventDIstributor eventDistributor, EventType[] acceptedEventType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 当<see cref="PerformAStep"/>被调用，且前端需要Stream的状态数据（如真正处理的streamRate等）时，使用此函数实现的东西返回
        /// </summary>
        public void CalAndReportSteamStatus()
        {

        }
    }
}
