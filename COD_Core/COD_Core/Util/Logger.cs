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
        private volatile static Logger logger = new Logger();
        private Logger()
        {
            _logDirPath = System.Environment.CurrentDirectory;
            logger.RunnLogFilePath = _logDirPath + "\\RunLog.log";
        }
        public static Logger GetInstance()
        {
            return logger;
        }

        private string _logDirPath;
        private string _runLogFilePath;
        private StreamWriter logFileWriter;

        /// <summary>
        /// 由于Logger使用单例模式，加锁的辅助object可以不是全局的，因为各个Thread得到的mutex都是同一个实例
        /// </summary>
        private readonly object mutex = new object();

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
                    logFileWriter = new StreamWriter(_runLogFilePath);
                }
            }

        }

        public void WriteLog(LogLevel level, string msg)
        {
            string time = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
            string line = "[" + level.ToString() + " || " + time + "]";
            line += msg;
            lock (mutex)
            {
                Console.WriteLine(line);
                if(logFileWriter != null)
                {
                    logFileWriter.WriteLine(line);
                    logFileWriter.Flush();
                }
            }
        }

        public void Debug(string msg)
        {
            WriteLog(LogLevel.DEBUG, msg);
        }

        public void Info(string msg)
        {
            WriteLog(LogLevel.INFO, msg);
        }

        public void Warn(string msg)
        {
            WriteLog(LogLevel.WARN, msg);
        }

        public void Error(string msg)
        {
            WriteLog(LogLevel.ERROR, msg);
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
