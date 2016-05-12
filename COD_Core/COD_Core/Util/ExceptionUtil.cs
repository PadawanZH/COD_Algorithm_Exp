using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COD_Base.Core;
using COD_Base.Interface;

namespace COD_Base.Util
{
    public class ExceptionUtil
    {
        public static void SendErrorEventAndLog(string sendDestription, string errorMsg)
        {
            Event errorEvent = new Event(sendDestription, EventType.Error);
            errorEvent.AddAttribute(EventAttributeType.Message, errorMsg);
            EventDistributor.GetInstance().SendEvent(errorEvent);

            Logger.GetInstance().Error(sendDestription, errorMsg);
        }
    }
}
