/*
    Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCGalaxy)
    
    Dual-licensed under the Educational Community License, Version 2.0 and
    the GNU General Public License, Version 3 (the "Licenses"); you may
    not use this file except in compliance with the Licenses. You may
    obtain a copy of the Licenses at
    
    http://www.opensource.org/licenses/ecl2.php
    http://www.gnu.org/licenses/gpl-3.0.html
    
    Unless required by applicable law or agreed to in writing,
    software distributed under the Licenses are distributed on an "AS IS"
    BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
    or implied. See the Licenses for the specific language governing
    permissions and limitations under the Licenses.
 */
using System;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace MCGalaxy {

    public delegate void LogHandler(LogType type, string message);
    
    
    /// <summary> Centralised class for outputting log messages. </summary>
    /// <remarks> Outputs can be a file on disc, GUI, the console, etc subscribed to the LogHandler delegate. </remarks>
    public static class Logger {
        
        public static LogHandler LogHandler;
        static readonly object logLock = new object();
        
        public static void Log(LogType type, string message) {
            lock (logLock) {
                try {
                    if (LogHandler != null) LogHandler(type, message);
                } catch (Exception ex) {
                    // a LogHandler threw an exception, try to log that error
                    LogLoggerError(ex);
                }
            }
        }
        
        static void LogLoggerError(Exception ex) {
            try {
                LogHandler(LogType.Error, FormatException(ex));
            } catch {
                // give up if the problematic LogHandler still throws an error
            }
        }
        
        public static void Log(LogType type, string format, object arg0) {
            Log(type, string.Format(format, arg0));
        }
        
        public static void Log(LogType type, string format, object arg0, object arg1) {
            Log(type, string.Format(format, arg0, arg1));
        }
        
        public static void Log(LogType type, string format, object arg0, object arg1, object arg2) {
            Log(type, string.Format(format, arg0, arg1, arg2));
        }
        
        public static void Log(LogType type, string format, params object[] args) {
            Log(type, string.Format(format, args));
        }
        
        
        public static void LogError(string action, Exception ex) {
            Log(LogType.Warning, action);
            Log(LogType.Error, FormatException(ex));
        }
        
        /// <summary> Logs a LogType.Error message consisting of full details for the given Exception. </summary>
        public static void LogError(Exception ex) {
            Log(LogType.Error, FormatException(ex));
        }
        
        /// <summary> Returns a string fully describing the given Exception. </summary>
        public static string FormatException(Exception ex) {
            StringBuilder sb = new StringBuilder();
            while (ex != null) {
                DescribeError(ex, sb);
                ex = ex.InnerException;
            }
            return sb.ToString();
        }

        static void DescribeError(Exception ex, StringBuilder sb) {
            // Attempt to gather this info. Skip anything that you can't read for whatever reason
            try { sb.AppendLine("Type: " + ex.GetType().Name); } catch { }
            try { sb.AppendLine("Source: " + ex.Source); } catch { }
            try { sb.AppendLine("Message: " + ex.Message); } catch { }
            try { sb.AppendLine("Trace: " + ex.StackTrace); } catch { }
            
            // Exception-specific extra details
            try {
                ReflectionTypeLoadException refEx = ex as ReflectionTypeLoadException;
                if (refEx != null) LogLoaderErrors(refEx, sb);
            } catch { }
            
            try {
                SocketException sockEx = ex as SocketException;
                if (sockEx != null) LogSocketErrors(sockEx, sb);
            } catch { }
        }
        
        static void LogLoaderErrors(ReflectionTypeLoadException ex, StringBuilder sb) {
            // For errors with loading plugins (e.g. missing dependancy) you get a 
            //   Message: Unable to load one or more of the requested types. Retrieve the LoaderExceptions property for more information.
            // which is pretty useless by itself, so specifically handle this case
            sb.AppendLine("## Loader exceptions ##");
            
            foreach (Exception loadEx in ex.LoaderExceptions) {
                DescribeError(loadEx, sb);
            }
        }
        
        static void LogSocketErrors(SocketException ex, StringBuilder sb) {
            sb.AppendLine("Error: " + ex.SocketErrorCode);
        }
    }
}