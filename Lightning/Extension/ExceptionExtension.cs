using System;
using System.IO;

using Lightning.Manager;

namespace Lightning.Extension
{
    public static class ExceptionExtension
    {
        public static void LogTo(this Exception exception, God god)
        {
            string errorMessage;
            if (exception.InnerException == null)
            {
                errorMessage = $"{DateTime.Now}:\n--{exception.Message}\n{exception.StackTrace}\n\n";
            }
            else
            {
                errorMessage = $"{DateTime.Now}:\n--{exception.Message}\n{exception.StackTrace}\n--{exception.InnerException.Message}\n{exception.InnerException.StackTrace}\n\n";
            }
            File.AppendAllText(god.ErrorLog, errorMessage);
        }
    }
}