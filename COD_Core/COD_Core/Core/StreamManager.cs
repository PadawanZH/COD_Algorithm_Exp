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
    /// 利用生产者消费者提供tuple
    /// </summary>
    class StreamManager : IStreamManager
    {
        private static StreamManager instance;
        private StreamManager()
        {
            isRunningTimerMode = false;
            ToDoBuffer = new Queue<ITuple>();
        }

        public static StreamManager GetInstance()
        {
            if(instance == null)
            {
                instance = new StreamManager();
                instance.Initialize();
            }
            return instance;
        }

        /// <summary>
        /// 对变量初始化，在EventDistributor注册listener等工作
        /// 同时也负责reset工作
        /// </summary>
        public void Initialize()
        {
            _curStreamStep = 0;
            _isProcessOver = false;

            _periodTimeBetweenTuple = 1000;
            millisecondsTimeoutOfWaitLock = _periodTimeBetweenTuple;

            simulateTimer = new Timer(new TimerCallback(TimerThreadFunc), this, Timeout.Infinite, _periodTimeBetweenTuple);
            tupleHandleThread = new Thread(TupleProcessorThreadFunc);

            if(ToDoBuffer != null && ToDoBuffer.Count != 0)
            {
                ToDoBuffer.Clear();
            }

            //StreamSimulator需要NoMoreTuple来停止Timer继续向dataAdapter索要Tuple
            EventType[] acceptedEventTypeList = { EventType.NoMoreTuple };
            EventDistributor.GetInstance().SubcribeListenerWithFullAcceptedTypeList(this, acceptedEventTypeList);
        }

        public void StartSimulationInTimerMode()
        {
            if(!isRunningTimerMode)
            {   
                simulateTimer.Change(0, _periodTimeBetweenTuple);
                Console.WriteLine("Timer Start");
                Thread.Sleep(10);
                tupleHandleThread.Start();
                isRunningTimerMode = true;
            }
            else
            {
                Event e = new Event(GetType().ToString(), EventType.Informative);
                e.AddAttribute(EventAttributeType.Message, "Already running in timer mode");
                EventDistributor.GetInstance().SendEvent(e);
            }
        }

        public void StopTimerModeSimulation()
        {
            if(isRunningTimerMode)
            {
                //To-do 实现停止Timer
                simulateTimer.Change(Timeout.Infinite, _periodTimeBetweenTuple);
                //To-do 重新恢复初始化时各个组件状态（如状态变量、DataAdapter的文件指针等）
                ResetState();

                isRunningTimerMode = false;
            }
            else
            {
                Event e = new Event(GetType().ToString(), EventType.Informative);
                e.AddAttribute(EventAttributeType.Message, "Simulation is not running in timer mode right now");
                EventDistributor.GetInstance().SendEvent(e);
            }
        }

        private void ResetState()
        {
            if (tupleHandleThread.IsAlive)
            {
                tupleHandleThread.Abort();
            }
            _dataAdapter.Disposal();

            Initialize();
        }

        #region properties need to get from CONFIGURATION
        //这些变量应该由Configuration处获得
        protected bool _isProcessOver;
        /// <summary>
        /// time interval of get 
        /// </summary>
        protected int _periodTimeBetweenTuple;
        
        #endregion

        #region state properties
        protected int _curStreamStep;
        /// <summary>
        /// 标记Algorithm是否在Timer的情况下运行（即是否在StreamSimulator的模拟流的情况下运行），当此值为false时，才可在运行<see cref="Experiment.PerformanceTest"/>中的静态数据模式
        /// </summary>
        public bool isRunningTimerMode;
        #endregion
        /// <summary>
        /// 当<see cref="UseSteadyStreamingMode"/>为false时需要实现该buffer
        /// </summary>
        protected Queue<ITuple> ToDoBuffer;

        protected int millisecondsTimeoutOfWaitLock;

        protected Timer simulateTimer;
        protected Thread tupleHandleThread;

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
        protected void TimerThreadFunc(object state)
        {
            ITuple newTuple = _dataAdapter.GetNextTuple();
            if(newTuple != null)
            {
                if (Monitor.TryEnter(mutex))
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
                        throw e;
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

                        PerformAStep(newTuple);
                    }
                }catch(Exception e)
                {
                    ExceptionUtil.SendErrorEventAndLog(GetType().ToString(), e.Message);
                    throw e;
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
            //return 100;
        }

        public bool IsProcessOver
        {
            get
            {
                return _isProcessOver;
            }
        }

        public double StreamRate
        {
            get
            {
                return (1000.0 / (double)_periodTimeBetweenTuple);
            }
            set
            {
                _periodTimeBetweenTuple = (int)(1000.0 / value);
            }
        }

        /// <summary>
        /// 新tuple的到来才导致了streamSimulator的一切状态变化，故以newTuple为参数
        /// </summary>
        /// <param name="newTuple"></param>
        public void PerformAStep(ITuple newTuple)
        {
            _curStreamStep += 1;
            //for test start
            Console.WriteLine( "In Step : " + _curStreamStep + " " + newTuple.Data[0].ToString());
            //for test end

        }

        public void SendEvent(IEvent anEvent)
        {
            throw new NotImplementedException();
        }

        public void PauseSimulate()
        {

        }

        public void OnEvent(IEvent anEvent)
        {
            switch (anEvent.Type)
            {
                case EventType.NoMoreTuple:
                    throw new NotImplementedException();
                    break;

                default:
                    //无法处理的Event种类
                    //HandleUnknownEventType(anEvent.Type.ToString());
                    break;
            }
        }

        /// <summary>
        /// 当<see cref="PerformAStep"/>被调用，且前端需要Stream的状态数据（如真正处理的streamRate等）时，使用此函数实现的东西返回
        /// </summary>
        public void CalAndReportSteamStatus()
        {

        }
    }
}
