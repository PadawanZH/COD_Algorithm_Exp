using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COD_Base.Interface;
using COD_Base.Util;
using COD_Base.Core;

namespace ContinuousOutlierDetection
{
    public class CODTuple
    {
        public ITuple tuple;
        public int numberOfSucceedingNeighbour;
        //key: ID, value: expTime
        public Dictionary<int, int> preceedingNeighboursExpTime;

        public static int duplicateCount = 0;

        public CODTuple(ITuple t)
        {
            tuple = t;
            numberOfSucceedingNeighbour = 0;
            preceedingNeighboursExpTime = new Dictionary<int, int>();
        }

        public CODEventTrigger FindMinExpTime()
        {
            int minTime = int.MaxValue;
            int PreceedingNeighborID = -1;
            foreach (KeyValuePair<int, int> kvp in preceedingNeighboursExpTime)
            {
                if (minTime > kvp.Value)
                {
                    minTime = kvp.Value;
                    PreceedingNeighborID = kvp.Key;
                }
            }
            return new CODEventTrigger(minTime,PreceedingNeighborID);
        }

        /// <summary>
        /// 用于在Outlier变成Inlier时
        /// </summary>
        /// <param name="currentStep"></param>
        public void ValidatePrecList(int currentStep)
        {
            List<int> InvalidKeyList = new List<int>();
            foreach (KeyValuePair<int, int> kvp in preceedingNeighboursExpTime)
            {
                if (kvp.Value <= currentStep)
                {
                    InvalidKeyList.Add(kvp.Key);
                }
            }
            foreach(int invalidKey in InvalidKeyList)
            {
                preceedingNeighboursExpTime.Remove(invalidKey);
            }
        }
        

        public void DeleteFromPreceedingExpTime(int tupleID)
        {
            if (preceedingNeighboursExpTime.Remove(tupleID) == false)
            {
                duplicateCount++;
                //throw new Exception("Algorithm " + GetType().ToString() + " is tring to DeleteFromPreceedingExpTime with the tupleID is" + tupleID + ". And the owner of this dictoinary is " + tuple.ID);
            }
        }

        public void Dispose()
        {
            tuple.Dispose();
            tuple = null;
            preceedingNeighboursExpTime.Clear();
            preceedingNeighboursExpTime = null;
        }
    }

    public class CODTupleComparorByExpTime : IComparer<CODTuple>
    {
        public int Compare(CODTuple x, CODTuple y)
        {
            if (x.tuple.DepartStep < y.tuple.DepartStep)
            {
                return 1;
            }
            else if (x.tuple.DepartStep == y.tuple.DepartStep)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
    }


    public class CODEventTrigger
    {
        public int eventTime;
        public int triggerTupleID;

        public CODEventTrigger(int eventTime, int triggerTupleID)
        {
            this.eventTime = eventTime;
            this.triggerTupleID = triggerTupleID;
        }
    }

    public class CODEvent
    {
        public CODTuple codTuple;
        public CODEventTrigger eventTrigger;

        public CODEvent(CODTuple t, CODEventTrigger e)
        {
            codTuple = t;
            eventTrigger = e;
        }

        public void Dispose()
        {
            codTuple.Dispose();
        }
    }

    /// <summary>
    /// 应当是从小到大排序(当eventTime相等时按tuple到达顺序排序)
    /// </summary>
    public class CODEventComparor : IComparer<CODEvent>
    {
        public int Compare(CODEvent x, CODEvent y)
        {
            if (x.eventTrigger.eventTime > y.eventTrigger.eventTime)
            {
                return 1;
            }
            else if (x.eventTrigger.eventTime == y.eventTrigger.eventTime)
            {
                if(x.eventTrigger.triggerTupleID > y.eventTrigger.triggerTupleID)
                {
                    return 1;
                }
                else if(x.eventTrigger.triggerTupleID == y.eventTrigger.triggerTupleID)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }
    }

