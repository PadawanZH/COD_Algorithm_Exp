using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COD_Base.Interface
{
    /// <summary>
    /// 作为与前端的配置初始化接口
    /// </summary>
    public interface IConfiguration
    {
        Object GetProperty(PropertiesType key);
        void SetProperty(PropertiesType key, Object value);
    }
}
