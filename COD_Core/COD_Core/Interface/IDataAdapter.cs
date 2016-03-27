using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COD_Base.Interface
{
    public interface IDataAdapter
    {
        int Dimension { get; set; }
        string DataFilePath { get; set; }


        void Init();
        ITuple CovertTuple(string data);

        ITuple GetNextTuple();

        /// <summary>
        /// 释放占用的资源，关于该文件被多线程同时读的时候怎么办有待解决，如何实现多线程运行不同算法的环境？如何多线程读取同一个文件？
        /// </summary>
        /// <returns></returns>
        void Disposal();
    }
}
