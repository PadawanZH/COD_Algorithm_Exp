using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COD_Base.Interface
{
    /// <summary>
    /// <para>维护一个HashTable，存储EventType-Target的k-v对，方便Event分发</para>
    /// <para>负责接收从<see cref="IStreamManager"/>传来的Event,并根据HashTable的连接调用响应接口函数。</para>
    /// 
    /// <para>可以考虑单例模式的实现，因为一般情况下只允许一个<see cref="IEventDIstributor"/>和<see cref="IStreamManager"/>存在,并且单例模式有利于其他组件找到上述两个类的单例引用</para>
    /// </summary>
    public interface IEventDIstributor
    {
        /// <summary>
        /// 不同<see cref="EventType"/>区别处理。
        /// </summary>
        /// <param name="anEvent"></param>
        void SendEvent(IEvent anEvent);

        void Subscribe(IListener listener, EventType eventType);
        void UnSubscribe(IListener listener, EventType eventType);

        void SubcribeListenerWithFullAcceptedTypeList(IListener listener, EventType[] acceptedList);

    }
}
