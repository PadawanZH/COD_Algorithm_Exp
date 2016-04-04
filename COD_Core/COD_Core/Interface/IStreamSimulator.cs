using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COD_Base.Interface
{
    /// <summary>
    /// IStreamController作为流数据模拟的管理模块,接收NoMoreTuple的事件
    /// </summary>
    public interface IStreamSimulator : IListener
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
        /// Tuple到来之间的间隔，可以通过外部调用来调节,in milisecond
        /// </summary>
        double StreamRate { get; set; }

        /// <summary>
        /// 实验控制变量
        /// </summary>
        #region ControlAttribute

        int SliedSpan { get; }
        int WindowSize { get; }
        #endregion

        void PauseSimulate();

        /// <summary>
        /// 作为流数据模拟的步骤函数,调用<see cref="IDataAdapter.GetNextTuple"/>取得下一个tuple，负责维护Window
        /// </summary>
        void PerformAStep(ITuple newTuple);

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

    }
}
