using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Globalization;

namespace Utils.Classes
{
    public sealed class Logger
    {
        #region CONSTRUCTOR
        private Logger()
        {
        } 
        #endregion

        #region FIELDS
        private static object _syncObject = new object();
        private static String _errorFilePath = String.Empty;
        private static String _activityFilePath = String.Empty;
        private static LogType _logType = LogType.None; 
        #endregion

        #region METHODS
        public static LogType LogType
        {
            get { return Logger._logType; }
            set { Logger._logType = value; }
        }

        public static Boolean WriteError(String className, String methodName, String description, out String errorText)
        {
            try
            {
                lock (_syncObject)
                {
                    errorText = String.Empty;
                    _logType = LogType.Error;
                    writeError(className, methodName, description);
                }
            }
            catch (Exception exp)
            {
                errorText = exp.Message;
                return false;
            }
            return true;
        }

        public static Boolean WriteError(String className, String methodName, String description)
        {
            try
            {
                lock (_syncObject)
                {
                    _logType = LogType.Error;
                    writeError(className, methodName, description);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static Boolean WriteError(Exception pobjExc)
        {
            try
            {
                lock (_syncObject)
                {
                    _logType = LogType.Error;
                    string errorToWrite = string.Empty;
                    errorToWrite = pobjExc.Message;
                    if (!string.IsNullOrEmpty(pobjExc.StackTrace))
                        errorToWrite = errorToWrite + pobjExc.StackTrace;
                    writeError(errorToWrite);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static Boolean WriteActivity(String className, String methodName, String description, out String errorText)
        {
            try
            {
                lock (_syncObject)
                {
                    errorText = String.Empty;
                    _logType = LogType.Activity;
                    writeError(className, methodName, description);
                }
            }
            catch (Exception exp)
            {
                errorText = exp.Message;
                return false;
            }
            return true;
        }

        public static Boolean WriteActivity(String className, String methodName, String description)
        {
            try
            {
                lock (_syncObject)
                {
                    _logType = LogType.Activity;
                    writeError(className, methodName, description);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        private static void writeError(String className, String methodName, String description)
        {
            using (StreamWriter sw = new StreamWriter(GetFilePath(), true))
            {
                sw.WriteLine("-------------------------------- " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + " --------------------------------");
                sw.Write(Environment.NewLine);
                sw.Write("[ClassName]= " + className + "  [MethodName]=  " + methodName + "  " + (_logType == LogType.Error ? "[Error]= " : "[Activity]= ") + description);
                sw.Write(Environment.NewLine);
                sw.WriteLine("------------------------------------------------------------------------------------------------");
                sw.Flush();
            }
        }

        private static void writeError(String description)
        {
            using (StreamWriter sw = new StreamWriter(GetFilePath(), true))
            {
                sw.WriteLine(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture) + "--->");
                sw.Write(description);
                sw.Flush();
            }
        }

        public static String GetFilePath()
        {
            try
            {
                if (_logType == LogType.Error)
                {
                    //if (String.IsNullOrEmpty(_errorFilePath) || !File.Exists(_errorFilePath))
                    //{
                        String filePath = getDayFilePath();
                        if (!File.Exists(filePath))
                        {
                            using (StreamWriter sr = File.CreateText(filePath))
                            {
                                sr.Close();
                            }
                        }
                        _errorFilePath = filePath;
                    //}
                    return _errorFilePath;
                }
                else if (_logType == LogType.Activity)
                {
                    //if (String.IsNullOrEmpty(_activityFilePath) || !File.Exists(_activityFilePath))
                    //{
                        String filePath = getDayFilePath();
                        if (!File.Exists(filePath))
                        {
                            using (StreamWriter sr = File.CreateText(filePath))
                            {
                                sr.Close();
                            }
                        }
                        _activityFilePath = filePath;
                    //}
                    return _activityFilePath;
                }
            }
            catch
            {
            }
            return String.Empty;
        }

        private static String getDayFilePath()
        {
            String dayFileName = DateTime.Now.ToString("yyyy MMM dd", CultureInfo.InvariantCulture) + ".txt";
            String dayFilePath = Path.Combine(getMonthFolderPath(), dayFileName);
            return dayFilePath;
        }

        private static String getMonthFolderPath()
        {
            String monthFolderPath = Path.Combine(getYearFolderPath(), DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture));
            if (!Directory.Exists(monthFolderPath))
                Directory.CreateDirectory(monthFolderPath);
            return monthFolderPath;
        }

        private static String getYearFolderPath()
        {
            String yearFolderPath = Path.Combine(getLogFolderPath(), DateTime.Now.Year.ToString(CultureInfo.InvariantCulture));
            if (!Directory.Exists(yearFolderPath))
                Directory.CreateDirectory(yearFolderPath);
            return yearFolderPath;
        }

        private static String getLogFolderPath()
        {
            String logFolderPath = Path.Combine(getCurrentFolderPath(), (_logType == LogType.Error ? "General Error Log" : "General Activity Log"));
            if (!Directory.Exists(logFolderPath))
                Directory.CreateDirectory(logFolderPath);
            return logFolderPath;
        }

        private static String getCurrentFolderPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        } 
        #endregion
    }

    public enum LogType
    {
        Error,
        Activity,
        None
    }
}
