using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COD_Base.Interface;
using COD_Base.Util;

namespace COD_Base.Core
{
    sealed class EventDistributor : IEventDIstributor
    {
        /// <summary>
        /// 单例模式引用
        /// </summary>
        private static EventDistributor instance;
        
        private EventDistributor()
        {
            _eventTable = new Hashtable();

        }
        public static EventDistributor GetInstance()
        {
            if(instance == null)
            {
                instance = new EventDistributor();
            }
            return instance;
        }

        private Hashtable _eventTable;

        public void SendEvent(IEvent anEvent)
        {
            ArrayList listenerListToSend = (ArrayList)_eventTable[anEvent.Type];
            Logger.GetInstance().Info(anEvent.SenderDescription + " SEND an event with type : " + anEvent.Type.ToString());
        }

        public void Subscribe(IListener listener, EventType eventType)
        {
            if (!_eventTable.Contains(eventType))
            {
                _eventTable[eventType] = new ArrayList();
            }
            //若该listener已经订阅该type，则无动作
            if( !((ArrayList)_eventTable[eventType]).Contains(listener))
            {
                ((ArrayList)_eventTable[eventType]).Add(listener);
                Logger.GetInstance().Info(listener.ToString() + " SUBSCRIBE the event with type : " + eventType.ToString());
            }
        }

        public void UnSubscribe(IListener listener, EventType eventType)
        {
            if( _eventTable.Contains(eventType))
            {
                ArrayList listenerList = (ArrayList)_eventTable[eventType];
                listenerList.Remove(listener);
                Logger.GetInstance().Info(listener.ToString() + " UNSUBSCRIBE the event with type : " + eventType.ToString());
            }
        }
    }
}
