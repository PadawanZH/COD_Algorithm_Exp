using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COD_Base.Interface;
using COD_Base.Util;
using COD_Base.Core;
using System.IO;

namespace COD_Base.Dynamic.DataAccess
{
    /// <summary>
    /// 数据默认按照Double处理
    /// </summary>
    class ForestConvertTypeAdaptor : IDataAdapter
    {
        protected string _dataFilePath;
        protected int _dataDimension;
        //protected List<Type> _typeForEachDimension;
        protected StreamReader fileReader;
        protected char[] _delimiter;

        public string DataFilePath
        {
            get
            {
                return _dataFilePath;
            }
            set
            {
                _dataFilePath = value;
                if (!File.Exists(_dataFilePath))
                {
                    fileReader = null;
                    ExceptionUtil.SendErrorEventAndLog(GetType().ToString(), "No data file with the DataFilePath, recheck");
                }
                else
                {
                    if(fileReader != null)
                    {
                        fileReader.Close();
                    }
                    fileReader = new StreamReader(_dataFilePath);
                }
            }
        }

        public int Dimension
        {
            get
            {
                if (_dataDimension <= 0)
                {
                    ExceptionUtil.SendErrorEventAndLog(GetType().ToString(), "Dimension <= 0");
                    return -1;
                }
                else
                {
                    return _dataDimension;
                }
            }
            set
            {
                if(value <= 0 || value.GetType() != typeof(int))
                {
                    ExceptionUtil.SendErrorEventAndLog(GetType().ToString(), "Dimension < 0 or the type is not int, recheck");
                }
                else
                {
                    _dataDimension = value;
                }
            }
        }

        public void Init()
        {
            DataFilePath = (string) Configuration.GetInstance().GetProperty(PropertiesType.DataFilePath);
            Dimension = (int)Configuration.GetInstance().GetProperty(PropertiesType.DataDimension);
            _delimiter = (char[])Configuration.GetInstance().GetProperty(PropertiesType.Delimiter);
            //_typeForEachDimension = (List<Type>)Configuration.GetInstance().GetProperty(PropertiesType.TypeListOfDimension);
        }

        public void TestInit()
        {
            DataFilePath = System.Environment.CurrentDirectory + "\\InputData\\covtype.data";
            Dimension = 55;
            _delimiter = new char[] { ',' };
        }

        public ITuple GetNextTuple()
        {
            if (fileReader != null)
            {
                try
                {
                    string line = fileReader.ReadLine();
                    if (line == null)
                    {
                        //End of file, send the eof event
                        Event EofEvent = new Event(GetType().ToString(), EventType.NoMoreTuple);
                        EventDistributor.GetInstance().SendEvent(EofEvent);
                        return null;
                    }
                    else
                    {
                        return CovertTuple(line);
                    }
                }
                catch (Exception e)
                {
                    ExceptionUtil.SendErrorEventAndLog(GetType().ToString(), e.Message);
                    return null;
                }
            }
            else
            {
                ExceptionUtil.SendErrorEventAndLog(GetType().ToString(), "fileReader is null");
                return null;
            }
        }

        public ITuple CovertTuple(string line)
        {
            string[] data = line.Split(_delimiter);
            if (ValidateData(data) != false)
            {
                return initTuple(data);
            }
            else
            {
                ExceptionUtil.SendErrorEventAndLog(GetType().ToString(), "Error occurred during the ValidateData process, check the validation of the data or the properties used in function ValidateData");
                return null;
            }
        }

        public void Disposal()
        {
            if(fileReader != null)
            {
                fileReader.Close();
                fileReader = null;
            }
        }

        /// <summary>
        /// 验证读取的一行数据是否能转换为一个tuple
        /// </summary>
        /// <returns></returns>
        public bool ValidateData(string[] data)
        {
            if(data.Count() != Dimension)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 对Tuple中的数据属性进行初始化
        /// </summary>
        /// <param name="data"></param>
        public virtual ITuple initTuple(string[] data)
        {
            ITuple tuple = new Entity.Tuple();
            List<double> NumData = new List<double>();
            for(int i = 0; i < data.Length; i++)
            {
                NumData.Add(Convert.ToDouble(data[i]));
            }
            tuple.Data = NumData;
            return tuple;
        }
    }
}
