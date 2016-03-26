using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COD_Base.Interface
{
    public interface IAlgorithm : IDisposable
    {
        string Description { get; }
        /// <summary>
        /// 新到Tuple时调用的方法
        /// <para>有关Tuple集管理的问题，可以维护一个Tuple全集，如key为是否是Outlier等状态，v是Tuple的HashTable的方式存储，存储一个Tuple的邻居时也可以比较方便得只存储ID即可，需要再到全集取出tuple</para>
        /// </summary>
        /// <param name="newTuple">新到的tuple对象</param>
        /// <param name="CurrentStep">当前的步数，流数据一个tuple到来算作一步</param>
        /// 
        void Arrive(ITuple newTuple, int currentStep);

        /// <summary>
        /// 旧Tuple depart时调用的方法
        /// </summary>
        /// <param name="oldTuple"></param>
        /// <param name="currentStep"></param>
        void Departure(ITuple oldTuple, int currentStep);

        void WindowSlide(IWindow newWindow);
    }
}
