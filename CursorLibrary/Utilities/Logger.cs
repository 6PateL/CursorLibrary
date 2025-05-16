using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursorLibrary.Utilities
{
    /// <summary>
    /// global logger
    /// </summary>
    public static class Logger
    {
        private static readonly List<string> _logs = new(); 

        public static void AddLog(string log)
        {
            _logs.Add(log); 
        }

        /// <summary>
        /// get logs
        /// </summary>
        /// <returns>copy of Logger._logs</returns>
        public static string[] GetLogs()
        {
            return _logs.ToArray();
        }

        public static void ExportLogs(string folderPath, string fileName = "defaultLogs")
        {
            string logs = string.Join('\n', _logs); 

            if(!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException(); 
            }

            File.WriteAllText(Path.Combine(folderPath, fileName + ".txt"), logs);    
        }
    }
}
