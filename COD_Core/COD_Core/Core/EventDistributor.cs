using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using COD_Base.Interface;
using COD_Base.Util;

namespace COD_Base.Core
{
    public sealed class EventDistributor : IEventDIstributor
    {
        /// <summary>
        /// 单例模式引用
        /// </summary>
        private static EventDistributor instance;
        
        private EventDistributor()
        {
            _eventTable = new Dictionary<EventType,List<IListener>>();
            ID = GetType().ToString();

        }
        public static EventDistributor GetInstance()
        {
            if(instance == null)
            {
                instance = new EventDistributor();
            }
            return instance;
        }

        /// <summary>
        /// 其中eventType为key，subscribe这种eventtype的的listener列表为value
        /// </summary>
        protected Dictionary<EventType, List<IListener>> _eventTable;
        public string ID;

        /// <summary>
        /// 在<see cref="GetInstance"/>的lazy实例化后调用，用以初始化一些变量
        /// </summary>
        public void Initialize()
        {
            
        }

        /// <summary>
        /// 没有实现,应该开后台线程去处理事件。
        /// </summary>
        /// <param name="anEvent"></param>
        public void SendEvent(IEvent anEvent)
        {
            HandleEventThreadFunction(anEvent);
            /*Thread handleEventThread = new Thread(new ParameterizedThreadStart(HandleEventThreadFunction));
            handleEventThread.Start(anEvent);*/
        }

        public void Subscribe(IListener listener, EventType eventType)
        {
            if (!_eventTable.ContainsKey(eventType))
            {
                _eventTable[eventType] = new List<IListener>();
            }
            //若该listener已经订阅该type，则无动作
            if( !(_eventTable[eventType]).Contains(listener))
            {
                _eventTable[eventType].Add(listener);
                Logger.GetInstance().Info(ID, " SUBSCRIBE the event with type : " + eventType.ToString());
            }
        }

        public void UnSubscribe(IListener listener, EventType eventType)
        {
            if( _eventTable.ContainsKey(eventType))
            {
                _eventTable[eventType].Remove(listener);
                Logger.GetInstance().Info(ID, " UNSUBSCRIBE the event with type : " + eventType.ToString());
            }
        }

        public void SubcribeListenerWithFullAcceptedTypeList(IListener listener, EventType[] acceptedList)
        {
            foreach(EventType eventType in acceptedList)
            {
                Subscribe(listener, eventType);
            }
        }

        public void HandleEventThreadFunction(Object anEvent)
        {
            IEvent eventToSend = (IEvent)anEvent;
            if (_eventTable.ContainsKey(eventToSend.Type))
            {
                List<IListener> listenerListToSend = _eventTable[eventToSend.Type];
                foreach (IListener listener in listenerListToSend)
                {
                    listener.OnEvent(eventToSend);
                }
                Logger.GetInstance().Info(ID, " SEND an event with type : " + eventToSend.Type.ToString());
            }
        }
    }
}
