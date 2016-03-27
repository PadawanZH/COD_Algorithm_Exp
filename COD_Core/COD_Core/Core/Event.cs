using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COD_Base.Interface;

namespace COD_Base.Core
{
    class Event : IEvent
    {
        EventType _type;
        string _senderDescription;
        Hashtable _attributeTable;
        DateTime _timeStamp;

        public Event(string senderDescription, EventType type)
        {
            _senderDescription = senderDescription;
            _type = type;
            _timeStamp = DateTime.Now;
            _attributeTable = new Hashtable();
            
        }
        public string Description
        {
            get
            {
                return this.ToString();
            }
        }

        public string SenderDescription
        {
            get
            {
                return _senderDescription;
            }
        }

        public DateTime TimeStamp
        {
            get
            {
                return _timeStamp;
            }
        }

        public EventType Type
        {
            get
            {
                return _type;
            }
        }

        public object GetAttribute(EventAttributeType key)
        {
            return _attributeTable[key];
        }

        public void AddAttribute(EventAttributeType key, Object value)
        {
            _attributeTable.Add(key, value);
        }

        public override string ToString()
        {
            return "Event send by " + _senderDescription + ". Event Type is " + Type + ". Timing : " + TimeStamp.ToString();
        }
    }
}
