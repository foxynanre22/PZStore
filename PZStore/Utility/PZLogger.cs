using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PZStore.Utility
{
    //this class was written with singleton pattern to use only one logger in the whole app
    public class PZLogger : IPZLogger
    {
        private static PZLogger instance;
        private static Logger logger;

        //private constructor
        private PZLogger()
        {
        }

        public static PZLogger GetInstance()
        {
            if(instance == null)
            {
                instance = new PZLogger();
            }
            
            return instance;
        }

        private Logger GetLogger(string theLogger)
        {
            if (PZLogger.logger == null)
            {
                PZLogger.logger = LogManager.GetLogger(theLogger);
            }

            return logger;
        }

        public void Debug(string message, string arg = null)
        {
            if (arg == null)
            {
                GetLogger("PZLoggerRule").Debug(message);
            }
            else
            {
                GetLogger("PZLoggerRule").Debug(message, arg);
            }
        }

        public void Error(string message, string arg = null)
        {
            if (arg == null)
            {
                GetLogger("PZLoggerRule").Error(message);
            }
            else
            {
                GetLogger("PZLoggerRule").Error(message, arg);
            }
        }

        public void Info(string message, string arg = null)
        {
            if (arg == null)
            {
                GetLogger("PZLoggerRule").Info(message);
            }
            else
            {
                GetLogger("PZLoggerRule").Info(message, arg);
            }
        }

        public void Warning(string message, string arg = null)
        {
            if (arg == null)
            {
                GetLogger("PZLoggerRule").Warn(message);
            }
            else
            {
                GetLogger("PZLoggerRule").Warn(message, arg);
            }
        }
    }
}