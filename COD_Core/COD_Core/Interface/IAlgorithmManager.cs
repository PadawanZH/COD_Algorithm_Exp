using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COD_Base.Interface
{
    /// <summary>
    /// 考虑用单例模式实现
    /// </summary>
    public interface IAlgorithmManager : IListener
    {
        /// <summary>
        /// 维护的算法列表
        /// </summary>
        ArrayList Algorithms { get; }
        /// <summary>
        /// 维护的算法指标，下标与算法列表统一
        /// </summary>
        Hashtable MetricsForAlgorithms { get; }

        /// <summary>
        /// 添加算法到算法列表中
        /// </summary>
        /// <param name="newAlgorithm"></param>
        void RegistAlgorithm(IAlgorithm newAlgorithm);

        /// <summary>
        /// 处理<see cref="EventType.NewTupleArrive"/>类型事件的函数,由<see cref="IAlgorithmManager.OnEvent"/>
        /// </summary>
        /// <param name="p">当事tuple</param>
        /// <param name="curStep">当前步数</param>
        void OnNewTupleArriveEvent(ITuple p);
    }
}