    /// <summary>
    /// 该Comparor是原文中的实现方式，仅比较eventTime
    /// </summary>
    public class CODEventOriginalComparor : IComparer<CODEvent>
    {
        public int Compare(CODEvent x, CODEvent y)
        {
            if (x.eventTrigger.eventTime > y.eventTrigger.eventTime)
            {
                return 1;
            }
            else if (x.eventTrigger.eventTime == y.eventTrigger.eventTime)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
    }
    /// <summary>
    /// WindowSlide和Departure应当在Algorithm内部完成
    /// 这里利用C#赋值时使用的浅拷贝，修改tuple相当于直接修改维护在Window中的tuple
    /// </summary>
    public class BasicContinuousOutlierDetection : IAlgorithm
    {

        public List<int> outliers = new List<int>();
        public IConfiguration _config;

        public Queue<CODTuple> window;

        /// <summary>
        /// 注意，问题在于CODEvent不能重复，就要保证CODEvent中的CODTuple不能是一样的
        /// </summary>
        public SortedSet<CODEvent> CODEventQueue;

        protected int _slideSpan;
        protected int _windowSize;
        protected int currentStep;

        protected double range;
        protected int neighbourThreshold;

        /// <summary>
        /// 实验数据
        /// </summary>
        public int SafeInlierCount = 0;
        public double TimeUsedByQueryRange = 0;
        public bool SendingEventToOutside = true;

        public BasicContinuousOutlierDetection()
        {
            _config = null;
        }

        public void Initialize(IConfiguration config)
        {
            if(     config != null
                && config.GetProperty(PropertiesType.SlideSpan) != null
                && config.GetProperty(PropertiesType.WindowSize) != null
                && config.GetProperty(PropertiesType.QueryRange) != null
                && config.GetProperty(PropertiesType.KNeighbourThreshold) != null)
            {
                _config = config;
                currentStep = 0;
                _slideSpan = (int)config.GetProperty(PropertiesType.SlideSpan);
                _windowSize = (int)config.GetProperty(PropertiesType.WindowSize);

                range = (double)config.GetProperty(PropertiesType.QueryRange);
                neighbourThreshold = (int)config.GetProperty(PropertiesType.KNeighbourThreshold);

                window = new Queue<CODTuple>();
                CODEventQueue = new SortedSet<CODEvent>(new CODEventComparor());
                //CODEventQueue = new SortedSet<CODEvent>(new CODEventOriginalComparor()); 
            }
            else
            {
                ExceptionUtil.SendErrorEventAndLog(GetType().ToString(), "初始算法错误，config中信息不足");
            }
            
        }

