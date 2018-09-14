/**********************************************************
** 文件名： Log.cs
** 文件作用:日志记录类
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2018-09-14    xwj       增加
**
**********************************************************/
using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.IO;

namespace AFC.MonitorMap.Core.Logging
{
    public class Log
    {
        // 日志备份目录
        public const string BackUpDirName = "backup";

        private const string errLogName = "ERR";
        private const string sysLogName = "SYS";
        private const string ntpLogName = "NTP";
        private const string allLineDeviceAlarmLogName = @"\A\l\a\r\m\L\o\g";
        private const string deviceConfigLogName = @"\d\e\f\L\o\g";

        private ILog errorLog;
        private ILog sysLog;
        private ILog ntpLog;
        private ILog allLineDeviceAlarmLog;
        private ILog deviceConfigLog;

        private static Log instance = new Log();

        public static Log Instance
        {
            get
            {
                return instance;
            }
        }

        private Log()
        {
            try
            {
                InitErrorLog();
                InitSysLog();
                InitNTPLog();
                InitAllLineDeviceAlarmLog();
                InitDeviceConfigLog();
            }
            catch
            {
                // 初始化日志失败
            }
        }

        private IAppender CreateRollingFileAppender(string logName)
        {
            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%d{yyyy-MM-dd HH:mm:ss} [%t] %-5level  %m%n";
            patternLayout.ActivateOptions();

            RollingFileAppender roller = new RollingFileAppender();
            roller.Layout = patternLayout;
            roller.LockingModel = new log4net.Appender.RollingFileAppender.MinimalLock();
            roller.Name = logName;
            roller.AppendToFile = true;
            roller.RollingStyle = RollingFileAppender.RollingMode.Date;
            roller.DatePattern = string.Format(@"\\\\yyyy-MM-dd\\\\{0}yyyyMMdd'.log'", logName);
            roller.StaticLogFileName = false;
            roller.File = string.Format("{0}\\", GetCurrentLogRootDirectory());
            roller.ActivateOptions();
            return roller;
        }

        private void InitDeviceConfigLog()
        {
            deviceConfigLog = LogManager.GetLogger(deviceConfigLogName);
            (deviceConfigLog.Logger as Logger).AddAppender(CreateRollingFileAppender(deviceConfigLogName));
            (deviceConfigLog.Logger as Logger).Hierarchy.Configured = true;
        }

        private void InitErrorLog()
        {
            errorLog = LogManager.GetLogger(errLogName);
            (errorLog.Logger as Logger).AddAppender(CreateRollingFileAppender(errLogName));
            (errorLog.Logger as Logger).Hierarchy.Configured = true;
        }

        private void InitSysLog()
        {
            sysLog = LogManager.GetLogger(sysLogName);
            (sysLog.Logger as Logger).AddAppender(CreateRollingFileAppender(sysLogName));
            (sysLog.Logger as Logger).Hierarchy.Configured = true;
        }

        private void InitNTPLog()
        {
            ntpLog = LogManager.GetLogger(ntpLogName);
            (ntpLog.Logger as Logger).AddAppender(CreateRollingFileAppender(ntpLogName));
            (ntpLog.Logger as Logger).Hierarchy.Configured = true;
        }

        private void InitAllLineDeviceAlarmLog()
        {
            allLineDeviceAlarmLog = LogManager.GetLogger(allLineDeviceAlarmLogName);
            (allLineDeviceAlarmLog.Logger as Logger).AddAppender(CreateRollingFileAppender(allLineDeviceAlarmLogName));
            (allLineDeviceAlarmLog.Logger as Logger).Hierarchy.Configured = true;
        }

        public string GetCurrentLogRootDirectory()
        {
            try
            {
                var logRoot = Path.Combine(Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().Location), "Log");
                if (!Directory.Exists(logRoot))
                {
                    Directory.CreateDirectory(logRoot);
                }
                return logRoot;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 记录Debug信息
        /// </summary>
        /// <param name="msg">信息</param>
        public void Debug(string msg)
        {
            sysLog.Debug(msg);
        }

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="e">异常</param>
        public void Error(string msg, Exception e)
        {
            errorLog.Error(msg, e);
        }

        public void LogNtpInfo(string info)
        {
            ntpLog.Info(info);
        }

        public void LogNtpError(string info)
        {
            ntpLog.Error(info);
        }

        public void LogNtpError(string info, Exception ex)
        {
            ntpLog.Error(info, ex);
        }

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="msg">信息</param>
        public void Error(string msg)
        {
            errorLog.Error(msg);
        }

        /// <summary>
        /// 记录重大错误信息
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="e">异常</param>
        public void Fatal(string msg, Exception e)
        {
            errorLog.Fatal(msg, e);
        }

        /// <summary>
        /// 记录重大错误信息
        /// </summary>
        /// <param name="msg">信息</param>
        public void Fatal(string msg)
        {
            errorLog.Fatal(msg);
        }

        /// <summary>
        /// 记录提示信息
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="e">异常</param>
        public void Info(string msg, Exception e)
        {
            sysLog.Info(msg, e);
        }

        /// <summary>
        /// 记录提示信息
        /// </summary>
        /// <param name="msg">信息</param>
        public void Info(string msg)
        {
            sysLog.Info(msg);
        }

        /// <summary>
        /// 记录警告信息
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="e">异常</param>
        public void Warn(string msg, Exception e)
        {
            sysLog.Warn(msg, e);
        }

        /// <summary>
        /// 记录警告信息
        /// </summary>
        /// <param name="msg">信息</param>
        public void Warn(string msg)
        {
            sysLog.Warn(msg);
        }

        public void AlarmLogInfo(string msg)
        {
            allLineDeviceAlarmLog.Info(msg);
        }

        public void DeviceConfigInfo(string msg)
        {
            deviceConfigLog.Info(msg);
        }

        public void DeviceConfigError(string msg, Exception ex)
        {
            deviceConfigLog.Error(msg, ex);
        }

        public void DeviceConfigError(string msg)
        {
            deviceConfigLog.Error(msg);
        }
    }
}