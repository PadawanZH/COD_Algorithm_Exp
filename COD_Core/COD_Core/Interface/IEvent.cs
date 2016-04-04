using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COD_Base.Interface
{
    public enum EventType : int
    {
        NewTupleArrive = 0,
        OldTupleDepart = 1,
        WindowSlide = 2,
        OutlierStateChange = 3,
        NoMoreTuple = 4,
        DataChanged = 5,
        Error = 6,
        /// <summary>
        /// 作为界面显示消息的MessageBox使用
        /// </summary>
        Informative = 7
    }

    public enum EventAttributeType
    {
        Message,
        Tuple,
        Window
    }
    public interface IEvent
    {
        EventType Type { get; }
        string Description { get; }

        /// <summary>
        /// Sender身份的描述
        /// </summary>
        string SenderDescription { get; }

        DateTime TimeStamp { get; }

        /// <summary>
        /// 从key-value对的hashtable之类的成员中获得Event所带的内容，可能是新的Outlier的信息，可能是过期的节点的信息
        /// 如NewTupleArrive事件，其Attribute就要带上NewTuple的数据供算法运行模块向算法调用方法并传递参数。
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>返回的信息Object</returns>
        object GetAttribute(EventAttributeType key);
    }
}
