using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using COD_Base.Interface;



namespace COD_Base.Util
{
    class Logger : ILog,IDisposable
    {
        /// <summary>
        /// 单例模式引用
        /// </summary>
        private volatile static Logger instance;
        private Logger()
        {
            _logDirPath = System.Environment.CurrentDirectory;
            RunnLogFilePath = _logDirPath + "\\RunLog.log";

            Init();
        }
        public static Logger GetInstance()
        {
            if(instance == null)
            {
                instance = new Logger();
            }
            return instance;
        }

        protected string _logDirPath;
        protected string _runLogFilePath;
        protected StreamWriter logFileWriter;

        /// <summary>
        /// 由于Logger使用单例模式，加锁的辅助object可以不是全局的，因为各个Thread得到的mutex都是同一个实例
        /// </summary>
        protected readonly object mutex = new object();

        public string RunnLogFilePath
        {
            get
            {
                return _runLogFilePath;
            }
            set
            {
                _runLogFilePath = value;
                if(_logDirPath == null)
                {
                    logFileWriter = null;
                }
                else
                {
                    //以append模式写log文件
                    logFileWriter = new StreamWriter(_runLogFilePath, true);
                }
            }
        }

        public void WriteLog(string sourceID, LogLevel level, string msg)
        {
            string time = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
            string line = "[" + level.ToString() + " || " + time + "] { " + sourceID + " } ==> ";
            line += msg;
            lock (mutex)
            {
                Console.WriteLine(line);
                if (logFileWriter != null)
                {
                    logFileWriter.WriteLine(line);
                    logFileWriter.Flush();
                }
            }
        }

        public void Debug(string sourceID, string msg)
        {
            WriteLog(sourceID, LogLevel.DEBUG, msg);
        }

        public void Info(string sourceID, string msg)
        {
            WriteLog(sourceID, LogLevel.INFO, msg);
        }

        public void Warn(string sourceID, string msg)
        {
            WriteLog(sourceID, LogLevel.WARN, msg);
        }

        public void Error(string sourceID, string msg)
        {
            WriteLog(sourceID, LogLevel.ERROR, msg);
        }

        public void Init()
        {
            WriteLog("Initialization", LogLevel.INIT, "New Running in time : " + " " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
        }
        public void Dispose()
        {
            if(logFileWriter != null)
            {
                logFileWriter.Close();
                logFileWriter.Dispose();
            }
        }
    }
}
