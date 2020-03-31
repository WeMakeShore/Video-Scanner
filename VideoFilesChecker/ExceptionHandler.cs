using System;
using System.IO;
using VideoChecking;

namespace ExceptionHandler
{
    public class Exceptions
    {
        private static string exceptionFile = Program.settings.ExceptionLoggerPath;

        public static bool checkForPreviousException()
        {
            return File.Exists(exceptionFile) ? true : false; 
        }

        public static void LogException(Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("An exception has occurred.\n\n" + e, ConsoleColor.Red);

            using (StreamWriter sw = new StreamWriter(exceptionFile))
            {
                sw.WriteLine(e);
            }

            Console.ReadKey();
            Environment.Exit(-1);
        }

        public static void RemovePreviousExceptionLog()
        {
            if (File.Exists(exceptionFile))
            {
                File.Delete(exceptionFile);
            }
        }
    }
}