        public bool IsReadyToRun()
        {
            if (    _config != null
                &&  _config.GetProperty(PropertiesType.SlideSpan) != null
                &&  _config.GetProperty(PropertiesType.WindowSize) != null
                &&  _config.GetProperty(PropertiesType.QueryRange) != null
                && _config.GetProperty(PropertiesType.KNeighbourThreshold) != null
                && CODEventQueue != null
                && window != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string Description
        {
            get
            {
                return GetType().ToString();
            }
        }

        /// <summary>
        /// 该函数为对外接口，负责接收新到的tuple
        /// </summary>
        /// <param name="newTuple"></param>
        public void ReceiveNewTupe(ITuple newTuple)
        {
            newTuple.ArrivalStep = currentStep;
            newTuple.DepartStep = ComputeDepartStep();
            newTuple.IsOutlier = false;

            if (SendingEventToOutside)
            {
                Event anEvent = new Event(GetType().ToString(), EventType.NewTupleArrive);
                anEvent.AddAttribute(EventAttributeType.Tuple, newTuple);
                EventDistributor.GetInstance().SendEvent(anEvent);
            }
            //目前一步一个tuple，故可以如此赋值
            CODTuple newCODTuple = new CODTuple(newTuple);
            if (ShouldWindowSlide())
            {
                SlideWindow();
            }

            //Arrive函数是否要在窗口弹出之前执行呢？我觉得是在之后，因为窗口的定义为”总是维护最近的n个object“
            //而arrive应当是以window之内作为执行范围的，若在窗口弹出前执行，岂不是Arrive的执行范围变成了n+1? 所以我将Arrive放在这里
            Arrive(newCODTuple, currentStep);

            AddTupleIntoWindow(newCODTuple);
            currentStep++;
        }

        /// <summary>
        /// 先RangeQuery，再将Tuple加入window
        /// </summary>
        /// <param name="newCODTuple"></param>
        /// <param name="currentStep"></param>
        public void Arrive(CODTuple newCODTuple, int currentStep)
        {
            long st = DateTime.Now.Ticks;
            SortedSet<CODTuple> neighbours = RangeQuery(newCODTuple.tuple, range, neighbourThreshold);
            TimeUsedByQueryRange += (DateTime.Now.Ticks - st) / (10000000.0);
            for (int i = 0; i < neighbours.Count; i++)
            {
                CODTuple q = neighbours.ElementAt(i);
                q.numberOfSucceedingNeighbour++;

                if(q.numberOfSucceedingNeighbour == neighbourThreshold)
                {
                    SafeInlierCount++;
                }

                if (q.tuple.IsOutlier && (q.numberOfSucceedingNeighbour + q.preceedingNeighboursExpTime.Count == neighbourThreshold))
                {
                    //由于EventQueue不能通知原本是Outlier的邻居，故原本是Outlier的tuple仍然可能保存着已过期的邻居，这里就是删除可能的已过期前驱邻居，保证正确性
                    q.ValidatePrecList(currentStep);
                    if(q.numberOfSucceedingNeighbour + q.preceedingNeighboursExpTime.Count == neighbourThreshold)
                    {
                        RemoveFromOutlier(q.tuple);

                        if (q.preceedingNeighboursExpTime.Count > 0)
                        {
                            Insert(q);
                        }
                    }
                }
            }
            AssignKNearestPreceedingNeighboursToTuple(newCODTuple, neighbours);

            ///
            if (newCODTuple.preceedingNeighboursExpTime.Count < neighbourThreshold)
            {
                AddToOutlier(newCODTuple.tuple);
            }
            else
            {
                Insert(newCODTuple);
            }
            //AddTupleIntoWindow(newCODTuple); //在ReceiveTuple中调用加入Window的动作
        }

        public int MisCalTimeCount = 0;
        public int ExtractCount = 0;
        public void Departure(CODTuple oldTuple, int currentStep)
        {
            CODEvent x = FindMin();
            if(x != null)
            {
                while (x.eventTrigger.eventTime == currentStep && FindMin() != null )
                {

                    if (FindMin().codTuple.preceedingNeighboursExpTime.Keys.Contains(oldTuple.tuple.ID))
                    {
                        x = ExtractMin();
                        ExtractCount++;
                        x.codTuple.DeleteFromPreceedingExpTime(oldTuple.tuple.ID);

                        if (x.codTuple.numberOfSucceedingNeighbour + x.codTuple.preceedingNeighboursExpTime.Count < neighbourThreshold)
                        {
                            AddToOutlier(x.codTuple.tuple);
                        }
                        else
                        {
                            //update the event time for next check
                            Insert(x.codTuple);
                        }
                        x = FindMin();
                    }
                    else
                    {
                        MisCalTimeCount++;
                        break;
                    }
                }
            }
            
            //free the memory
            oldTuple.Dispose();
        }

        public void DepartureWithNotConsiderSlideSpan(CODTuple oldTuple, int currentStep)
        {
            CODEvent x = FindMin();
            if (x != null)
            {
                while (x.eventTrigger.eventTime == currentStep && FindMin() != null)
                {
                    x = ExtractMin();
                    ExtractCount++;
                    if (x.codTuple.preceedingNeighboursExpTime.Keys.Contains(oldTuple.tuple.ID))
                    {
                        x.codTuple.DeleteFromPreceedingExpTime(oldTuple.tuple.ID);

                        if (x.codTuple.numberOfSucceedingNeighbour + x.codTuple.preceedingNeighboursExpTime.Count < neighbourThreshold)
                        {
                            AddToOutlier(x.codTuple.tuple);
                        }
                        else
                        {
                            //update the event time for next check
                            Insert(x.codTuple);
                        }
                        x = FindMin();
                    }
                    else
                    {
                        //把x加回去
                        CODEventQueue.Add(x);
                    }
                }
            }

            //free the memory
            oldTuple.Dispose();
        }

        public void DepartureNotIncludingSafeInlier(CODTuple oldTuple, int currentStep)
        {
            CODEvent x = FindMin();
            if (x != null)
            {
                while (x.eventTrigger.eventTime == currentStep && FindMin() != null)
                {
                    if (FindMin().codTuple.preceedingNeighboursExpTime.Keys.Contains(oldTuple.tuple.ID))
                    {
                        x = ExtractMin();
                        ExtractCount++;
                        x.codTuple.DeleteFromPreceedingExpTime(oldTuple.tuple.ID);

                        if (x.codTuple.numberOfSucceedingNeighbour + x.codTuple.preceedingNeighboursExpTime.Count < neighbourThreshold)
                        {
                            AddToOutlier(x.codTuple.tuple);
                        }
                        else if (x.codTuple.numberOfSucceedingNeighbour < neighbourThreshold)
                        {
                            //update the event time for next check
                            Insert(x.codTuple);
                        }
                        else if(x.codTuple.numberOfSucceedingNeighbour >= neighbourThreshold)
                        {
                            SavedCount++;
                        }
                        x = FindMin();
                    }
                    else
                    {
                        MisCalTimeCount++;
                        break;
                    }
                }
            }
        }

        public int SavedCount = 0;
        public void DepartureNotDuplicateCompute(CODTuple oldTuple, int currentStep)
        {
            CODEvent x = FindMin();
            while (x != null && x.eventTrigger.eventTime < currentStep)
            {
                x = ExtractMin();
            }

            x = FindMin();
            if (x != null)
            {
                while (x.eventTrigger.eventTime == currentStep && FindMin() != null)
                {
                    if (FindMin().codTuple.preceedingNeighboursExpTime.Keys.Contains(oldTuple.tuple.ID))
                    {
                        x = ExtractMin();
                        ExtractCount++;
                        x.codTuple.DeleteFromPreceedingExpTime(oldTuple.tuple.ID);

                        if (x.codTuple.numberOfSucceedingNeighbour + x.codTuple.preceedingNeighboursExpTime.Count < neighbourThreshold)
                        {
                            AddToOutlier(x.codTuple.tuple);
                        }
                        else if (x.codTuple.numberOfSucceedingNeighbour < neighbourThreshold)
                        {
                            //update the event time for next check
                            Insert(x.codTuple);
                        }
                        else if (x.codTuple.numberOfSucceedingNeighbour >= neighbourThreshold)
                        {
                            SavedCount++;
                        }
                        x = FindMin();
                    }
                    else
                    {
                        MisCalTimeCount++;
                        break;
                    }

                }
            }
        }

        public void Dispose()
        {
            window.Clear();
            window = null;
            foreach (CODEvent codEvent in CODEventQueue)
            {
                codEvent.Dispose();
            }
            CODEventQueue.Clear();
            CODEventQueue = null;
        }

        protected SortedSet<CODTuple> RangeQuery(ITuple newTuple, double range, int neighbourThreshold)
        {
            double distance;
            double qurc_rang = range * range;
            bool flag;
            SortedSet<CODTuple> neighbours = new SortedSet<CODTuple>(new CODTupleComparorByExpTime());
            foreach (CODTuple codTuple in window)
            {
                flag = false;
                distance = 0;
                for (int i = 0; i < codTuple.tuple.Dimension; i++)
                {
                    distance += (newTuple.Data[i] - codTuple.tuple.Data[i])* (newTuple.Data[i] - codTuple.tuple.Data[i]);
                    if (distance > qurc_rang)
                    {
                        flag = true;
                        break;
                    }
                }
                if(flag == false)
                {
                    neighbours.Add(codTuple);
                }
            }
            return neighbours;
        }

        public void RemoveFromOutlier(ITuple tuple)
        {
            tuple.IsOutlier = false;
            if (SendingEventToOutside)
            {
                Event e = new Event("an tuple become inlier", EventType.OutlierBecomeInlier);
                e.AddAttribute(EventAttributeType.TupleID, tuple.ID);
                EventDistributor.GetInstance().SendEvent(e);
            }
        }

        public void AddToOutlier(ITuple tuple)
        {
            tuple.IsOutlier = true;
            if (SendingEventToOutside)
            {
                Event e = new Event("an tuple become outlier", EventType.InlierBecomeOutlier);
                e.AddAttribute(EventAttributeType.TupleID, tuple.ID);
                EventDistributor.GetInstance().SendEvent(e);
            }
            outliers.Add(tuple.ID);
        }

        public void SlideWindow()
        {
            
            for (int i = 0; i < _slideSpan; i++)
            {
                CODTuple oldTuple = window.Dequeue();
                if (SendingEventToOutside)
                {
                    Event anEvent = new Event("Object Depart", EventType.OldTupleDepart);
                    anEvent.AddAttribute(EventAttributeType.TupleID, oldTuple.tuple.ID);
                    anEvent.AddAttribute(EventAttributeType.Tuple, oldTuple.tuple);
                    EventDistributor.GetInstance().SendEvent(anEvent);
                }
                //Departure(oldTuple, currentStep);
                DepartureNotIncludingSafeInlier(oldTuple, currentStep);
                //DepartureWithNotConsiderSlideSpan(oldTuple, currentStep);
            }

            if (SendingEventToOutside)
            {
                Event e = new Event("window has just Slided", EventType.WindowSlide);
                EventDistributor.GetInstance().SendEvent(e);
            }
        }

        public void AssignKNearestPreceedingNeighboursToTuple(CODTuple newTuple, SortedSet<CODTuple> neighbours)
        {
            Dictionary<int, int> k_NearestNeighbour = new Dictionary<int, int>();
            for (int i = 0; i < neighbourThreshold && i < neighbours.Count; i++)
            {
                int neighbourID, neighbourExpTime;
                neighbourID = neighbours.ElementAt(i).tuple.ID;
                neighbourExpTime = neighbours.ElementAt(i).tuple.DepartStep;
                k_NearestNeighbour.Add(neighbourID, neighbourExpTime);
            }
            newTuple.preceedingNeighboursExpTime = k_NearestNeighbour;
        }

        public void AddTupleIntoWindow(CODTuple newTuple)
        {
            window.Enqueue(newTuple);
        }

        public bool ShouldWindowSlide()
        {
            if (window.Count == _windowSize)
            {
                return true;
            }
            else if (window.Count < _windowSize)
            {
                return false;
            }
            else
            {
                throw new Exception("Window count is " + window.Count + " exceed the windowSize");
            }
        }

        /// <summary>
        /// 要求currentStep在将Tuple处理完并添加到window之后才加一，否则会影响正确性
        /// </summary>
        /// <returns></returns>
        protected int ComputeDepartStep()
        {
            int departStep = _windowSize + _slideSpan * (int)(currentStep / _slideSpan);
            return departStep;
        }

        #region Functions For EventQueue
        public int insertCount = 0;
        protected void Insert(CODTuple q)
        {
            /*if (q.numberOfSucceedingNeighbour > neighbourThreshold)
            {
                throw new Exception("错误，将SafeInlier插入了");
            }*/
            CODEvent newCODEvent = new CODEvent(q, q.FindMinExpTime());
            if(newCODEvent.eventTrigger.eventTime != int.MaxValue)
            {
                CODEventQueue.Add(newCODEvent);
                insertCount++;
            }
        }

        protected CODEvent FindMin()
        {
            if (CODEventQueue.Count != 0)
            {
                return CODEventQueue.ElementAt(0);
            }
            else
            {
                return null;
            }
        }

        protected CODEvent ExtractMin()
        {
            if (CODEventQueue.Count != 0)
            {
                CODEvent minEvent = CODEventQueue.ElementAt(0);
                CODEventQueue.Remove(minEvent);
                return minEvent;
            }
            else
            {
                return null;
            }
        }

        public int GetEventListCount()
        {
            return CODEventQueue.Count;
        }
        #endregion


    }
}
