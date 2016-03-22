using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COD_Base.Interface
{
    /// <summary>
    /// Event的发送目标，关于这方面的编码可以参看OpenMI的思路
    /// 继承了该接口表明此类可以接收IEvent事件，需要在初始化的时候使用<see cref="RegistToDistributor(IEventDIstributor)"/>注册到<see cref="IEventDIstributor"/>中
    /// </summary>
    public interface IListener
    {
        void OnEvent(IEvent anEvent);

        /// <summary>
        /// 具体参数待定
        /// </summary>
        void RegistToDistributor(IEventDIstributor eventDistributor);
    }
}
