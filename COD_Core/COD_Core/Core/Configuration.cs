using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COD_Base.Interface;
using COD_Base.Util;

public enum PropertiesType
{
    LogFilePath,
    StreamSimulateRate,
    WindowSize,
    SlideSpan,
    DataFilePath,
    DataDimension,
    TypeListOfDimension,
    Delimiter,
    QueryRange,
    KNeighbourThreshold
}

namespace COD_Base.Core
{
    public class Configuration : IConfiguration
    {
        public Configuration()
        {
            properties = new Hashtable();
        }

        public Configuration(Configuration copy)
        {
            properties = new Hashtable();
            foreach(DictionaryEntry configDE in copy.properties)
            {
                properties[configDE.Key] = configDE.Value;
            }
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

        public void SetProperty(PropertiesType key, Object value)
        {
            properties[key] = value;
        }

        public void SetupConfiguration(string LogFilePath, int WindowSize, int SildeSpan, string DataFilePath, int DataDimension, char[] Delimiter, double queryRange, int kNighbourThreshold)
        {
            properties[PropertiesType.LogFilePath] = LogFilePath;
            properties[PropertiesType.WindowSize] = WindowSize;
            properties[PropertiesType.SlideSpan] = SildeSpan;
            properties[PropertiesType.DataFilePath] = DataFilePath;
            properties[PropertiesType.DataDimension] = DataDimension;
            properties[PropertiesType.Delimiter] = Delimiter;
            properties[PropertiesType.QueryRange] = queryRange;
            properties[PropertiesType.KNeighbourThreshold] = kNighbourThreshold;
        }
    }
}
