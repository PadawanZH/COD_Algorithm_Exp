using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COD_Base.Interface;
using COD_Base.Util;
using COD_Base.Core;

namespace COD_Base.Algorithms
{
    /// <summary>
    /// struct 的复制是浅拷贝还是深拷贝还有待研究，要是深拷贝的话就直接改成class
    /// </summary>
    public struct CODTuple
    {
        public ITuple tuple;
        public int NumberOfSucceedingNeighbour;
        //key: ID, value: expTime
        public Dictionary<int, int> PreceedingNeighboursExpTime;

        public CODTuple(ITuple t)
        {
            tuple = t;
            NumberOfSucceedingNeighbour = 0;
            PreceedingNeighboursExpTime = new Dictionary<int, int>();
        }

        public int FindMinExpTime()
        {
            int minTime = int.MaxValue;
            foreach(int expTime in PreceedingNeighboursExpTime.Values)
            {
                if(minTime > expTime)
                {
                    minTime = expTime;
                }
            }
            return minTime;
        }

        public void DeleteFromPreceedingExpTime(int tupleID)
        {
            PreceedingNeighboursExpTime.Remove(tupleID);
        }
    }

    public struct CODEvent
    {
        public CODTuple codTuple;
        public int eventTime;

        public CODEvent(CODTuple t, int e)
        {
            codTuple = t;
            eventTime = e;
        }
    }

    /// <summary>
    /// 应当是从小到大排序
    /// </summary>
    public class CODEventComparor : IComparer<CODEvent>
    {
        public int Compare(CODEvent x, CODEvent y)
        {
            if(x.eventTime > y.eventTime)
            {
                return 1;
            }
            else if(x.eventTime == y.eventTime)
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
    public class ContinuousOutlierDetection : IAlgorithm
    {
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

        public ContinuousOutlierDetection()
        {
            Initialize();
        }

        public void Initialize()
        {
            currentStep = 0;
            CODEventQueue = new SortedSet<CODEvent>(new CODEventComparor());
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
            CODTuple newCODTuple = new CODTuple(newTuple);

            if (ShouldWindowSlide())
            {
                for(int i = 0; i < _slideSpan; i++)
                {
                    CODTuple oldTuple = window.Dequeue();
                    Departure(oldTuple, currentStep);
                }
            }

            //Arrive函数是否要在窗口弹出之前执行呢？我觉得是在之后，因为窗口的定义为”总是维护最近的n个object“
            //而arrive应当是以window之内作为执行范围的，若在窗口弹出前执行，岂不是Arrive的执行范围变成了n+1? 所以我将Arrive放在这里
            Arrive(newCODTuple, currentStep);

            AddTupleIntoWindow(newCODTuple);
        }

        /// <summary>
        /// 先RangeQuery，再将Tuple加入window
        /// </summary>
        /// <param name="newCODTuple"></param>
        /// <param name="currentStep"></param>
        public void Arrive(CODTuple newCODTuple, int currentStep)
        {
            Dictionary<CODTuple, double> neighbours = RangeQuery(newCODTuple.tuple, range, neighbourThreshold);
            for(int i=0; i < neighbours.Keys.Count; i++)
            {
                CODTuple q = neighbours.Keys.ElementAt(i);
                q.NumberOfSucceedingNeighbour++;
                
                if(q.tuple.IsOutlier && (q.NumberOfSucceedingNeighbour + q.PreceedingNeighboursExpTime.Count == neighbourThreshold) )
                {
                    RemoveFromOutlier(q.tuple);

                    //该if语句是否应当在这里还有待考证（【28】中是在这里的，【原文】不在，但是感觉在这里是正确的）
                    if (q.PreceedingNeighboursExpTime.Count > 0)
                    {
                        Insert(q);
                    }
                }
            }
            AssignKNearestPreceedingNeighboursToTuple(newCODTuple, neighbours);
            
            ///
            if(newCODTuple.PreceedingNeighboursExpTime.Count < neighbourThreshold)
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
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            window.Clear();
            window = null;
            CODEventQueue.Clear();
            CODEventQueue = null;
        }

        protected Dictionary<CODTuple, double> RangeQuery(ITuple newTuple, double range, int neighbourThreshold)
        {
            double distance;
            Dictionary<CODTuple, double> neighbours = new Dictionary<CODTuple, double>();
            foreach(CODTuple codTuple in window)
            {
                if(TupleComputation.IsInRange(newTuple, codTuple.tuple, range, out distance))
                {
                    neighbours.Add(codTuple, distance);
                }
            }
            return neighbours;
        }

        public void RemoveFromOutlier(ITuple tuple)
        {
            tuple.IsOutlier = false;

            Event e = new Event("an tuple become inlier", EventType.OutlierStateChange);
            e.AddAttribute(EventAttributeType.Tuple, tuple);
            EventDistributor.GetInstance().SendEvent(e);
        }

        public void AddToOutlier(ITuple tuple)
        {
            tuple.IsOutlier = true;

            Event e = new Event("an tuple become outlier", EventType.OutlierStateChange);
            e.AddAttribute(EventAttributeType.Tuple, tuple);
            EventDistributor.GetInstance().SendEvent(e);
        }

        public void Insert(CODTuple q)
        {
            CODEvent newCODEvent = new CODEvent(q, q.FindMinExpTime());
            CODEventQueue.Add(newCODEvent);
        }

        public void AssignKNearestPreceedingNeighboursToTuple(CODTuple newTuple, Dictionary<CODTuple, double> neighbours)
        {
            Dictionary<CODTuple, double> sortedNeighbour = (from entry in neighbours orderby entry.Value ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
            Dictionary<int,int> k_NearestNeighbour = new Dictionary<int, int>();
            for(int i = 0; i < neighbourThreshold && i < sortedNeighbour.Count; i++)
            {
                int neighbourID, neighbourExpTime;
                neighbourID = sortedNeighbour.ElementAt(i).Key.tuple.ID;
                neighbourExpTime = sortedNeighbour.ElementAt(i).Key.tuple.DepartStep;
                k_NearestNeighbour.Add(neighbourID, neighbourExpTime);
            }
            newTuple.PreceedingNeighboursExpTime = k_NearestNeighbour;
        }

        public void AddTupleIntoWindow(CODTuple newTuple)
        {
            window.Enqueue(newTuple);
        }

        public bool ShouldWindowSlide()
        {
            if(window.Count == _windowSize)
            {
                return true;
            }else if(window.Count < _windowSize)
            {
                return false;
            }
            else
            {
                throw new Exception("Window count is " + window.Count + " exceed the windowSize");
            }
        }
    }
}
