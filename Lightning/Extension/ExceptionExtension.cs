using System;
using System.IO;

namespace Lightning.Extension
{
    public static class ExceptionExtension
    {
        public static void Log(this Exception exception, string file)
        {
            File.AppendAllText(file, DateTime.Now.ToString() + " " + exception.Message + "\n" + exception.StackTrace + "\n");
        }
    }
}