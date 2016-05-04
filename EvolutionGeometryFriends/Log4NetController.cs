using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionGeometryFriends {
    public class Log4NetController
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public enum LogLevel
        {
            Error, Debug, Fatal, Info, Warn
        }

        public static void Log(string message, LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Error:
                    log.Error(message);
                    break;
                case LogLevel.Debug:
                    log.Debug(message);
                    break;
                case LogLevel.Fatal:
                    log.Fatal(message);
                    break;
                 case LogLevel.Info:
                    log.Info(message);
                    break;
                 case LogLevel.Warn:
                    log.Warn(message);
                    break;
            }
        }
    }
}
