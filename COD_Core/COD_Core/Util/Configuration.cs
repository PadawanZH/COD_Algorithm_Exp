using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COD_Base.Interface;
using COD_Base.Core;

public enum PropertiesType
{
    LogFilePath,
    StreamSimulateRate,
    WindowSize,
    SlideSpan,
    DataFilePath,
    DataDimension,
    TypeListOfDimension,
    Delimiter
}

namespace COD_Base.Util
{
    class Configuration : IConfiguration
    {
        private static Configuration instance;

        private Configuration()
        {
            properties = new Hashtable();
        }

        public static Configuration GetInstance()
        {
            if(instance == null)
            {
                instance = new Configuration();
            }
            return instance;
        }

        protected Hashtable properties;

        public Object GetProperty(PropertiesType key)
        {
            try
            {
                if (properties.ContainsKey(key))
                {
                    return properties[key];
                }
                else
                {
                    throw new Exception("Something is trying to get the property with TYPE = " + key.ToString() + ", which is not exist yet, maybe you forget to set it?");
                }
            }
            catch(Exception e)
            {
                ExceptionUtil.SendErrorEventAndLog(GetType().ToString(), e.Message);
                throw e;
                //return null;
            }
        }
    }
}
