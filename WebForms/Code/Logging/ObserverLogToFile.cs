using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebForms.Code.Logging
{
    
    // writes log events to a local file.
    // ** Design Pattern: Observer

    public class ObserverLogToFile : ILog
    {
        private readonly string _fileName;

        public ObserverLogToFile(string fileName)
        {
            _fileName = fileName;
        }

        // write a log request to a file

        public void Log(object sender, LogEventArgs e)
        {
            string message = "[" + e.Date.ToString() + "] " +
                e.SeverityString + ": " + e.Message;
            
            FileStream fileStream;

            // create directory, if necessary

            try
            {
                fileStream = new FileStream(_fileName, FileMode.Append);
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory((new FileInfo(_fileName)).DirectoryName);
                fileStream = new FileStream(_fileName, FileMode.Append);
            }

            // NOTE: be sure you have write privileges to folder

            var writer = new StreamWriter(fileStream);
            try
            {
                writer.WriteLine(message);
            }
            catch { /* do nothing */}
            finally
            {
                try
                {
                    writer.Close();
                }
                catch { /* do nothing */}
            }
        }
    }
}
