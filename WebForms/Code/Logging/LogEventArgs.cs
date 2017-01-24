using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebForms.Code.Logging
{
    /// <summary>
    /// Contains log specific event data for log events.
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor of LogEventArgs.
        /// </summary>
        /// <param name="severity">Log severity.</param>
        /// <param name="message">Log message</param>
        /// <param name="exception">Inner exception.</param>
        /// <param name="date">Log date.</param>
        public LogEventArgs(LogSeverity severity, string message, Exception exception, DateTime date)
        {
            Severity = severity;
            Message = message;
            Exception = exception;
            Date = date;
        }

        /// <summary>
        /// Gets and sets the log severity.
        /// </summary>        
        public LogSeverity Severity { get; }

        /// <summary>
        /// Gets and sets the log message.
        /// </summary>        
        public string Message { get; }

        /// <summary>
        /// Gets and sets the optional inner exception.
        /// </summary>        
        public Exception Exception { get; }

        /// <summary>
        /// Gets and sets the log date and time.
        /// </summary>        
        public DateTime Date { get; }

        /// <summary>
        /// Friendly string that represents the severity.
        /// </summary>
        public string SeverityString
        {
            get { return Severity.ToString("G"); }
        }

        /// <summary>
        /// LogEventArgs as a string representation.
        /// </summary>
        /// <returns>String representation of the LogEventArgs.</returns>
        public override string ToString()
        {
            return "" + Date
                + " - " + SeverityString
                + " - " + Message
                + " - " + Exception.ToString();
        }
    }
}
