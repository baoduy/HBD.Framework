using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace HBD.Framework.Log
{
    public static class LogManager
    {
        public static class LogCategories
        {
            public const string Default = "General";
            public const string Error = "Error";
        }

        public static void Write(object message, string category = null)
        {
            try
            {
                if (message == null)
                    return;

                if (message is Exception)
                {
                    if (string.IsNullOrEmpty(category))
                        category = LogCategories.Error;
                    Logger.Write(message as Exception, category);
                    Console.WriteLine((message as Exception).ToString());
                }
                else
                {
                    if (string.IsNullOrEmpty(category))
                        category = LogCategories.Default;
                    Logger.Write(message, category);
                    Console.WriteLine(message);
                }
            }
            catch { }//If Log is not configured then do nothing.
        }

        public static void WriteError(object message)
        {
            try
            {
                Logger.Write(message, LogCategories.Error);
            }
            catch { }//If Log is not configured then do nothing.
        }
    }
}
