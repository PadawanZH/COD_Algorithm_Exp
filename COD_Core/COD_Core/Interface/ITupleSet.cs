using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COD_Base.Interface
{
    /// <summary>
    /// 作为数据集合，提供一定的数据集操作,作为某个Tuple的前驱或后继邻居的管理等等
    /// 要提供一定的对Tuple进行访问的接口
    /// </summary>
    public interface ITupleSet
    {
        void AddTuple(ITuple tuple);
        void DeleteTupleFromSet(ITuple tuple);
    }
}
