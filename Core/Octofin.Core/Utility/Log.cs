using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Octofin.Core.Utility
{
    public sealed class Log
    {
        private Log() { }

        static Log()
        {
            logLocation = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Logs" + Path.DirectorySeparatorChar + "octofin";

            Directory.CreateDirectory(Path.GetDirectoryName(logLocation));
            writer = File.CreateText(logLocation + ".log");

            writeData("Log file for Codename Octofin - Opened " + timestamp());
            writeData("Version: " + Game.Version + " " + Game.Build);
            writeData(breakLine);

            var domain = AppDomain.CurrentDomain;
            domain.UnhandledException += new UnhandledExceptionEventHandler(unhandledException);
            domain.ProcessExit += new EventHandler(processExit);
        }

        public static bool Debugging = true;

        private const string breakLine = "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~";
        private static readonly StreamWriter writer;
        private static readonly string logLocation;
        private static int flushRate = 1000; //ms

        private static DateTime lastWrite = new DateTime(1970, 1, 1);

        public static void info(string message)
        {
            writeData(timestamp() + " [INFO] - " + message);
        }

        public static void warn(string message)
        {
            writeData(timestamp() + " [WARN] - " + message);
        }

        public static void warn(string message, Exception e)
        {
            writeData(breakLine);
            writeData(timestamp() + " [WARN] - " + message);
            writeData("Trace - " + e.Source);
            writeData(e.StackTrace);
            writeData(breakLine);
        }

        public static void error(string message)
        {
            writeData(timestamp() + " [ERROR] - " + message);
        }

        public static void error(string message, Exception e)
        {
            writeData(breakLine);
            writeData(timestamp() + " [ERROR] - " + message);
            writeData("Trace - " + e.Source);
            writeData(e.StackTrace);
            writeData(breakLine);
        }

        private static void writeData(string data)
        {
            writer.WriteLine(data);

            DateTime now = DateTime.Now;

            if(now.Subtract(lastWrite).TotalMilliseconds > flushRate)
            {
                writer.Flush();
                lastWrite = now;
            }
        }

        private static void unhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            writeData(breakLine);
            writeData("!!!!!!!!! UNHANDLED EXCEPTION !!!!!!!!!");
            writeData(timestamp() + " - " + args.ExceptionObject);
            writeData(breakLine);

            if(args.IsTerminating)
            {
                writeData("This exception was fatal.");
                cleanup();
            }
        }

        private static void processExit(object sender, EventArgs args)
        {
            writeData(breakLine);
            info("Processs is exiting.");
            cleanup();
        }

        private static void cleanup()
        {
            writer.Close();
            File.Move(logLocation + ".log", logLocation + "-" + DateTime.Now.ToString("yyyyMMdd-H-mm-ss") + ".log");
        }

        private static string timestamp()
        {
            return DateTime.Now.ToString("MM/dd/yy H:mm:ss.fff");
        }
    }
}
