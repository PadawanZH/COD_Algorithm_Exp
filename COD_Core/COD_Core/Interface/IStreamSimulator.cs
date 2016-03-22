using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COD_Base.Interface
{
    /// <summary>
    /// IStreamController作为流数据模拟的管理模块
    /// </summary>
    public interface IStreamSimulator
    {
        /// <summary>
        /// 是否已经处理完成，当该项为true，整个算法的处理进程应当完成
        /// </summary>
        bool IsProcessOver { get; }

        /// <summary>
        /// 当前步数
        /// </summary>
        int CurrentStep { get; }

        /// <summary>
        /// Tuple到来之间的间隔，可以通过外部调用来调节
        /// </summary>
        double StreamRate { get; set; }

        /// <summary>
        /// 实验控制变量
        /// </summary>
        #region ControlAttribute

        int SliedSpan { get; }
        int WindowSize { get; }
        #endregion

        /// <summary>
        /// 作为对外的接口，开始读取流数据并进行模拟，应当另开一个线程，以StreamRate进行流数据模拟
        /// </summary>
        void StartStreamSimulate();

        void PauseSimulate();

        /// <summary>
        /// 作为流数据模拟的步骤函数,调用<see cref="IDataAdapter.GetNextTuple"/>取得下一个tuple，负责维护Window
        /// </summary>
        void PerformAStep();

        /// <summary>
        /// 维护窗口，当其认为窗口滑动，则调用<see cref="IStreamSimulator.OnWindowSlide"/>。
        /// </summary>
        void CheckWindow();

        /// <summary>
        /// 窗口滑动
        /// </summary>
        void OnWindowSlide();
        void OnNewTupleArrive();
        void OnOldTupleDepart();

        /// <summary>
        /// 发送Event，一般是维护一个hashtable，key为eventtype，value为IListener的k-v对
        /// </summary>
        void SendEvent(IEvent anEvent);

    }
}
