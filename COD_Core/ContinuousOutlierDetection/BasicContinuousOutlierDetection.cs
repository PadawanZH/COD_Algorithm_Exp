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

        public CODTuple(ITuple t)
        {
            tuple = t;
            numberOfSucceedingNeighbour = 0;
            preceedingNeighboursExpTime = new Dictionary<int, int>();
        }

        public int FindMinExpTime()
        {
            int minTime = int.MaxValue;
            foreach (int expTime in preceedingNeighboursExpTime.Values)
            {
                if (minTime > expTime)
                {
                    minTime = expTime;
                }
            }
            return minTime;
        }

        public void DeleteFromPreceedingExpTime(int tupleID)
        {
            if (preceedingNeighboursExpTime.Remove(tupleID) == false)
            {
                throw new Exception("Algorithm " + GetType().ToString() + " is tring to DeleteFromPreceedingExpTime with the tupleID is" + tupleID + ". And the owner of this dictoinary is " + tuple.ID);
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

    public class CODEvent
    {
        public CODTuple codTuple;
        public int eventTime;

        public CODEvent(CODTuple t, int e)
        {
            codTuple = t;
            eventTime = e;
        }

        public void Dispose()
        {
            codTuple.Dispose();
        }
    }

    /// <summary>
    /// 应当是从小到大排序
    /// </summary>
    public class CODEventComparor : IComparer<CODEvent>
    {
        public int Compare(CODEvent x, CODEvent y)
        {
            if (x.eventTime > y.eventTime)
            {
                return 1;
            }
            else if (x.eventTime == y.eventTime)
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
        public Configuration _config;

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

        public BasicContinuousOutlierDetection(Configuration config)
        {
            _config = config;
            Initialize();
        }

        public BasicContinuousOutlierDetection()
        {
            _config = null;
        }

        public void Initialize()
        {
            currentStep = 0;
            _slideSpan = (int)_config.GetProperty(PropertiesType.SlideSpan);
            _windowSize = (int)_config.GetProperty(PropertiesType.WindowSize);

            range = (double)_config.GetProperty(PropertiesType.QueryRange);
            neighbourThreshold = (int)_config.GetProperty(PropertiesType.KNeighbourThreshold);

            CODEventQueue = new SortedSet<CODEvent>(new CODEventComparor());
        }

        public bool IsReadyToRun()
        {
            if (    _config != null
                &&  _config.GetProperty(PropertiesType.SlideSpan) != null
                &&  _config.GetProperty(PropertiesType.WindowSize) != null
                &&  _config.GetProperty(PropertiesType.QueryRange) != null
                && _config.GetProperty(PropertiesType.KNeighbourThreshold) != null)
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

        public void FillWindow()
        {
            for(int i = 0; i < _windowSize; i++)
            {

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
            //目前一步一个tuple，故可以如此赋值
            newTuple.ID = currentStep;
            CODTuple newCODTuple = new CODTuple(newTuple);
            if (currentStep >= _windowSize)
            {
                if (ShouldWindowSlide())
                {
                    for (int i = 0; i < _slideSpan; i++)
                    {
                        CODTuple oldTuple = window.Dequeue();
                        Departure(oldTuple, currentStep);
                    }
                }

                //Arrive函数是否要在窗口弹出之前执行呢？我觉得是在之后，因为窗口的定义为”总是维护最近的n个object“
                //而arrive应当是以window之内作为执行范围的，若在窗口弹出前执行，岂不是Arrive的执行范围变成了n+1? 所以我将Arrive放在这里
                Arrive(newCODTuple, currentStep);
            }
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
            Dictionary<CODTuple, double> neighbours = RangeQuery(newCODTuple.tuple, range, neighbourThreshold);
            for (int i = 0; i < neighbours.Keys.Count; i++)
            {
                CODTuple q = neighbours.Keys.ElementAt(i);
                q.numberOfSucceedingNeighbour++;

                if (q.tuple.IsOutlier && (q.numberOfSucceedingNeighbour + q.preceedingNeighboursExpTime.Count == neighbourThreshold))
                {
                    RemoveFromOutlier(q.tuple);

                    //该if语句是否应当在这里还有待考证（【28】中是在这里的，【原文】不在，但是感觉在这里是正确的）
                    if (q.preceedingNeighboursExpTime.Count > 0)
                    {
                        Insert(q);
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
                int eventTriggerTime = newCODTuple.FindMinExpTime();
                Insert(newCODTuple);
            }
            //AddTupleIntoWindow(newCODTuple); //在ReceiveTuple中调用加入Window的动作
        }

        public void Departure(CODTuple oldTuple, int currentStep)
        {
            CODEvent x = FindMin();
            while (x.eventTime == currentStep)
            {
                x = ExtractMin();
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
            }

            //free the memory
            oldTuple.Dispose();
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

        protected Dictionary<CODTuple, double> RangeQuery(ITuple newTuple, double range, int neighbourThreshold)
        {
            double distance = 0;
            Dictionary<CODTuple, double> neighbours = new Dictionary<CODTuple, double>();
            foreach (CODTuple codTuple in window)
            {
                for (int i = 0; i < codTuple.tuple.Dimension; i++)
                {
                    distance += (codTuple.tuple.Data[i] * codTuple.tuple.Data[i]) + (codTuple.tuple.Data[i] * codTuple.tuple.Data[i]);
                    if (distance > range)
                    {
                        continue;
                    }
                }
                neighbours.Add(codTuple, distance);
            }
            return neighbours;
        }

        public void RemoveFromOutlier(ITuple tuple)
        {
            tuple.IsOutlier = false;

            Event e = new Event("an tuple become inlier", EventType.OutlierBecomeInlier);
            e.AddAttribute(EventAttributeType.Tuple, tuple);
            EventDistributor.GetInstance().SendEvent(e);
        }

        public void AddToOutlier(ITuple tuple)
        {
            tuple.IsOutlier = true;

            Event e = new Event("an tuple become outlier", EventType.InlierBecomeOutlier);
            e.AddAttribute(EventAttributeType.Tuple, tuple);
            EventDistributor.GetInstance().SendEvent(e);
        }

        public void AssignKNearestPreceedingNeighboursToTuple(CODTuple newTuple, Dictionary<CODTuple, double> neighbours)
        {
            Dictionary<CODTuple, double> sortedNeighbour = (from entry in neighbours orderby entry.Value ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
            Dictionary<int, int> k_NearestNeighbour = new Dictionary<int, int>();
            for (int i = 0; i < neighbourThreshold && i < sortedNeighbour.Count; i++)
            {
                int neighbourID, neighbourExpTime;
                neighbourID = sortedNeighbour.ElementAt(i).Key.tuple.ID;
                neighbourExpTime = sortedNeighbour.ElementAt(i).Key.tuple.DepartStep;
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
            int departStep = _windowSize + 3 * (int)(currentStep / 3);
            return departStep;
        }

        #region Functions For EventQueue
        protected void Insert(CODTuple q)
        {
            CODEvent newCODEvent = new CODEvent(q, q.FindMinExpTime());
            CODEventQueue.Add(newCODEvent);
        }

        protected CODEvent FindMin()
        {
            if (CODEventQueue.Count != 0)
            {
                return CODEventQueue.ElementAt(0);
            }
            else
            {
                throw new Exception("Algorithm " + GetType().ToString() + " is tring to \"findMin\" in eventQueue but actually noting in Queue.");
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
                throw new Exception("Algorithm " + GetType().ToString() + " is tring \"extractMin\" in eventQueue but actually noting in Queue.");
            }
        }
        #endregion
    }
}
