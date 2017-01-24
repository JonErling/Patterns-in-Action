using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebForms.Code.Logging
{
    // singleton logger class through which all log events are processed.
    
    // ** Design Patterns: Singleton, Observer.
    
    public sealed class Logger
    {
        // delegate event handler that hooks up requests.

        public delegate void LogEventHandler(object sender, LogEventArgs e);

        // the Log event.

        public event LogEventHandler Log;

        #region The Singleton definition

        
        // the one and only Singleton Logger instance. 
        
        private static readonly Logger instance = new Logger();

        // private constructor. Initializes default severity to "Error".
        
        private Logger()
        {
            // default severity is Error level

            Severity = LogSeverity.Error;
        }

        
        // gets the instance of the singleton logger object
        
        public static Logger Instance
        {
            get { return instance; }
        }

        #endregion

        private LogSeverity _severity;

        // these booleans are used to improve performance.

        private bool _isDebug;
        private bool _isInfo;
        private bool _isWarning;
        private bool _isError;
        private bool _isFatal;

        
        // gets and sets the severity level of logging activity.
        
        public LogSeverity Severity
        {
            get { return _severity; }
            set
            {
                _severity = value;

                // set booleans to help improve performance

                int sev = (int)_severity;

                _isDebug = ((int)LogSeverity.Debug) >= sev ? true : false;
                _isInfo = ((int)LogSeverity.Info) >= sev ? true : false;
                _isWarning = ((int)LogSeverity.Warning) >= sev ? true : false;
                _isError = ((int)LogSeverity.Error) >= sev ? true : false;
                _isFatal = ((int)LogSeverity.Fatal) >= sev ? true : false;
            }
        }

       
        // log a message when severity level is "Debug" or higher.
        
        public void Debug(string message)
        {
            if (_isDebug)
                Debug(message, null);
        }

        // log a message when severity level is "Debug" or higher.
        
        public void Debug(string message, Exception exception)
        {
            if (_isDebug)
                OnLog(new LogEventArgs(LogSeverity.Debug, message, exception, DateTime.Now));
        }

        
        // log a message when severity level is "Info" or higher.

        public void Info(string message)
        {
            if (_isInfo)
                Info(message, null);
        }

       
        // log a message when severity level is "Info" or higher.

        public void Info(string message, Exception exception)
        {
            if (_isInfo)
                OnLog(new LogEventArgs(LogSeverity.Info, message, exception, DateTime.Now));
        }

        // log a message when severity level is "Warning" or higher.

        public void Warning(string message)
        {
            if (_isWarning)
                Warning(message, null);
        }

        
        // log a message when severity level is "Warning" or higher.

        public void Warning(string message, Exception exception)
        {
            if (_isWarning)
                OnLog(new LogEventArgs(LogSeverity.Warning, message, exception, DateTime.Now));
        }

        
        // log a message when severity level is "Error" or higher.
        
        public void Error(string message)
        {
            if (_isError)
                Error(message, null);
        }

        // log a message when severity level is "Error" or higher.
        
        public void Error(string message, Exception exception)
        {
            if (_isError)
                OnLog(new LogEventArgs(LogSeverity.Error, message, exception, DateTime.Now));
        }

        
        // log a message when severity level is "Fatal"
        
        public void Fatal(string message)
        {
            if (_isFatal)
                Fatal(message, null);
        }

        // log a message when severity level is "Fatal"

        public void Fatal(string message, Exception exception)
        {
            if (_isFatal)
                OnLog(new LogEventArgs(LogSeverity.Fatal, message, exception, DateTime.Now));
        }

        // invokes the Log event

        public void OnLog(LogEventArgs e)
        {
            if (Log != null)
            {
                Log(this, e);
            }
        }

        // attach a listening observer logging device to logger
        
        public void Attach(ILog observer)
        {
            Log += observer.Log;
        }

        
        // detach a listening observer logging device from logger

        public void Detach(ILog observer)
        {
            Log -= observer.Log;
        }
    }
}
